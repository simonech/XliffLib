﻿using System;
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
			Bundle bundle = new Bundle();
			Document doc = new Document();
			Property prop = new Property("title");
			doc.Properties.Add(prop);
			bundle.Documents.Add(doc);

            string actual = bundle.ToJson();

            Assert.AreEqual("{\"documents\":[{\"propertyGroups\":[],\"properties\":[{\"name\":\"title\",\"value\":null}]}]}",actual);
        }

        [Test]
        public void CanGetBundleFromJsonString()
        {
            string json = @"{
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

            Bundle actual = json.ToBundle();

            Assert.IsNotNull(actual);

            var prop = actual.Documents[0].Properties[0];

            Assert.AreEqual("title", prop.Name);
        }
    }
}
