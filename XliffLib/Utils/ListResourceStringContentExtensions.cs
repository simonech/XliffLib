﻿using Localization.Xliff.OM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.HtmlProcessing;

namespace XliffLib.Utils
{
    public static class ListResourceStringContentExtensions
    {

        public static bool HasSubFlow(this IList<ResourceStringContent> xliffValue)
        {
            foreach (var item in xliffValue)
            {
                var code = item as CodeBase;
                if (code != null && code.HasSubFlows)
                    return true;
            }
            return false;
        }


        public static string ConvertToHtml(this IList<ResourceStringContent> xliffValue, Func<string, IEnumerable<KeyValuePair<string, string>>> subflowsSelector = null)
        {
            var sb = new StringBuilder();
            foreach (var item in xliffValue)
            {
                string attributes = string.Empty;
                switch (item.GetType().Name)
                {
                    case nameof(PlainText):
                        var text = item as PlainText;
                        sb.Append(text.Text);
                        break;
                    case nameof(StandaloneCode):
                        var ph = item as StandaloneCode;
                        if (ph.HasSubFlows)
                        {
                            attributes = subflowsSelector(ph.SubFlows).FormatAsHtmlAttributeString(true);
                        }
                        string autoclosingTag;
                        switch (ph.Type)
                        {
                            case CodeType.Formatting:
                                switch (ph.SubType)
                                {
                                    case "xlf:lb":
                                        autoclosingTag = "br";
                                        break;
                                    default:
                                        autoclosingTag = string.Empty;
                                        break;
                                }
                                break;
                            case CodeType.Image:
                                autoclosingTag = "img";
                                break;
                            default:
                                autoclosingTag = string.Empty;
                                break;
                        }

                        sb.AppendFormat($"<{autoclosingTag}{attributes}/>");
                        break;
                    case nameof(SpanningCode):
                        var pc = item as SpanningCode;
                        var content = pc.Text.ConvertToHtml();
                        if (pc.HasSubFlows)
                        {
                            attributes = subflowsSelector(pc.SubFlowsStart).FormatAsHtmlAttributeString(true);
                        }
                        string tag;
                        switch (pc.Type)
                        {
                            case CodeType.Formatting:
                                switch (pc.SubType)
                                {
                                    case "xlf:b":
                                        tag = "strong";
                                        break;
                                    case "xlf:i":
                                        tag = "em";
                                        break;
                                    case "xlf:u":
                                        tag = "u";
                                        break;
                                    case "html:sup":
                                        tag = "sup";
                                        break;
                                    case "html:sub":
                                        tag = "sub";
                                        break;
                                    default:
                                        tag = string.Empty;
                                        break;
                                }
                                break;
                            case CodeType.Link:
                                tag = "a";
                                break;
                            default:
                                tag = string.Empty;
                                break;
                        }
                        sb.AppendFormat($"<{tag}{attributes}>{content}</{tag}>");
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
