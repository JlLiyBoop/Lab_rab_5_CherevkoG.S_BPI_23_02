using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.ViewModel;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.View
{
    public partial class WindowNewEmployee : Window
    {
        public ComboBox CbRole => cbRole;

        public WindowNewEmployee()
        {
            InitializeComponent();
            LoadRoles();
            Loaded += WindowNewEmployee_Loaded;
        }
        private void LoadRoles()
        {
            var roleViewModel = new RoleViewModel();
            cbRole.ItemsSource = roleViewModel.ListRole;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is Role role)
            {
                role.SetDialogWindow(this);
            }
            else if (DataContext is PersonDpo person)
            {
                person.SetDialogWindow(this);
            }
        }
        private void tbBirthday_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbBirthday.Visibility == Visibility.Hidden)
            {
                ClBirthday.Visibility = Visibility.Visible;
                Dispatcher.BeginInvoke(new Action(() => {
                    ClBirthday.Focus();
                    ClBirthday.IsDropDownOpen = true;
                }));
            }
            else
            {
                ClBirthday.Visibility = Visibility.Hidden;
            }
        }

        private void WindowNewEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is PersonDpo person && !string.IsNullOrEmpty(person.RoleName))
            {
                foreach (Role role in cbRole.Items)
                {
                    if (role.NameRole == person.RoleName)
                    {
                        cbRole.SelectedItem = role;
                        break;
                    }
                }
            }
        }

    }

    public class StringToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString && !string.IsNullOrWhiteSpace(dateString))
            {
                if (DateTime.TryParseExact(dateString, "dd.MM.yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToString("dd.MM.yyyy");
            }
            return string.Empty;
        }
    }
}