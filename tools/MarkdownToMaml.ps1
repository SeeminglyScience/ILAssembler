using namespace System.Collections.Generic
using namespace System.Xml.Linq
using namespace Markdig.Extensions.Tables
using namespace Markdig.Syntax
using namespace Markdig.Syntax.Inlines

[CmdletBinding()]
param(
    [string] $DocsPath,

    [string] $DestinationFile
)
begin {
    class TokenWalker {
        hidden [IList[Block]] $_blocks;

        hidden [int] $_i;

        [Block] $Current;

        TokenWalker([IList[Block]] $blocks) {
            $this._blocks = $blocks
        }

        [Block] Peek() {
            if ($this._i + 1 -eq $this._blocks.Count) {
                return $null
            }

            return $this._blocks[$this._i + 1]
        }

        [bool] MoveNext() {
            if ($this._i -ge $this._blocks.Count - 1) {
                $this.Current = $null
                $this._i = $this._blocks.Count
                return $false
            }

            $this.Current = $this._blocks[++$this._i]
            return $true
        }

        [bool] MovePrevious() {
            if ($this._i -le 0) {
                $this._i = -1
                $this.Current = $null
                return $false
            }

            $this.Current = $this._blocks[--$this._i]
            return $true
        }
    }

    class MamlBuilder {
        static hidden [XNamespace] $maml = 'http://schemas.microsoft.com/maml/2004/10'
        static hidden [XNamespace] $cmd = 'http://schemas.microsoft.com/maml/dev/command/2004/10'
        static hidden [XNamespace] $dev = 'http://schemas.microsoft.com/maml/dev/2004/10'
        static hidden [XNamespace] $mshelp = 'http://msdn.microsoft.com/mshelp'
        static hidden [XNamespace] $xmlns = 'http://msh'

        hidden [string] $_text;

        hidden [string] $_md;

        hidden [TokenWalker] $_blocks;

        hidden [XElement] $_syntax;

        hidden [XElement] $_synopsis;

        hidden [XElement] $_description;

        hidden [string] $_name;

        MamlBuilder([string] $text, [MarkdownDocument] $md, [string] $name) {
            $this._text = $text
            $this._md = $md
            $this._blocks = [TokenWalker]::new($md)
            $this._name = $name
        }

        static [void] ParseAll([string[]] $source, [string] $destination) {
            $doc = [MamlBuilder]::ParseAll($source).ToString()
            [IO.File]::WriteAllText(
                $destination,
                '<?xml version="1.0" encoding="utf-8"?>' +
                    [Environment]::NewLine +
                    $doc)
        }

        static [XElement] ParseAll([string[]] $paths) {
            $elements = foreach ($path in $paths) {
                [MamlBuilder]::Parse($path)
            }

            return [XElement]::new([MamlBuilder]::xmlns + 'helpItems',
                @(
                    [XAttribute]::new('schema', 'maml')
                    $elements))
        }

        static [XElement] Parse([string] $path) {
            $text = Get-Content -Raw $path
            $md = $text | ConvertFrom-Markdown
            $name = [System.IO.Path]::GetFileNameWithoutExtension($path)
            $builder = [MamlBuilder]::new($text, $md.Tokens, $name)
            return $builder.Parse()
        }

        [XElement] Parse() {
            $uri = ''
            while ($this._blocks.MoveNext()) {
                if ($this._blocks.Current -isnot [HeadingBlock]) {
                    continue
                }

                foreach ($inline in $this._blocks.Current.Inline) {
                    if ($inline -is [LinkInline]) {
                        $uri = $inline.Content?.ToString() ?? ''
                        break
                    }
                }

                break
            }

            $this.MoveToHeader('SYNOPSIS')
            $synopsis = $this.ReadUntilNextHeader()

            $this.MoveToHeader('SYNTAX')
            $syntaxItems = $(
                while ($true) {
                    if (-not $this._blocks.MoveNext()) { throw }
                    if ($this._blocks.Current -isnot [HeadingBlock]) {
                        # yield
                        [XElement]::new($this::cmd + 'syntaxItem',
                            [XElement]::new($this::maml + 'name',
                                $this._blocks.Current.Lines.Slice[0]))
                        break
                    }

                    $header = $this.GetHeaderName($this._blocks.Current)
                    if ($header -eq 'DESCRIPTION') {
                        $null = $this._blocks.MovePrevious()
                        break
                    }

                    if (-not $this._blocks.MoveNext()) { throw }
                    # yield
                    [XElement]::new($this::cmd + 'syntaxItem',
                        [XElement]::new($this::maml + 'name',
                            $this._blocks.Current.Lines.Slice[0] -replace ' \[<CommonParameters>\]'))
                })

            $this.MoveToHeader('DESCRIPTION')
            $description = $this.ReadUntilNextHeader()

            $examples = @()
            if ($this._blocks.MoveNext()) {
                if ($this.GetHeaderName($this._blocks.Current) -eq 'EXAMPLES') {
                    $examples = $this.ReadExamples()
                } else {
                    $this._blocks.MovePrevious()
                }
            }

            $parameters = @()
            if ($this._blocks.MoveNext()) {
                if ($this.GetHeaderName($this._blocks.Current) -eq 'PARAMETERS') {
                    while ($parameterName = $this.MoveToNextHeader() -replace '^-*') {
                        if ($parameterName -eq 'CommonParameters') {
                            break
                        }

                        if ($parameterName -eq 'INPUTS') {
                            $this._blocks.MovePrevious()
                            break
                        }

                        $type = [string]::Empty
                        $aliases = [string]::Empty
                        $required = [string]::Empty
                        $position = [string]::Empty
                        $default = [string]::Empty
                        $pipeline = [string]::Empty
                        $wildcard = [string]::Empty
                        $elements = while ($this._blocks.MoveNext()) {
                            if ($this._blocks.Current -is [FencedCodeBlock]) {
                                $pattern = '[.\s]+?Type: (?<Type>.+)\s+' +
                                    'Parameter Sets: (?<Sets>.+)\s+' +
                                    'Aliases: ?(?<Aliases>.*)\s*' +
                                    'Required: (?<Required>.+)\s+' +
                                    'Position: (?<Position>.+)\s+' +
                                    'Default value: (?<Default>.+)\s*' +
                                    'Accept pipeline input: (?<Pipeline>.+)\s+' +
                                    'Accept wildcard characters: (?<Wildcard>.+)'

                                if ($this.GetText($this._blocks.Current) -notmatch $pattern) {
                                    throw
                                }

                                $type = $matches['Type'].Trim()
                                $aliases = $matches['Aliases'].Trim()
                                $required = $matches['Required'].Trim()
                                $position = $matches['Position'].Trim()
                                $default = $matches['default'].Trim()
                                $pipeline = $matches['pipeline'].Trim()
                                $wildcard = $matches['wildcard'].Trim()
                                break
                            }

                            $this.Convert($this._blocks.Current)
                        }

                        $parameters += [XElement]::new($this::cmd + 'parameter',
                            [XAttribute]::new('required', $required.ToLower()),
                            [XAttribute]::new('variableLength', 'false'),
                            [XAttribute]::new('globbing', $wildcard.ToLower()),
                            [XAttribute]::new('pipelineInput', $pipeline.ToLower()),
                            [XAttribute]::new('position', $position.ToLower()),
                            [XAttribute]::new('aliases', $aliases),
                            [XElement]::new($this::maml + 'name', $parameterName),
                            [XElement]::new($this::maml + 'Description', $elements),
                            [XElement]::new($this::cmd + 'parameterValue',
                                [XAttribute]::new('required', $required.ToLower()),
                                [XAttribute]::new('variableLength', 'false'),
                                $type),
                            [XElement]::new($this::dev + 'type',
                                [XElement]::new($this::maml + 'name', $type),
                                [XElement]::new($this::maml + 'uri', @())),
                            [XElement]::new($this::dev + 'defaultValue', $default))
                    }
                } else {
                    $this._blocks.MovePrevious()
                }
            }

            $this.MoveToHeader('INPUTS')
            if (-not $this._blocks.MoveNext()) { throw }
            $inputType = $this.GetHeaderName($this._blocks.Current)
            $inputDesc = $this.ReadUntilNextHeader()

            $this.MoveToHeader('OUTPUTS')
            if (-not $this._blocks.MoveNext()) { throw }
            $outputType = $this.GetHeaderName($this._blocks.Current)
            $outputDesc = $this.ReadUntilNextHeader()

            $verb, $noun = $this._name -split '-', 2
            return [XElement]::new($this::cmd + 'command',
                    [XElement]::new($this::cmd + 'details',
                        [XElement]::new($this::cmd + 'name', $this._name),
                        [XElement]::new($this::cmd + 'verb', $verb),
                        [XElement]::new($this::cmd + 'noun', $noun),
                        [XElement]::new($this::maml + 'description', $synopsis)),
                    [XElement]::new($this::maml + 'description', $description),
                    [XElement]::new($this::cmd + 'syntax', $syntaxItems),
                    [XElement]::new($this::cmd + 'parameters', $parameters),
                    [XElement]::new($this::cmd + 'inputTypes',
                        [XElement]::new($this::cmd + 'inputType',
                            [XElement]::new($this::dev + 'type',
                                [XElement]::new($this::maml + 'name', $inputType)),
                            [XElement]::new($this::maml + 'description', $inputDesc))),
                    [XElement]::new($this::cmd + 'returnValues',
                        [XElement]::new($this::cmd + 'returnValue',
                            [XElement]::new($this::dev + 'type',
                                [XElement]::new($this::maml + 'name', $outputType)),
                            [XElement]::new($this::maml + 'description', $outputDesc))),
                    [XElement]::new($this::maml + 'alertSet',
                        [XElement]::new($this::maml + 'alert',
                            [XElement]::new($this::maml + 'para', @()))),
                    [XElement]::new($this::cmd + 'examples', $examples),
                    [XElement]::new($this::cmd + 'relatedLinks',
                        [XElement]::new($this::maml + 'navigationLink',
                            [XElement]::new($this::maml + 'linkText', 'Online Version:'),
                            [XElement]::new($this::maml + 'uri', $uri))))

        }

        [object] ReadExamples() {
            return $(
                while ($this._blocks.MoveNext()) {
                    $exampleTitle = $this.GetHeaderName($this._blocks.Current)
                    if ($exampleTitle -eq 'PARAMETERS') {
                        $null = $this._blocks.MovePrevious()
                        break
                    }

                    if (-not $this._blocks.MoveNext() -or $this._blocks.Current -isnot [FencedCodeBlock]) {
                        throw
                    }

                    $code = $this.GetText($this._blocks.Current)

                    $description = $this.ReadUntilNextHeader()

                    # yield
                    [XElement]::new($this::cmd + 'example',
                        [XElement]::new($this::maml + 'title', $exampleTitle),
                        [XElement]::new($this::dev + 'code', $code),
                        [XElement]::new($this::dev + 'remarks', $description))
                })
        }

        [string] GetHeaderName([HeadingBlock] $block) {
            return $this._blocks.Current.Inline.FirstChild.Content.ToString()
        }

        [object[]] ReadUntilNextHeader() {
            $objs = [List[object]]::new()
            while ($this._blocks.MoveNext()) {
                $current = $this._blocks.Current
                if ($current -is [HeadingBlock]) {
                    $this._blocks.MovePrevious()
                    return $objs.ToArray()
                }

                $objs.Add($this.Convert($current))
            }

            return $objs.ToArray()
        }

        [string] MoveToNextHeader() {
            while ($this._blocks.MoveNext()) {
                if ($this._blocks.Current -isnot [HeadingBlock]) {
                    continue
                }

                return $this.GetHeaderName($this._blocks.Current)
            }

            return [string]::Empty
        }

        [void] MoveToHeader([string] $name) {
            while ($this._blocks.MoveNext()) {
                if ($this._blocks.Current -isnot [HeadingBlock]) {
                    continue
                }

                if ($this.GetHeaderName($this._blocks.Current) -ne $name) {
                    continue
                }

                return
            }

            throw "Could not find header '$name'."
        }

        [string] GetText([MarkdownObject] $obj) {
            return $this._text.Substring($obj.Span.Start, $obj.Span.Length)
        }

        [XElement] Convert([MarkdownObject] $obj) {
            if ($obj -is [Table]) {
                return [XElement]::new(
                    $this::maml + 'para',
                    $this._text.Substring(
                        $obj.Span.Start - 1,
                        $obj.Span.Length + 2))
            }

            return [XElement]::new($this::maml + 'para', $this.GetText($obj))
        }
    }
}
end {
    $file = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($DestinationFile)
    $directory = $file | Split-Path -Parent
    if (-not (Test-Path $directory -PathType Container)) {
        $null = New-Item -ItemType Directory $directory -ErrorAction Stop
    }

    $filesToProcess = Get-ChildItem $DocsPath\*.md | Where-Object BaseName -NotLike about_*

    [MamlBuilder]::ParseAll(
        $filesToProcess.FullName,
        $file)

    $destinationDirectory = $DestinationFile | Split-Path
    $aboutBadFormat = (Get-Item $DocsPath\about_BadImageFormat.md).FullName
    $oldText = [System.IO.File]::ReadAllText($aboutBadFormat)
    $newText = $oldText -replace '(?m)^#{1,} '
    [System.IO.File]::WriteAllText(
        "$destinationDirectory\about_BadImageFormat.help.txt",
        $newText)

    [System.IO.File]::WriteAllText(
        "$destinationDirectory\about_InvalidProgram.help.txt",
        $newText)
}
