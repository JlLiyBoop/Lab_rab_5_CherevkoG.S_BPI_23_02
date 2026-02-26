using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Helper;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.View;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataModels", "RoleData.json");

        private Role selectedRole;
        string _jsonRoles = String.Empty;
        public string Error { get; set; }

        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged("SelectedRole");
                if (EditRole != null) EditRole.CanExecute(true);
            }
        }

        public ObservableCollection<Role> ListRole { get; set; } = new ObservableCollection<Role>();

        public RoleViewModel()
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            ListRole = LoadRole();

            if (ListRole == null || ListRole.Count == 0)
            {
                ListRole = new ObservableCollection<Role>();
                InitializeDefaultRoles();
                SaveChanges(ListRole);
            }
        }

        public ObservableCollection<Role> LoadRole()
        {
            if (!File.Exists(path)) return null;

            try
            {
                _jsonRoles = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(_jsonRoles))
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<Role>>(_jsonRoles);
                }
            }
            catch (Exception e)
            {
                Error = "Ошибка чтения json файла \n" + e.Message;
                return null;
            }
                return null;

        }

        private void SaveChanges(ObservableCollection<Role> listRole)
        {
            var jsonRole = JsonConvert.SerializeObject(listRole, Formatting.Indented);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonRole);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла \n" + e.Message;
            }
        }

        private void InitializeDefaultRoles()
        {
            ListRole.Add(new Role { Id = 1, NameRole = "Директор" });
            ListRole.Add(new Role { Id = 2, NameRole = "Бухгалтер" });
            ListRole.Add(new Role { Id = 3, NameRole = "Менеджер" });
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListRole)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                }
            }
            return max;
        }

        private RelayCommand addRole;
        public RelayCommand AddRole
        {
            get
            {
                return addRole ??
                    (addRole = new RelayCommand(obj =>
                    {
                        WindowNewRole wnRole = new WindowNewRole
                        {
                            Title = "Новая должность",
                        };

                        int maxIdRole = MaxId() + 1;
                        Role role = new Role { Id = maxIdRole };
                        wnRole.DataContext = role;

                        if (wnRole.ShowDialog() == true)
                        {
                            ListRole.Add(role);
                            SaveChanges(ListRole);
                        }

                        SelectedRole = role;
                    }));
            }
        }

        private RelayCommand editRole;
        public RelayCommand EditRole
        {
            get
            {
                return editRole ?? (editRole = new RelayCommand(obj =>
                {
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Редактирование должности",
                    };

                    Role role = SelectedRole;
                    Role tempRole = role.ShallowCopy();

                    wnRole.DataContext = tempRole;
                    tempRole.SetDialogWindow(wnRole);

                    if (wnRole.ShowDialog() == true)
                    {
                        role.NameRole = tempRole.NameRole;
                        SaveChanges(ListRole);

                        int index = ListRole.IndexOf(role);
                        if (index != -1)
                        {
                            ListRole[index] = null;
                            ListRole[index] = role;
                        }
                    }
                }, (obj) => SelectedRole != null));
            }
        }

        private RelayCommand deleteRole;
        public RelayCommand DeleteRole
        {
            get
            {
                return deleteRole ??
                    (deleteRole = new RelayCommand(obj =>
                    {
                        Role role = SelectedRole;
                        MessageBoxResult result = MessageBox.Show(
                            "Удалить данные по должности: " + role.NameRole,
                            "Предупреждение",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.OK)
                        {
                            ListRole.Remove(role);
                            SaveChanges(ListRole);
                        }
                    },
                    (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}