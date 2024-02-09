using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }


    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;

        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            headerLines.Add(contentType);
            headerLines.Add(content.Length.ToString());
            headerLines.Add(DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'EST'"));




            // TODO: Create the request string
            string status = GetStatusLine(code);
            int statusCode = (int)code;
            if (statusCode == 301)
            {
                headerLines.Add(redirectoinPath);

                responseString = status + "\r\n" + "Content-Type: " + headerLines[0] + "\r\n" + "Content-Length: " + headerLines[1] + "\r\n" + "Date: " + headerLines[2] + "\r\n" + "Location: " + headerLines[3] + "\r\n" + "\r\n" + content;
            }
            else
            {
                responseString = status + "\r\n" + "Content-Type: " + headerLines[0] + "\r\n" + "Content-Length: " + headerLines[1] + "\r\n" + "Date: " + headerLines[2] + "\r\n" + status + "\r\n" + "Content-Type: " + headerLines[0] + "\r\n" + "Content-Length: " + headerLines[1] + "\r\n" + "Date: " + headerLines[2] + "\r\n" + "\r\n" + content;
            }
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            int statusCode = (int)code;
            string statusLine = Configuration.ServerHTTPVersion + " " + statusCode.ToString() + " " + code + "\r\n";

            //if (statusCode == 200)
            //{
            //    statusLine = "HTTP/1.1 " + code + " Ok";
            //}
            //else if (statusCode == 301)
            //{
            //    statusLine = "HTTP/1.1 " + code + " Redirect";
            //}
            //else if (statusCode == 400)
            //{
            //    statusLine = "HTTP/1.1 " + code + " Bad Request";
            //}
            //else if (statusCode == 500)
            //{
            //    statusLine = "HTTP/1.1 " + code + " Internal Server Error";
            //}
            //else if (statusCode == 404)
            //{
            //    statusLine = "HTTP/1.1 " + code + " Not Found";
            //}


            return statusLine;
        }
    }
}
