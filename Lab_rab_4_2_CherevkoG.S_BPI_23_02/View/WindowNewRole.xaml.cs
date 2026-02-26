using System.Windows;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.View
{
    public partial class WindowNewRole : Window
    {
        public WindowNewRole()
        {
            InitializeComponent();
            this.Loaded += WindowNewRole_Loaded;
        }
        private void WindowNewRole_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is Role role)
            {
                role.SetDialogWindow(this);
            }
        }
    }
}