using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using DBMigratePro.Entities;
using DBMigratePro.BusinessLogic;
using System.Configuration;

namespace DBMigratePro.View.Views
{
    /// <summary>
    /// Lógica de interacción para ViewCustomer.xaml
    /// </summary>
    public partial class ViewCustomer : UserControl
    {
        private ClienteService _clienteService;
        public ObservableCollection<Cliente> Clientes { get; set; } = new ObservableCollection<Cliente>();
        string connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
        string databaseName = "Clientes";
        public ViewCustomer()
        {
            InitializeComponent();
            _clienteService = new ClienteService(connectionString, databaseName);
            Loaded += ViewCustomer_Loaded;
        }

        private async void ViewCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            var clientes = await _clienteService.ObtenerClientesAsync();
            Clientes.Clear();
            foreach (var cliente in clientes)
            {
                Clientes.Add(cliente);
            }
            ClientesDataGrid.ItemsSource = Clientes;
        }
    }
}
