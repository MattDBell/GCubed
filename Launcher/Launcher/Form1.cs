using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace Launcher
{
    public partial class Form1 : Form
    {
        public static string s_GameDir = "";
        public static string s_GameExe = "WizardsPlatformer.exe";
        public static string s_IniPath = s_GameDir + "Content\\";
        public static string s_IniName = "WIZZ.ini";

        public static string s_currentDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private void launchBtn_Click(object sender, EventArgs e)
        {
            launchGame();
            //launchGameAndQuit();
        }

        void launchGame()
        {
            System.Diagnostics.Process.Start(s_currentDirectory + s_GameDir + s_GameExe);
        }
        void launchGameAndQuit()
        {
            launchGame();
            Application.Exit();
        }
    }
}
