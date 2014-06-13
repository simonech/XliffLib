# Xliff Lib

This is a focused utility library to generated xml files compatible with the [XLIFF standard](http://docs.oasis-open.org/xliff/xliff-core/xliff-core.html) used by Computer Aided Translation tools like Trados Studio.

This is just the generator and doesn't include any connector to any content producing tool.

## Main stories

User stories this tools aims at addressing are:
 * Pass in a simple text and generate an XLIFF file by separating all paragraphs into translation sections
 * Pass in a document made of properties and generate various resource groups, each of which contain translation section
 * Possibility to add custom tool sections to pass back and forth info needed when importing the content back into the origiating system
 * possibility to get bilingual xliff file when serializing documents translated already languages


## Integration

After the core lib is ready I'll integrate it with [Umbraco](http://umbraco.org)
