using DBMigratePro.View.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBMigratePro.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Suscribirse a los eventos de clic de los elementos del menú
            Inicio.Click += OnInicioClick;
            ViewDataMongoDB.Click += OnViewMongoDBDataClick;
            MongoDBaSQLServer.Click += OnMongoToSqlClick;
            SQLServeraMongoDB.Click += OnSqlToMongoClick;
            CustomerRegister.Click += OnCustomerRegisterClick;
            ViewCustomer.Click += OnViewCustomerDataClick;
            GetCustomerDataSQL.Click += OnViewCustomerDataSqlClick;
            GetCustomerDataMongoDB.Click += OnViewCustomerDataMongoDBClick;
            DocumentacionDelProyecto.Click += OnDocumentationClick;

            // Mostrar InicioView al iniciar la aplicación
            MainContent.Content = new InicioView();
        }

        private void OnCustomerRegisterClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CustomerRegister();
        }

        private void OnViewCustomerDataClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ViewCustomer();
        }

        private void OnViewCustomerDataSqlClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new GetCustomerDataSQL();
        }

        private void OnViewCustomerDataMongoDBClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new GetCustomerDataMongoDB();
        }

        private void OnInicioClick(object sender, RoutedEventArgs e)
        {
            // Aquí deberás reemplazar InicioView con tu UserControl de WPF
            MainContent.Content = new InicioView();
        }

        private void OnViewMongoDBDataClick(object sender, RoutedEventArgs e)
        {
            // Aquí deberás reemplazar MongoDBDataView con tu UserControl de WPF
            MainContent.Content = new MongoDBDataView();
        }

        private void OnMongoToSqlClick(object sender, RoutedEventArgs e)
        {
            // Asegúrate de que MigrationMongoToSqlView sea accesible y esté correctamente definido en WPF
            MainContent.Content = new MigrationMongoToSqlView();
        }

        private void OnSqlToMongoClick(object sender, RoutedEventArgs e)
        {
            // Cambia el contenido al UserControl para SQL Server a MongoDB
            MainContent.Content = new MigrationSqlToMongoView();
        }

        private void OnDocumentationClick(object sender, RoutedEventArgs e)
        {
            // Cambia el contenido al UserControl de documentación
            MainContent.Content = new DocumentationView();
        }
    }
}