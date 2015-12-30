using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XliffLib.Readers;

namespace XliffLib.Test.Reader
{
    [TestClass]
    public class XliffReaderV20Test
    {
        const string basePath= @"TestFiles\Reader\v20";
        [TestMethod]
        public void NoValidationErrorsWhenXliffFileIsValid()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);

            XliffReaderV20 reader = new XliffReaderV20();
            reader.Read(Path.Combine(directory, basePath, "ValidXliffFile.xlf"));
            Assert.IsTrue(reader.IsValid);
            Assert.AreEqual(0, reader.ValidationErrors.Count);
        }

        [TestMethod]
        public void SyntaxErrorsWhenXliffFileIsMalformatted()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);

            XliffReaderV20 reader = new XliffReaderV20();
            reader.Read(Path.Combine(directory, basePath, "MalformattedXliffFile.xlf"));
            Assert.IsFalse(reader.IsValid);
            Assert.AreEqual(1, reader.ValidationErrors.Count);
            Assert.AreEqual(ErrorType.Syntax, reader.ValidationErrors[0].Type);
        }

        [TestMethod]
        public void ValidationErrorsWhenXliffFileIsNotValid()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);

            XliffReaderV20 reader = new XliffReaderV20();
            reader.Read(Path.Combine(directory, basePath, "NotValidXliffFile.xlf"));
            Assert.IsFalse(reader.IsValid);
            Assert.AreEqual(2, reader.ValidationErrors.Count);
            Assert.AreEqual(ErrorType.Validation, reader.ValidationErrors[0].Type);
        }
    }
}
