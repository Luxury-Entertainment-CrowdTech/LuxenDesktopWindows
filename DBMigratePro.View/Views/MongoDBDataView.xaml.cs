using DBMigratePro.BusinessLogic;
using DBMigratePro.View.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBMigratePro.View.Views
{
    /// <summary>
    /// Lógica de interacción para MongoDBDataView.xaml
    /// </summary>
    public partial class MongoDBDataView : UserControl
    {
        public MongoDBDataView()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Asumiendo que tienes almacenadas la cadena de conexión y el nombre de la base de datos en un lugar seguro
                string connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
                string databaseName = "tangle";
                var usuarioService = new UsuarioService(connectionString, databaseName);
                var usuarios = await usuarioService.GetUsuariosAsync();

                var usuariosViewModel = usuarios.Select(u => new UsuarioViewModel
                {
                    NombreCompleto = $"{u.Nombre} {u.Apellido}",
                    Email = u.Email,
                    UsuarioName = u.UsuarioName,
                    NumeroTelefono = u.NumeroTelefono,
                    FechaCreacion = u.FechaCreacion
                }).ToList();

                dataGridMongoDB.ItemsSource = usuariosViewModel;
            }
            catch (Exception ex)
            {
                // Manejar excepción, por ejemplo, mostrando un mensaje al usuario
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }
    }
}
