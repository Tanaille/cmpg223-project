using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsAdopters
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Adopter manipulation methods
         */

        /// <summary>
        /// Add an adopter to the database
        /// </summary>
        /// <param name="name">Adopter name.</param>
        /// <param name="surname">Adopter surname.</param>
        /// <param name="contactNumber">Adopter contact number.</param>
        /// <param name="streetNumber">Adopter street number.</param>
        /// <param name="streetName">Adopter street name.</param>
        /// <param name="cityName">Adopter city name.</param>
        public static void AddAdopter(string name, string surname, string contactNumber, string streetNumber, string streetName, string cityName)
        {
            try
            {
                // Check if the adopter already exists
                if (DBMethods.CheckExistence("ADOPTER", "Adopter_Name", name))
                {
                    MessageBox.Show("Cannot add adopter " + name + ". The adopter name already exists.");
                    return;
                }

                int cityID = DBMethods.GetCityIDFromCityName(cityName);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "INSERT INTO ADOPTER (Adopter_Name, Adopter_Surname, Adopter_Contact_Number, Adopter_Street_Number, Adopter_Street_Name, City_ID) " +
                        "VALUES (@name, @surname, @contactNumber, @streetNumber, @streetName, @cityID)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@surname", surname);
                        cmd.Parameters.AddWithValue("@contactNumber", contactNumber);
                        cmd.Parameters.AddWithValue("@streetNumber", streetNumber);
                        cmd.Parameters.AddWithValue("@streetName", streetName);
                        cmd.Parameters.AddWithValue("@cityID", cityID);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Added adopter.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error adding adopter " + name + " " + surname;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Modify an adopter in the database.
        /// </summary>
        /// <param name="oldName">Old adopter name.</param>
        /// <param name="name">New dopter name.</param>
        /// <param name="surname">Adopter surname.</param>
        /// <param name="contactNumber">Adopter contact number.</param>
        /// <param name="streetNumber">Adopter street name.</param>
        /// <param name="streetName">Adopter street name.</param>
        /// <param name="cityName">Adopter city name.</param>
        public static void ModifyAdopter(string oldName, string name, string surname, string contactNumber, string streetNumber, string streetName, string cityName)
        {
            try
            {
                int cityID = DBMethods.GetCityIDFromCityName(cityName);
                string oldSurname = DBMethods.GetAdopterSurnameFromName(oldName);
                int adopterNumber = DBMethods.GetAdopterNumberFromName(oldName, oldSurname);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "UPDATE ADOPTER " +
                        "SET Adopter_Name = @name, Adopter_Surname = @surname, Adopter_Contact_Number = @contactNumber, Adopter_Street_Number = @streetNumber, Adopter_Street_Name = @streetName, City_ID = @cityID " +
                        "WHERE Adopter_Number = @adopterNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@surname", surname);
                        cmd.Parameters.AddWithValue("@contactNumber", contactNumber);
                        cmd.Parameters.AddWithValue("@streetNumber", streetNumber);
                        cmd.Parameters.AddWithValue("@streetName", streetName);
                        cmd.Parameters.AddWithValue("@cityID", cityID);
                        cmd.Parameters.AddWithValue("@adopterNumber", adopterNumber);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Modified adopter.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error modifying adopter " + name + " " + surname;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
        
        /// <summary>
        /// Delete an adopter from the database.
        /// </summary>
        /// <param name="name">Adopter name.</param>
        public static void DeleteAdopter(string name)
        {
            try
            {
                // Confirm deletion
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this adopter?", "Confirm deletion", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.No)
                    return;

                string surname = DBMethods.GetAdopterSurnameFromName(name);
                int adopterNumber = DBMethods.GetAdopterNumberFromName(name, surname);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "DELETE FROM ADOPTER " +
                        "WHERE Adopter_Number = @adopterNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@adopterNumber", adopterNumber);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Deleted adopter.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error deleting adopter " + name;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
