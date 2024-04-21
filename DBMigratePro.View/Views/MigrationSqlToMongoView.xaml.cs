using DBMigratePro.BusinessLogic;
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

namespace DBMigratePro.View.Views
{
    /// <summary>
    /// Lógica de interacción para MigrationSqlToMongoView.xaml
    /// </summary>
    public partial class MigrationSqlToMongoView : UserControl
    {
        private ClienteService _clienteService;
        public MigrationSqlToMongoView()
        {
            InitializeComponent();
        }

        private void ClientesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnMigrateClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
