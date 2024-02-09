using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        //RequestMethod method;
        string method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter 
            string[] stringSeparators = new string[] { "\r\n" };
            requestLines = requestString.Split(stringSeparators, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            // Parse Request line
            string[] line = requestLines[0].Split(' ');
            method = line[0];
            relativeURI = line[2];

            int i = 1;
            int j = 0;
            string[] stringseparators2 = new string[] { ": " };
            while (!string.IsNullOrEmpty(requestLines[i]))
            {
                string header_content = requestLines[i];
                string[] data = header_content.Split(stringSeparators, StringSplitOptions.None);
                headerLines.Add(data[0], data[1]);
                i++;
                j = i;
            }
            if (string.IsNullOrEmpty(requestLines[j]))
            {
                return true;
            }
            else
            {
                return false;
            }

            // Validate blank line exists

            // Load header lines into HeaderLines dictionary
        }

        private bool ParseRequestLine()
        {
            throw new NotImplementedException();
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            throw new NotImplementedException();
        }

        private bool ValidateBlankLine()
        {
            throw new NotImplementedException();
        }

    }
}