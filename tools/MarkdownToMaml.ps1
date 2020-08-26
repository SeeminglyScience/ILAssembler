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
            $this._blocks.MoveNext()
            $syntax = $this._blocks.Current.Lines.Slice[0].ToString()

            $this.MoveToHeader('DESCRIPTION')
            $description = $this.ReadUntilNextHeader()

            return [XElement]::new($this::cmd + 'command',
                    [XElement]::new($this::cmd + 'details',
                        [XElement]::new($this::cmd + 'name', $this._name),
                        [XElement]::new($this::cmd + 'verb', $this._name),
                        [XElement]::new($this::cmd + 'noun', ''),
                        [XElement]::new($this::maml + 'description', $synopsis)),
                    [XElement]::new($this::maml + 'description', $description),
                    [XElement]::new($this::cmd + 'syntax',
                        [XElement]::new($this::cmd + 'syntaxItem',
                            [XElement]::new($this::maml + 'name', $syntax))),
                    [XElement]::new($this::cmd + 'parameters', @()),
                    [XElement]::new($this::cmd + 'inputTypes',
                        [XElement]::new($this::cmd + 'inputType',
                            [XElement]::new($this::dev + 'type',
                                [XElement]::new($this::maml + 'name', 'None')),
                            [XElement]::new($this::maml + 'description',
                                [XElement]::new($this::maml + 'para', 'This function cannot be used with the pipeline.')))),
                    [XElement]::new($this::cmd + 'returnValues',
                        [XElement]::new($this::cmd + 'returnValue',
                            [XElement]::new($this::dev + 'type',
                                [XElement]::new($this::maml + 'name', 'None')),
                            [XElement]::new($this::maml + 'description',
                                [XElement]::new($this::maml + 'para', 'This function cannot be used with the pipeline.')))),
                    [XElement]::new($this::maml + 'alertSet',
                        [XElement]::new($this::maml + 'alert',
                            [XElement]::new($this::maml + 'para', @()))),
                    [XElement]::new($this::cmd + 'examples'),
                    [XElement]::new($this::cmd + 'relatedLinks',
                        [XElement]::new($this::maml + 'navigationLink',
                            [XElement]::new($this::maml + 'linkText', 'Online Version:'),
                            [XElement]::new($this::maml + 'uri', $uri))))

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
                $sb = [System.Text.StringBuilder]::new($obj.Span.Length)
                $null = foreach ($row in $obj) {
                    $sb.Append('|').
                        Append($this.GetText($row)).
                        Append('|').
                        AppendLine()
                }

                return [XElement]::new($this::maml + 'para', $sb.ToString())
            }

            return [XElement]::new($this::maml + 'para', $this.GetText($obj))
        }
    }
}
end {
    [MamlBuilder]::ParseAll(
        (Get-ChildItem $DocsPath\*.md).FullName,
        $PSCmdlet.GetUnresolvedProviderPathFromPSPath($DestinationFile))
}
