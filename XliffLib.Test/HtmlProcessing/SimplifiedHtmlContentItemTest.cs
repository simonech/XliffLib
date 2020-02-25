using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XliffLib.HtmlProcessing;

namespace XliffLib.Test.HtmlProcessing
{
    [TestFixture()]
    public class SimplifiedHtmlContentItemTest
    {
        [Test()]
        public void CanGiveCorrectHtmlForOneLevelHtmlElement()
        {
            var item = new SimplifiedHtmlContentItem()
            {
                Name = "p",
                Content = "the content"
            };

            Assert.AreEqual("<p>the content</p>", item.ToHtmlElement());
        }

        [Test()]
        public void CanGiveCorrectHtmlForForNestedHtmlElement()
        {
            var item = new SimplifiedHtmlContentItem()
            {
                Name = "ul",
                ChildElements = new List<SimplifiedHtmlContentItem>
                {
                    new SimplifiedHtmlContentItem() { Name = "li", Content = "item 1" },
                    new SimplifiedHtmlContentItem() { Name = "li", Content = "item 2" },
                }
            };

            Assert.AreEqual("<ul><li>item 1</li><li>item 2</li></ul>", item.ToHtmlElement(true));
        }

        [Test()]
        public void CanGiveCorrectHtmlForOneLevelHtmlElementWithAttributes()
        {
            var item = new SimplifiedHtmlContentItem()
            {
                Name = "p",
                Content = "the content",
                Attributes = 
                {
                    { "class","odd" },
                    { "style","color:blue;"}
                }
            };

            Assert.AreEqual("<p class=\"odd\" style=\"color:blue;\">the content</p>", item.ToHtmlElement());
        }

        [Test()]
        public void CanGiveCorrectHtmlForForNestedHtmlElementWithAttributes()
        {
            var item = new SimplifiedHtmlContentItem()
            {
                Name = "ul",
                ChildElements = new List<SimplifiedHtmlContentItem>
                {
                    new SimplifiedHtmlContentItem() { Name = "li", Content = "item 1", Attributes = { { "class", "odd" } } },
                    new SimplifiedHtmlContentItem() { Name = "li", Content = "item 2", Attributes = { { "class", "even" } } },
                }
            };

            Assert.AreEqual("<ul><li class=\"odd\">item 1</li><li class=\"even\">item 2</li></ul>", item.ToHtmlElement(true));
        }

    }
}
