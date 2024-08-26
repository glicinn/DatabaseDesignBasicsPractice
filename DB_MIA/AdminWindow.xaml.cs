using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Animation;

namespace DB_MIA
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        PerformanceCounter cpucounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        PerformanceCounter memcounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
        PerformanceCounter hardcounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        PerformanceCounter netcounter = new PerformanceCounter("Process", "IO Read Bytes/sec", "System");
        string EmployeeID = null;
        public AdminWindow()
        {
            InitializeComponent();
            sqlConnectionClass sql = new sqlConnectionClass();
            List<string> Position = new List<string> { };
            DataTable table = sql.SQLCommand($"select [Name_Position] from [dbo].[Position]", sqlConnectionClass.act.select, null);
            for(int i = 0; i<table.Rows.Count; i++)
            {
                Position.Add(table.Rows[i][0].ToString());
            }
            Position_E.ItemsSource = Position;
            List<string> Rank = new List<string> { };
            table = sql.SQLCommand($"select [Name_Rank] from [dbo].[Rank]", sqlConnectionClass.act.select, null);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Rank.Add(table.Rows[i][0].ToString());
            }
            Rank_E.ItemsSource = Rank;
            List<string> Departament = new List<string> { };
            table = sql.SQLCommand($"select [Name_Departament] from [dbo].[Departament]", sqlConnectionClass.act.select, null);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Departament.Add(table.Rows[i][0].ToString());
            }
            Departament_E.ItemsSource = Departament;
            List<string> Candidate = new List<string> { };
            table = sql.SQLCommand($"select [First_Name_Candidate]+' '+[Name_Candidate]+' '+[Middle_Name_Candidate]+' '+[Login_Candidate] from [dbo].[Candidate] where [Zayavka_ID] = 2", sqlConnectionClass.act.select, null);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Candidate.Add(table.Rows[i][0].ToString());
            }
            Candidate_E.ItemsSource = Candidate;
        }
        private void ApplicationFill()
        {
            sqlConnectionClass sql = new sqlConnectionClass();
            DataTable tableApp = sql.SQLCommand("select [ID_Candidate], [First_Name_Candidate]+' '+[Name_Candidate]+' '+[Middle_Name_Candidate] as 'ФИО кандидата', [Login_Candidate] as 'Логин', [Password_Candidate] as 'Пароль', [Passport_Series]+' '+[Passport_Number] as 'Данные паспорта', [SNILS] as 'СНИЛС', [TIN] as 'ИНН', [Policy] as 'Полис', [Military_ID_Series]+' '+[Military_ID_Number] as 'Военный билет'," +
                $"[VPO_Ending_Diploma] as 'Диплом ВПО', [Status] as 'Статус' from [dbo].[Candidate] inner join [dbo].[Zayavka] on [ID_Zayavka] = [Zayavka_ID]", sqlConnectionClass.act.select, null);
            sql.dependency.OnChange += Dependency_OnChange_App;
            Action action = () =>
            {
                ApplicationDG.ItemsSource = tableApp.DefaultView;
                ApplicationDG.Columns[0].Visibility = Visibility.Hidden;
            };
            Dispatcher.Invoke(action);
        }

        private void EmployeeFill()
        {
            sqlConnectionClass sql = new sqlConnectionClass();
            DataTable tableApp = sql.SQLCommand("select [ID_C_Employee] as 'Код сотрудника', [Name_Candidate]+' '+[First_Name_Candidate]+' '+[Middle_Name_Candidate] as 'ФИО', " +
                "[Login_C_Employee] as 'Логин', [Password_C_Employee] as 'Пароль', [Name_Position] as 'Должность', [Name_Rank] as 'Звание', [Name_Departament] as 'Отдел', " +
                "[Private_Dossier_Number] as 'Номер личного дела', [Service_Weapon_Number] as 'Номер оружия', [Service_Weapon_Sort] as 'Вид оружия', [Schedule] as 'График' from [dbo].[C_Employee] inner join [dbo].[Candidate] on [ID_Candidate] = [Candidate_ID] inner join [dbo].[Position] on [ID_Position] = [Position_ID] " +
                "inner join [dbo].[Office] on [ID_Office] = [Office_ID] inner join [dbo].[Departament] on [ID_Departament] = [Departament_ID] inner join [dbo].[Rank] on [ID_Rank] = [Rank_ID]", sqlConnectionClass.act.select, null);
            sql.dependency.OnChange += Dependency_OnChange_Emp;
            Action action = () =>
            {
                EmployeeDG.ItemsSource = tableApp.DefaultView;
                EmployeeDG.Columns[0].Visibility = Visibility.Hidden;
            };
            Dispatcher.Invoke(action);
        }
        /*
         * Candidate - ФИО, ИНН, СНИЛС
         * Rank - Звание
         * Position - Должность
         * Departament - Отдел
         * Login_C_Employee [Поле] - Логин
         * Password_C_Employee [Поле] - Пароль
         * Private_Dossier_Number [Поле] - Номер личного дела
         * Service_Weapon_Number [Поле] - Номер оружия
         * Service_Weapon_Sort [Поле] - Вид оружия
         * Schedule [Поле] - График
         */
        private void Dependency_OnChange_App(object sender, System.Data.SqlClient.SqlNotificationEventArgs e)
        {
            if (e.Info != System.Data.SqlClient.SqlNotificationInfo.Invalid) ApplicationFill();
        }
        private void Dependency_OnChange_Emp(object sender, System.Data.SqlClient.SqlNotificationEventArgs e)
        {
            if (e.Info != System.Data.SqlClient.SqlNotificationInfo.Invalid) EmployeeFill();
        }
        private void AdmWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TranslateTransform trans = new TranslateTransform();
            ApplicationDG.RenderTransform = trans;
            DoubleAnimation doubleAnimation = new DoubleAnimation(-600, 0, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
            var threadCpu = new Thread(CPUUsage);
            threadCpu.IsBackground = true;
            threadCpu.Start();
            var threadMem = new Thread(MemUsage);
            threadMem.IsBackground = true;
            threadMem.Start();
            var threadHard = new Thread(HardUsage);
            threadHard.IsBackground = true;
            threadHard.Start();
            var threadNet = new Thread(NetUsage);
            threadNet.IsBackground = true;
            threadNet.Start();
            ApplicationFill();
            EmployeeFill();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ApplicationAdd_Click(object sender, RoutedEventArgs e)
        {
            if(Name_C.Text != "" && LastName_C.Text != "" && Login_C.Text != "" && Surname_C.Text != "" && Passport_C.Text != "" && SNILS_C.Text != "" && INN_C.Text != "" && Polis_C.Text != "" && Bilet_C.Text != "" && Diplom_C.Text != "")
            {
                string[] passport = Passport_C.Text.Split(' ');
                string[] military = Bilet_C.Text.Split(' ');
                sqlConnectionClass sql = new sqlConnectionClass();
                DataTable table = sql.SQLCommand($"insert into [dbo].[Candidate] ([Name_Candidate], [First_Name_Candidate], [Middle_Name_Candidate], [Login_Candidate], [Password_Candidate], [Passport_Series], [Passport_Number], [SNILS], [TIN], [Zayavka_ID], [Policy], [Military_ID_Series], [Military_ID_Number], [VPO_Ending_Diploma]) values " +
                    $"('{Name_C.Text}', '{Surname_C.Text}', '{LastName_C.Text}', '{Login_C.Text}', 'Pa$$w0rd', '{passport[0]}', '{passport[1]}', '{SNILS_C.Text}', '{INN_C.Text}', 2, '{Polis_C.Text}', '{military[0]}', '{military[1]}', '{Diplom_C.Text}')", sqlConnectionClass.act.update, null);
                List<string> Candidate = new List<string> { };
                table = sql.SQLCommand($"select [First_Name_Candidate]+' '+[Name_Candidate]+' '+[Middle_Name_Candidate]+' '+[Login_Candidate] from [dbo].[Candidate] where [Zayavka_ID] = 2", sqlConnectionClass.act.select, null);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    Candidate.Add(table.Rows[i][0].ToString());
                }
                Candidate_E.ItemsSource = Candidate;
            }
        }

        private void EmployeeAdd_Click(object sender, RoutedEventArgs e)
        {
            if(Candidate_E.Text != "" && Login_E.Text != "" && Password_E.Text != "" && Position_E.Text != "" && Rank_E.Text != "" && Departament_E.Text != "" && PDNumber_E.Text != "" && SWNumber_E.Text != "" && SWSort_E.Text != "" && Schedule_E.Text != "")
            {
                int count = 0;
                sqlConnectionClass sql = new sqlConnectionClass();
                string[] candidate = Candidate_E.Text.Split(' ');
                DataTable table = sql.SQLCommand($"select [ID_Candidate] from [dbo].[Candidate] where [Login_Candidate] = '{candidate[3]}'", sqlConnectionClass.act.select, null);
                string idCandidate = table.Rows[0][0].ToString();
                table = sql.SQLCommand($"select [ID_C_Employee] from [dbo].[C_Employee]", sqlConnectionClass.act.select, null);
                count = table.Rows.Count;
                table = sql.SQLCommand($"select [ID_Departament] from [dbo].[Departament] where [Name_Departament] = '{Departament_E.Text}'", sqlConnectionClass.act.select, null);
                string idDep = table.Rows[0][0].ToString();
                table = sql.SQLCommand($"select [ID_Office] from [dbo].[Office] where [Departament_ID] = {idDep}", sqlConnectionClass.act.select, null);
                string idOffice = table.Rows[0][0].ToString();
                table = sql.SQLCommand($"select [ID_Rank] from [dbo].[Rank] where [Name_Rank] = '{Rank_E.Text}'", sqlConnectionClass.act.select, null);
                string idRank = table.Rows[0][0].ToString();
                table = sql.SQLCommand($"select [ID_Position] from [dbo].[Position] where [Name_Position] = '{Position_E.Text}'", sqlConnectionClass.act.select, null);
                string idPosition = table.Rows[0][0].ToString();

                table = sql.SQLCommand($"insert into [dbo].[C_Employee] ([Candidate_ID], [Position_ID], [Office_ID], [Rank_ID], [Private_Dossier_Number], [Service_Weapon_Number], [Service_Weapon_Sort], [Schedule], [Login_C_Employee], [Password_C_Employee]) values " +
                    $"({idCandidate}, {idPosition}, {idOffice}, {idRank}, '{PDNumber_E.Text}', '{SWNumber_E.Text}', '{SWSort_E.Text}', '{Schedule_E.Text}', '{Login_E.Text}', '{Password_E.Text}')", sqlConnectionClass.act.update, null);
                int newcount = 0;
                table = sql.SQLCommand($"select [ID_C_Employee] from [dbo].[C_Employee]", sqlConnectionClass.act.select, null);
                newcount = table.Rows.Count;
                if(newcount > count)
                {
                    table = sql.SQLCommand($"update [dbo].[Candidate] set [Zayavka_ID] = 1 where [ID_Candidate] = {idCandidate}", sqlConnectionClass.act.update, null);
                    List<string> Candidate = new List<string> { };
                    table = sql.SQLCommand($"select [First_Name_Candidate]+' '+[Name_Candidate]+' '+[Middle_Name_Candidate]+' '+[Login_Candidate] from [dbo].[Candidate] where [Zayavka_ID] = 2", sqlConnectionClass.act.select, null);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        Candidate.Add(table.Rows[i][0].ToString());
                    }
                    Candidate_E.ItemsSource = Candidate;
                }
            }
            EmployeeID = null;
        }

        private void ApplicationDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)ApplicationDG.SelectedItems[0];
                string[] candidateInfo = row[1].ToString().Split(' ');
                Login_C.Text = row[2].ToString();
                Surname_C.Text = candidateInfo[0];
                Name_C.Text = candidateInfo[1];
                LastName_C.Text = candidateInfo[2];
                Passport_C.Text = row[4].ToString();
                SNILS_C.Text = row[5].ToString();
                INN_C.Text = row[6].ToString();
                Polis_C.Text = row[7].ToString();
                Bilet_C.Text = row[8].ToString();
                Diplom_C.Text = row[9].ToString();
            }
            catch { }
        }

        private void EmployeeDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)EmployeeDG.SelectedItems[0];
                EmployeeID = row[0].ToString();
                Login_E.Text = row[2].ToString();
                Password_E.Text = row[3].ToString();
                Position_E.Text = row[4].ToString();
                Rank_E.Text = row[5].ToString();
                Departament_E.Text = row[6].ToString();
                PDNumber_E.Text = row[7].ToString();
                SWNumber_E.Text = row[8].ToString();
                SWSort_E.Text = row[9].ToString();
                Schedule_E.Text = row[10].ToString();
            }
            catch { }
        }

        private void ApplicationEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)ApplicationDG.SelectedItems[0];
                if (Name_C.Text != "" && LastName_C.Text != "" && Login_C.Text != "" && Surname_C.Text != "" && Passport_C.Text != "" && SNILS_C.Text != "" && INN_C.Text != "" && Polis_C.Text != "" && Bilet_C.Text != "" && Diplom_C.Text != "")
                {
                    string[] passport = Passport_C.Text.Split(' ');
                    string[] military = Bilet_C.Text.Split(' ');
                    sqlConnectionClass sql = new sqlConnectionClass();
                    DataTable table = sql.SQLCommand($"update [dbo].[Candidate] set [Name_Candidate] = '{Name_C.Text}', [First_Name_Candidate] = '{Surname_C.Text}', [Middle_Name_Candidate] = '{LastName_C.Text}', [Login_Candidate] = '{Login_C.Text}', [Passport_Series] = '{passport[0]}'," +
                        $" [Passport_Number] = '{passport[1]}', [SNILS] = '{SNILS_C.Text}', [TIN] = '{INN_C.Text}', [Policy] = '{Polis_C.Text}', [Military_ID_Series] = '{military[0]}', [Military_ID_Number] = '{military[1]}', [VPO_Ending_Diploma] = '{Diplom_C.Text}' where [ID_Candidate] = {row[0].ToString()}", sqlConnectionClass.act.update, null);
                    List<string> Candidate = new List<string> { };
                    table = sql.SQLCommand($"select [First_Name_Candidate]+' '+[Name_Candidate]+' '+[Middle_Name_Candidate]+' '+[Login_Candidate] from [dbo].[Candidate] where [Zayavka_ID] = 2", sqlConnectionClass.act.select, null);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        Candidate.Add(table.Rows[i][0].ToString());
                    }
                    Candidate_E.ItemsSource = Candidate;
                }
            }
            catch
            {
                MessageBox.Show("Не был выбран элемент для изменения.", "Ошибка изменения данных.");
            }
        }

        private void ApplicationDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)ApplicationDG.SelectedItems[0];
                sqlConnectionClass sql = new sqlConnectionClass();
                DataTable table = sql.SQLCommand($"delete from [dbo].[Candidate] where [ID_Candidate] = {row[0].ToString()}", sqlConnectionClass.act.delete, null);
                List<string> Candidate = new List<string> { };
                table = sql.SQLCommand($"select [First_Name_Candidate]+' '+[Name_Candidate]+' '+[Middle_Name_Candidate]+' '+[Login_Candidate] from [dbo].[Candidate] where [Zayavka_ID] = 2", sqlConnectionClass.act.select, null);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    Candidate.Add(table.Rows[i][0].ToString());
                }
                Candidate_E.ItemsSource = Candidate;
            }
            catch
            {
                MessageBox.Show("Не был выбран элемент для изменения.", "Ошибка изменения данных.");
            }
        }

        private void EmployeeUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(EmployeeID != null)
                {
                    if (Login_E.Text != "" && Password_E.Text != "" && Position_E.Text != "" && Rank_E.Text != "" && Departament_E.Text != "" && PDNumber_E.Text != "" && SWNumber_E.Text != "" && SWSort_E.Text != "" && Schedule_E.Text != "")
                    {
                        sqlConnectionClass sql = new sqlConnectionClass();
                        DataTable table = sql.SQLCommand($"select [ID_Departament] from [dbo].[Departament] where [Name_Departament] = '{Departament_E.Text}'", sqlConnectionClass.act.select, null);
                        string idDep = table.Rows[0][0].ToString();
                        table = sql.SQLCommand($"select [ID_Office] from [dbo].[Office] where [Departament_ID] = {idDep}", sqlConnectionClass.act.select, null);
                        string idOffice = table.Rows[0][0].ToString();
                        table = sql.SQLCommand($"select [ID_Rank] from [dbo].[Rank] where [Name_Rank] = '{Rank_E.Text}'", sqlConnectionClass.act.select, null);
                        string idRank = table.Rows[0][0].ToString();
                        table = sql.SQLCommand($"select [ID_Position] from [dbo].[Position] where [Name_Position] = '{Position_E.Text}'", sqlConnectionClass.act.select, null);
                        string idPosition = table.Rows[0][0].ToString();

                        table = sql.SQLCommand($"update [dbo].[C_Employee] set [Login_C_Employee] = '{Login_E.Text}', [Password_C_Employee] = '{Password_E.Text}', [Position_ID] = {idPosition}, [Rank_ID] = {idRank}," +
                            $" [Office_ID] = {idOffice}, [Private_Dossier_Number] = '{PDNumber_E.Text}', [Service_Weapon_Number] = '{SWNumber_E.Text}', [Service_Weapon_Sort] = '{SWSort_E.Text}', [Schedule] = '{Schedule_E.Text}' where [ID_C_Employee] = {EmployeeID}", sqlConnectionClass.act.update, null);
                    }
                }
            }
            catch {
                MessageBox.Show("Не был выбран элемент для изменения.", "Ошибка изменения данных.");
            }
            EmployeeID = null;
        }

        private void EmployeeDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(EmployeeID != null)
                {
                    sqlConnectionClass sql = new sqlConnectionClass();
                    DataTable table = sql.SQLCommand($"select [Candidate_ID] from [dbo].[C_Employee] where [ID_C_Employee] = {EmployeeID}", sqlConnectionClass.act.select, null);
                    string idCandidate = table.Rows[0][0].ToString();
                    table = sql.SQLCommand($"delete from [dbo].[C_Employee] where [ID_C_Employee] = {EmployeeID}", sqlConnectionClass.act.delete, null);
                    table = sql.SQLCommand($"update [dbo].[Candidate] set [Zayavka_ID] = 3 where [ID_Candidate] = {idCandidate}", sqlConnectionClass.act.update, null);
                }
            }
            catch
            {
                MessageBox.Show("Не был выбран элемент для изменения.", "Ошибка изменения данных.");
            }
            EmployeeID = null;
        }
        private void NetUsage()
        {
            while (true)
            {
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        int value = Convert.ToInt32(netcounter.NextValue());
                        Net.Content = "Сеть: " + value.ToString() + "%";
                        NetProc.Value = value;
                    });

                    Thread.Sleep(5000);
                }
                catch { }
            }
        }
        private void CPUUsage()
        {
            while (true)
            {
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        int value = Convert.ToInt32(cpucounter.NextValue());
                        Proclbl.Content = "Процессор: " + value.ToString() + "%";
                        Proc.Value = value;
                    });

                    Thread.Sleep(5000);
                }
                catch { }
            }
        }
        private void MemUsage()
        {
            while (true)
            {
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        int value = Convert.ToInt32(memcounter.NextValue());
                        TempMem.Content = "ОЗУ: " + value.ToString() + "%";
                        Mem.Value = value;
                    });

                    Thread.Sleep(5000);
                }
                catch { }
            }
        }
        private void HardUsage()
        {
            while (true)
            {
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        int value = Convert.ToInt32(hardcounter.NextValue());
                        Hard.Content = "Диск: " + value.ToString() + "%";
                        HardProc.Value = value;
                    });

                    Thread.Sleep(5000);
                }
                catch { }
            }
        }

        private void AdmWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F1)
            {
                HelpWindow help = new HelpWindow();
                help.Show();
            }
        }
    }
}