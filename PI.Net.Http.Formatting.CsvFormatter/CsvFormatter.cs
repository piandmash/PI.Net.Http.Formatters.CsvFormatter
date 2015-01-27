using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

using PI.Net.Http.Formatting.CsvFormatter.Models;

namespace PI.Net.Http.Formatting.Formatters
{
    /// <markdown>
    /// #PI.Net.Http.Formatting.Formatters.CsvFormatter
    /// File: CsvFormatter.cs
    /// </markdown>
    /// <summary>
    /// Formatter to create and deliver a CSV file from data provided
    /// </summary>
    public class CsvFormatter : BufferedMediaTypeFormatter
    {
        #region Constructors

        /// <markdown>
        /// ###public CsvFormatter()
        /// </markdown>
        /// <summary>
        /// Base constructor for the class.
        /// Adds in the supported media type and the encoding types
        /// </summary>
        public CsvFormatter()
        {
            // Add the supported media type.
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
            // add encoding types:
            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
            this.MediaTypeMappings.Add(new QueryStringMapping("type", "csv", new MediaTypeHeaderValue("text/csv")));
        }

        #endregion

        #region Properties
        
        /// <markdown>
        /// ###string CsvFileName
        /// </markdown>
        /// <summary>
        /// The file name of the CSV to be delivered
        /// default: "download.csv"
        /// </summary>
        private string CsvFileName = "download.csv";
        #endregion

        #region Methods

        /// <markdown>
        /// ###public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        /// </markdown>
        /// <summary>
        /// Overrides the request formatter instance so as to build a CsvFileName from the request object
        /// </summary>
        /// <param name="type">The type to format.</param>
        /// <param name="request">The request</param>
        /// <param name="mediaType">The media type</param>
        /// <returns>The MediaTypeFormatter for the request</returns>
        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            CsvFileName = GenerateCsvFileName(request.RequestUri.LocalPath);
            //set file name based on request
            return base.GetPerRequestFormatterInstance(type, request, mediaType);
        }

        /// <markdown>
        /// ###public string GenerateCsvFileName(string path)
        /// </markdown>
        /// <summary>
        /// Generates a file name from the path sent removing unwated characters
        /// </summary>
        /// <param name="path">The path to generate the file name from.</param>
        /// <returns>The generated path</returns>
        public string GenerateCsvFileName(string path)
        {
            string fileName = (path.EndsWith(".csv")) ? path : path + ".csv";
            return fileName.Replace("/api/", "").Replace("/", "-");
        }
        /// <markdown>
        /// ###public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        /// </markdown>
        /// <summary>
        /// Sets the attachment header to the response
        /// </summary>
        /// <param name="type">The type of the object being serialized. See System.Net.Http.ObjectContent.</param>
        /// <param name="headers">The content headers that should be configured.</param>
        /// <param name="mediaType">The authoritative media type. Can be null.</param>
        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = CsvFileName };
        }

        /// <markdown>
        /// ###public override bool CanWriteType(System.Type type)
        /// </markdown>
        /// <summary>
        /// Queries whether this System.Net.Http.Formatting.MediaTypeFormatter can serializean object of the specified type.
        /// Checking for the PI.Net.Http.Formatting.CsvFormatter.Models.ICsvFormat
        /// </summary>
        /// <param name="type">The type to serialize.</param>
        /// <returns>true if the System.Net.Http.Formatting.MediaTypeFormatter can serialize the type; otherwise, false.</returns>
        public override bool CanWriteType(System.Type type)
        {
            if (type.IsAssignableFrom(typeof(ICsvFormat)))
            {
                return true;
            }
            else
            {
                Type returnType = null;
                return (CheckInterface(type, typeof(IEnumerable<>), out returnType) && CheckInterface(returnType, typeof(ICsvFormat), out returnType));
            }
        }

        /// <markdown>
        /// ###private bool CheckInterface(Type someType, Type interfaceType, out Type returnType)
        /// </markdown>
        /// <summary>
        /// Checks the sent type against an interface type returning the someType as ther returnType
        /// </summary>
        /// <param name="someType">The type to test</param>
        /// <param name="interfaceType">The interface type to test against</param>
        /// <param name="returnType">output of the return type</param>
        /// <returns>true if the type matches the interface and false if it fails</returns>
        private bool CheckInterface(Type someType, Type interfaceType, out Type returnType)
        {
            returnType = someType;
            try
            {
                Type[] listInterfaces = someType.GetInterfaces();
                foreach (Type t in listInterfaces)
                {
                    try
                    {
                        returnType = t.GetGenericArguments()[0];
                    }
                    catch { }
                    try
                    {
                        if (t.GetGenericTypeDefinition() == interfaceType) return true;
                    }
                    catch { }
                    try
                    {
                        if (t == interfaceType) return true;
                    }
                    catch { }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <markdown>
        /// ###public override bool CanReadType(Type type)
        /// </markdown>
        /// <summary>
        /// Queries whether this System.Net.Http.Formatting.MediaTypeFormatter can deserializean object of the specified type.
        /// ALWAYS returns false
        /// </summary>
        /// <param name="type">The type to query</param>
        /// <returns>returns false</returns>
        public override bool CanReadType(Type type)
        {
            return false;
        }

        /// <markdown>
        /// ###public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        /// </markdown>
        /// <summary>
        /// Formats the sent object to a CSV string and writes it to the stream
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object value to write. It may be null.</param>
        /// <param name="writeStream">The System.IO.Stream to which to write.</param>
        /// <param name="content">The System.Net.Http.HttpContent if available. It may be null.</param>
        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            Encoding effectiveEncoding = (content != null) ? SelectCharacterEncoding(content.Headers) : Encoding.Unicode;

            var writer = new StreamWriter(writeStream, effectiveEncoding);
            //using (var writer = new StreamWriter(writeStream, effectiveEncoding))
            //{
                var items = value as IEnumerable<ICsvFormat>;
                if (items != null)
                {
                    bool header = true;
                    foreach (var item in items)
                    {
                        if (header)
                        {
                            writer.WriteLine(item.BuildCsvHeader());
                            header = false;
                        }
                        writer.WriteLine(item.BuildCsvItem());
                    }
                }
                else
                {
                    var singleItem = value as ICsvFormat;
                    if (singleItem == null)
                    {
                        throw new InvalidOperationException("Cannot serialize type");
                    }
                    writer.WriteLine(singleItem.BuildCsvHeader());
                    writer.WriteLine(singleItem.BuildCsvItem());
                }
                writer.Flush();
            //}
        }

        #endregion
    }
}