using System;
using NUnit.Framework;
using XliffLib.Model;
using XliffLib.Utils;

namespace XliffLib.Test
{
    [TestFixture]
    public class JsonTests
    {
        [Test]
        public void CanSerializeToJson()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var prop = new Property("title", "my title");
            doc.Properties.Add(prop);
            bundle.Documents.Add(doc);

            var actual = bundle.ToJson();

            Assert.AreEqual("{\"documents\":[{\"propertyGroups\":[],\"properties\":[{\"name\":\"title\",\"value\":\"my title\"}]}]}", actual);
        }

        [Test]
        public void CanGetBundleFromJsonString()
        {
            var json = @"{
                'documents':[
                    {
                    'propertyGroups': [],
                    'properties': [
                        {
                            'name': 'title',
                            'value': null
                        }
                    ]
                    }
                ]
            }";

            var actual = json.ToBundle();

            Assert.IsNotNull(actual);

            var prop = actual.Documents[0].Properties[0];

            Assert.AreEqual("title", prop.Name);
        }
    }
}
