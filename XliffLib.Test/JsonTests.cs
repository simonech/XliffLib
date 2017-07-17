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
            doc.Containers.Add(prop);
            bundle.Documents.Add(doc);

            var actual = bundle.ToJson();

            Assert.AreEqual("{\"documents\":[{\"containers\":[{\"$type\":\"XliffLib.Model.Property, XliffLib\",\"value\":\"my title\",\"name\":\"title\"}]}]}", actual);
        }

		[Test]
		public void CanSerializeWithAttributesToJson()
		{
			var bundle = new Bundle();
			var doc = new Document();
            doc.Attributes.Add("CmsId","12345");
			var prop = new Property("title", "my title");
			doc.Containers.Add(prop);
			bundle.Documents.Add(doc);

			var actual = bundle.ToJson();

            Assert.AreEqual("{\"documents\":[{\"containers\":[{\"$type\":\"XliffLib.Model.Property, XliffLib\",\"value\":\"my title\",\"name\":\"title\"}],\"attributes\":{\"CmsId\":\"12345\"}}]}", actual);
		}

        [Test]
        public void CanGetBundleFromJsonString()
        {
            var json = @"{
                'documents':[
                    {
                    'containers': [
                        {
                            '$type': 'XliffLib.Model.Property, XliffLib',
                            'name': 'title',
                            'value': null
                        }
                    ]
                    }
                ]
            }";

            var actual = json.ToBundle();

            Assert.IsNotNull(actual);

            var prop = actual.Documents[0].Containers[0];

            Assert.AreEqual("title", prop.Name);
        }
    }
}
