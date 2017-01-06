//********************************************************************************************
//Author: Sergey Stoyan
//        sergey.stoyan@gmail.com
//        http://www.cliversoft.com
//********************************************************************************************
using System;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace Cliver.PdfFixer
{
    class Program
    {
        static void Main()
        {
            try
            {
                string[] ps = Environment.GetCommandLineArgs();
                if (ps.Length < 2)
                {
                    Console.WriteLine("USAGE: #<application.exe> <pdf>");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return;
                }

                string input_pdf = ps[1];
                if (!input_pdf.Contains(":"))
                    input_pdf = Cliver.ProgramRoutines.GetAppDirectory() + "\\" + input_pdf;

                string f = Pdf.Fix(input_pdf, Cliver.PathRoutines.InsertSuffixBeforeFileExtension(input_pdf, "_fixed"));
                Console.WriteLine("Created pdf: " + f);
                Process.Start(f);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                for (; e.InnerException != null; e = e.InnerException)
                    Console.WriteLine("<= " + e.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
        }
    }
}