using System.Data;
using Microsoft.Data.SqlClient;


namespace ContactApp_WInform2
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-VG4QO0B\SQLEXPRESS;Initial Catalog=ContactApp_WInform2;Integrated Security=True;Trust Server Certificate=True");
        int ContactID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            { 
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                    sqlConnection.Open();
                if (btnSave.Text == "SAVE")
                {
                    SqlCommand sqlCommand = new SqlCommand("Contact_AddOrEdit", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@mode", "Add");
                    sqlCommand.Parameters.AddWithValue("ContactID", 0);
                    sqlCommand.Parameters.AddWithValue("@NAME", textBox1.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("MobileNumber", textBox2.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("Address", textBox3.Text.Trim());
                    sqlCommand.ExecuteNonQuery();
                    
                    MessageBox.Show(".........Saved Successfully.............");
                }
                else
                {
                    SqlCommand sqlCommand = new SqlCommand("Contact_AddOrEdit", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@mode", "Update");
                    sqlCommand.Parameters.AddWithValue("ContactID", ContactID);
                    sqlCommand.Parameters.AddWithValue("@Name", textBox1.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", textBox2.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("@Address", textBox3.Text.Trim());
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show(".........Updated Successfully...................");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {

            }
        }


        //Function

        void FillDataGridView()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("Contact_SearchByID", sqlConnection);
            sqlData.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlData.SelectCommand.Parameters.AddWithValue("@ContactName", textBox4.Text.Trim());
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns[0].Visible = false;
            sqlConnection.Close();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {

            }
        }

        //Function
        void Reset()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = "";
            btnSave.Text = "SAVE";
            ContactID = 0;
            btnDelete.Enabled = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reset();
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    

                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    if (btnDelete.Text == "DELETE")
                    {
                        SqlCommand sqldata = new SqlCommand("Contact_DeleteByID2", sqlConnection);
                        sqldata.CommandType = CommandType.StoredProcedure;
                        sqldata.Parameters.AddWithValue("@mode", "Delete");
                        sqldata.Parameters.AddWithValue("@ContactID", ContactID);

                        int rowsAffected = sqldata.ExecuteNonQuery();
                        if(rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Please select a record to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        //MessageBox.Show("Deleted Successfully");
                        Reset();
                    }

                    //MessageBox.Show("Record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //else
                //{
                //    MessageBox.Show("Please select a record to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                ContactID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                btnSave.Text = "UPDATE";
                btnDelete.Enabled = true;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reset();
            FillDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
