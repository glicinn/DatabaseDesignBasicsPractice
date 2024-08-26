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

namespace DB_MIA
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void WindowSQL_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindow load = new LoadWindow();
            load.ShowDialog();
            DataBaseChoose dataBaseChoose = new DataBaseChoose();
            bool? dialog = dataBaseChoose.ShowDialog();
            switch (dialog)
            {
                case true:
                    WindowSQL.Title = $"{Data.Server} + {Data.Database}";
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.ShowDialog();
                    break;
                default:
                    WindowSQL.Title = "Не подключено.";
                    break;
            }
        }
    }
}
