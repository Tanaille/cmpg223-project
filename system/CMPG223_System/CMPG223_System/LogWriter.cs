using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CMPG223_System
{
    class LogWriter
    {
        /// <summary>
        /// Create an error log in the /logs folder.
        /// </summary>
        /// <param name="errorMessage">Custom friendly error message.</param>
        /// <param name="exception">Exception message.</param>
        public static void WriteErrorLog(string errorMessage, string exception)
        {
            if (!File.Exists(GlobalVariables.errorLogPath))
            {
                using (StreamWriter sw = File.CreateText(GlobalVariables.errorLogPath))
                {
                    sw.WriteLine("Error logfile created: " + DateTime.Now);
                    sw.WriteLine("---------------------------------------------\n\n");
                }
            }

            using (StreamWriter sw = File.AppendText(GlobalVariables.errorLogPath))
            {
                sw.WriteLine(DateTime.Now + ":\t" + errorMessage);
                sw.WriteLine("\t\t\t" + exception + "\n");
            }
        }

        /// <summary>
        /// Create an access log in the /logs folder.
        /// </summary>
        /// <param name="username">Name of the user.</param>
        /// <param name="result">Access result (success / fail).</param>
        public static void WriteAccessLog(string username, string result)
        {
            if (!File.Exists(GlobalVariables.accessLogPath))
            {
                using (StreamWriter sw = File.CreateText(GlobalVariables.accessLogPath))
                {
                    sw.WriteLine("Access logfile created: " + DateTime.Now);
                    sw.WriteLine("---------------------------------------------\n\n");
                    sw.Write("Access attempt\t\tUsername\tResult\n\n");
                }
            }

            using (StreamWriter sw = File.AppendText(GlobalVariables.accessLogPath))
            {
                sw.WriteLine(DateTime.Now + ":\t" + username + "\t\t" + result);
            }
        }

        /// <summary>
        /// Create a referential integrity log in the /logs folder.
        /// </summary>
        /// <param name="operationType">Type of RI operation.</param>
        /// <param name="table">Table affected by the RI operation.</param>
        /// <param name="field">Field affected by the RI operation</param>
        public static void WriteReferentialIntergrityLog(string operationType, string table, string field)
        {
            if (!File.Exists(GlobalVariables.riLogPath))
            {
                using (StreamWriter sw = File.CreateText(GlobalVariables.riLogPath))
                {
                    sw.WriteLine("Referential integrity logfile created: " + DateTime.Now);
                    sw.WriteLine("----------------------------------------------------------\n\n");
                    sw.Write("Date\t\t\tOperation type\t\tAffected table\tAffected field\n\n");
                }
            }

            using (StreamWriter sw = File.AppendText(GlobalVariables.riLogPath))
            {
                sw.WriteLine(DateTime.Now + ":\t" + operationType + "\t\t" + table + "\t\t" + field);
            }
        }
    }
}
