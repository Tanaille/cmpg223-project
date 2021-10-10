using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsUsers
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * User manipulation methods
         */

        /// <summary>
        /// Add a user to the database..
        /// </summary>
        /// <param name="name">User name.</param>
        /// <param name="surname">User surname.</param>
        /// <param name="password">User password.</param>
        /// <param name="userType">User type / role.</param>
        public static void AddUser(string name, string surname, string password, string userType)
        {
            try
            {
                // Check if the type already exists
                if (DBMethods.CheckExistence("USERS", "User_Name", name))
                {
                    MessageBox.Show("Cannot add user " + name + ". The user name already exists.");
                    return;
                }

                byte typeNumber = DBMethods.GetUserTypeNumberFromTypeName(userType);
                string salt = Authentication.GenerateSalt(70);
                string hash = Authentication.HashPassword(password, salt, 1000, 70);


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "INSERT INTO USERS (User_Type, User_Name, User_Surname) " +
                        "VALUES (@typeNumber, @name, @surname)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeNumber", typeNumber);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@surname", surname);

                        cmd.ExecuteNonQuery();
                    }

                    // Create a password for the user
                    int userNumber = DBMethods.GetUserNumberFromUserName(name, surname);
                    DBMethodsPasswords.AddPassword(userNumber, hash, salt);

                    MessageBox.Show("User added.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error adding new user " + name + " " + surname;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Modify a user in the database.
        /// </summary>
        /// <param name="userName">New user name.</param>
        /// <param name="oldName">Old user name.</param>
        /// <param name="userSurname">User surname.</param>
        /// <param name="password">User password.</param>
        /// <param name="typeDescription">User type description.</param>
        public static void ModifyUser(string userName, string oldName, string userSurname, string password, string typeDescription)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    byte typeNumber = DBMethods.GetUserTypeNumberFromTypeName(typeDescription);
                    string oldSurname = DBMethods.GetUserSurnameFromName(oldName);

                    int userNumber = DBMethods.GetUserNumberFromUserName(oldName, oldSurname);

                    string query =
                        "UPDATE USERS " +
                        "SET User_Name = @userName, User_Surname = @userSurname, User_Type = @typeNumber " +
                        "WHERE User_Number = @userNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.Parameters.AddWithValue("@userSurname", userSurname);
                        cmd.Parameters.AddWithValue("@typeNumber", typeNumber);
                        cmd.Parameters.AddWithValue("@userNumber", userNumber);

                        cmd.ExecuteNonQuery();

                    }

                    DBMethodsPasswords.ModifyPassword(password, userNumber);

                    MessageBox.Show("User modified.");
                }

            }
            catch (SqlException ex)
            {
                string errorMessage = "Error modifying user " + userName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Delete a user from the database.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public static void DeleteUser(string userName)
        {
            try
            {
                // Confirm deletion
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this user?", "Confirm deletion", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.No)
                    return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string userSurname = DBMethods.GetUserSurnameFromName(userName);
                    int userNumber = DBMethods.GetUserNumberFromUserName(userName, userSurname);

                    DBMethodsPasswords.DeletePassword(userNumber);

                    string query =
                        "DELETE FROM USERS " +
                        "WHERE User_Number = @userNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@userNumber", userNumber);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("User deleted.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error deleting user " + userName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
