using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XliffLib.HtmlProcessing
{
    public class HierarchicHtmlParser : BaseHtmlParser
    {
        public override bool SupportsAttributes => true;

        public override SimplifiedHtmlContentItem[] SplitHtml(string htmlText)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            var list = ExtactContent(doc.DocumentNode);

            if (list.Count == 1 && list[0].IsLeaf)
                return new List<SimplifiedHtmlContentItem>().ToArray();

            return NormalizeList(list).ToArray();
        }

        private static IList<SimplifiedHtmlContentItem> ExtactContent(HtmlNode node)
        {
            var list = new List<SimplifiedHtmlContentItem>();
            SimplifiedHtmlContentItem aggregator = null;
            foreach (var child in node.ChildNodes)
            {
                SimplifiedHtmlContentItem contentItem = null;
                if (child.IsBlock())
                {
                    if (aggregator != null)
                    {
                        list.Add(aggregator);
                        aggregator = null;
                    }
                    contentItem = new SimplifiedHtmlContentItem() { Name = child.Name };
                    contentItem.Attributes = CopyAttributes(child.Attributes);
                    contentItem.ChildElements = ExtactContent(child);
                }
                else if (child.IsTextOrInline())
                {
                    if (aggregator == null) aggregator = new SimplifiedHtmlContentItem() { IsLeaf = true };
                    aggregator.Content += child.OuterHtml;
                }
                if (contentItem != null) list.Add(contentItem);
            }
            if (aggregator != null)
            {
                list.Add(aggregator);
            }
            return list;
        }


        private static IList<SimplifiedHtmlContentItem> NormalizeList(IList<SimplifiedHtmlContentItem> list)
        {
            var newList = new List<SimplifiedHtmlContentItem>();
            foreach (var item in list)
            {
                SimplifiedHtmlContentItem newItem = item;
                if (item.ChildElements.Count == 1 && item.ChildElements[0].IsLeaf)
                {
                    newItem = new SimplifiedHtmlContentItem() { Name = item.Name };
                    newItem.Content = item.ChildElements[0].Content;
                    newItem.Attributes = item.Attributes;
                }
                else
                {
                    newItem.ChildElements = NormalizeList(item.ChildElements);
                }
                newList.Add(newItem);
            }
            return newList;
        }

        private static Dictionary<string, string> CopyAttributes(HtmlAttributeCollection attributes)
        {
            var list = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                list.Add(attribute.Name, attribute.Value);
            }
            return list;
        }
    }
}
