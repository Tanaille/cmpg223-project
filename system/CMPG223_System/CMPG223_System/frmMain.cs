using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CMPG223_System
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Initialize the dashboard elements
            string[] userTypeNullsTables = { "USERS" };
            string[] userTypeNullsColumns = { "User_Name", "User_Surname" };

            DBMethodsNulls.DisplayNulls(lstDashboardRIUsers, userTypeNullsTables, userTypeNullsColumns, "User_Type", "", ", ", 1);

            string[] adopterNullsTables = { "ADOPTER" };
            string[] adopterNullsColumns = { "Adopter_Surname", "Adopter_Name" };

            DBMethodsNulls.DisplayNulls(lstDashboardRIAdopters, adopterNullsTables, adopterNullsColumns, "City_ID", "", ", ", 1);

            string[] animalNullsTables = { "ANIMAL" };
            string[] animalNullsColumns = { "Animal_Name" };
            DBMethodsNulls.DisplayNulls(lstDashboardRIAnimals, animalNullsTables, animalNullsColumns, "Type_Number", "", "", 1);

            // Display the animal summary chart
            Dictionary<string, int> typeCount = Charting.GetNumberOfAnimalsPerType();

            for (int i = 0; i < typeCount.Count; i++)
            {
                crtDashboardAnimalTypesSummary.Series["AnimalsPerType"].Points.AddXY(typeCount.ElementAt(i).Key, typeCount.ElementAt(i).Value);
            }

            lblDashboardDate.Text = DateTime.Today.ToShortDateString();

            // Disable the admin panel for non-admin users
            string currentUserType = DBMethods.GetUserTypeNameFromUserTypeNumber(DBMethods.GetUserTypeNumberFromUserName(GlobalVariables.currentUser));

            if (!(currentUserType == "Administrator"))
                btnAdminMain.Enabled = false;
        }

        /* --------------------------------
         * Sidebar menu button methods.
         * --------------------------------
         */

        /// <summary>
        /// Open the Dashboard tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpDashboard;
        }

        /// <summary>
        /// Open the Animals Main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalsMain_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAnimalsMain;
        }

        /// <summary>
        /// Open the Adoptions Main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptionsMain_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdoptionsMain;
        }

        /// <summary>
        /// Open the Adopters Main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptersMain_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdoptersMain;
        }

        /// <summary>
        /// Open the Admin Main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminMain_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdminMain;
        }

        /// <summary>
        /// Open the Reporting Main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReportingMain_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpReportingMain;
        }

        /// <summary>
        /// Close the application when the main form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }




        /* --------------------------------
         * Animal event handlers
         * --------------------------------
         */

        /// <summary>
        /// Initializes controls on the animals main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAnimalsMain_Enter(object sender, EventArgs e)
        {
            string columns = "a.Animal_Number, a.Animal_Name, b.Type_Name, a.Add_Date";
            string[] tables = { "ANIMAL", "ANIMAL_TYPE" };
            DBMethodsPopulateControls.PopulateDataGridView(dgvAnimalsMainAnimals, tables, columns, 2, "Type_Number");

            // Rename column headers
            dgvAnimalsMainAnimals.Columns[0].HeaderText = "Number";
            dgvAnimalsMainAnimals.Columns[1].HeaderText = "Name";
            dgvAnimalsMainAnimals.Columns[2].HeaderText = "Type";
            dgvAnimalsMainAnimals.Columns[3].HeaderText = "Date added";
        }

        /// <summary>
        /// Open the Add Animal tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalMainAdd_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAnimalsAdd;
        }

        /// <summary>
        /// Open the Modify Animal tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalMainModify_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAnimalsModify;
        }

        /// <summary>
        /// Open the Delete Animal tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalMainDelete_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAnimalsDelete;
        }

        /// <summary>
        /// Open the Animal Types tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAnimalTypesMain;
        }

        /// <summary>
        /// Add a new animal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalsAddAdd_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAnimalsAddErrorAnimalName.Visible = false;
            lblAnimalsAddErrorType.Visible = false;

            // Validate input
            if (txtAnimalsAddName.Text == string.Empty)
            {
                lblAnimalsAddErrorAnimalName.Visible = true;
                return;
            }

            if (cbxAnimalsAddAnimalType.SelectedIndex == -1)
            {
                lblAnimalsAddErrorType.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsAnimals.AddAnimal(txtAnimalsAddName.Text, cbxAnimalsAddAnimalType.SelectedItem.ToString());

            // Clear input controls
            txtAnimalsAddName.Clear();
            cbxAnimalsAddAnimalType.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAnimalsMain;
        }

        /// <summary>
        /// Modify the selected animal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalsAddModify_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAnimalsModifyErrorNewName.Visible = false;
            lblAnimalsModifyErrorOldName.Visible = false;
            lblAnimalsModifyErrorType.Visible = false;

            // Validate input

            if (lstAnimalsModifyNames.SelectedIndex == -1)
            {
                lblAnimalsModifyErrorOldName.Visible = true;
                return;
            }

            if (txtAnimalsModifyNewName.Text == string.Empty)
            {
                lblAnimalsModifyErrorNewName.Visible = true;
                return;
            }

            if (cbxAnimalsModifyType.SelectedIndex == -1)
            {
                lblAnimalsModifyErrorType.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsAnimals.ModifyAnimal(lstAnimalsModifyNames.SelectedItem.ToString(), txtAnimalsModifyNewName.Text, cbxAnimalsModifyType.SelectedItem.ToString());

            // Clear input controls
            txtAnimalsModifyNewName.Clear();
            txtAnimalsModifySearch.Clear();
            cbxAnimalsModifyType.SelectedIndex = -1;
            lstAnimalsModifyNames.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAnimalsMain;
        }

        /// <summary>
        /// Delete the selected animal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalsDeleteDelete_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAnimalsDeleteErrorName.Visible = false;

            // Validate input
            if (lstAnimalsDeleteAnimals.SelectedIndex == -1)
            {
                lblAnimalsDeleteErrorName.Visible = true;
                return;
            }

            // Execute methods
            string[] columns = { "Animal_Name" };

            DBMethodsAnimals.DeleteAnimal(lstAnimalsDeleteAnimals.SelectedItem.ToString());
            DBMethodsPopulateControls.PopulateListBox(lstAnimalsDeleteAnimals, "ANIMAL", columns, 1, "");

            // Clear input controls
            txtAnimalsDeleteSearch.Clear();
            lstAnimalsDeleteAnimals.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAnimalsMain;
        }

        /// <summary>
        /// Populate the animals add ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAnimalsAdd_Enter(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.PopulateComboBox(cbxAnimalsAddAnimalType, "ANIMAL_TYPE", "Type_Name");
        }

        /// <summary>
        /// Populate animals modify ListBox and ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAnimalsModify_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Animal_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAnimalsModifyNames, "ANIMAL", columns, 1, "");
            DBMethodsPopulateControls.PopulateComboBox(cbxAnimalsModifyType, "ANIMAL_TYPE", "Type_Name");
        }

        /// <summary>
        /// Populate the animals delete ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAnimalsDelete_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Animal_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAnimalsDeleteAnimals, "ANIMAL", columns, 1, "");
        }

        /// <summary>
        /// Filter the animals modify ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAnimalsModifyName_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAnimalsModifyNames, "ANIMAL", "Animal_Name", txtAnimalsModifySearch.Text);
        }

        /// <summary>
        /// Filter the animals delete ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAnimalsDeleteSearch_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAnimalsDeleteAnimals, "ANIMAL", "Animal_Name", txtAnimalsDeleteSearch.Text);
        }




        /* --------------------------------
         * Animal types event handlers
         * --------------------------------
         */

        /// <summary>
        /// Initialize controls on the animal types main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAnimalTypesMain_Enter(object sender, EventArgs e)
        {
            string[] tables = { "ANIMAL_TYPE" };
            string columns = "Type_Number, Type_Name";
            DBMethodsPopulateControls.PopulateDataGridView(dgvAnimalTypesMainAnimalTypes, tables, columns, 1, string.Empty);

            // Rename column headers
            dgvAnimalTypesMainAnimalTypes.Columns[0].HeaderText = "Type number";
            dgvAnimalTypesMainAnimalTypes.Columns[1].HeaderText = "Type name";

        }

        /// <summary>
        /// Open the Add Animal Type tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalTypesMainAdd_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAnimalTypesAdd;
        }

        /// <summary>
        /// Open the Modify Animal Type tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalTypesMainModify_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAnimalTypesModify;
        }

        /// <summary>
        /// Open the Delete Animal Type tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalTypesMainDelete_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAnimalTypesDelete;
        }

        /// <summary>
        /// Add a new animal type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalTypesAddAdd_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAnimalTypesAddErrorName.Visible = false;

            // Validate input
            if (txtAnimalTypeAddTypeName.Text == string.Empty)
            {
                lblAnimalTypesAddErrorName.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsAnimalTypes.AddAnimalType(txtAnimalTypeAddTypeName.Text);

            // Clear input controls
            txtAnimalTypeAddTypeName.Clear();

            // Return to submenu
            tbcMain.SelectedTab = tbpAnimalTypesMain;
        }

        /// <summary>
        /// Modify the selected animal type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalTypesModifyModify_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAnimalTypesModifyErrorOldName.Visible = false;
            lblAnimalTypesModifyErrorNewName.Visible = false;

            // Validate input
            if (lstAnimalTypesModifyNames.SelectedIndex == -1)
            {
                lblAnimalTypesModifyErrorOldName.Visible = true;
                return;
            }

            if (txtAnimalTypesModifyName.Text == string.Empty)
            {
                lblAnimalTypesModifyErrorNewName.Visible = true;
                return;
            }

            // Execute methods
            string[] columns = { "Type_Name" };

            DBMethodsAnimalTypes.ModifyAnimalType(lstAnimalTypesModifyNames.SelectedItem.ToString(), txtAnimalTypesModifyName.Text);
            DBMethodsPopulateControls.PopulateListBox(lstAnimalTypesModifyNames, "ANIMAL_TYPE", columns, 1, "");

            // Clear input controls
            txtAnimalTypesModifyName.Clear();
            txtAnimalTypesModifySearch.Clear();
            lstAnimalTypesModifyNames.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAnimalTypesMain;
        }

        /// <summary>
        /// Delete the selected animal type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnimalTypesDeleteDelete_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAnimalTypesDeleteErrorOldName.Visible = false;

            // Validate input
            if (lstAnimalTypesDeleteNames.SelectedIndex == -1)
            {
                lblAnimalTypesDeleteErrorOldName.Visible = true;
                return;
            }

            // Execute methods
            string[] columns = { "Type_Name" };

            DBMethodsAnimalTypes.DeleteAnimalType(lstAnimalTypesDeleteNames.SelectedItem.ToString());
            DBMethodsPopulateControls.PopulateListBox(lstAnimalTypesDeleteNames, "ANIMAL_TYPE", columns, 1, "");

            // Clear input controls
            txtAnimalTypesDeleteSearch.Clear();
            lstAnimalTypesDeleteNames.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAnimalTypesMain;
        }

        /// <summary>
        /// Populate the animal types modify ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAnimalTypesModify_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Type_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAnimalTypesModifyNames, "ANIMAL_TYPE", columns, 1, "");
        }

        /// <summary>
        /// Populate the animal types delete ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAnimalTypesDelete_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Type_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAnimalTypesDeleteNames, "ANIMAL_TYPE", columns, 1, "");
        }

        /// <summary>
        /// Filter the animal types modify ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAnimalTypesModifySearch_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAnimalTypesModifyNames, "ANIMAL_TYPE", "Type_Name", txtAnimalTypesModifySearch.Text);
        }

        /// <summary>
        /// Filter the animal type delete ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAnimalTypesDeleteSearch_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAnimalTypesDeleteNames, "ANIMAL_TYPE", "Type_Name", txtAnimalTypesDeleteSearch.Text);
        }




        /* --------------------------------
         * Cities event handlers
         * --------------------------------
         */

        /// <summary>
        /// Add a new city.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCitiesAdd_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblCitiesErrorCitiesDelete.Visible = false;
            lblCitiesErrorName.Visible = false;

            // Validate input
            if (txtCitiesName.Text == string.Empty)
            {
                lblCitiesErrorName.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsCities.AddCity(txtCitiesName.Text);

            // Clear input controls
            txtCitiesName.Clear();
            lstCitiesNames.SelectedItem = -1;

            // Repopulate the cities ListBox
            string[] columns = { "City_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstCitiesNames, "CITY", columns, 1, "");

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptersMain;
        }

        /// <summary>
        /// Modify a city.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCitiesModify_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblCitiesErrorCitiesDelete.Visible = false;
            lblCitiesErrorName.Visible = false;

            // Validate input
            if (lstCitiesNames.SelectedIndex == -1)
            {
                lblCitiesErrorCitiesDelete.Text = "Please select a city to modify";
                lblCitiesErrorCitiesDelete.Visible = true;
                return;
            }

            if (txtCitiesName.Text == string.Empty)
            {
                lblCitiesErrorName.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsCities.ModifyCity(lstCitiesNames.SelectedItem.ToString(), txtCitiesName.Text);

            // Clear input controls
            txtCitiesName.Clear();
            lstCitiesNames.SelectedIndex = -1;

            // Repopulate the cities ListBox
            string[] columns = { "City_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstCitiesNames, "CITY", columns, 1, "");

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptersMain;
        }

        /// <summary>
        /// Delete a city.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCitiesDelete_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblCitiesErrorCitiesDelete.Visible = false;
            lblCitiesErrorName.Visible = false;

            if (lstCitiesNames.SelectedIndex == -1)
            {
                lblCitiesErrorCitiesDelete.Text = "Pleas select a city to delete";
                lblCitiesErrorCitiesDelete.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsCities.DeleteCity(lstCitiesNames.SelectedItem.ToString());

            // Clear input controls
            txtCitiesName.Clear();
            lstCitiesNames.SelectedIndex = -1;

            // Repopulate the cities ListBox
            string[] columns = { "City_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstCitiesNames, "CITY", columns, 1, "");

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptersMain;
        }

        /// <summary>
        /// Populate the cities ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpCities_Enter(object sender, EventArgs e)
        {
            string[] columns = { "City_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstCitiesNames, "CITY", columns, 1, "");
        }




        /* --------------------------------
         * Adopters event handlers
         * --------------------------------
         */

        /// <summary>
        /// Initialize controls on the adopters main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdoptersMain_Enter(object sender, EventArgs e)
        {
            string[] tables = { "ADOPTER" };
            string columns = "Adopter_Number, Adopter_Name, Adopter_Surname, " +
                "Adopter_Contact_Number, Adopter_Street_Number, Adopter_Street_Name";

            DBMethodsPopulateControls.PopulateDataGridView(dgvAdoptersMain, tables, columns, 1, string.Empty);

            dgvAdoptersMain.Columns[0].HeaderText = "Adopter ID";
            dgvAdoptersMain.Columns[1].HeaderText = "Name";
            dgvAdoptersMain.Columns[2].HeaderText = "Surname";
            dgvAdoptersMain.Columns[3].HeaderText = "Contact Number";
            dgvAdoptersMain.Columns[4].HeaderText = "Street Number";
            dgvAdoptersMain.Columns[5].HeaderText = "Street Name";
        }

        /// <summary>
        /// Open the Adopters Add tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptersMainAdd_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdoptersAdd;
        }

        /// <summary>
        /// Open the Adopters Modify tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptersMainModify_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdoptersModify;
        }

        /// <summary>
        /// Open the Adopters Delete tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptersMainDelete_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdoptersDelete;
        }

        /// <summary>
        /// Open the Cities main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptersCities_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpCities;
        }

        /// <summary>
        /// Add a new adopter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptersAddAdd_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdoptersAddErrorName.Visible = false;
            lblAdoptersAddErrorSurname.Visible = false;
            lblAdoptersAddErrorContactNumber.Visible = false;
            lblAdoptersAddErrorStreetNumber.Visible = false;
            lblAdoptersAddErrorStreetName.Visible = false;
            lblAdoptersAddErrorCity.Visible = false;

            // Validate input
            if (txtAdoptersAddName.Text == string.Empty)
            {
                lblAdoptersAddErrorName.Visible = true;
                return;
            }

            if (txtAdoptersAddSurname.Text == string.Empty)
            {
                lblAdoptersAddErrorSurname.Visible = true;
                return;
            }

            if (txtAdoptersAddContactNumber.Text == string.Empty)
            {
                lblAdoptersAddErrorContactNumber.Visible = true;
                return;
            }

            if (txtAdoptersAddStreetNumber.Text == string.Empty)
            {
                lblAdoptersAddErrorStreetNumber.Visible = true;
                return;
            }

            if (txtAdoptersAddStreetName.Text == string.Empty)
            {
                lblAdoptersAddErrorStreetName.Visible = true;
                return;
            }

            if (cbxAdoptersAddCity.SelectedIndex == -1)
            {
                lblAdoptersAddErrorCity.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsAdopters.AddAdopter(
                txtAdoptersAddName.Text, txtAdoptersAddSurname.Text, txtAdoptersAddContactNumber.Text,
                txtAdoptersAddStreetNumber.Text, txtAdoptersAddStreetName.Text, cbxAdoptersAddCity.SelectedItem.ToString());

            // Clear input controls
            txtAdoptersAddName.Clear();
            txtAdoptersAddSurname.Clear();
            txtAdoptersAddContactNumber.Clear();
            txtAdoptersAddStreetNumber.Clear();
            txtAdoptersAddStreetName.Clear();
            cbxAdoptersAddCity.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptersMain;
        }

        /// <summary>
        /// Modify an adopter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptersModifyModify_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdoptersModifyErrorName.Visible = false;
            lblAdoptersModifyErrorSurname.Visible = false;
            lblAdoptersModifyErrorContactNumber.Visible = false;
            lblAdoptersModifyErrorStreetNumber.Visible = false;
            lblAdoptersModifyErrorStreetName.Visible = false;
            lblAdoptersModifyErrorCity.Visible = false;
            lblAdoptersModifyErrorAdopter.Visible = false;

            // Validate input
            if (lstAdoptersModifyNames.SelectedIndex == -1)
            {
                lblAdoptersModifyErrorAdopter.Visible = true;
                return;
            }

            if (txtAdoptersModifyName.Text == string.Empty)
            {
                lblAdoptersModifyErrorName.Visible = true;
                return;
            }

            if (txtAdoptersModifySurname.Text == string.Empty)
            {
                lblAdoptersModifyErrorSurname.Visible = true;
                return;
            }

            if (txtAdoptersModifyContactNumber.Text == string.Empty)
            {
                lblAdoptersModifyErrorContactNumber.Visible = true;
                return;
            }

            if (txtAdoptersModifyStreetNumber.Text == string.Empty)
            {
                lblAdoptersModifyErrorStreetNumber.Visible = true;
                return;
            }

            if (txtAdoptersModifyStreetName.Text == string.Empty)
            {
                lblAdoptersModifyErrorStreetName.Visible = true;
                return;
            }

            if (cbxAdoptersModifyCity.SelectedIndex == -1)
            {
                lblAdoptersModifyErrorCity.Visible = true;
                return;
            }

            // Execute methods
            string oldName = lstAdoptersModifyNames.SelectedItem.ToString();
            int index = oldName.IndexOf(" ");
            if (index >= 0)
                oldName = oldName.Substring(index + 1);

            DBMethodsAdopters.ModifyAdopter(
                oldName, txtAdoptersModifyName.Text, txtAdoptersModifySurname.Text, txtAdoptersModifyContactNumber.Text,
                txtAdoptersModifyStreetNumber.Text, txtAdoptersModifyStreetName.Text, cbxAdoptersModifyCity.SelectedItem.ToString());

            // Clear input controls
            txtAdoptersModifyName.Clear();
            txtAdoptersModifySurname.Clear();
            txtAdoptersModifyContactNumber.Clear();
            txtAdoptersModifyStreetNumber.Clear();
            txtAdoptersModifyStreetName.Clear();
            cbxAdoptersModifyCity.SelectedIndex = -1;
            lstAdoptersModifyNames.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptersMain;
        }

        /// <summary>
        /// Delete an adopter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptersDeleteDelete_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdoptersDeleteErrorAdopter.Visible = false;

            // Validate input
            if (lstAdoptersDeleteNames.SelectedIndex == -1)
            {
                lblAdoptersDeleteErrorAdopter.Visible = true;
                return;
            }

            // Execute methods
            string oldName = lstAdoptersDeleteNames.SelectedItem.ToString();
            int index = oldName.IndexOf(" ");
            if (index >= 0)
                oldName = oldName.Substring(index + 1);

            DBMethodsAdopters.DeleteAdopter(oldName);

            // Clear input controls
            txtAdoptersDeleteSearch.Clear();
            lstAdoptersDeleteNames.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptersMain;
        }

        /// <summary>
        /// Populate the cities ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdoptersAdd_Enter(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.PopulateComboBox(cbxAdoptersAddCity, "CITY", "CITY_NAME");
        }

        /// <summary>
        /// Populate the adopters modify ListBox and ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdoptersModify_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Adopter_Surname", "Adopter_Name" };

            DBMethodsPopulateControls.PopulateComboBox(cbxAdoptersModifyCity, "CITY", "City_Name");
            DBMethodsPopulateControls.PopulateListBox(lstAdoptersModifyNames, "ADOPTER", columns, 2, ", ");
        }

        /// <summary>
        /// Populate the adopters delete ListBox
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdoptersDelete_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Adopter_Surname", "Adopter_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAdoptersDeleteNames, "ADOPTER", columns, 2, ", ");
        }

        /// <summary>
        /// Filter the adopters delete ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdoptersDeleteSearch_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAdoptersDeleteNames, "ADOPTER", "Adopter_Name", txtAdoptersDeleteSearch.Text);
        }




        /* --------------------------------
         * User type event handlers
         * --------------------------------
         */

        /// <summary>
        /// Add a new user type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUserTypesAdd_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblUserTypesErrorName.Visible = false;
            lblUserTypesErrorType.Visible = false;

            // Validate input
            if (txtUserTypesType.Text == string.Empty)
            {
                lblUserTypesErrorName.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsUserTypes.AddUserType(txtUserTypesType.Text);

            // Clear input controls
            txtUserTypesType.Clear();
            lstUserTypesTypes.SelectedItem = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdminMain;
        }

        /// <summary>
        /// Delete a user type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUserTypesDelete_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblUserTypesErrorName.Visible = false;
            lblUserTypesErrorType.Visible = false;

            // Validate input
            if (lstUserTypesTypes.SelectedIndex == -1)
            {
                lblUserTypesErrorType.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsUserTypes.DeleteUserType(lstUserTypesTypes.SelectedItem.ToString());

            // Clear input controls
            txtUserTypesType.Clear();
            lstUserTypesTypes.SelectedItem = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdminMain;
        }

        /// <summary>
        /// Populate the user types ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpUserTypes_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Type_Description" };

            DBMethodsPopulateControls.PopulateListBox(lstUserTypesTypes, "USER_TYPE", columns, 1, "");
        }




        /* --------------------------------
         * Users event handlers
         * --------------------------------
         */

        /// <summary>
        /// Initialize controls on the admin main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdminMain_Enter(object sender, EventArgs e)
        {
            string[] tables = { "USERS" };
            string columns = "User_Number, User_Name, User_Surname, User_Type";
            DBMethodsPopulateControls.PopulateDataGridView(dgvAdminMain, tables, columns, 1, string.Empty);

            dgvAdminMain.Columns[0].HeaderText = "User Number";
            dgvAdminMain.Columns[1].HeaderText = "User Name";
            dgvAdminMain.Columns[2].HeaderText = "User Surname";
            dgvAdminMain.Columns[3].HeaderText = "User Role";
        }

        /// <summary>
        /// Open the Admin Add tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminMainAdd_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdminAdd;
        }

        /// <summary>
        /// Open the Admin Modify tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminMainModify_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdminModify;
        }

        /// <summary>
        /// Open the Admin Delete tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminMainDelete_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdminDelete;
        }

        /// <summary>
        /// Open the User Types tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminMainUserTypes_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpUserTypes;
        }

        /// <summary>
        /// Add a new user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminAddAdd_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdminAddErrorName.Visible = false;
            lblAdminAddErrorSurname.Visible = false;
            lblAdminAddErrorPassword.Visible = false;
            lblAdminAddErrorRole.Visible = false;

            // Validate input
            if (txtAdminAddUserName.Text == string.Empty)
            {
                lblAdminAddErrorName.Visible = true;
                return;
            }

            if (txtAdminAddUserSurname.Text == string.Empty)
            {
                lblAdminAddErrorSurname.Visible = true;
                return;
            }

            if (txtAdminAddPassword.Text == string.Empty)
            {
                lblAdminAddErrorPassword.Visible = true;
                return;
            }

            if (cbxAdminAddRole.SelectedIndex == -1)
            {
                lblAdminAddErrorRole.Visible = true;
                return;
            }

            // Execute methods
            DBMethodsUsers.AddUser(txtAdminAddUserName.Text, txtAdminAddUserSurname.Text, txtAdminAddPassword.Text, cbxAdminAddRole.SelectedItem.ToString());

            // Clear input controls
            txtAdminAddUserName.Clear();
            txtAdminAddUserSurname.Clear();
            txtAdminAddPassword.Clear();
            cbxAdminAddRole.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdminMain;
        }

        /// <summary>
        /// Modify a user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminModifyModify_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdminModifyErrorName.Visible = false;
            lblAdminModifyErrorSurname.Visible = false;
            lblAdminModifyErrorPassword.Visible = false;
            lblAdminModifyErrorRole.Visible = false;
            lblAdminModifyErrorUsers.Visible = false;

            // Validate input
            if (txtAdminModifyUserName.Text == string.Empty)
            {
                lblAdminModifyErrorName.Visible = true;
                return;
            }

            if (txtAdminModifyUserSurname.Text == string.Empty)
            {
                lblAdminModifyErrorSurname.Visible = true;
                return;
            }

            if (txtAdminModifyPassword.Text == string.Empty)
            {
                lblAdminModifyErrorPassword.Visible = true;
                return;
            }

            if (cbxAdminModifyRole.SelectedIndex == -1)
            {
                lblAdminModifyErrorRole.Visible = true;
                return;
            }

            if (lstAdminModifyName.SelectedIndex == -1)
            {
                lblAdminModifyErrorUsers.Visible = true;
                return;
            }

            // Execute methods
            string oldName = lstAdminModifyName.SelectedItem.ToString();
            int index = oldName.IndexOf(" ");
            if (index >= 0)
                oldName = oldName.Substring(index + 1);

            DBMethodsUsers.ModifyUser(txtAdminModifyUserName.Text, oldName, txtAdminModifyUserSurname.Text, txtAdminModifyPassword.Text, cbxAdminModifyRole.SelectedItem.ToString());

            // Clear input controls
            txtAdminModifyUserName.Clear();
            txtAdminModifyUserSurname.Clear();
            txtAdminModifyPassword.Clear();
            cbxAdminModifyRole.SelectedIndex = -1;
            lstAdminModifyName.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdminMain;
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminDeleteDelete_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdminDeleteErrorUser.Visible = false;

            // Validate input
            if (lstAdminDeleteNames.SelectedIndex == -1)
            {
                lblAdminDeleteErrorUser.Visible = true;
                return;
            }

            // Execute methods
            string oldName = lstAdminDeleteNames.SelectedItem.ToString();
            int index = oldName.IndexOf(" ");
            if (index >= 0)
                oldName = oldName.Substring(index + 1);

            DBMethodsUsers.DeleteUser(oldName);

            // Clear input controls
            txtAdminDeleteSearch.Clear();
            lstAdminDeleteNames.SelectedItem = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdminMain;
        }

        /// <summary>
        /// Populate the admin add ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdminAdd_Enter(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.PopulateComboBox(cbxAdminAddRole, "USER_TYPE", "Type_Description");
        }

        /// <summary>
        /// Populate the admin modify ListBox and ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdminModify_Enter(object sender, EventArgs e)
        {
            string[] columns = { "User_Surname",  "User_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAdminModifyName, "USERS", columns, 2, ", ");
            DBMethodsPopulateControls.PopulateComboBox(cbxAdminModifyRole, "USER_TYPE", "Type_Description");
        }

        /// <summary>
        /// Populate the admin delete ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdminDelete_Enter(object sender, EventArgs e)
        {
            string[] columns = { "User_Surname", "User_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAdminDeleteNames, "USERS", columns, 2, ", ");
        }

        /// <summary>
        /// Filter the admin delete TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdminDeleteSearch_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAdminDeleteNames, "USERS", "User_Name", txtAdminDeleteSearch.Text);
        }




        /* --------------------------------
         * Adoptions event handlers
         * --------------------------------
         */

        /// <summary>
        /// Initialize controls on the adoptions main tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdoptionsMain_Enter(object sender, EventArgs e)
        {
            string[] tables = { "ADOPTION", "ADOPTER", "USER", "ANIMAL"};
            string columns = "a.Adoption_Number, b.Adopter_Name, d.Animal_Name, c.User_Name, a.Result_Description, a.Adoption_Date";
            DBMethodsPopulateControls.PopulateDataGridView(dgvAdoptionsMain, tables, columns, 4, "Adopter_Number");

            dgvAdoptionsMain.Columns[0].HeaderText = "Adoption number";
            dgvAdoptionsMain.Columns[1].HeaderText = "Adopter number";
            dgvAdoptionsMain.Columns[2].HeaderText = "Animal number";
            dgvAdoptionsMain.Columns[3].HeaderText = "User name";
            dgvAdoptionsMain.Columns[4].HeaderText = "Result";
            dgvAdoptionsMain.Columns[5].HeaderText = "Adoption date";
        }

        /// <summary>
        /// Open the Adoptions Add tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptionsMainAdd_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdoptionsAdd;
        }

        /// <summary>
        /// Open the Adoptions Modify tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptionsMainModify_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdoptionsModify;
        }

        /// <summary>
        /// Open the Adoptions Delete tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptionsMainDelete_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpAdoptionsDelete;
        }

        /// <summary>
        /// Add an adoption.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptionsAddAdd_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdoptionsAddErrorAdopters.Visible = false;
            lblAdoptionsAddErrorAnimals.Visible = false;
            lblAdoptionsAddErrorResult.Visible = false;

            // Validate input
            if (lstAdoptionsAddAdopters.SelectedIndex == -1)
            {
                lblAdoptionsAddErrorAdopters.Visible = true;
                return;
            }

            if (lstAdoptionsAddAnimals.SelectedIndex == -1)
            {
                lblAdoptionsAddErrorAnimals.Visible = true;
                return;
            }

            if (cbxAdoptionsAddResult.SelectedIndex == -1)
            {
                lblAdoptionsAddErrorResult.Visible = true;
                return;
            }

            // Execute methods
            string adopterName = lstAdoptionsAddAdopters.SelectedItem.ToString();
            int index = adopterName.IndexOf(" ");
            if (index >= 0)
                adopterName = adopterName.Substring(index + 1);

            MessageBox.Show(adopterName);

            DBMethodsAdoptions.AddAdoption(adopterName, lstAdoptionsAddAnimals.SelectedItem.ToString(), cbxAdoptionsAddResult.SelectedItem.ToString());

            // Clear input controls
            lstAdoptionsAddAdopters.SelectedIndex = -1;
            lstAdoptionsAddAnimals.SelectedIndex = -1;
            cbxAdoptionsAddResult.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptionsMain;
        }

        /// <summary>
        /// Modify an adoption.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptionsModifyModify_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdoptionsModifyErrorAdopters.Visible = false;
            lblAdoptionsModifyErrorAnimals.Visible = false;
            lblAdoptionsModifyErrorResult.Visible = false;
            
            // Validate input
            if (lstAdoptionsModifyAdopters.SelectedIndex == -1)
            {
                lblAdoptionsModifyErrorAdopters.Visible = true;
                return;
            }

            if (lstAdoptionsModifyAnimals.SelectedIndex == -1)
            {
                lblAdoptionsModifyErrorAnimals.Visible = true;
                return;
            }

            if (cbxAdoptionsModifyResult.SelectedIndex == -1)
            {
                lblAdoptionsModifyErrorResult.Visible = true;
                return;
            }

            // Execute methods
            string adopterName = lstAdoptionsAddAdopters.SelectedItem.ToString();
            int index = adopterName.IndexOf(" ");
            if (index >= 0)
                adopterName = adopterName.Substring(index + 1);

            DBMethodsAdoptions.ModifyAdoption(adopterName, lstAdoptionsModifyAnimals.SelectedItem.ToString(), cbxAdoptionsModifyResult.SelectedItem.ToString());

            // Clear input controls
            lstAdoptionsModifyAdopters.SelectedIndex = -1;
            lstAdoptionsModifyAnimals.SelectedIndex = -1;
            cbxAdoptionsModifyResult.SelectedIndex = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptionsMain;
        }

        /// <summary>
        /// Delete an adoption.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdoptionsDeleteDelete_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblAdoptionsDeleteErrorAdoption.Visible = false;

            // Validate input
            if (lstAdoptionsDeleteSearchAdoptions.SelectedIndex == -1)
            {
                lblAdoptionsDeleteErrorAdoption.Visible = true;
                return;
            }

            // Execute methods
            string aNumber = lstAdoptionsDeleteSearchAdoptions.SelectedItem.ToString();
            int index = aNumber.IndexOf(" ");
            if (index >= 0)
                aNumber = aNumber.Substring(0, index);

            int adoptionNumber = Int32.Parse(aNumber);

            DBMethodsAdoptions.DeleteAdoption(adoptionNumber);
            
            // Clear input controls
            txtAdoptionsDeleteSearch.Clear();
            lstAdoptionsDeleteSearchAdoptions.SelectedItem = -1;

            // Return to submenu
            tbcMain.SelectedTab = tbpAdoptionsMain;
        }

        /// <summary>
        /// Populate the adoptions add ListBoxes and ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdoptionsAdd_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Adopter_Surname", "Adopter_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAdoptionsAddAdopters, "ADOPTER", columns, 2, ", ");

            string[] columns2 = { "Animal_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAdoptionsAddAnimals, "ANIMAL", columns2, 1, "");

            cbxAdoptionsAddResult.Items.Add("Success");
            cbxAdoptionsAddResult.Items.Add("Failure");
            cbxAdoptionsAddResult.Items.Add("Hold");
        }

        /// <summary>
        /// Populate the adoptions modify ListBoxes and ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdoptionsModify_Enter(object sender, EventArgs e)
        {
            string[] columns = { "Adopter_Surname", "Adopter_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAdoptionsModifyAdopters, "ADOPTER", columns, 2, ", ");

            string[] columns2 = { "Animal_Name" };

            DBMethodsPopulateControls.PopulateListBox(lstAdoptionsModifyAnimals, "ANIMAL", columns2, 1, "");

            cbxAdoptionsModifyResult.Items.Add("Success");
            cbxAdoptionsModifyResult.Items.Add("Failure");
            cbxAdoptionsModifyResult.Items.Add("Hold");
        }

        /// <summary>
        /// Populate the adoptions delete ListBoxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpAdoptionsDelete_Enter(object sender, EventArgs e)
        {
            string[] columns = { "a.Adoption_Number", "b.Animal_Name" };
            string[] tables = { "ADOPTION", "ANIMAL" };


            DBMethodsPopulateControls.PopulateListBox(lstAdoptionsDeleteSearchAdoptions, tables, columns, "Animal_Number", 2, " - ");
        }

        /// <summary>
        /// Filter the adoptions add adopter ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdoptionsAddAdopter_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAdoptionsAddAdopters, "ADOPTER", "Adopter_Name", txtAdoptionsAddAdopter.Text);
        }

        /// <summary>
        /// Filter the adoptions add animal ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdoptionsAddAnimal_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAdoptionsAddAnimals, "ANIMAL", "Animal_Name", txtAdoptionsAddAnimal.Text);
        }

        /// <summary>
        /// Filter the adoptions modify adopters ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdoptionsModifyAdoptersSearch_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAdoptionsModifyAdopters, "ADOPTER", "Adopter_Name", txtAdoptionsModifyAdoptersSearch.Text);
        }

        /// <summary>
        /// Filter the adoptions modify animals ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdoptionsModifyAnimalsSearch_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAdoptionsModifyAnimals, "ANIMAL", "Animal_Name", txtAdoptionsModifyAnimalsSearch.Text);
        }

        /// <summary>
        /// Filter the adoptions delete ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdoptionsDeleteSearch_TextChanged(object sender, EventArgs e)
        {
            DBMethodsPopulateControls.FilterListBox(lstAdoptionsDeleteSearchAdoptions, "ADOPTION", "Animal_Number", txtAdoptionsDeleteSearch.Text);
        }




        /* --------------------------------
         * Reporting event handlers
         * --------------------------------
         */

        /// <summary>
        /// Generate and display a report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReportingGenerate_Click(object sender, EventArgs e)
        {
            // Reset error messages
            lblReportingMainErrorType.Visible = false;
            lblReportingMainErrorStartDate.Visible = false;
            lblReportingMainErrorOrderBy.Visible = false;

            DateTime start = dtpReportingMainStartDate.Value;
            DateTime end = dtpReportingMainEndDate.Value;
            
            // Validate input
            if (cbxReportingType.SelectedIndex == -1)
            {
                lblReportingMainErrorType.Visible = true;
                return;
            }

            if (start > end)
            {
                lblReportingMainErrorStartDate.Visible = true; ;
                return;
            }

            if (cbxReportingOderBy.SelectedIndex == -1)
            {
                lblReportingMainErrorOrderBy.Visible = true;
                return;
            }

            lblReportingDisplayCurrentDate.Text = DateTime.Now.ToShortDateString(); // Get current date

            // Select order by type
            string orderBy = string.Empty;

            if (cbxReportingOderBy.SelectedItem.ToString() == "Ascending")
                orderBy = "ASC";

            if (cbxReportingOderBy.SelectedItem.ToString() == "Descending")
                orderBy = "DESC";

            switch (cbxReportingType.SelectedIndex)
            {
                case 0:
                    {
                        int totalAdded = 0;

                        DBMethodsReporting.GenerateReportAnimals(dgvReportingDisplay, start, end, orderBy, ref totalAdded);

                        lblReportingDisplayTitle.Text = "Animals received from " + start.ToShortDateString() + " to " + end.ToShortDateString();
                        lblReportingDisplayTotal.Text = "Total animals: " + totalAdded;

                        if (cbxReportingOderBy.SelectedIndex == 0)
                            lblReportingDisplaySortDesc.Text = "Ordered by animal name ascending.";

                        if (cbxReportingOderBy.SelectedIndex == 1)
                            lblReportingDisplaySortDesc.Text = "Ordered by animal name descending.";

                        tbcMain.SelectedTab = tbpReportingDisplay;

                        break;
                    }

                case 1:
                    {
                        int totalAdoptions = 0;
                        DBMethodsReporting.GenerateReportAdoptions(dgvReportingDisplay, start, end, orderBy, ref totalAdoptions);

                        lblReportingDisplayTitle.Text = "Animal adoptions from " + start.ToShortDateString() + " to " + end.ToShortDateString();
                        lblReportingDisplayTotal.Text = "Total adoptions: " + totalAdoptions;

                        if (cbxReportingOderBy.SelectedIndex == 0)
                            lblReportingDisplaySortDesc.Text = "Ordered by adoption date ascending.";

                        if (cbxReportingOderBy.SelectedIndex == 1)
                            lblReportingDisplaySortDesc.Text = "Ordered by adoption date descending.";

                        tbcMain.SelectedTab = tbpReportingDisplay;

                        break;
                    }

                default:
                    {
                        lblReportingMainErrorType.Visible = true;

                        break;
                    }
            }
        }

        /// <summary>
        /// Change ordering label based on ordering selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxReportingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxReportingType.SelectedIndex)
            {
                case 0:
                    {
                        lblReportingOrderBy.Text = "Sort by name:";

                        break;
                    }
                case 1:
                    {
                        lblReportingOrderBy.Text = "Sort by date:";

                        break;
                    }
            }
        }




        /* --------------------------------
         * Dashboard event handlers
         * --------------------------------
         */

        /// <summary>
        /// Display dashboard elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpDashboard_Enter(object sender, EventArgs e)
        {
            string[] userTypeNullsTables = { "USERS" };
            string[] userTypeNullsColumns = { "User_Name", "User_Surname" };

            DBMethodsNulls.DisplayNulls(lstDashboardRIUsers, userTypeNullsTables, userTypeNullsColumns, "User_Type", "", ", ", 1);

            string[] adopterNullsTables = { "ADOPTER" };
            string[] adopterNullsColumns = { "Adopter_Surname", "Adopter_Name" };

            DBMethodsNulls.DisplayNulls(lstDashboardRIAdopters, adopterNullsTables, adopterNullsColumns, "City_ID", "", ", ", 1);

            string[] animalNullsTables = { "ANIMAL" };
            string[] animalNullsColumns = { "Animal_Name" };
            DBMethodsNulls.DisplayNulls(lstDashboardRIAnimals, animalNullsTables, animalNullsColumns, "Type_Number", "", "", 1);

            // Display the animal types summary chart
            foreach (var series in crtDashboardAnimalTypesSummary.Series)
                series.Points.Clear(); // Clear data each reload

            Dictionary<string, int> typeCount = Charting.GetNumberOfAnimalsPerType();

            for (int i = 0; i < typeCount.Count; i++)
            { 
                crtDashboardAnimalTypesSummary.Series["AnimalsPerType"].Points.AddXY(typeCount.ElementAt(i).Key, typeCount.ElementAt(i).Value);
            }

            lblDashboardDate.Text = DateTime.Today.ToShortDateString();
        }




        /* --------------------------------
         * Help button methods/
         * --------------------------------
         */

        private void btnAnimalsMainHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to maintain animals.\n\nClick Add to add an animal.\nClick Modify to Modify an animal.\nClick Delete to delete an animal.\nClick Animal Types to maintain animal types.";
            MessageBox.Show(helpMessage);
        }

        private void btnAnimalsAddHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to add a new animal to the system.\n\nEnter the animal name and select an animal type, then click Add to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnAnimalsModifyHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to change animal details.\n\nYou can search for an animal in the Search bar.\n\nSelect an animal from the list, enter a new name and select a new type, then click Modify to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnAnimalsDeleteHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to delete an animal from the system.\n\nYou can search for an animal in the Search bar.\n\nSelect the animal name, then click Delete to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnAnimalTypesMainHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to maintain animal types.\n\nClick Add to add an animal type.\nClick Modify to modify an animal type.\nClick Delete to delete an animal type.";
            MessageBox.Show(helpMessage);
        }

        private void btnAnimalTypesAddHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to add a new animal type to the system.\n\nEnter the animal type name, then click Add to finish.";
            MessageBox.Show(helpMessage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to change animal type details.\n\nYou can search for an animal type in the Search bar.\n\nSelect an animal type from the list, enter a new name, then click Modify to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnAnimalTypesDeleteHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to delete an animal type from the system.\n\nYou can search for an animal in the Search bar.\n\nSelect the animal type name, then click Delete to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnAdoptionsMainHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to maintain adoptions.\n\nClick Add to add an adoption.\nClick Modify to modify an adoption.\nClick Delete to delete an adoption.";
            MessageBox.Show(helpMessage);
        }

        private void btnAdoptionsAddHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to add a new adoption to the system.\n\nYou can search for an animal and adopter in the Search bars.\n\nSelect the animal and adopter from the lists, select an adoption result, then click Add to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnAdoptionsModifyHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to change adoption details.\n\nYou can search for an animal and adopter in the Search bars.\n\nSelect the animal and adopter from the lists, select a adoption result, then click Modify to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnAdoptionsDeleteHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to delete an adoption from the system.\n\nYou can search for an adoption in the Search bar.\n\nSelect the adoption from the list, then click Delete to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnAdoptersMainHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to maintain adopters.\n\nClick Add to add an adopter.\nClick Modify to modify an adopter.\nClick Delete to delete an adopter.";
            MessageBox.Show(helpMessage);
        }

        private void btnAdoptersAddHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to add a new adopter to the system.\n\nEnter all the required adopter information, then click Add to finish";
            MessageBox.Show(helpMessage);
        }

        private void btnAdoptersModifyHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to modify adopter details.\n\nYou can search for an adopter using the Search bar.\n\nSelect an adopter from the lsit, enter all the updated required adopter information, then click Add to finish";
            MessageBox.Show(helpMessage);
        }

        private void btnAdoptersDeleteHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to delete an adopter from the system.\n\nYou can search for an adopter in the Search bar.\n\nSelect the adopter from the list, then click Delete to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnCitiesHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to maintain cities.\n\nYou can search for cities using the Search bar.\n\nSelect a city to perform an operation.\n\nClick Add to add a city.\nClick Modify to modify a city.\nClick Delete to delete a city.";
            MessageBox.Show(helpMessage);
        }

        private void btnAdminMainHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to maintain users.\n\nClick Add to add a user.\nClick Modify to modify a user.\nClick Delete to delete a user.\nClick User Types to maintain user types.";
            MessageBox.Show(helpMessage);
        }

        private void btnAdminAddHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to add a new user to the system.\n\nEnter all the required user information, then click Add to finish";
            MessageBox.Show(helpMessage);
        }

        private void btnAdminModifyHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to modify user details.\n\nSelect an adopter from the list, enter all the updated required user information, then click Add to finish";
            MessageBox.Show(helpMessage);
        }

        private void btnAdminDeleteHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to delete a user from the system.\n\nYou can search for a user in the Search bar.\n\nSelect the user from the list, then click Delete to finish.";
            MessageBox.Show(helpMessage);
        }

        private void btnUserTypesHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = "This page allows you to maintain user types.\n\nEnter a new type name, then click Add to add a user type or click Delete to delete a user type.";
            MessageBox.Show(helpMessage);
        }

        private void btnReportingMainHelp_Click(object sender, EventArgs e)
        {
            string HelpMessage = "This page allows you to generate and view reports.\n\nSelect the report type, set a start and end date range, choose the sorting type and click Generate Report.";
            MessageBox.Show(HelpMessage);
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
