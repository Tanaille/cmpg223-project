using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace CMPG223_System
{
    class DBMethodsPopulateControls
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";
        
        /*
         * Populate control methods
         */
        /// <summary>
        /// Populate a ComboBox with values from a database table.
        /// </summary>
        /// <param name="cbx">The ComboBox to be populated.</param>
        /// <param name="table">The table from which to retrieve values.</param>
        /// <param name="column">The column from which to retrieve values.</param>
        public static void PopulateComboBox(ComboBox cbx, string table, string column)
        {
            try
            {
                cbx.Items.Clear();
            
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT " + column + " " +
                        "FROM " + table + " " +
                        "ORDER BY " + column + " ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                cbx.Items.Add(reader[0].ToString());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error populating the ComboBox.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Populate a DataGridView with values from database tables.
        /// First overload - single table.
        /// </summary>
        /// <param name="dgv">The DataGridView to be populated.</param>
        /// <param name="tables">The tables from which to retrieve values.</param>
        /// <param name="columns">Columns to be displayed in the DataGridView.</param>
        public static void PopulateDataGridView(DataGridView dgv, string[] tables, string columns, int numTables, string key)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = string.Empty;

                    // Build query based on the number of tables required.
                    switch (numTables)
                    {
                        case 1:
                            {
                                query =
                                    "SELECT " + columns + " " +
                                    "FROM " + tables[0];

                                break;
                            }

                        case 2:
                            {
                                query =
                                    "SELECT " + columns + " " +
                                    "FROM " + tables[0] + " a, " + tables[1] + " b " +
                                    "WHERE a." + key + " = b." + key;

                                break;
                            }

                        case 3:
                            {
                                query =
                                    "SELECT " + columns + " " +
                                    "FROM " + tables[0] + " a, " + tables[1] + " b, " + tables[2] + " c " +
                                    "WHERE a." + key + " = b." + key + " AND b." + key + " = c." + key;

                                break;
                            }

                        case 4:
                            {
                                query =
                                    "SELECT " + columns + " " +
                                    "FROM ADOPTION a, ADOPTER b, USERS c, ANIMAL d " +
                                    "WHERE a.Adopter_Number = b.Adopter_Number AND a.User_Number = c.User_Number AND a.Animal_Number = d.Animal_Number";

                                break;
                            }

                        default:
                            {
                                MessageBox.Show("Invalid query. Check table / column names and number of tables in DBMethodsPopulateControls.PopulateDataGridView method call.");

                                return;
                            }
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        DataTable dt = new DataTable();
                        dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
                        da.Fill(dt);

                        BindingSource bs = new BindingSource();
                        bs.DataSource = dt;
                        dgv.DataSource = bs;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error populating the DataGridView.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Popluate a ListBox with values from a database table.
        /// </summary>
        /// <param name="lst">The ListBox to be populated.</param>
        /// <param name="table">The table from which to retrieve values.</param>
        /// <param name="columns">The column from which to retrieve values.</param>
        /// <param name="seperator">Column seperator string.</param>
        /// <param name="numColumns">Number of columns to display.</param>
        public static void PopulateListBox(ListBox lst, string table, string[] columns, int numColumns, string seperator)
        {
            try
            {
                lst.Items.Clear();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = string.Empty;

                    switch (numColumns)
                    {
                        case 1:
                            {
                                query =
                                    "SELECT " + columns[0] + " " +
                                    "FROM " + table + " " +
                                    "ORDER BY " + columns[0] + " ASC";

                                break;
                            }

                        case 2:
                            {
                                query =
                                    "SELECT " + columns[0] + ", " + columns[1] + " " +
                                    "FROM " + table + " " +
                                    "ORDER BY " + columns[0] + " ASC";

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
                                switch (numColumns)
                                {
                                    case 1:
                                        {
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
                string errorMessage = "Error populating the ListBox.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Popluate a ListBox with values from a multiple database tables.
        /// </summary>
        /// <param name="lst">The ListBox to be populated.</param>
        /// <param name="table">The table from which to retrieve values.</param>
        /// <param name="columns">The column from which to retrieve values.</param>
        /// <param name="key">Key linking tables.</param>
        /// <param name="numColumns">Number of columns to display.</param>
        /// <param name="seperator">Column seperator string.</param>
        public static void PopulateListBox(ListBox lst, string[] table, string[] columns, string key, int numColumns, string seperator)
        {
            try
            {
                lst.Items.Clear();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = string.Empty;

                    switch (numColumns)
                    {
                        case 1:
                            {
                                query =
                                    "SELECT " + columns[0] + " " +
                                    "FROM " + table + " a " +
                                    "ORDER BY " + columns[0] + " ASC";

                                break;
                            }

                        case 2:
                            {
                                query =
                                    "SELECT " + columns[0] + ", " + columns[1] + " " +
                                    "FROM " + table[0] + " a, " + table[1] + " b " +
                                    "WHERE a." + key + " = b." + key + " " +
                                    "ORDER BY " + columns[0] + " ASC";

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
                                switch (numColumns)
                                {
                                    case 1:
                                        {
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
                string errorMessage = "Error populating the ListBox.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /*
         * Filter control methods
         */
        /// <summary>
        /// Filters a ListBox according to a filter string.
        /// </summary>
        /// <param name="lst">The ListBox to be filtered.</param>
        /// <param name="table">The table containing the items to be filtered.</param>
        /// <param name="column">The column by which to filter.</param>
        /// <param name="filter">Filter string.</param>
        public static void FilterListBox(ListBox lst, string table, string column, string filter)
        {
            try
            {
                lst.Items.Clear();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT " + column + " " +
                        "FROM " + table + " " +
                        "WHERE " + column + " " +
                        "LIKE '%" + filter + "%' " +
                        "ORDER BY " + column + " ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                lst.Items.Add(reader.GetValue(0).ToString());
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error filtering the ListBox.\n\n" + ex.Message);
            }
        }
    }
}
