using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            FileStream fs = new FileStream(@"C:\Users\M7md\Desktop\New\Template[2021-2022]\HTTPServer\bin\Debug\log.txt", FileMode.OpenOrCreate);
            //Datetime:      
            //message:
            // for each exception write its details associated with datetime
            StreamWriter fw = new StreamWriter(fs);

            fw.WriteLine("Date Time :" + DateTime.Now.ToString());
            fw.WriteLine("Message :" + ex.Message);

            fs.Close();

        }
    }
}