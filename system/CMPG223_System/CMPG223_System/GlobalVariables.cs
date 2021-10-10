using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace CMPG223_System
{
    class GlobalVariables
    {
        // Category: Paths

        // Error log path
        private static string workingDirectory = Environment.CurrentDirectory;
        public static string errorLogPath = Directory.GetParent(workingDirectory).Parent.FullName + @"\logs\errorlog.txt";
        public static string accessLogPath = Directory.GetParent(workingDirectory).Parent.FullName + @"\logs\accesslog.txt";
        public static string dbLogPath = Directory.GetParent(workingDirectory).Parent.FullName + @"\logs\dblog.txt";
        public static string riLogPath = Directory.GetParent(workingDirectory).Parent.FullName + @"\logs\referentialintegrity.txt";

        // Current user
        public static string currentUser = string.Empty;
    }
}
