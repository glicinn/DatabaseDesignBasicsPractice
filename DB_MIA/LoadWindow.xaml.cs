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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DB_MIA
{
    /// <summary>
    /// Логика взаимодействия для LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public LoadWindow()
        {
            InitializeComponent();
        }
        int i = 0;
        private void Loading_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(0.05);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            i += 1;
            if (i < 10) ProgressLoading.Value = i;
            if (i > 30 && i < 52) ProgressLoading.Value = i;
            if (i > 76 && i < 92) ProgressLoading.Value = i;
            if (i == 100) ProgressLoading.Value = i;
            if (i > 100)
            {
                DialogResult = true;
                timer.Stop();
                Close();
            }
        }
    }
}
