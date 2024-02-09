using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;
        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);

            //TODO: initialize this.serverSocket

            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(endpoint);

        }

        public void StartServer()
        {
            Console.WriteLine("Start listening.....");
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(100);

            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("New client accepted: {0}", clientSocket.RemoteEndPoint);
                Thread newthread = new Thread(new ParameterizedThreadStart(HandleConnection));

                newthread.Start(clientSocket);

            }
        }

        public void HandleConnection(object obj)
        {
            Console.WriteLine("Connection Accepted...");
            // TODO: Create client socket 
            Socket clientSock = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSock.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] recievedata = new byte[1024];
                    int recievedLen = clientSock.Receive(recievedata);
                    string data = Encoding.ASCII.GetString(recievedata);
                    // TODO: break the while loop if receivedLen==0
                    if (recievedLen == 0) break;
                    // TODO: Create a Request object using received request string
                    Request clientRequest = new Request(data);
                    // TODO: Call HandleRequest Method that returns the response
                    Response serverResponse = HandleRequest(clientRequest);
                    string res = serverResponse.ResponseString;
                    Console.WriteLine(res);

                    byte[] response = Encoding.ASCII.GetBytes(res);
                    // TODO: Send Response back to client
                    clientSock.Send(response);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSock.Shutdown(SocketShutdown.Both);
            clientSock.Close();
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content;

            //string code = "";
            int statusCode;

            try
            {
                //TODO: check for bad request
                if (!request.ParseRequest())
                {
                    statusCode = 400;
                    content = "<!DOCTYPE html>< html >< body >< h1 > 400 Bad Request</ h1>< p > 400 Bad Request </ p></ body></ html>";
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.
                string[] name = request.relativeURI.Split('/');
                string physical_path = Configuration.RootPath + '\\' + name[1];
                //TODO: check for redirect
                for (int i = 0; i < Configuration.RedirectionRules.Count; i++)
                {
                    if ('/' + Configuration.RedirectionRules.Keys.ElementAt(i).ToString() == request.relativeURI)
                    {
                        statusCode = 301;
                        request.relativeURI = '/' + Configuration.RedirectionRules.Values.ElementAt(i).ToString();
                        name[1] = Configuration.RedirectionRules.Values.ElementAt(i).ToString();

                        physical_path = Configuration.RootPath + '\\' + name[1];
                        content = File.ReadAllText(physical_path);
                        string location = "http://localhost:1000/" + name[1];

                        Response res = new Response((StatusCode)statusCode, "text/html", content, location);
                        return res;
                    }
                }
                //TODO: check file exists
                if (!File.Exists(physical_path))
                {
                    physical_path = Configuration.RootPath + '\\' + "NotFound.html";
                    statusCode = 404;


                    content = File.ReadAllText(physical_path);
                }
                //TODO: read the physical file
                else
                {
                    content = File.ReadAllText(physical_path);
                    statusCode = 200;
                }
                // Create OK response
                StatusCode cod;

                Response re = new Response((StatusCode)statusCode, "text/html", content, physical_path);

                return re;
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                // TODO: in case of exception, return Internal Server Error. 
                Logger.LogException(ex);
                string physical_path = Configuration.RootPath + '\\' + "InternalError.html";

                statusCode = 500;
                content = File.ReadAllText(physical_path);
                Response re = new Response((StatusCode)statusCode, "text/html", content, physical_path);

                return re;
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            for (int i = 0; i < Configuration.RedirectionRules.Count; i++)
            {
                if (relativePath == '/' + Configuration.RedirectionRules.Keys.ElementAt(i).ToString())
                {
                    string redirected_path = Configuration.RedirectionRules.Values.ElementAt(i).ToString();
                    string physical_path = Configuration.RootPath + '\\' + redirected_path;
                    return physical_path;
                }
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string content = " ";
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            try
            {
                if (File.Exists(filePath))
                    content = File.ReadAllText(filePath);

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return content;

            // else read file and return its content
            //return string.Empty;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                FileStream fs = new FileStream(filePath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                // then fill Configuration.RedirectionRules dictionary 
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    string[] data = line.Split(',');
                    if (data[0] == " ")
                        break;

                    Configuration.RedirectionRules = new Dictionary<string, string>();
                    Configuration.RedirectionRules.Add(data[0], data[1]);
                }
                fs.Close();
                sr.Close();
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                //Console.WriteLine(ex);
                Logger.LogException(ex);
                //Environment.Exit(1);
            }

        }
    }
}
