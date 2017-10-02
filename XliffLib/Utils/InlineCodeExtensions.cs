using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Localization.Xliff.OM.Core;

namespace XliffLib.Utils
{
    public static class InlineCodeExtensions
    {

        public static IDictionary<string, InlineCodeType> Map = new Dictionary<string, InlineCodeType>()
        {
            {"b", new InlineCodeType(CodeType.Formatting, "xlf:b")},
            {"strong", new InlineCodeType(CodeType.Formatting, "xlf:b")},
            {"i", new InlineCodeType(CodeType.Formatting, "xlf:i")},
            {"em", new InlineCodeType(CodeType.Formatting, "xlf:i")},
            {"u", new InlineCodeType(CodeType.Formatting, "xlf:u")},
            {"br", new InlineCodeType(CodeType.Formatting, "xlf:lb")},
        };


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
                    tagCounter++;
                    var startTagId = AddOriginalData(originalData, "<" + node.Name + ">", ref originalDataCounter);
                    var endTagId = AddOriginalData(originalData, "</" + node.Name + ">", ref originalDataCounter);
                    var pc = new SpanningCode(tagCounter.ToString());
                    pc.DataReferenceStart = startTagId;
                    pc.DataReferenceEnd = endTagId;
                    pc.Text.Add(new PlainText(node.InnerText));
                    var inlineCodeType = Map[node.Name];
                    pc.Type = inlineCodeType.Type;
                    pc.SubType = inlineCodeType.Subtype;
                    text.Add(pc);
                }
                if (node.NodeType == HtmlNodeType.Element && !node.HasChildNodes)
                {
                    tagCounter++;
                    var tagId = AddOriginalData(originalData, "<" + node.Name + "/>", ref originalDataCounter);
                    var ph = new StandaloneCode(tagCounter.ToString());
                    ph.DataReference = tagId;
                    var inlineCodeType = Map[node.Name];
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

        private static string AddOriginalData(Dictionary<String, String> originalData, string tagName, ref int lastOriginalData)
        {
            string tagValue = tagName.Replace("<", "&lt;");
            tagValue = tagValue.Replace(">", "&gt;");
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
        public CodeType Type;
        public string Subtype;

    }

}
