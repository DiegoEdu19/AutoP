using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System;

namespace AutoP
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper databaseHelper; // Instancia del helper de base de datos

        public MainWindow()
        {
            InitializeComponent();
            databaseHelper = new DatabaseHelper();
        }

        private void IniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contraseña = txtPassword.Password;

            if (ValidarCredenciales(usuario, contraseña))
            {
                string rol = ObtenerRolUsuario(usuario);

                if (rol == "Empleado")
                {
                    // Crear y mostrar la ventana de bienvenida
                    VentanaEmpleado ventanaBienvenida = new VentanaEmpleado();
                    ventanaBienvenida.Show();

                    // Luego, abre la ventana principal de empleado
                    VentanaEmpleado ventanaempleado = new VentanaEmpleado();
                    ventanaempleado.Show();

                    this.Close(); // Cierra la ventana actual
                }
                else
                {
                    MessageBox.Show("Rol no reconocido. Consulta con el administrador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Método para obtener el rol del usuario
        private string ObtenerRolUsuario(string usuario)
        {
            string query = "SELECT Rol FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
            try
            {
                using (SqlConnection connection = new SqlConnection(databaseHelper.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreUsuario", usuario);
                    connection.Open();
                    var rol = command.ExecuteScalar();
                    return rol != null ? rol.ToString() : null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el rol: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private bool ValidarCredenciales(string usuario, string contraseña)
        {
            string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Contraseña = @Contraseña";

            try
            {
                using (SqlConnection connection = new SqlConnection(databaseHelper.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.VarChar) { Value = usuario });
                        command.Parameters.Add(new SqlParameter("@Contraseña", SqlDbType.VarChar) { Value = contraseña });

                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }
    }

}
