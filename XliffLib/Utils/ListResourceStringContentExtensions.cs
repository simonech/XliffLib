using Localization.Xliff.OM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib.Utils
{
    public static class ListResourceStringContentExtensions
    {
        public static string ConvertToHtml(this IList<ResourceStringContent> xliffValue)
        {
            var sb = new StringBuilder();
            foreach (var item in xliffValue)
            {
                switch (item.GetType().Name)
                {
                    case nameof(PlainText):
                        var text = item as PlainText;
                        sb.Append(text.Text);
                        break;
                    case nameof(StandaloneCode):
                        var ph = item as StandaloneCode;
                        string autoclosingTag;
                        switch (ph.SubType)
                        {
                            case "xlf:lb":
                                autoclosingTag = "br";
                                break;
                            default:
                                autoclosingTag = string.Empty;
                                break;
                        }
                        sb.AppendFormat($"<{autoclosingTag}/>");
                        break;
                    case nameof(SpanningCode):
                        var pc = item as SpanningCode;
                        var content = pc.Text.ConvertToHtml();
                        string tag;
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
                            case "x-xlf:sup":
                                tag = "sup";
                                break;
                            case "x-xlf:sub":
                                tag = "sub";
                                break;
                            default:
                                tag = string.Empty;
                                break;
                        }
                        sb.AppendFormat($"<{tag}>{content}</{tag}>");
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
