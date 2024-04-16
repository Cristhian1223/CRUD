using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDApp
{
    public partial class Form1 : Form

    {

        private SqlConnection con = new SqlConnection("Data Source=DESKTOP-C4FJH0E;Initial Catalog=TuBaseDeDatos;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadData();
        }

        private void InitializeDatabase()
        {
            using (SqlCommand checkTableCmd = new SqlCommand("IF OBJECT_ID('Users', 'U') IS NULL CREATE TABLE Users (ID INT PRIMARY KEY IDENTITY, Name NVARCHAR(50), Age INT, Apellido_Paterno NVARCHAR(50), Apellido_Materno NVARCHAR(50), Telefono NVARCHAR(20), Email NVARCHAR(100), Numero_casa INT, Tipo NVARCHAR(50), Fecha_alta DATE)", con))
            {
                con.Open();
                checkTableCmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void LoadData()
        {
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Users", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Usa el nombre completamente calificado
                this.dataGridView.DataSource = dt;
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Intenta convertir txtAge.Text a un valor numérico
            if (int.TryParse(txtAge.Text, out int age))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Users (Name, Age, Apellido_Paterno, Apellido_Materno, Telefono, Email, Numero_casa, Tipo, Fecha_alta) VALUES (@Name, @Age, @ApellidoPaterno, @ApellidoMaterno, @Telefono, @Email, @NumeroCasa, @Tipo, @FechaAlta); SELECT SCOPE_IDENTITY()", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Age", age);  // Usa la variable age convertida
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", txtApellidoPaterno.Text);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", txtApellidoMaterno.Text);
                    cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@NumeroCasa", int.Parse(txtNumeroCasa.Text));
                    cmd.Parameters.AddWithValue("@Tipo", txtTipo.Text);
                    cmd.Parameters.AddWithValue("@FechaAlta", dateTimePickerFechaAlta.Value.Date);

                    // Ejecutar el comando y obtener el ID del usuario recién agregado
                    int newUserID = Convert.ToInt32(cmd.ExecuteScalar());

                    con.Close();

                    // Mostrar el formulario FormAutos y pasar el ID del nuevo usuario
                    FormAutos formAutos = new FormAutos(newUserID);
                    formAutos.Show();
                }

                // Vaciar las casillas después de agregar
                txtName.Text = string.Empty;
                txtAge.Text = string.Empty;
                txtApellidoPaterno.Text = string.Empty;
                txtApellidoMaterno.Text = string.Empty;
                txtTelefono.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtNumeroCasa.Text = string.Empty;
                txtTipo.Text = string.Empty;

                LoadData();
            }
            else
            {
                MessageBox.Show("La edad debe ser un número entero válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                int selectedID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["ID"].Value);

                using (SqlCommand cmd = new SqlCommand("UPDATE Users SET Name = @Name, Age = @Age, Apellido_Paterno = @ApellidoPaterno, Apellido_Materno = @ApellidoMaterno, Telefono = @Telefono, Email = @Email, Numero_casa = @NumeroCasa, Tipo = @Tipo, Fecha_alta = @FechaAlta WHERE ID = @ID", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", selectedID);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);

                    if (int.TryParse(txtAge.Text, out int age))
                    {
                        cmd.Parameters.AddWithValue("@Age", age);
                    }
                    else
                    {
                        con.Close();
                        MessageBox.Show("La edad debe ser un número entero válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cmd.Parameters.AddWithValue("@ApellidoPaterno", txtApellidoPaterno.Text);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", txtApellidoMaterno.Text);
                    cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@NumeroCasa", int.Parse(txtNumeroCasa.Text));
                    cmd.Parameters.AddWithValue("@Tipo", txtTipo.Text);
                    cmd.Parameters.AddWithValue("@FechaAlta", dateTimePickerFechaAlta.Value.Date);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                LoadData();
            }
            else
            {
                MessageBox.Show("No hay filas seleccionadas para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];

                if (selectedRow.Cells["ID"].Value != null)
                {
                    int selectedID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE ID = @ID", con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@ID", selectedID);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    LoadData();
                }
                else
                {
                    MessageBox.Show("No se puede obtener el ID de la fila seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No hay filas seleccionadas para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                txtName.Text = dataGridView.SelectedRows[0].Cells["Name"].Value.ToString();
                txtAge.Text = dataGridView.SelectedRows[0].Cells["Age"].Value.ToString();
                txtApellidoPaterno.Text = dataGridView.SelectedRows[0].Cells["Apellido_Paterno"].Value.ToString();
                txtApellidoMaterno.Text = dataGridView.SelectedRows[0].Cells["Apellido_Materno"].Value.ToString();
                txtTelefono.Text = dataGridView.SelectedRows[0].Cells["Telefono"].Value.ToString();
                txtEmail.Text = dataGridView.SelectedRows[0].Cells["Email"].Value.ToString();
                txtNumeroCasa.Text = dataGridView.SelectedRows[0].Cells["Numero_casa"].Value.ToString();
                txtTipo.Text = dataGridView.SelectedRows[0].Cells["Tipo"].Value.ToString();
                dateTimePickerFechaAlta.Value = (DateTime)dataGridView.SelectedRows[0].Cells["Fecha_alta"].Value;
            }
        }

        private void txtApellidoMaterno_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregarMascotas_Click(object sender, EventArgs e)
        {
            // Suponiendo que newUserID es el ID del usuario recién agregado
            int newUserID = ObtenerNuevoUserID();
            // Crear una instancia del formulario FormAutos
            FormMascotas formMascotas = new FormMascotas(newUserID);

            // Mostrar el formulario FormAutos
            formMascotas.Show();

            // Ocultar el formulario actual (Form1)
            this.Hide();

        }

        private int ObtenerNuevoUserID()
        {
            int nuevoID = 0; // Suponiendo que 0 es el valor por defecto si no se puede obtener el ID
                             // Aquí debes escribir la lógica para obtener el ID del usuario recién agregado
                             // Por ejemplo, podrías hacer una consulta a la base de datos para obtener el último ID agregado
                             // y asignarlo a la variable nuevoID

            // Ejemplo de consulta a la base de datos para obtener el último ID agregado
            string query = "SELECT MAX(ID) FROM Users";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                nuevoID = (int)cmd.ExecuteScalar();
                con.Close();
            }

            return nuevoID;
        }

        private void btnAgregarAutos_Click(object sender, EventArgs e)
        {
            // Suponiendo que newUserID es el ID del usuario recién agregado
            int newUserID = ObtenerNuevoUserID();
            // Crear una instancia del formulario FormAutos
            FormAutos formAutos = new FormAutos(newUserID);

            // Mostrar el formulario FormAutos
            formAutos.Show();

            // Ocultar el formulario actual (Form1)
            this.Hide();

        }

    }
}
    

