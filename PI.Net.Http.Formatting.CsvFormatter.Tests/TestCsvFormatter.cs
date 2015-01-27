using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

using System.IO;

using PI.Net.Http.Formatting.Formatters;
using PI.Net.Http.Formatting.CsvFormatter.Models;
using PI.Net.Http.Formatting.CsvFormatter.Tests.Helpers;

namespace PI.Net.Http.Formatting.CsvFormatter.Tests
{
    /// <markdown>
    /// #PI.Net.Http.Formatting.CsvFormatter.Tests.TestCsvFormatter 
    /// File: TestCsvFormatter.cs
    /// </markdown>
    /// <summary>
    /// Testing Class for CsvFormatter
    /// </summary>
    [TestClass]
    public class TestCsvFormatter
    {
        #region Helpers

        /// <markdown>
        /// ###private TestAccount GetAccount()
        /// </markdown>
        /// <summary>
        /// Creates a TestAccount to use within tests
        /// </summary>
        /// <returns>A populated PI.Net.Http.Formatting.CsvFormatter.Tests.Helpers.TestAccount</returns>
        private TestAccount GetAccount()
        {
            return new TestAccount()
            {
                Id = "123",
                Name = "Pete",
                Email = "pete@piandash.com"
            };
        }

        /// <markdown>
        /// ###private List[TestAccount] GetAccount()
        /// </markdown>
        /// <summary>
        /// Creates a List of TestAccounts to use within tests
        /// </summary>
        /// <returns>A populated List of PI.Net.Http.Formatting.CsvFormatter.Tests.Helpers.TestAccount</returns>
        private List<TestAccount> GetAccountList()
        {
            List<TestAccount> list = new List<TestAccount>();
            list.Add(new TestAccount()
            {
                Id = "123",
                Name = "Pete",
                Email = "pete@piandash.com"
            });
            list.Add(new TestAccount()
            {
                Id = "456",
                Name = "Ian",
                Email = "ian@piandash.com"
            });
            return list;
        }

        #endregion

        #region Tests

        /// <markdown>
        /// ###CsvFormatter_Constructor_ShouldBeOK()
        /// </markdown>
        /// <summary>
        /// Tests the constructor method
        /// </summary>
        [TestMethod]
        public void CsvFormatter_Constructor_ShouldBeOK()
        {
            //Arrange

            //Act
            PI.Net.Http.Formatting.Formatters.CsvFormatter obj = new PI.Net.Http.Formatting.Formatters.CsvFormatter();
            var mapping = obj.MediaTypeMappings[0];
            System.Net.Http.Formatting.QueryStringMapping mapping2 = (System.Net.Http.Formatting.QueryStringMapping)mapping;

            ///Assert
            ///* AreEqual: MediaTypeMappings.Count = 1
            Assert.AreEqual(obj.MediaTypeMappings.Count, 1);
            ///* AreEqual: MediaType = test/csv
            Assert.AreEqual(mapping.MediaType.MediaType, "text/csv");
            ///* IsInstanceOfType: mapping is QueryStringMapping
            Assert.IsInstanceOfType(mapping, typeof(System.Net.Http.Formatting.QueryStringMapping));
            ///* AreEqual: QueryStringParameterName = type
            Assert.AreEqual(mapping2.QueryStringParameterName, "type");
            ///* AreEqual: QueryStringParameterValue = csv
            Assert.AreEqual(mapping2.QueryStringParameterValue, "csv");
        }

        /// <markdown>
        /// ###CsvFormatter_CanReadType_ShouldBeFalse_ICsvFormatObject()
        /// </markdown>
        /// <summary>
        /// Tests that the formatters CanReadType is flase even for an ICsvFormat object
        /// </summary>
        [TestMethod]
        public void CsvFormatter_CanReadType_ShouldBeFalse_ICsvFormatObject()
        {
            //Arrange
            List<TestAccount> accs = GetAccountList();
            PI.Net.Http.Formatting.Formatters.CsvFormatter obj = new PI.Net.Http.Formatting.Formatters.CsvFormatter();

            //Act
            bool result = obj.CanReadType(accs.GetType());

            ///Assert
            ///* IsFalse: result
            Assert.IsFalse(result);
        }

        /// <markdown>
        /// ###CsvFormatter_CanReadType_ShouldBeFalse_AnyObject()
        /// </markdown>
        /// <summary>
        /// Tests that the formatters CanReadType is flase even for an ICsvFormat object
        /// </summary>
        [TestMethod]
        public void CsvFormatter_CanReadType_ShouldBeFalse_AnyObject()
        {
            //Arrange
            string test = "";
            PI.Net.Http.Formatting.Formatters.CsvFormatter obj = new PI.Net.Http.Formatting.Formatters.CsvFormatter();

            //Act
            bool result = obj.CanReadType(test.GetType());

            ///Assert
            ///* IsFalse: result
            Assert.IsFalse(result);
        }

