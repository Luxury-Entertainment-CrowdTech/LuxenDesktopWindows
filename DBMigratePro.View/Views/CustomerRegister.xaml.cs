using DBMigratePro.BusinessLogic;
using DBMigratePro.Entities;
using DBMigratePro.Entities.Enums;
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
    /// Lógica de interacción para CustomerRegister.xaml
    /// </summary>
    public partial class CustomerRegister : UserControl
    {
        public CustomerRegister()
        {
            InitializeComponent();
            Loaded += CustomerRegister_Loaded;
        }
        private void CustomerRegister_Loaded(object sender, RoutedEventArgs e)
        {
            // Carga los tipos de cliente
            Tipo.ItemsSource = Enum.GetValues(typeof(TipoCliente))
                                    .Cast<TipoCliente>()
                                    .Select(value => new { Value = value, Name = value.ToString() })
                                    .ToList();
            Tipo.DisplayMemberPath = "Name";
            Tipo.SelectedValuePath = "Value";

            // Carga los gestores de BD
            GestorBD.ItemsSource = Enum.GetValues(typeof(GestorBD))
                                      .Cast<GestorBD>()
                                      .Select(value => new { Value = value, Name = value.ToString() })
                                      .ToList();
            GestorBD.DisplayMemberPath = "Name";
            GestorBD.SelectedValuePath = "Value";
        }

        private async void RegistrarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
                string databaseName = "Clientes";

                int tipoClienteSeleccionado = TipoClienteSeleccionado();
                if (tipoClienteSeleccionado == 0)
                {
                    MessageBox.Show("Debe seleccionar un tipo de cliente");
                    return;
                }

                int gestorBDSeleccionado = GestorBDSeleccionado();
                if (gestorBDSeleccionado == 0)
                {
                    MessageBox.Show("Debe seleccionar un gestor de BD");
                    return;
                }

                // Crea un objeto Cliente con los datos del formulario
                var cliente = new Cliente
                {
                    Nombre = Nombre.Text,
                    Tipo = tipoClienteSeleccionado,
                    RucDniPasaporte = RucDniPas.Text,
                    GestorBD = gestorBDSeleccionado,
                    CadenaConexion = CadenaConexion.Text,
                    BasesDeDatos = PanelBasesDeDatos.Children
                                                    .OfType<TextBox>()
                                                    .Select(textBox => textBox.Text)
                                                    .ToList()
                };

                var clienteService = new ClienteService(connectionString, databaseName);
                await clienteService.RegistrarCliente(cliente);

                MessageBox.Show("Cliente registrado con éxito", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);

                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }

        public int GestorBDSeleccionado()
        {
            int GestorBDSeleccionado = 0;
            switch (GestorBD.SelectedValue)
            {
                case Entities.Enums.GestorBD.SQLServer:
                    GestorBDSeleccionado = (int)Entities.Enums.GestorBD.SQLServer;
                    break;
                case Entities.Enums.GestorBD.MySQL:
                    GestorBDSeleccionado = (int)Entities.Enums.GestorBD.MySQL;
                    break;
                case Entities.Enums.GestorBD.Oracle:
                    GestorBDSeleccionado = (int)Entities.Enums.GestorBD.Oracle;
                    break;
                case Entities.Enums.GestorBD.MongoDB:
                    GestorBDSeleccionado = (int)Entities.Enums.GestorBD.MongoDB;
                    break;
                default:
                    break;
            }
            return GestorBDSeleccionado;
        }

        public int TipoClienteSeleccionado ()
        {
            int TipoClienteSeleccionado = 0;
            switch (Tipo.SelectedValue)
            {
                case TipoCliente.Empresa:
                    TipoClienteSeleccionado = (int)TipoCliente.Empresa;
                    break;
                case TipoCliente.Persona:
                    TipoClienteSeleccionado = (int)TipoCliente.Persona;
                    break;
                default:
                    break;
            }
            return TipoClienteSeleccionado;
        }

        private void AñadirBaseDeDatos_Click(object sender, RoutedEventArgs e)
        {
            // Crea un nuevo TextBox y lo añade al panel
            var nuevoTextBox = new TextBox { Margin = new Thickness(0, 5, 0, 5) };
            PanelBasesDeDatos.Children.Add(nuevoTextBox);
        }

        private void QuitarBaseDeDatos_Click(object sender, RoutedEventArgs e)
        {
            if (PanelBasesDeDatos.Children.Count > 0)
            {
                // Remueve el último TextBox añadido al panel
                PanelBasesDeDatos.Children.RemoveAt(PanelBasesDeDatos.Children.Count - 1);
            }
        }

        private async void ProbarConexion_Click(object sender, RoutedEventArgs e)
        {
            if (GestorBD.SelectedValue is GestorBD gestorSeleccionado)
            {
                string cadenaConexion = CadenaConexion.Text;
                bool conexionExitosa = false;

                switch (gestorSeleccionado)
                {
                    case Entities.Enums.GestorBD.SQLServer:
                        cadenaConexion += "Database=master;";
                        ConexionService conexionServiceSQL = new ConexionService();
                        conexionExitosa = await conexionServiceSQL.ProbarConexionAsync(cadenaConexion, gestorSeleccionado);
                        break;
                    case Entities.Enums.GestorBD.MongoDB:
                        // Implementa la lógica de prueba para MongoDB
                        break;
                    case Entities.Enums.GestorBD.PostgreSQL:
                        // Implementa la lógica de prueba para PostgreSQL
                        break;
                        // Añade más casos según sea necesario
                }

                if (conexionExitosa)
                {
                    // Mostrar resultado de la prueba de conexión
                    MessageBox.Show(conexionExitosa ? "Conexión exitosa." : "Falló la prueba de conexión.", "Probar Conexión",
                        MessageBoxButton.OK, conexionExitosa ? MessageBoxImage.Information : MessageBoxImage.Error);
                } 
                else
                {
                    MessageBox.Show("Falló la prueba de conexión.", "Probar Conexión", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un gestor de base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LimpiarCampos()
        {
            // Limpia el TextBox del nombre
            Nombre.Text = string.Empty;

            // Restablece el ComboBox del tipo de cliente
            Tipo.SelectedIndex = -1;

            // Restablece el ComboBox del gestor de BD
            GestorBD.SelectedIndex = -1;

            // Limpia la cadena de conexión
            CadenaConexion.Text = string.Empty;

            // Limpia el TextBox de RUC/DNI/Pasaporte
            RucDniPas.Text = string.Empty;

            // Limpia todos los TextBoxes de bases de datos
            PanelBasesDeDatos.Children.Clear();

            AñadirBaseDeDatos_Click(null, null); // Esto añade un nuevo TextBox al panel como si se hubiera hecho clic en el botón.
        }
    }
}
