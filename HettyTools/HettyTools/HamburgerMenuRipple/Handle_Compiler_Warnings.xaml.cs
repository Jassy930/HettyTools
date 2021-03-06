﻿using System.Windows.Controls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System;

namespace HettyTools
{
    public partial class Handle_Compiler_Warnings : UserControl
    {
        public Handle_Compiler_Warnings()
        {
            InitializeComponent();
        }


        void CWHandle(string filename)
        {
            string outfile = filename + ".txt";

            string vob = CWvobtextbox.Text;

            StreamReader sr = new StreamReader(filename, Encoding.Default);
            StreamWriter sw = File.CreateText(outfile);

            int flag = 0;
            string branch33 = string.Empty;
            sw.WriteLine("The following warnings are not on the list of acceptable warnings"); sw.WriteLine("");
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (!Regex.IsMatch(line, "1643|1479|1518|1606|1522|1517|1516|1604|1772") && Regex.IsMatch(line, "dcc:") && !Regex.IsMatch(line, "1004|1025|1481|1500|1551|1552|1554|1555|1607|1608") && Regex.IsMatch(line, vob))
                {
                    sw.WriteLine(line);
                }

                if (Regex.IsMatch(line, "dcc:1004|dcc:1025|dcc:1481|dcc:1500|dcc:1551|dcc:1552|dcc:1554|dcc:1555|dcc:1607|dcc:1608"))
                    branch33 += System.Environment.NewLine + line;
            }
            sw.WriteLine("\n\nThe following warnings have been converted to errors as of Branch 33\n");
            sw.Write(branch33);

            sr.Close();
            sw.Close();
        }

        private void Grid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 0 && (e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            foreach (string file in files)
            {
                try
                {
                    switch (e.Effects)
                    {
                        case DragDropEffects.Copy:
                            CWHandle(file);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        //private void Grid_DragOver(object sender, DragEventArgs e)
        //{
        //    e.Effects = DragDropEffects.Copy;
        //}
    }
}
