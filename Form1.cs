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

namespace CRAD_DataBase
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=MSI\SQLEXPRESS;Initial Catalog=""Crud Operation"";Integrated Security=True;");
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // First, check if the ID already exists
                string checkQuery = "SELECT COUNT(*) FROM Employee_Details WHERE ID = @ID";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    // Assign the ID parameter
                    checkCmd.Parameters.AddWithValue("@ID", textBox1.Text);

                    // Open the connection
                    conn.Open();

                    // Execute the scalar query to get the count of records with the given ID
                    int recordCount = (int)checkCmd.ExecuteScalar();

                    // If recordCount is greater than 0, it means the ID already exists
                    if (recordCount > 0)
                    {
                        MessageBox.Show("ID already exists. Please use a different ID.");
                    }
                    else
                    {
                        // If the ID does not exist, insert the new record
                        string insertQuery = "INSERT INTO Employee_Details (ID, Name, Age, Salary, Gender) VALUES (@ID, @Name, @Age, @Salary, @Gender)";

                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            // Assign parameters for the new record
                            insertCmd.Parameters.AddWithValue("@ID", textBox1.Text);
                            insertCmd.Parameters.AddWithValue("@Name", textBox2.Text);
                            insertCmd.Parameters.AddWithValue("@Age", int.Parse(textBox3.Text));
                            insertCmd.Parameters.AddWithValue("@Salary", decimal.Parse(textBox4.Text));
                            insertCmd.Parameters.AddWithValue("@Gender", comboBox1.SelectedItem.ToString());

                            // Execute the insert command
                            int rows = insertCmd.ExecuteNonQuery();

                            if (rows > 0)
                                MessageBox.Show("Insertion successful.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that occurs and display an error message
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                comboBox1.Items.Clear();
                textBox1.Focus();
                // Ensure the connection is closed
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string query = "Select * from Employee_Details";

            SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();  //blank table

            sda.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Prepare the SQL query to update Name, Age, Salary, and Gender based on the ID
                string query = "UPDATE Employee_Details SET Name = @Name, Age = @Age, Salary = @Salary, Gender = @Gender WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Assign values to the parameters from textboxes and combobox
                    cmd.Parameters.AddWithValue("@ID", textBox1.Text); // ID (used to identify the row to update)
                    cmd.Parameters.AddWithValue("@Name", textBox2.Text); // New Name value
                    cmd.Parameters.AddWithValue("@Age", int.Parse(textBox3.Text)); // New Age value
                    cmd.Parameters.AddWithValue("@Salary", decimal.Parse(textBox4.Text)); // New Salary value
                    cmd.Parameters.AddWithValue("@Gender", comboBox1.SelectedItem.ToString()); // New Gender value

                    // Open the connection
                    conn.Open();

                    // Check if connection is open
                    if (conn.State == ConnectionState.Open)
                    {
                        // Execute the query and get the number of rows affected
                        int rows = cmd.ExecuteNonQuery();

                        // If rows were updated, display success message
                        if (rows > 0)
                            MessageBox.Show("Update is successful");
                        else
                            MessageBox.Show("No record found with the provided ID");
                    }
                }
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // Close the connection if it's open
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.Items.Clear();
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Prepare the SQL query to delete a record based on the ID
                string query = "DELETE FROM Employee_Details WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Assign the ID parameter from textBox1
                    cmd.Parameters.AddWithValue("@ID", textBox1.Text); // ID (used to identify the row to delete)

                    // Open the connection
                    conn.Open();

                    // Check if connection is open
                    if (conn.State == ConnectionState.Open)
                    {
                        // Execute the query and get the number of rows affected
                        int rows = cmd.ExecuteNonQuery();

                        // If rows were deleted, display success message
                        if (rows > 0)
                            MessageBox.Show("Deletion is successful");
                        else
                            MessageBox.Show("No record found with the provided ID");
                    }
                }
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // Close the connection if it's open
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure the clicked row is valid
            if (e.RowIndex >= 0)
            {
                // Get the currently selected row
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Assuming the ID is in the first column (index 0)
                textBox1.Text = selectedRow.Cells[0].Value.ToString();
            }
        }
    }
}
