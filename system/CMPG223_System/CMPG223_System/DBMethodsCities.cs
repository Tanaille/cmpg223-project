using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsCities
    {       
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Cities manipulation methods
         */

        /// <summary>
        /// Add a city to the database.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        public static void AddCity(string cityName)
        {
            // Check if the city already exists
            if (DBMethods.CheckExistence("CITY", "City_Name", cityName))
            {
                MessageBox.Show("Cannot add city " + cityName + ". The city already exists.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    int cityID = DBMethods.GetCityIDFromCityName(cityName);

                    string query =
                        "INSERT INTO CITY (City_Name) " +
                        "VALUES (@cityName)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@cityName", cityName);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Added city.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error adding city " + cityName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Modify a city in the database.
        /// </summary>
        /// <param name="oldCityName">Name of the existing city.</param>
        /// <param name="cityName">New name of the city.</param>
        public static void ModifyCity(string oldCityName, string cityName)
        {
            try
            {
                // Check if the city name already exists
                if (DBMethods.CheckExistence("CITY", "City_Name", cityName))
                {
                    MessageBox.Show("Cannot modify city " + cityName + ". The city name already exists.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    int cityID = DBMethods.GetCityIDFromCityName(oldCityName);

                    string query =
                        "UPDATE CITY " +
                        "SET City_Name = @cityName " +
                        "WHERE City_ID = @cityID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@cityName", cityName);
                        cmd.Parameters.AddWithValue("@cityID", cityID);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Modified city.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error modifying city " + cityName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Delete a city from the database.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        public static void DeleteCity(string cityName)
        {
            try
            {                
                // Confirm deletion
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this city?", "Confirm deletion", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.No)
                    return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "DELETE FROM CITY " +
                        "WHERE City_Name = @cityName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@cityName", cityName);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Deleted city.");
                }

            }
            catch (SqlException ex)
            {
                string errorMessage = "Error deleting city " + cityName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
