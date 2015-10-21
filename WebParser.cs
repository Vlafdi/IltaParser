using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace Kikkare
{
    /// <summary>
    /// A parser which loads a web page from a given url and contains methods providing access to parts of the page
    /// </summary>
    class WebParser
    {
        /// <summary>
        /// The URL of the document to parse
        /// </summary>
        public string URL { get; set; }

        private HtmlDocument document;

        /// <summary>
        /// Method to read the document into a stream and build a document object for the page
        /// </summary>
        /// <param name="url">The url which to get and load into a document object</param>
        /// <param name="encoding">The encoding for the page</param>
        public void Load(string url, Encoding encoding)
        {
            this.URL = url;
            document = new HtmlDocument();

            Stream dataStream;
            using (var webClient = new WebClient())
            {
                dataStream = webClient.OpenRead(url);
            }
            document.Load(dataStream, encoding);
        }

        /// <summary>
        /// Parses for h1 headers in the document loaded in memory
        /// </summary>
        /// <returns>An iterable of string headers in the document</returns>
        public IEnumerable<string> GetHeaders(string tag = "h1")
        {
            if (document == null || String.IsNullOrEmpty(URL))
                throw new ApplicationException("Trying to process document without calling Load first");

            var headers = new List<String>();

            foreach (HtmlNode headerNode in document.DocumentNode.SelectNodes(String.Format("//{0}", tag)))
            {
                String headerText = headerNode.InnerText;
                
                // Decode html entities, trim whitespace and newlines
                headerText = HttpUtility.HtmlDecode(headerText).Trim().Replace('\n', ' ').Replace("  ", " ");
                if (!String.IsNullOrEmpty(headerText))
                    headers.Add(headerText);
            }

            return headers;
        }
    }
}
