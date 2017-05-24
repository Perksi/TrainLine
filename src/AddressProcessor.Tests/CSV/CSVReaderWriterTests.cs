using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AddressProcessing.CSV;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private const string TestOutputDir = @"c:\Test_Data\";

        [SetUp]
        public void SetUp()
        {
            Directory.CreateDirectory(TestOutputDir);
        }

        [Test]
        public void Should_create_email_mailshot()
        {
            using (var reader = new CSVReaderWriter()) {
                bool status = reader.Open(TestOutputDir + "TestEmail.csv", CSVReaderWriter.Mode.Write);
                if (status) reader.Write("A Test Name", "ATestEmail@Test.Com");
            }

            using (var reader = new CSVReaderWriter())
            {
                bool status = reader.Open(TestOutputDir + "TestEmail.csv", CSVReaderWriter.Mode.Read);
                if (status) {
                    string column1, column2;
                    reader.Read(out column1, out column2);
                    Assert.AreEqual("A Test Name", column1);
                }
            }
        }

        [TearDown] 
        public void CSVReaderWriterTestsTearDown() 
        {
            string[] filePaths = Directory.GetFiles(TestOutputDir); 
            foreach (string filePath in filePaths) 
                File.Delete(filePath); 
        } 
    }
}
