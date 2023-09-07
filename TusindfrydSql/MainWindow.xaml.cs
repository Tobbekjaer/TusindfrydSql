using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Data.SqlClient;
using System.Configuration;
using static System.Configuration.ConfigurationManager;
using System.Drawing.Drawing2D;
using System.Data.Common;

namespace TusindfrydSql
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            txtName.Clear();
            txtTime.Clear();
            txtHalfLife.Clear();
            txtSize.Clear();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {

            string ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseServerInstance"].ConnectionString;

            try {
                using (SqlConnection con = new SqlConnection(ConnectionString)) {

                    con.Open();

                    // Kræver at brugeren har udfyldt data i alle felterne i WPF 
                    SqlCommand cmd = new SqlCommand("INSERT INTO FlowerSort(FlowerName, ProductionTimeInDays, HalfLife, SizeInSquareMeters)" + 
                        "VALUES(@FlowerName, @ProductionTimeInDays, @HalfLife, @SizeInSquareMeters)", con); 
                    cmd.Parameters.Add("@FlowerName", SqlDbType.NVarChar).Value = txtName.Text.Trim();
                    cmd.Parameters.Add("@ProductionTimeInDays", SqlDbType.Int).Value = txtTime.Text.Trim();
                    cmd.Parameters.Add("@HalfLife", SqlDbType.Int).Value = txtHalfLife.Text.Trim();
                    cmd.Parameters.Add("@SizeInSquareMeters", SqlDbType.Float).Value = txtSize.Text.Trim();

                    if (cmd.ExecuteNonQuery() == 1) {
                        Clear();
                        // Returnerer en besked, hvis der er sket ændringer i databasen
                        MessageBox.Show("1 row effected.");
                        return;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Read_Click(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseServerInstance"].ConnectionString;

            try {
                using (SqlConnection con = new SqlConnection(ConnectionString)) {

                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT FlowerName, ProductionTimeInDays, HalfLife, SizeInSquareMeters FROM FlowerSort", con);

                    using(SqlDataReader reader = cmd.ExecuteReader()) {
                        while(reader.Read()) {

                            string line = ""; 

                            foreach (DbDataRecord row in reader) {
                                
                                string flowerName = reader["FlowerName"].ToString();
                                int productionTime = int.Parse(reader["ProductionTimeInDays"].ToString());
                                int halfLife = int.Parse(reader["HalfLife"].ToString());
                                double size = double.Parse(reader["SizeInSquareMeters"].ToString());

                                line += $"{flowerName}, {productionTime}, {halfLife}, {size}\n";

                            }
                            
                            tbSorts.Text = line;
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseServerInstance"].ConnectionString;

            try {
                using (SqlConnection con = new SqlConnection(ConnectionString)) {

                    con.Open();

                    // Kræver at brugeren har udfyldt data i Flowername feltet i WPF 
                    SqlCommand cmd = new SqlCommand("DELETE FROM FlowerSort WHERE FlowerName = @FlowerName", con);
                    cmd.Parameters.Add("@FlowerName", SqlDbType.NVarChar).Value = txtName.Text.Trim();

                    if (cmd.ExecuteNonQuery() == 1) {
                        Clear();
                        MessageBox.Show("1 row effected.");
                        return;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }

    }
    
}
