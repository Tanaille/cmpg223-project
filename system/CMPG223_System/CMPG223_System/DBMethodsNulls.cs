using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsNulls
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";


        /*
         * Display NULL value methods
         */

        /// <summary>
        /// Display all records in a table where the column values are NULL.
        /// </summary>
        /// <param name="lst">Output ListBox.</param>
        /// <param name="table">Table containing the records to display.</param>
        /// <param name="column">Column to check for NULL values.</param>
        public static void DisplayNulls(ListBox lst, string[] table, string[] columns, string nullValue, string key, string seperator, int columnCount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    lst.Items.Clear();

                    string query = string.Empty;

                    switch (columnCount)
                    {
                        case 1:
                            {
                                if (columns.Count() > 1)
                                {
                                    query =
                                        "SELECT " + columns[0] + ", " + columns[1] + " " +
                                        "FROM " + table[0] + " " +
                                        "WHERE " + nullValue + " IS NULL";

                                    break;
                                }
                                
                                query =
                                    "SELECT " + columns[0] + " " +
                                    "FROM " + table[0] + " " +
                                    "WHERE " + nullValue + " IS NULL";

                                break;
                            }

                        case 2:
                            {
                                query =
                                    "SELECT " + columns[0] + ", " + columns[1] + " " +
                                    "FROM " + table[0] + " a," + table[1] + " b " +
                                    "WHERE a." + nullValue + " IS NULL";

                                break;
                            }
                    }
                    

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                switch (columnCount)
                                {
                                    case 1:
                                        {
                                            if (columns.Count() > 1)
                                                lst.Items.Add(reader.GetValue(0).ToString() + seperator + reader.GetValue(1).ToString());
                                            else
                                                lst.Items.Add(reader.GetValue(0).ToString());

                                            break;
                                        }

                                    case 2:
                                        {
                                            lst.Items.Add(reader.GetValue(0).ToString() + seperator + reader.GetValue(1).ToString());

                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error displaying null types.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
