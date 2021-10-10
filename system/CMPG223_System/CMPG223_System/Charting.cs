using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CMPG223_System
{
    class Charting
    {
        /*
         * Global runtime variables
         */
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=False";


        public static void PopulatePieChart(Chart crt, List<string> categories, List<int> values)
        {
            try
            {

            }
            catch (Exception ex)
            {
                string errorMessage = "Error populating the pie chart.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Get the animal type names.
        /// </summary>
        /// <returns>List containing the animal type names.</returns>
        public static List<string> GetAnimalTypes()
        {
            List<string> animalTypes = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = 
                        "SELECT Type_Name " +
                        "FROM ANIMAL_TYPE";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                animalTypes.Add(reader[0].ToString());
                        }

                        return animalTypes;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error retrieving animal types.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                return animalTypes;
            }
        }

        /// <summary>
        /// Get the number of animals per type.
        /// </summary>
        /// <returns>Dictionary containing the animal type name and number of animals of that type.</returns>
        public static Dictionary<string, int> GetNumberOfAnimalsPerType()
        {
            Dictionary<string, int> typeCount = new Dictionary<string, int>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = 
                        "SELECT Type_Name, COUNT(*) " +
                        "FROM ANIMAL a, ANIMAL_TYPE b " +
                        "WHERE a.Type_Number = b.Type_Number " +
                        "GROUP BY Type_Name";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                typeCount.Add(reader[0].ToString(), reader.GetInt32(1));
                            }
                        }

                        return typeCount;
                    }
                }   
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error retrieving animal types.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                // Error case
                return typeCount;
            }
        }

        public static Dictionary<DateTime, int> GetNumberOfAnimalsAddedPerTimePeriod()
        {
            Dictionary<DateTime, int> animalsAdded = new Dictionary<DateTime, int>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query =
                        "SELECT Add_Date, COUNT(*) " +
                        "FROM ANIMAL GROUP BY Add_Date";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                animalsAdded.Add(reader.GetDateTime(0), reader.GetInt32(1));
                            }
                        }

                        return animalsAdded;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error retrieving animals added dates.";
                MessageBox.Show(errorMessage + "\n\n" + ex.Message);

                // Error case
                return animalsAdded;
            }
        }
    }
}
