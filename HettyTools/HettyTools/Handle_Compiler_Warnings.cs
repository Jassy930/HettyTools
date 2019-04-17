using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace HettyTools
{
    static class Handle_Compiler_Warnings
    {
        static void Handle(string filename)
        {
            string outfile = filename + ".txt";

            Console.WriteLine("Please input the vob:");
            Console.WriteLine("(or if you don't need, simply type Enter");
            string vob = Console.ReadLine();

            StreamReader sr = new StreamReader(filename, Encoding.Default);
            StreamWriter sw = File.CreateText(outfile);

            int flag = 0;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (flag == 0) { sw.WriteLine("The following warnings are not on the list of acceptable warnings"); sw.WriteLine(""); flag++; }
                if (flag == 1)
                {
                    if (line.Contains("CRITICAL WARNING SUMMARY"))
                    {
                        flag++;
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("The following warnings have been converted to errors as of Branch 33");
                        sw.WriteLine("");
                    }
                    else
                    {
                        string no = line.Split(' ')[0];
                        if (!Regex.IsMatch(no, "1643|1479|1518|1606|1522|1517|1516|1604|1772") && Regex.IsMatch(line, "dcc:") && !Regex.IsMatch(no, "1004|1025|1481|1500|1551|1552|1554|1555|1607|1608") && Regex.IsMatch(line, vob))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                if (flag == 2)
                {
                    if (Regex.IsMatch(line, "dcc:1004|dcc:1025|dcc:1481|dcc:1500|dcc:1551|dcc:1552|dcc:1554|dcc:1555|dcc:1607|dcc:1608"))
                    {
                        sw.WriteLine(line);
                    }
                }
            }

            sr.Close();
            sw.Close();

            Console.WriteLine("Done...");
            Console.WriteLine("Wish you a happy day~~~");

            Console.ReadLine();
        }
    }
}
