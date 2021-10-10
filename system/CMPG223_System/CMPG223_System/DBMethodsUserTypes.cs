using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsUserTypes
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * User type manipulation methods
         */

        /// <summary>
        /// Add a user type to the database.
        /// </summary>
        /// <param name="typeDescription">Name of the new user type.</param>
        public static void AddUserType(string typeDescription)
        {
            try
            {
                // Check if the type already exists
                if (DBMethods.CheckExistence("USER_TYPE", "Type_Description", typeDescription))
                {
                    MessageBox.Show("Cannot add user type" + typeDescription + ". The user type already exists.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "INSERT INTO USER_TYPE (Type_Description) " +
                        "VALUES (@typeDescription)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeDescription", typeDescription);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Added user type.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error adding user type " + typeDescription;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Delete a user type from the database.
        /// </summary>
        /// <param name="typeDescription">Name of the user type.</param>
        public static void DeleteUserType(string typeDescription)
        {
            try
            {
                // Confirm deletion
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this user type?", "Confirm deletion", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.No)
                    return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "DELETE FROM USER_TYPE " +
                        "WHERE Type_Description = @typeDescription";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeDescription", typeDescription);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Deleted user type.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error deleting user type " + typeDescription;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
