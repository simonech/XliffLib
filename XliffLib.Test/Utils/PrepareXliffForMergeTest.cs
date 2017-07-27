using NUnit.Framework;
using System;
using Localization.Xliff.OM.Core;

namespace XliffLib.Test.Utils
{

    public static class PrepareXliffForMergeTest
    {
        public static XliffDocument SetupXliffFile(bool withCData = false, bool withGroup = false)
        {
            var contentValue = "contenuto tradotto";
            if (withCData)
                contentValue = "<![CDATA[<p>Ciao Mondo!</p>]]>";

            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <xliff srcLang=""en-US"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
                  <file id=""f1"" original=""cmsId"">
                    <unit id=""u1"" name=""title"">
                      <segment>
                        <source>content</source>
                        <target>" + contentValue + @"</target>
                      </segment>
                    </unit>
                  </file>
                </xliff>";

            if (withGroup)
                xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <xliff srcLang=""en-US"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
                  <file id=""f1"" original=""cmsId"">
                    <group id=""g1"" name=""Content"">
                    <unit id=""u1"" name=""title"">
                      <segment>
                        <source>content</source>
                        <target>" + contentValue + @"</target>
                      </segment>
                    </unit>
                    </group>
                  </file>
                </xliff>";

            return Merger.Read(xliff);
        }
    }
}
