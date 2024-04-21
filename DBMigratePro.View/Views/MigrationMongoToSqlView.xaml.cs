using DBMigratePro.BusinessLogic;
using DBMigratePro.Entities;
using DBMigratePro.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para MigrationMongoToSqlView.xaml
    /// </summary>
    public partial class MigrationMongoToSqlView : UserControl
    {
        private ClienteService _clienteService;
        public ObservableCollection<Cliente> Clientes { get; set; } = new ObservableCollection<Cliente>();
        string connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
        string databaseName = "Clientes";
        public MigrationMongoToSqlView()
        {
            InitializeComponent();
            _clienteService = new ClienteService(connectionString, databaseName);
            Loaded += MigrationMongoToSqlView_Loaded;
        }

        private async void MigrationMongoToSqlView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Asumiendo que tienes un método que obtiene solo los clientes con GestorBD = SQLServer
                var clientesSql = await _clienteService.ObtenerClientesPorGestorBDAsync((int)GestorBD.SQLServer);
                ClientesComboBox.ItemsSource = clientesSql;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}");
            }
        }

        private async void OnMigrateClick(object sender, RoutedEventArgs e)
        {
            // Verifica si se ha seleccionado un cliente.
            if (ClientesComboBox.SelectedItem is Cliente clienteSeleccionado)
            {
                // Verifica si se ha seleccionado una base de datos.
                if (BasesDeDatosComboBox.SelectedItem is string baseDeDatosSeleccionada)
                {
                    try
                    {
                        // Construye la cadena de conexión completa.
                        string cadenaConexionCompleta = $"{clienteSeleccionado.CadenaConexion}Database={baseDeDatosSeleccionada};";

                        string mongoDBConection = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
                        string databaseName = "tangle";
                        var usuarioService = new UsuarioService(mongoDBConection, databaseName);
                        var usuarios = await usuarioService.GetUsuariosAsyncSinDesencriptar();

                        // Llamada al método de negocio para realizar la migración
                        await usuarioService.MigrarUsuariosASqlAsync(cadenaConexionCompleta, usuarios);

                        MessageBox.Show("Migración completada con éxito.");

                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error durante la migración: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una base de datos.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un cliente.");
            }
        }

        private void ClientesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientesComboBox.SelectedItem is Cliente clienteSeleccionado)
            {
                // Actualiza el TextBox del tipo de cliente
                TipoClienteTextBox.Text = clienteSeleccionado.Tipo == (int)TipoCliente.Empresa ? "Empresa" : "Persona";

                // Actualiza la cadena de conexión SQL Server
                sqlServerConnectionString.Text = clienteSeleccionado.CadenaConexion;

                // Rellena el ComboBox de bases de datos
                BasesDeDatosComboBox.ItemsSource = clienteSeleccionado.BasesDeDatos;
            }
        }

        private void LimpiarCampos()
        {
            ClientesComboBox.SelectedItem = null;
            TipoClienteTextBox.Text = string.Empty;
            sqlServerConnectionString.Text = string.Empty;
            BasesDeDatosComboBox.ItemsSource = null;
            BasesDeDatosComboBox.SelectedItem = null;
        }
    }
}
