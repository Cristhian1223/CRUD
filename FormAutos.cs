using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDApp
{
    public partial class FormAutos : Form
    {
        private SqlConnection con = new SqlConnection("Data Source=DESKTOP-C4FJH0E;Initial Catalog=TuBaseDeDatos;Integrated Security=True");
        private int userID;

        public FormAutos(int userID)
        {
            InitializeComponent();
            this.userID = userID;
        }
        private void FormAutos_Load(object sender, EventArgs e)
        {
            // Cargar datos de la tabla de autos en el DataGridView
            LoadData();
        }


        private void LoadData()
        {
            // Consulta SQL para seleccionar todos los registros de la tabla de autos
            string query = "SELECT * FROM Autos";

            // Crear un adaptador de datos y un conjunto de datos
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();

            // Llenar el DataTable con los datos de la tabla de autos
            da.Fill(dt);

            // Asignar el DataTable como origen de datos del DataGridView
            dataGridView1.DataSource = dt;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            // Mostrar el formulario Form1
            Form1 form1 = new Form1();
            form1.Show();

            // Cerrar este formulario (FormAutos)
            this.Close();
        }


        private void btnAgregarAuto_Click(object sender, EventArgs e)
        {
            // Obtener los valores de los campos de texto
            string numeroAuto = txtNumeroAuto.Text;
            string color = txtColor.Text;
            string modelo = txtModelo.Text;

            // Validar que se hayan ingresado todos los datos necesarios
            if (string.IsNullOrEmpty(numeroAuto) || string.IsNullOrEmpty(color) || string.IsNullOrEmpty(modelo))
            {
                MessageBox.Show("Por favor ingrese todos los datos requeridos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insertar el nuevo auto en la base de datos
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Autos (UserID, NumeroAuto, Color, Modelo) VALUES (@UserID, @NumeroAuto, @Color, @Modelo)", con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@NumeroAuto", numeroAuto);
                cmd.Parameters.AddWithValue("@Color", color);
                cmd.Parameters.AddWithValue("@Modelo", modelo);

                cmd.ExecuteNonQuery();
                con.Close();
            }

            // Mostrar el formulario para agregar mascotas
            FormMascotas formMascotas = new FormMascotas(userID);
            formMascotas.Show();

            // Ocultar el formulario actual (FormAutos)
            this.Hide();
        }

        private void btnNoAuto_Click(object sender, EventArgs e)
        {
            // Mostrar el formulario FormMascotas
            FormMascotas formMascotas = new FormMascotas(userID);
            formMascotas.Show();

            // Ocultar el formulario actual (FormAutos)
            this.Hide();
        }
    }
}
