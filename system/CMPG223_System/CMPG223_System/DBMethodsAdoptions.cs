using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsAdoptions
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Adoption manipulation methods
         */

        /// <summary>
        /// Add an adoption to the database.
        /// </summary>
        /// <param name="adopterName">Adopter name.</param>
        /// <param name="animalName">Animal name.</param>
        /// <param name="resultDescription">Adoption result description.</param>
        public static void AddAdoption(string adopterName, string animalName, string resultDescription)
        {
            try
            {
                string adopterSurname = DBMethods.GetAdopterSurnameFromName(adopterName);
                int adopterNumber = DBMethods.GetAdopterNumberFromName(adopterName, adopterSurname);
                int animalNumber = DBMethods.GetAnimalNumberFromAnimalName(animalName);
                int userNumber = DBMethods.GetUserNumberFromUserName("admin", "admin");
                DateTime adoptionDate = DateTime.Today;


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "INSERT INTO ADOPTION (Adopter_Number, Animal_Number, User_Number, Result_Description, Adoption_Date) " +
                        "VALUES (@adopterNumber, @animalNumber, @userNumber, @resultDescription, @adoptionDate)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@adopterNumber", adopterNumber);
                        cmd.Parameters.AddWithValue("@animalNumber", animalNumber);
                        cmd.Parameters.AddWithValue("@userNumber", userNumber);
                        cmd.Parameters.AddWithValue("@resultDescription", resultDescription);
                        cmd.Parameters.AddWithValue("@adoptionDate", adoptionDate);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Added adoption.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error adding adoption";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Modify an adoption in the database.
        /// </summary>
        /// <param name="adopterName">Adopter name.</param>
        /// <param name="animalName">Animal name.</param>
        /// <param name="resultDescription">Adoption result description.</param>
        public static void ModifyAdoption(string adopterName, string animalName, string resultDescription)
        {
            try
            {
                string adopterSurname = DBMethods.GetAdopterSurnameFromName(adopterName);
                int adoptionNumber = DBMethods.GetAdoptionNumberFromAdopterName(adopterName);
                int adopterNumber = DBMethods.GetAdopterNumberFromName(adopterName, adopterSurname);
                int animalNumber = DBMethods.GetAnimalNumberFromAnimalName(animalName);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "UPDATE ADOPTION " +
                        "SET Adopter_Number = @adopterNumber, Animal_Number = @animalNumber, Result_Description = @resultDescription " +
                        "WHERE Adoption_Number = @adoptionNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@adopterNumber", adopterNumber);
                        cmd.Parameters.AddWithValue("@animalNumber", animalNumber);
                        cmd.Parameters.AddWithValue("@resultDescription", resultDescription);
                        cmd.Parameters.AddWithValue("@adoptionNumber", adoptionNumber);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Modified adoption.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error modifying adoption.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Delete an adoption from the database.
        /// </summary>
        /// <param name="animalNumber">Animal number.</param>
        public static void DeleteAdoption(int adoptionNumber)
        {
            try
            {
                // Confirm deletion
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this adoption?", "Confirm deletion", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.No)
                    return;

                //int adoptionNumber = DBMethods.GetAdoptionNumberFromAnimalNumber(animalNumber);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "DELETE FROM ADOPTION " +
                        "WHERE Adoption_Number = @adoptionNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@adoptionNumber", adoptionNumber);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Deleted adoption.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error deleting adoption. ";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
