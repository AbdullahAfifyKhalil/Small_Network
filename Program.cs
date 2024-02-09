using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
            String filePath = @"C:\Users\M7md\Desktop\New\Template[2021-2022]\HTTPServer\bin\Debug\RedirectionRules.txt";
            // 1) Make server object on port 1000
            Server httpserver = new Server(1000, filePath);
            // 2) Start Server
            httpserver.StartServer();
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            FileStream fs = new FileStream(@"C:\Users\M7md\Desktop\New\Template[2021-2022]\HTTPServer\bin\Debug\RedirectionRules.txt", FileMode.OpenOrCreate);
            StreamWriter fw = new StreamWriter(fs);
            fw.WriteLine(@"aboutus2.html,/aboutus.html");
            fw.Close();
            fs.Close();
        }

    }
}
