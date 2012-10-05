using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
namespace Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 && File.Exists(args[0]) && Path.GetExtension(args[0]).ToLower() == ".xml")
            {
                string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
                string firstLevel = Path.GetFileNameWithoutExtension(args[0]);

                //Try to open the ini
                IniLoader gIni = new IniLoader(exeDir + "Content\\WIZZ.ini");
                gIni.ModifyValue("startOnLevel", firstLevel);
                gIni.ModifyValue("showMenuOnStart", "false");
                gIni.WriteOutModified(exeDir + "Content\\WIZZ.ini");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
