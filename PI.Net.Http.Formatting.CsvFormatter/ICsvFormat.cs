using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PI.Net.Http.Formatting.CsvFormatter.Models
{
    /// <markdown>
    /// #PI.Net.Http.Formatting.CsvFormatter.Models.ICsvFormat
    /// File: ICsvFormat.cs
    /// </markdown>
    /// <summary>
    /// Interface for a csv formattable object
    /// </summary>
    public interface ICsvFormat
    {
        #region Methods
        /// <markdown>
        /// ###string BuildCsvHeader()
        /// </markdown>
        /// <summary>
        /// Method to build a CSV string for the header
        /// For example this might be the table column titles or properties of the first object in an array
        /// </summary>
        /// <returns>A string delimeted and formated for CSV</returns>
        string BuildCsvHeader();

        /// <markdown>
        /// ###string BuildCsvItem()
        /// </markdown>
        /// <summary>
        /// Method to build a CSV string for the item
        /// </summary>
        /// <returns>A string delimeted and formated for CSV</returns>
        string BuildCsvItem();

        #endregion
    }

    /// <markdown>
    /// #PI.Net.Http.Formatting.CsvFormatter.Models.CsvFormatItem
    /// File: ICsvFormat.cs
    /// </markdown>
    /// <summary>
    /// Static class to build a CSV Escaped string
    /// </summary>
    public static class CsvFormatItem
    {

        #region Methods

        /// <markdown>
        /// ###public static string Escape(object o)
        /// </markdown>
        /// <summary>
        /// Escapes the objects ToString value into a CSV friendly format
        /// </summary>
        /// <param name="o">Object to escape</param>
        /// <returns>Escaped string</returns>
        public static string Escape(object o)
        {
            char[] _specialChars = new char[] { ',', '\n', '\r', '"' };

            if (o == null)
            {
                return "";
            }
            string field = o.ToString();
            if (field.IndexOfAny(_specialChars) != -1)
            {
                // Delimit the entire field with quotes and replace embedded quotes with "".
                return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
            }
            else return field;
        }

        #endregion
    }
}