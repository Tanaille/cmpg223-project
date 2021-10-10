using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsAnimals
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Animal manipulation methods
         */

        /// <summary>
        /// Add an animal to the database.
        /// </summary>
        /// <param name="animalName">Animal name.</param>
        /// <param name="animalType">Animal type name.</param>
        public static void AddAnimal(string animalName, string animalType)
        {
            try
            {
                // Check if the animal already exists
                if (DBMethods.CheckExistence("ANIMAL", "Animal_Name", animalName))
                {
                    MessageBox.Show("Cannot add animal " + animalName + ". The animal already exists.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    int animalTypeNumber = DBMethods.GetAnimalTypeNumberFromTypeName(animalType);
                    DateTime addDate = DateTime.Today;

                    string query = 
                        "INSERT INTO ANIMAL (Animal_Name, Type_Number, Add_Date) " +
                        "VALUES (@animalName, @animalTypeNumber, @addDate)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@animalName", animalName);
                        cmd.Parameters.AddWithValue("@animalTypeNumber", animalTypeNumber);
                        cmd.Parameters.AddWithValue("@addDate", addDate);
                        
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Animal added.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error adding animal " + animalName + " to the database.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Modify an animal type in the database.
        /// </summary>
        /// <param name="oldName">Old name of the animal.</param>
        /// <param name="newName">New name of the animal.</param>
        /// <param name="animalType">New type name of the animal.</param>
        public static void ModifyAnimal(string oldName, string newName, string animalType)
        {
            try
            {
                // Check if the new animal name already exists
                if (DBMethods.CheckExistence("ANIMAL", "Animal_Name", newName))
                {
                    MessageBox.Show("Cannot change animal name to " + newName + ". The animal name already exists.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    int oldNumber = DBMethods.GetAnimalNumberFromAnimalName(oldName);
                    int newNumber = DBMethods.GetAnimalTypeNumberFromTypeName(animalType);

                    string query =
                        "UPDATE ANIMAL " +
                        "SET Animal_Name = @newName, Type_Number = @newNumber " +
                        "WHERE Animal_Number = @oldNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@newName", newName);
                        cmd.Parameters.AddWithValue("@newNumber", newNumber);
                        cmd.Parameters.AddWithValue("@oldNumber", oldNumber);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Animal modified.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error modifying animal " + oldName + ".";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Delete an animal from the database.
        /// </summary>
        /// <param name="name">Name of the animal to be deleted.</param>
        public static void DeleteAnimal(string name)
        {
            try
            {
                // Confirm deletion
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this animal?", "Confirm deletion", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.No)
                    return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "DELETE FROM ANIMAL " +
                        "WHERE Animal_Name = @name";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error deleting animal " + name + ".";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
