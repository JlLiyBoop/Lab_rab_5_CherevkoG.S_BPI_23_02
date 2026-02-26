using System.Windows;
using System.Windows.Input;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Helper;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.View;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.ViewModel
{
    public class MainViewModel
    {
        private RelayCommand openEmployeeCommand;
        public RelayCommand OpenEmployeeCommand
        {
            get
            {
                return openEmployeeCommand ??
                (openEmployeeCommand = new RelayCommand(obj =>
                {
                    WindowEmployee wEmployee = new WindowEmployee();
                    wEmployee.Owner = Application.Current.MainWindow;
                    wEmployee.ShowDialog();
                }));
            }
        }

        private RelayCommand openRoleCommand;
        public RelayCommand OpenRoleCommand
        {
            get
            {
                return openRoleCommand ??
                (openRoleCommand = new RelayCommand(obj =>
                {
                    WindowRole wRole = new WindowRole();
                    wRole.Owner = Application.Current.MainWindow;
                    wRole.ShowDialog();
                }));
            }
        }

        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ??
                (exitCommand = new RelayCommand(obj =>
                {
                    Application.Current.MainWindow?.Close();
                }));
            }
        }
    }
}