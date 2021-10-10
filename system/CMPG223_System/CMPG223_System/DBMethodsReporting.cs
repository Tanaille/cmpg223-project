using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsReporting
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Report generation methods
         */

        /// <summary>
        /// Generate report - Animals adopted per time period.
        /// </summary>
        /// <param name="dgv">DataGridView to populate.</param>
        /// <param name="start">Start date of time period.</param>
        /// <param name="end">End date of time period.</param>
        /// <param name="total">Total number of items.</param>
        public static void GenerateReportAdoptions(DataGridView dgv, DateTime start, DateTime end, string orderBy, ref int total)
        {
            try
            {
                // Populate the datagridview
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT a.Animal_Name, a.Animal_Number, b.Adopter_Name, c.Adoption_Date, d.User_Name " +
                        "FROM ANIMAL a, ADOPTER b, ADOPTION c, USERS d " +
                        "WHERE a.Animal_Number = c.Animal_Number AND b.Adopter_Number = c.Adopter_Number AND d.User_Number = c.User_Number AND CAST(c.Adoption_Date AS date) BETWEEN CAST(@start AS date) AND CAST(@end AS DATE) " +
                        "ORDER BY c.Adoption_Date " + orderBy;

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);

                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = cmd;

                            DataTable tb = new DataTable();
                            tb.Locale = System.Globalization.CultureInfo.InvariantCulture;
                            da.Fill(tb);

                            BindingSource bs = new BindingSource();
                            bs.DataSource = tb;
                            dgv.DataSource = bs;
                        }

                        dgv.Columns[0].HeaderText = "Animal";
                        dgv.Columns[1].HeaderText = "Animal number";
                        dgv.Columns[2].HeaderText = "Adopter";
                        dgv.Columns[3].HeaderText = "Adoption date";
                        dgv.Columns[4].HeaderText = "User";
                    }
                }

                // Get the total number of adoptions
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = 
                        "SELECT COUNT(1) " +
                        "FROM ADOPTION " +
                        "WHERE Adoption_Date BETWEEN CAST(@start AS date) AND CAST(@end AS date)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                total = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error generating adoption report.\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Generate report - Animals received per time period.
        /// </summary>
        /// <param name="dgv">DataGridView to populate.</param>
        /// <param name="start">Start date of time period.</param>
        /// <param name="end">End date of time period.</param>
        /// <param name="total">Total number of items.</param>
        public static void GenerateReportAnimals(DataGridView dgv, DateTime start, DateTime end, string orderBy, ref int total)
        {
            try
            {
                // Populate the datagridview
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT a.Animal_Number, a.Animal_Name, b.Type_Name, a.Add_Date " +
                        "FROM ANIMAL a, ANIMAL_TYPE b " +
                        "WHERE a.Type_Number = b.Type_Number AND a.Add_Date BETWEEN CAST(@start AS date) AND CAST(@end AS date) " +
                        "ORDER BY a.Animal_Name " + orderBy;

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);

                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = cmd;

                            DataTable tb = new DataTable();
                            tb.Locale = System.Globalization.CultureInfo.InvariantCulture;
                            da.Fill(tb);

                            BindingSource bs = new BindingSource();
                            bs.DataSource = tb;
                            dgv.DataSource = bs;
                        }

                        dgv.Columns[0].HeaderText = "Animal number";
                        dgv.Columns[1].HeaderText = "Name";
                        dgv.Columns[2].HeaderText = "Type";
                        dgv.Columns[3].HeaderText = "Add date";
                    }
                }




                // Get the total number of adoptions
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = 
                        "SELECT COUNT(1) " +
                        "FROM ANIMAL " +
                        "WHERE Add_Date BETWEEN CAST(@start AS date) AND CAST(@end AS date)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                total = reader.GetInt32(0);
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error generating animal report.\n\n" + ex.Message);
            }
        }
    }
}
