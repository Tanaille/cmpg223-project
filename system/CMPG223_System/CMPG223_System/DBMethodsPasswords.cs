using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsPasswords
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Password manipulation methods
         */

        /// <summary>
        /// Add a password to the database.
        /// </summary>
        /// <param name="userNumber">Name of the user associated with a password.</param>
        /// <param name="hash">Hashed password.</param>
        /// <param name="salt">Password salt value.</param>
        public static void AddPassword(int userNumber, string hash, string salt)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "INSERT INTO PASSWORDS (User_Number, Hash, Salt) " +
                        "VALUES (@userNumber, @hash, @salt)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@userNumber", userNumber);
                        cmd.Parameters.AddWithValue("@hash", hash);
                        cmd.Parameters.AddWithValue("@salt", salt);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error adding the password to the databse.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Modify a password in the database.
        /// </summary>
        /// <param name="password">Old password.</param>
        /// <param name="userNumber">User name associated with a password.</param>
        public static void ModifyPassword(string password, int userNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "UPDATE PASSWORDS " +
                        "SET Hash = @hash, Salt = @salt " +
                        "WHERE User_Number = @userNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        string salt = Authentication.GenerateSalt(70);
                        string hash = Authentication.HashPassword(password, salt, 1000, 70);

                        conn.Open();

                        cmd.Parameters.AddWithValue("@hash", hash);
                        cmd.Parameters.AddWithValue("@salt", salt);
                        cmd.Parameters.AddWithValue("@userNumber", userNumber);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error modifying password.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Delete a password from the database.
        /// </summary>
        /// <param name="userNumber">User number of a user.</param>
        public static void DeletePassword(int userNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "DELETE FROM PASSWORDS " +
                        "WHERE User_Number = @userNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@userNumber", userNumber);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error deleting password.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
