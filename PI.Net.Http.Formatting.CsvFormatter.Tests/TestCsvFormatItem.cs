using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PI.Net.Http.Formatting.CsvFormatter.Models;

using PI.Net.Http.Formatting.CsvFormatter.Tests.Helpers;

namespace PI.Net.Http.Formatting.CsvFormatter.Tests
{    
    /// <markdown>
    /// #PI.Net.Http.Formatting.CsvFormatter.Tests.TestCsvFormatItem 
    /// File: TestCsvFormatItem.cs
    /// </markdown>
    /// <summary>
    /// Testing Class for CsvFormatItem
    /// </summary>
    [TestClass]
    public class TestCsvFormatItem
    {

        #region Tests

        /// <markdown>
        /// ###CsvFormatItem_Escape_ShouldReturnOK()
        /// </markdown>
        /// <summary>
        /// Tests the escape method on simple string
        /// </summary>
        [TestMethod]
        public void CsvFormatItem_Escape_ShouldReturnOK()
        {
            //Arrange
            string test = "abc123";

            //Act
            string result = CsvFormatItem.Escape(test);

            ///Assert
            ///* AreEqual: result = test
            Assert.AreEqual(result, test);
        }

        /// <markdown>
        /// ###CsvFormatItem_Escape_ShouldReturnOK()
        /// </markdown>
        /// <summary>
        /// Tests the escape method on a string containing a comma
        /// </summary>
        [TestMethod]
        public void CsvFormatItem_Escape_ShouldReturnOK_Comma()
        {
            string test = "abc123 , next line";
            string returned = CsvFormatItem.Escape(test);
            Assert.AreEqual("\"abc123 , next line\"", returned);
        }

        /// <markdown>
        /// ###CsvFormatItem_Escape_ShouldReturnOK_SlashN()
        /// </markdown>
        /// <summary>
        /// Tests the escape method on a string containing /n
        /// </summary>
        [TestMethod]
        public void CsvFormatItem_Escape_ShouldReturnOK_SlashN()
        {
            //Arrange
            string test = "abc123 \n next line";

            //Act
            string result = CsvFormatItem.Escape(test);

            ///Assert
            string expected = "\"abc123 \n next line\"";
            ///* AreEqual: result = expected
            Assert.AreEqual(result, expected);
        }


        /// <markdown>
        /// ###CsvFormatItem_Escape_ShouldReturnOK_SlashR()
        /// </markdown>
        /// <summary>
        /// Tests the escape method on a string containing /r
        /// </summary>
        [TestMethod]
        public void CsvFormatItem_Escape_ShouldReturnOK_SlashR()
        {
            //Arrange
            string test = "abc123 \r next line";

            //Act
            string result = CsvFormatItem.Escape(test);

            ///Assert
            string expected = "\"abc123 \r next line\"";
            ///* AreEqual: result = expected
            Assert.AreEqual(result, expected);
        }

        /// <markdown>
        /// ###CsvFormatItem_Escape_ShouldReturnOK_Quotes()
        /// </markdown>
        /// <summary>
        /// Tests the escape method on a string containing quotes
        /// </summary>
        [TestMethod]
        public void CsvFormatItem_Escape_ShouldReturnOK_Quotes()
        {
            //Arrange
            string test = @"abc123 "" next line";

            //Act
            string result = CsvFormatItem.Escape(test);

            ///Assert
            string expected = "\"abc123 \"\" next line\"";
            ///* AreEqual: result = expected
            Assert.AreEqual(result, expected);
        }

        /// <markdown>
        /// ###CsvFormatItem_Escape_ShouldReturnOK_WithAll()
        /// </markdown>
        /// <summary>
        /// Tests the escape method on a string containing all special characters
        /// </summary>
        [TestMethod]
        public void CsvFormatItem_Escape_ShouldReturnOK_WithAll()
        {
            //Arrange
            string test = "abc123 , \n \r next line";

            //Act
            string result = CsvFormatItem.Escape(test);

            ///Assert
            string expected = "\"abc123 , \n \r next line\"";
            ///* AreEqual: result = expected
            Assert.AreEqual(result, expected);
        }

        #endregion
    }
}
