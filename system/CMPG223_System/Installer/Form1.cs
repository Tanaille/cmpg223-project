using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace Installer
{
    public partial class frmInstaller : Form
    {
        public frmInstaller()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initiate the installation process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstall_Click_1(object sender, EventArgs e)
        {
            ProcessStartInfo main = new ProcessStartInfo();
            main.FileName = @"bin\setup.exe";
            Process p1 = Process.Start(main);
            p1.WaitForExit();


            ProcessStartInfo sqlLocalDB = new ProcessStartInfo();
            sqlLocalDB.FileName = @"bin\SqlLocalDB.msi";
            Process p2 = Process.Start(sqlLocalDB);
            p2.WaitForExit();

            Application.Exit();
        }
    }
}
