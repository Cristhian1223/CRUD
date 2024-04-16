using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDApp
{
    public partial class FormMascotas : Form
    {
        private SqlConnection con = new SqlConnection("Data Source=DESKTOP-C4FJH0E;Initial Catalog=TuBaseDeDatos;Integrated Security=True");
        private int userID;

        public FormMascotas(int userID)
        {
            InitializeComponent();
            this.userID = userID;
        }

        private void FormMascotas_Load(object sender, EventArgs e)
        {
            // Cargar datos de la tabla de mascotas en el DataGridView
            LoadData();
        }

        private void LoadData()
        {
            // Consulta SQL para seleccionar todas las mascotas del usuario actual
            string query = "SELECT * FROM Mascotas WHERE UserID = @UserID";

            // Crear un adaptador de datos y un conjunto de datos
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.SelectCommand.Parameters.AddWithValue("@UserID", userID);
            DataTable dt = new DataTable();

            // Llenar el DataTable con los datos de las mascotas del usuario actual
            da.Fill(dt);

            // Asignar el DataTable como origen de datos del DataGridView
            dataGridView1.DataSource = dt;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            // Mostrar el formulario Form1
            Form1 form1 = new Form1();
            form1.Show();

            // Cerrar este formulario (FormMascotas)
            this.Close();
        }

        private void btnAgregarMascota_Click(object sender, EventArgs e)
        {
            // Obtener los valores de los campos de texto
            string nombre = txtnombre.Text;
            string genero = txtgenero.Text;
            string raza = txtraza.Text;
            string tamaño = txttamaño.Text;
            string caracteristica = txtcaracteristica.Text;

            // Validar que se hayan ingresado todos los datos necesarios
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(genero) || string.IsNullOrEmpty(raza) || string.IsNullOrEmpty(tamaño) || string.IsNullOrEmpty(caracteristica))
            {
                MessageBox.Show("Por favor ingrese todos los datos requeridos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insertar la nueva mascota en la base de datos
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Mascotas (UserID, NombreMascota, Genero, Raza, Tamano, CaracteristicaParticular) VALUES (@UserID, @NombreMascota, @Genero, @Raza, @Tamano, @CaracteristicaParticular)", con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@NombreMascota", nombre);
                cmd.Parameters.AddWithValue("@Genero", genero);
                cmd.Parameters.AddWithValue("@Raza", raza);
                cmd.Parameters.AddWithValue("@Tamano", tamaño);
                cmd.Parameters.AddWithValue("@CaracteristicaParticular", caracteristica);

                cmd.ExecuteNonQuery();
                con.Close();
            }

            // Recargar los datos en el DataGridView
            LoadData();
        }
    }
}
