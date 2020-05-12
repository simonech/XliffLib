using System.Collections.Generic;

namespace XliffLib.HtmlProcessing
{
    public class SimplifiedHtmlContentItem
    {
        public SimplifiedHtmlContentItem()
        {
            ChildElements = new List<SimplifiedHtmlContentItem>();
            Attributes = new Dictionary<string, string>();
        }
        public string Name { get; set; }
        public string Content { get; set; }
        public IList<SimplifiedHtmlContentItem> ChildElements { get; set; }
        public bool IsLeaf { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public string InnerContent()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Equals("#text"))
            {
                return Content;
            }
            string innerContent = Content;

            foreach (var child in ChildElements)
            {
                innerContent += child.ToHtmlElement(true);
            }

            return innerContent;
        }

        public string ToHtmlElement(bool withChildElements = false)
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Equals("#text"))
            {
                return Content;
            }

            var attributeList = Attributes.FormatAsHtmlAttributeString();

            var content = Content;

            if (withChildElements)
            {
                content = InnerContent();
            }

            return string.Format("<{0}{2}>{1}</{0}>", Name, content, attributeList);
        }
    }
}
