using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CMPG223_System
{
    class DBMethodsAnimalTypes
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";

        /*
         * Animal type manipulation methods
         */

        /// <summary>
        /// Add an animal type to the database.
        /// </summary>
        /// <param name="typeName">Name of the new type.</param>
        public static void AddAnimalType(string typeName)
        {
            try
            {
                // Check if the type already exists
                if (DBMethods.CheckExistence("ANIMAL_TYPE", "Type_Name", typeName))
                {
                    MessageBox.Show("Cannot add animal type" + typeName + ". The animal type already exists.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "INSERT INTO ANIMAL_TYPE (Type_Name) " +
                        "VALUES (@typeName)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeName", typeName);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Animal type added.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error adding animal type " + typeName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Modify an animal type in the databse.
        /// </summary>
        /// <param name="oldTypeName">Old type name.</param>
        /// <param name="newTypeName">New type name.</param>
        public static void ModifyAnimalType(string oldTypeName, string newTypeName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "UPDATE ANIMAL_TYPE " +
                        "SET Type_Name = @newTypeName " +
                        "WHERE Type_Name = @oldTypeName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@newTypeName", newTypeName);
                        cmd.Parameters.AddWithValue("@oldTypeName", oldTypeName);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Animal type modified.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error modifying animal type " + oldTypeName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Delete an animal type from the database.
        /// </summary>
        /// <param name="typeName">Name of the animal type.</param>
        public static void DeleteAnimalType(string typeName)
        {
            try
            {
                // Confirm deletion
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this animal type?", "Confirm deletion", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.No)
                    return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "DELETE FROM ANIMAL_TYPE " +
                        "WHERE Type_Name = @typeName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@typeName", typeName);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Animal type deleted.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error deleting animal type " + typeName;
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }
    }
}