        /// <markdown>
        /// ###CsvFormatter_CanWriteType_ShouldBeTrue_ICsvFormatObject()
        /// </markdown>
        /// <summary>
        /// Tests that the formatters CanWriteType for an ICsvFormat object
        /// </summary>        
        [TestMethod]
        public void CsvFormatter_CanWriteType_ShouldBeTrue_ICsvFormatObject()
        {
            //Arrange
            List<TestAccount> accs = GetAccountList();
            PI.Net.Http.Formatting.Formatters.CsvFormatter obj = new PI.Net.Http.Formatting.Formatters.CsvFormatter();

            //Act
            bool result = obj.CanWriteType(accs.GetType());

            ///Assert
            ///* IsTrue: result
            Assert.IsTrue(result);
        }

        /// <markdown>
        /// ###CsvFormatter_CanWriteType_ShouldBeFalse_NonICsvFormatObject()
        /// </markdown>
        /// <summary>
        /// Tests that the formatters CanWriteType for a string object
        /// </summary>        
        [TestMethod]
        public void CsvFormatter_CanWriteType_ShouldBeFalse_NonICsvFormatObject()
        {
            //Arrange
            string test = "";
            PI.Net.Http.Formatting.Formatters.CsvFormatter obj = new PI.Net.Http.Formatting.Formatters.CsvFormatter();

            //Act
            bool result = obj.CanWriteType(test.GetType());

            ///Assert
            ///* IsFalse: result
            Assert.IsFalse(result);
        }

        /// <markdown>
        /// ###CsvFormatter_GenerateCsvFileName_ShouldMatchCsvFileName()
        /// </markdown>
        /// <summary>
        /// Tests that the generated file name is built correctly from the string sent
        /// </summary>        
        [TestMethod]
        public void CsvFormatter_GenerateCsvFileName_ShouldMatchCsvFileName()
        {
            //Arrange
            string test = "/api/test/download";
            PI.Net.Http.Formatting.Formatters.CsvFormatter obj = new PI.Net.Http.Formatting.Formatters.CsvFormatter();

            //Act
            string result = obj.GenerateCsvFileName(test);

            ///Assert
            string fileName = "test-download.csv";
            ///* AreEqual: result = fileName
            Assert.AreEqual(result, fileName);
        }

        /// <markdown>
        /// ###CsvFormatter_GenerateCsvFileName_ShouldMatchCsvFileNameWithCsv()
        /// </markdown>
        /// <summary>
        /// Tests that the generated file name is built correctly from the string sent
        /// </summary>        
        [TestMethod]
        public void CsvFormatter_GenerateCsvFileName_ShouldMatchCsvFileNameWithCsv()
        {
            //Arrange
            string test = "/api/test/download.csv";
            PI.Net.Http.Formatting.Formatters.CsvFormatter obj = new PI.Net.Http.Formatting.Formatters.CsvFormatter();

            //Act
            string result = obj.GenerateCsvFileName(test);

            ///Assert
            string fileName = "test-download.csv";
            ///* AreEqual: result = fileName
            Assert.AreEqual(result, fileName);
        }

        /// <markdown>
        /// ###CsvFormatter_WriteToStream_ShouldBeOK()
        /// </markdown>
        /// <summary>
        /// Tests that the WriteToStream method writes the correct data
        /// </summary>        
        [TestMethod]
        public void CsvFormatter_WriteToStream_ShouldBeOK()
        {
            //Arrange
            Type type = typeof(TestAccount);
            List<TestAccount> accs = GetAccountList();
            HttpContent content = null;
            var ms = new MemoryStream();
            var sr = new StreamReader(ms);
            PI.Net.Http.Formatting.Formatters.CsvFormatter obj = new PI.Net.Http.Formatting.Formatters.CsvFormatter();

            //Act
            obj.WriteToStream(type, accs, ms, content);

            //reset position for read
            ms.Position = 0;
            var result = sr.ReadToEnd();

            ///Assert
            string csvString = "Id,Name,Email\r\n123,Pete,pete@piandash.com\r\n456,Ian,ian@piandash.com\r\n";
            ///* AreEqual: result = csvString
            Assert.AreEqual(result, csvString);
        }

        #endregion
    }
}
