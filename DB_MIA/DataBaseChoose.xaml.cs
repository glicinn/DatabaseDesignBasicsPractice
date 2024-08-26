using System.Data;
using Microsoft.Win32;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows;
using System;

namespace DB_MIA
{
    /// <summary>
    /// Логика взаимодействия для DataBaseChoose.xaml
    /// </summary>
    public partial class DataBaseChoose : Window
    {
        public DataBaseChoose()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryView reg = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, reg))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        ServerList.Items.Add(Environment.MachineName + @"\" + instanceName);
                    }
                }
            }
            ServerList.IsEnabled = true;
        }

        private void ConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            sqlConnectionClass sqlConnect = new sqlConnectionClass();
            if (ServerList.SelectedIndex >= 0 && DatabaseList.SelectedIndex >= 0)
            {
                if (sqlConnect.Connection(sqlConnect.SQLConnect(ServerList.Items[ServerList.SelectedIndex].ToString(), DatabaseList.Items[DatabaseList.SelectedIndex].ToString())))
                {
                    Data.Server = ServerList.Items[ServerList.SelectedIndex].ToString();
                    Data.Database = DatabaseList.Items[DatabaseList.SelectedIndex].ToString();
                    DialogResult = true;

                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка подключения к базе данных.", "SQL Connection Error.");
                }
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void ServerList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateDatabaseInfo();
        }
        public void UpdateDatabaseInfo()
        {
            DatabaseList.Items.Clear();
            SqlConnection conn = new SqlConnection($"Data Source = {ServerList.Items[ServerList.SelectedIndex].ToString()}; Initial Catalog = master; Integrated Security = True;");
            SqlCommand cmd = new SqlCommand("select name from sys.databases", conn);
            DataTable table = new DataTable();
            try
            {
                conn.Open();
                table.Load(cmd.ExecuteReader());
                foreach (DataRow row in table.Rows)
                {
                    DatabaseList.Items.Add(row[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

