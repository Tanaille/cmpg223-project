using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class Authentication
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Password generation and comparison methods
         */
        /// <summary>
        /// Generates a salt value to be used with password hashing.
        /// </summary>
        /// <param name="nSalt">Size of salt to be generated.</param>
        /// <returns>String value containing the salt.</returns>
        public static string GenerateSalt(int nSalt)
        {
            var saltBytes = new byte[nSalt];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        ///  Generates an salted hashed password using PBKDF2.
        /// </summary>
        /// <param name="password">Password to be encrypted.</param>
        /// <param name="salt">Salt value to be used during encryption</param>
        /// <param name="nIterations">Number of hashing algorithm iterations</param>
        /// <param name="nHash">Size of hash salt.</param>
        /// <returns>String value containing the salted hash.</returns>
        public static string HashPassword(string password, string salt, int nIterations, int nHash)
        {
            var saltBytes = Convert.FromBase64String(salt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
            }
        }

        /// <summary>
        /// Compares the hash of a provided password with that of a stored hash.
        /// </summary>
        /// <param name="password">Plaintext password to be hashed and compared.</param>
        /// <param name="hashedPassword">Hash of a stored password.</param>
        /// <param name="salt">Salt value to be used during encryption</param>
        /// <param name="nIterations">Number of hashing algorithm iterations</param>
        /// <param name="nHash">Size of hash salt.</param>
        /// <returns>Bool value true if the hashes match. Bool value false if the hashes do not match.</returns>
        public static bool CompareHash(string password, string hashedPassword, string salt, int nIterations, int nHash)
        {
            return (HashPassword(password, salt, nIterations, nHash) == hashedPassword);
        }

        /// <summary>
        /// Authenticates a user against the database.
        /// </summary>
        /// <param name="username">Name of the user to be authenticated.</param>
        /// <param name="password">Password of the user to be authenticated.</param>
        /// <returns></returns>
        public static bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT USERS.User_Name, PASSWORDS.Hash, PASSWORDS.Salt " +
                        "FROM USERS " +
                        "INNER JOIN PASSWORDS " +
                        "ON USERS.User_Number = PASSWORDS.User_Number " +
                        "WHERE USERS.User_Name = @userName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", username);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Compare password hashes to authenticate
                                    if (Authentication.CompareHash(password, reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), 1000, 70))
                                    {
                                        LogWriter.WriteAccessLog(reader.GetValue(0).ToString(), "Success");
                                        Console.WriteLine("Log: Login successful.");

                                        isAuthenticated = true;
                                    }
                                    else
                                    {
                                        LogWriter.WriteAccessLog(reader.GetValue(0).ToString(), "Fail");
                                        Console.WriteLine("Log: Login unsuccessful.");
                                        isAuthenticated = false;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                string logMessage = "Log: Error connecting to database or incorrect username / password combination.";
                LogWriter.WriteErrorLog(logMessage, ex.Message);

                MessageBox.Show(logMessage + "\n\n" + ex.Message);
            }

            return isAuthenticated;
        }
    }
}
