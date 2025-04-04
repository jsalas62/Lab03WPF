using Microsoft.Data.SqlClient;
using System;
using System.Data;
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

namespace Lab03
{

    public partial class MainWindow : Window
    {
        List<Product> products = new List<Product>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            //Forma desconectada

            SqlConnection connection = new SqlConnection("Data Source=LAB1502-09\\SQLEXPRESS;Initial Catalog=Tecsup2023DB;Integrated Security=True; TrustServerCertificate=True");

            connection.Open();
            DataTable dataTable = new DataTable();
            SqlCommand command = new SqlCommand("MyProc", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);
            connection.Close();

            dataGrid.ItemsSource = dataTable.DefaultView;

        }

        private void btnReadConnect_Click(object sender, RoutedEventArgs e)
        {
            // Cadena de conexión a la base de datos
            SqlConnection connection = new SqlConnection("Data Source=LAB1502-09\\SQLEXPRESS;Initial Catalog=Tecsup2023DB;Integrated Security=True; TrustServerCertificate=True");

            

            try
            {
               

                connection.Open();

                SqlCommand command = new SqlCommand("MyProc", connection);

              
                SqlDataReader dataReader = command.ExecuteReader();

             
                while (dataReader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dataReader[0]),  
                        Name = dataReader[1].ToString(),             
                        Price = Convert.ToDecimal(dataReader[2])    
                    });
                }

                dataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión
                connection.Close();
            }
        }




        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower(); 

            string connectionString = "Data Source=LAB1502-09\\SQLEXPRESS;Initial Catalog=Tecsup2023DB;Integrated Security=True; TrustServerCertificate=True";

            try
            {
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                 
                    string query = "SearchProducts";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                   
                        command.Parameters.AddWithValue("@searchText", searchText);

                       
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                 
                            List<Product> filteredProducts = new List<Product>();

                            while (reader.Read())
                            {
                                filteredProducts.Add(new Product
                                {
                                    ProductID = reader.GetInt32(0), 
                                    Name = reader.GetString(1),     
                                    Price = reader.GetDecimal(2)   
                                });
                            }

                            
                            dataGrid.ItemsSource = filteredProducts;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
