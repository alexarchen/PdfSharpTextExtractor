# PdfSharpTextExtractor

Simple Pdf text extractor based on PDFSharp for NET Standard.\
Supports both single and two-byte fonts, ToUnicode maps, Encodings.\
Doesn't support (yet) precise symbol positioning on page so text order can differ from the original.

## Install

Install via nuget: PdfSharpTextExtractor package

```sh
dotnet add package PdfSharpTextExtractor
```

Install via git:

```sh
git clone https://github.com/alexarchen/PdfSharpTextExtractor
```

## Use

As static full text extarctor

```
string text = PdfSharpTextExtractor.PdfToText(file)
```

Or as page-by-page extractor:

```
using (doc  = PdfReader.Open(file, PdfDocumentOpenMode.ReadOnly))
{
   StringBuilder ta = new StringBuilder();
   using (PdfSharpTextExtractor.Extractor extractor = new PdfSharpTextExtractor.Extractor(doc))
    {
       foreach (PdfPage page in doc.Pages)
        {
           extractor.ExtractText(page, ta);
        }

     }
}
```

