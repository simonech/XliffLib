using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Localization.Xliff.OM.Core;


namespace XliffLib.Utils
{
    public static class InlineCodeExtensions
    {

        private static IDictionary<string, InlineCodeType> Map = new Dictionary<string, InlineCodeType>()
        {
            {"b", new InlineCodeType(CodeType.Formatting, "xlf:b")},
            {"strong", new InlineCodeType(CodeType.Formatting, "xlf:b")},
            {"i", new InlineCodeType(CodeType.Formatting, "xlf:i")},
            {"em", new InlineCodeType(CodeType.Formatting, "xlf:i")},
            {"u", new InlineCodeType(CodeType.Formatting, "xlf:u")},
            {"br", new InlineCodeType(CodeType.Formatting, "xlf:lb")},
            {"sup", new InlineCodeType(CodeType.Formatting, "html:sup")},
            {"sub", new InlineCodeType(CodeType.Formatting, "html:sub")},
            {"a", new InlineCodeType(CodeType.Link, "")},
            {"img", new InlineCodeType(CodeType.Image, "")},
        };

        private static InlineCodeType ExtractInlineCodeType(string tagName)
        {
            var inlineCodeType = Map.GetValueOrDefault(tagName);
            return inlineCodeType;
        }

        public static InlineCodeExtractionResult ConvertHtmlTagsInInLineCodes(this string htmlText)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var text = new List<ResourceStringContent>();
            var originalData = new Dictionary<String, String>();
            int tagCounter = 0;
            int originalDataCounter = 1;
            foreach (var node in doc.DocumentNode.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Text)
                {
                    text.Add(new PlainText(node.InnerText));
                }
                if (node.NodeType == HtmlNodeType.Element && node.HasChildNodes)
                {
                    var inlineCodeType = ExtractInlineCodeType(node.Name);
                    if (inlineCodeType.Type == null)
                    {
                        text.Add(new PlainText(node.InnerText));
                    }
                    else
                    {
                        tagCounter++;
                        var startTagId = AddOriginalData(originalData, "<" + node.Name + ">", ref originalDataCounter);
                        var endTagId = AddOriginalData(originalData, "</" + node.Name + ">", ref originalDataCounter);
                        var pc = new SpanningCode(tagCounter.ToString());
                        pc.DataReferenceStart = startTagId;
                        pc.DataReferenceEnd = endTagId;
                        pc.Text.Add(new PlainText(node.InnerText));
                        pc.Type = inlineCodeType.Type;
                        pc.SubType = inlineCodeType.Subtype;
                        text.Add(pc);
                    }
                }
                if (node.NodeType == HtmlNodeType.Element && !node.HasChildNodes)
                {
                    var inlineCodeType = ExtractInlineCodeType(node.Name);
                    if (inlineCodeType.Type == null) continue;
                    tagCounter++;
                    var tagId = AddOriginalData(originalData, "<" + node.Name + "/>", ref originalDataCounter);
                    var ph = new StandaloneCode(tagCounter.ToString());
                    ph.DataReference = tagId;
                    ph.Type = inlineCodeType.Type;
                    ph.SubType = inlineCodeType.Subtype;
                    text.Add(ph);

                }

            }
            return new InlineCodeExtractionResult()
            {
                OriginalData = originalData,
                Text = text
            };

        }

        private static string AddOriginalData(Dictionary<String, String> originalData, string tagValue, ref int lastOriginalData)
        {
            if (originalData.ContainsKey(tagValue))
            {
                return originalData[tagValue];
            }
            string dataId = "d" + lastOriginalData++;
            originalData.Add(tagValue, dataId);
            return dataId;
        }
    }

    public class InlineCodeExtractionResult
    {
        public InlineCodeExtractionResult()
        {
            OriginalData = new Dictionary<string, string>();
        }

        public IList<ResourceStringContent> Text
        {
            get;
            set;
        }

        public Dictionary<string, string> OriginalData
        {
            get;
            set;
        }
    }

    public struct InlineCodeType
    {
        public InlineCodeType(CodeType type, string subtype)
        {
            Type = type;
            Subtype = subtype;
        }
        public CodeType? Type;
        public string Subtype;

    }

}
