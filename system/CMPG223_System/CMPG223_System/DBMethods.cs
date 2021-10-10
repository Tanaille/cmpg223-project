using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace CMPG223_System
{
    class DBMethods
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Validation methods
         */
        public static bool CheckExistence(string table, string column, string value)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                       "SELECT TOP 1 " + column + " " +
                       "FROM " + table + " " +
                       "WHERE " + column + " = @value";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@value", value);

                        if (cmd.ExecuteScalar() == null)
                            return false;
                        else
                            return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error checking for " + value + " in column " + column + " in table " + table + ".";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return false;
            }
        }

        /*
         * Value derivation methods
         */

        /// <summary>
        /// Retrieve the animal type number associated with a type name.
        /// </summary>
        /// <param name="typeName">The type name corrosponding with a type number.</param>
        /// <returns>Integer value containing the animal type number.</returns>
        public static int GetAnimalTypeNumberFromTypeName(string typeName)
        {
            int typeNumber = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Type_Number " +
                        "FROM ANIMAL_TYPE " +
                        "WHERE Type_Name = @typeName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeName", typeName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                    typeNumber = reader.GetInt32(0);
                            }
                        }

                        return typeNumber;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting animal type number for " + typeName;
                LogWriter.WriteErrorLog(errorMessage, ex.Message);
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                // Error case
                return typeNumber;
            }
        }

        /// <summary>
        /// Retrieve the animal type number associated with an animal name.
        /// </summary>
        /// <param name="name">The animal type number corrosponding with an animal name.</param>
        /// <returns>Integer value containing the animal type number.</returns>
        public static int GetAnimalTypeNumberFromAnimalName(string name)
        {
            int typeNumber = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Type_Number " +
                        "FROM ANIMAL " +
                        "WHERE Animal_Name = @name AND Type_Number IS NOT NULL";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@name", name);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                    typeNumber = reader.GetInt32(0);
                            }

                            return typeNumber;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting animal type number for " + name;
                LogWriter.WriteErrorLog(errorMessage, ex.Message);
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                // Error case
                return typeNumber;
            }
        }

        /// <summary>
        /// Retrieve the animal number associated with an animal name.
        /// </summary>
        /// <param name="animalName">The animal name corrosponding with an animal number.</param>
        /// <returns>Integer value containing the animal number.</returns>
        public static int GetAnimalNumberFromAnimalName(string animalName)
        {
            int animalNumber = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Animal_Number " +
                        "FROM ANIMAL " +
                        "WHERE Animal_Name = @animalName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("animalName", animalName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                animalNumber = reader.GetInt32(0);
                        }

                        return animalNumber;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting animal number for " + animalName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                // Error case
                return animalNumber;
            }
        }

        /// <summary>
        /// Retrieve the animal type name associated with an animal type number.
        /// </summary>
        /// <param name="typeNumber">The animal type number corrosponding with an animal name.</param>
        /// <returns></returns>
        public static string GetTypeNameFromTypeNumber(int typeNumber)
        {
            string typeName = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Type_Name " +
                        "FROM ANIMAL_TYPE " +
                        "WHERE Type_Number = @typeNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeNumber", typeNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                    typeName = reader[0].ToString();
                            }

                            return typeName;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting animal type name for type number " + typeNumber;
                LogWriter.WriteErrorLog(errorMessage, ex.Message);
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }

            // Error case
            return typeName;
        }

        /// <summary>
        /// Retrieve the user type number associated with a user type name.
        /// </summary>
        /// <param name="typeName">Name of the user type.</param>
        /// <returns>Byte value containing the type number. Returns 0 for errors.</returns>
        public static byte GetUserTypeNumberFromTypeName(string typeName)
        {
            try
            {
                byte userType = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT User_Type " +
                        "FROM USER_TYPE " +
                        "WHERE Type_Description = @typeName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeName", typeName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userType = reader.GetByte(0);
                            }
                        }

                        return userType;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting type number from " + typeName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return 0;
            }
        }

        /// <summary>
        /// Retrieve the user number associated with a user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userSurname">Surname of the user.</param>
        /// <returns>Integer value containing the user number. -1 for errors.</returns>
        public static int GetUserNumberFromUserName(string userName, string userSurname)
        {
            int userNumber = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT User_Number " +
                        "FROM USERS " +
                        "WHERE User_Name = @userName AND User_Surname = @userSurname";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.Parameters.AddWithValue("@userSurname", userSurname);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                userNumber = reader.GetInt32(0);

                            return userNumber;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting user number for user " + userName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return userNumber; ;
            }
        }

        /// <summary>
        /// Retrieve the user surname associated with a user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>String value containing the surname of the user.</returns>
        public static string GetUserSurnameFromName(string userName)
        {
            string surname = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT User_Surname " +
                        "FROM USERS " +
                        "WHERE User_Name = @userName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@userName", userName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                surname = reader[0].ToString();

                            return surname;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting user surname for " + userName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return surname;
            }
        }

        /// <summary>
        /// Retrieve the city ID associated with a city name.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        /// <returns>Integer value containing the city ID.</returns>
        public static int GetCityIDFromCityName(string cityName)
        {
            int cityID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT City_ID " +
                        "FROM CITY " +
                        "WHERE City_Name = @cityName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@cityName", cityName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                cityID = reader.GetInt32(0);

                            return cityID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting city ID for " + cityName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return cityID;
            }
        }

        /// <summary>
        /// Retrieve the adopter surname associated with an adopter name.
        /// </summary>
        /// <param name="name">Name of the adopter.</param>
        /// <returns>String value containing the surname of the adopter.</returns>
        public static string GetAdopterSurnameFromName(string name)
        {
            string surname = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Adopter_Surname " +
                        "FROM ADOPTER " +
                        "WHERE Adopter_Name = @adopterName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@adopterName", name);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                surname = reader[0].ToString();

                            return surname;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting adopter surname for " + name;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return surname;
            }
        }

        /// <summary>
        /// Retrieve the adopter number associated with an adopter name.
        /// </summary>
        /// <param name="name">The name of the adopter.</param>
        /// <param name="surname">The surname of the adopter.</param>
        /// <returns>Integer value containing the adopter number.</returns>
        public static int GetAdopterNumberFromName(string name, string surname)
        {
            int userNumber = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Adopter_Number " +
                        "FROM ADOPTER " +
                        "WHERE Adopter_Name = @adopterName AND Adopter_Surname = @adopterSurname";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@adopterName", name);
                        cmd.Parameters.AddWithValue("@adopterSurname", surname);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                userNumber = reader.GetInt32(0);

                            return userNumber;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting adopter number for user " + name;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return userNumber; ;
            }
        }

        /// <summary>
        /// Retrieve the adoption number associated with an animal number.
        /// </summary>
        /// <param name="animalNumber">The animal number of an animal.</param>
        /// <returns>Integer value containing the adoption number.</returns>
        public static int GetAdoptionNumberFromAnimalNumber(int animalNumber)
        {
            int adoptionNumber = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Adoption_Number " +
                        "FROM ADOPTION " +
                        "WHERE Animal_Number = @animalNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@animalNumber", animalNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                adoptionNumber = reader.GetInt32(0);

                            return adoptionNumber;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting adoption number.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return adoptionNumber;
            }
        }

        /// <summary>
        /// Retrieve the adoption number associated with an adopter number
        /// </summary>
        /// <param name="adopterName">Name of the adopter.</param>
        /// <returns>Integer value containing the adoption number.</returns>
        public static int GetAdoptionNumberFromAdopterName(string adopterName)
        {
            int adoptionNumber = -1;

            try
            {
                string adopterSurname = GetAdopterSurnameFromName(adopterName);
                int adopterNumber = GetAdopterNumberFromName(adopterName, adopterSurname);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Adoption_Number " +
                        "FROM ADOPTION " +
                        "WHERE Adopter_Number = @adopterNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@adopterNumber", adopterNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                adoptionNumber = reader.GetInt32(0);

                            return adoptionNumber;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting adoption number.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return adoptionNumber; ;
            }
        }

        /// <summary>
        /// Retrieve the user name and surname associated with an user number.
        /// </summary>
        /// <param name="adopterName">Name of the adopter.</param>
        /// <returns>Integer value containing the adoption number.</returns>
        public static string GetUserFullNameFromUserNumber(int userNumber)
        {
            string userFullName = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT User_Name, User_Surname " +
                        "FROM USERS " +
                        "WHERE User_Number = @userNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@userNumber", userNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                userFullName = reader[0].ToString() + " " + reader[1].ToString();

                            return userFullName;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting user full name for user number " + userNumber + ".";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return userFullName; ;
            }
        }

        /// <summary>
        /// Retrieve the user type number associated with a user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>User type number.</returns>
        public static byte GetUserTypeNumberFromUserName(string userName)
        {
            byte typeNumber = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT User_Type " +
                        "FROM USERS " +
                        "WHERE User_Name = @userName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@userName", userName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                typeNumber = reader.GetByte(0);
                            }
                        }

                        return typeNumber;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting type number for user " + userName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return typeNumber;
            }
        }

        public static string GetUserTypeNameFromUserTypeNumber(byte typeNumber)
        {
            string typeName = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Type_Description " +
                        "FROM USER_TYPE " +
                        "WHERE User_Type = @typeNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeNumber", typeNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                typeName = reader.GetString(0);
                            }
                        }

                        return typeName;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error getting type description for type number " + typeNumber;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return typeName;
            }
        }
    }
}
