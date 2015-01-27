using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PI.Net.Http.Formatting.CsvFormatter.Models;

namespace PI.Net.Http.Formatting.CsvFormatter.Tests.Helpers
{
    /// <markdown>
    /// #PI.Net.Http.Formatting.CsvFormatter.Tests.Helpers.TestAccount
    /// File: TestAccount.cs
    /// </markdown>
    /// <summary>
    /// Testing Account Class implementing the ICsvFormat interface
    /// </summary> 
    public class TestAccount : ICsvFormat
    {
        #region Properties

        /// <markdown>
        /// ###string Id
        /// </markdown>
        /// <summary>
        /// Account id
        /// </summary>
        public string Id { get; set; }

        /// <markdown>
        /// ###string Name
        /// </markdown>
        /// <summary>
        /// Account Name
        /// </summary>
        public string Name { get; set; }

        /// <markdown>
        /// ###string Email
        /// </markdown>
        /// <summary>
        /// Account email
        /// </summary>
        public string Email { get; set; }
        #endregion

        #region ICsvFormat Interface Methods

        /// <markdown>
        /// ###public string BuildCsvHeader()
        /// </markdown>
        /// <summary>
        /// Implementation of the ICsvFormat interface method for building the header
        /// </summary>       
        public string BuildCsvHeader()
        {
            string header = "Id,Name,Email";
            return header;
        }

        /// <markdown>
        /// ###public string BuildCsvItem()
        /// </markdown>
        /// <summary>
        /// Implementation of the ICsvFormat interface method for building the item
        /// </summary>       
        public string BuildCsvItem()
        {
            string item = String.Format("{0},{1},{2}"
                , CsvFormatItem.Escape(Id)
                , CsvFormatItem.Escape(Name)
                , CsvFormatItem.Escape(Email));

            //PI.Net.Http.Formatting.CsvFormatter.Models.CsvFormatItem will take the value and call ToString() 
            //it will then clean the string for a CSV file removing line spaces etc
            return item;
        }
        #endregion
    }
}
