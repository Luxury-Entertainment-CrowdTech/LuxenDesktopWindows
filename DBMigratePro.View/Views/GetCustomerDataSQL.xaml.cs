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
    /// Lógica de interacción para GetCustomerDataSQL.xaml
    /// </summary>
    public partial class GetCustomerDataSQL : UserControl
    {
        private ClienteService _clienteService;
        public ObservableCollection<Cliente> Clientes { get; set; } = new ObservableCollection<Cliente>();
        string connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
        string databaseName = "Clientes";
        public GetCustomerDataSQL()
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
            TablasComboBox.ItemsSource = null;
            TablasComboBox.SelectedItem = null;
        }

        private async void BasesDeDatosComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BasesDeDatosComboBox.SelectedItem is string baseDeDatosSeleccionada)
            {
                var databaseService = new DatabaseService(connectionString);
                try
                {
                    var tablas = await databaseService.ObtenerTablasAsync(sqlServerConnectionString.Text, baseDeDatosSeleccionada);
                    TablasComboBox.ItemsSource = tablas;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar tablas: {ex.Message}");
                }
            }
        }

        private async void MigrarDatos_Click(object sender, RoutedEventArgs e)
        {
            if (TablasComboBox.SelectedItem is string tablaSeleccionada && BasesDeDatosComboBox.SelectedItem is string baseDeDatosSeleccionada)
            {
                var databaseService = new DatabaseService(connectionString);
                try
                {
                    var datos = await databaseService.ObtenerDatosDeTablaAsync(sqlServerConnectionString.Text, baseDeDatosSeleccionada, tablaSeleccionada);
                    await databaseService.MigrarDatosAMongoDB(datos);
                    MessageBox.Show("Datos migrados exitosamente.");
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al migrar datos: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una tabla para migrar.");
            }
        }
    }
}
