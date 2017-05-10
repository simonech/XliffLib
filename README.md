# Xliff.NET

>This is a work in progress and not complete yet.

This is an **opinionated** extraction/merging library to [XLIFF v2.0 and 2.1](http://docs.oasis-open.org/xliff/xliff-core/v2.1/xliff-core-v2.1.html), which follows the extraction best practices as recommended by the XLIFF group.

It is decoupled from any content production system, so it can be used with any CMS, XML or directly to extact HTML documents.

It doesn't implement directly the serialization to XMIFF but relies on the [XLIFF 2.0 Object Model](https://github.com/Microsoft/XLIFF2-Object-Model) library developed as OSS by Microsoft.

It is **opinionated** because extraction and merging to and from XLIFF is a very personal matter, which can be done in many different ways. This library is based on how I think it's a good way of extracting content into XLIFF, and follows the best practices laid in the standard and gathered with discussion and numerous email exchanges with the standardization body.

## Main stories

User stories this tools aims at addressing are:
 * Pass in a simple text and generate an XLIFF file by separating all paragraphs into translation sections.
 * Pass in a document made of properties and generate various groups and units.
 * Possibility to add custom tool sections to pass back and forth info needed when importing the content back into the origiating system.
 * Possibility to get bilingual xliff file when serializing documents translated already languages.


## How it works

The library works on a generic content structure:

 * **Bundle**, which defines a group of documents you want to translate.
    * **Documents**, each of which is standalone piece of content, like a new document, or file.
        * **Property Groups**, within a document, properties can be grouped together.
            * **Property Groups**, there can be many levels of groupings.
            * **Properties**, finally, the element that actually holds the value to translate is the property.
        * **Properties**, whicj can also be directly in the root of the document if the information has a plain structure.

## License
The library is provided as OpenSource with the MIT License.