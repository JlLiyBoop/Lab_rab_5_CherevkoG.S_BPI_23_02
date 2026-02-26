using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Helper;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.View;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataModels", "PersonData.json");

        private PersonDpo selectedPersonDpo;

        string _jsonPersons = String.Empty;
        public string Error { get; set; }
        public string Message { get; set; }

        public PersonDpo SelectedPersonDpo
        {
            get { return selectedPersonDpo; }
            set
            {
                selectedPersonDpo = value;
                OnPropertyChanged("SelectedPersonDpo");
            }
        }

        public ObservableCollection<Person> ListPerson { get; set; }
        public ObservableCollection<PersonDpo> ListPersonDpo { get; set; }

        public PersonViewModel()
        {
            ListPerson = new ObservableCollection<Person>();
            ListPersonDpo = new ObservableCollection<PersonDpo>();

            ListPerson = LoadPerson();

            if (ListPerson == null || ListPerson.Count == 0)
            {
                ListPerson = new ObservableCollection<Person>();
                InitializeDefaultPersons();
                SaveChanges(ListPerson);
            }

            ListPersonDpo = GetListPersonDpo();
        }

        public ObservableCollection<Person> LoadPerson()
        {
            if (!File.Exists(path)) return null;
            try
            {
                _jsonPersons = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(_jsonPersons))
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<Person>>(_jsonPersons);
                }
            }
            catch (Exception e)
            {
                Error = "Ошибка загрузки json \n" + e.Message;
            }
            return null;
        }

        private void SaveChanges(ObservableCollection<Person> listPersons)
        {
            var jsonPerson = JsonConvert.SerializeObject(listPersons, Formatting.Indented);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonPerson);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла \n" + e.Message;
            }
        }

        private void InitializeDefaultPersons()
        {
            ListPerson.Add(new Person { Id = 1, RoleId = 1, FirstName = "Иван", LastName = "Иванов", Birthday = "28.02.1980" });
            ListPerson.Add(new Person { Id = 2, RoleId = 2, FirstName = "Петр", LastName = "Петров", Birthday = "20.03.1981" });
            ListPerson.Add(new Person { Id = 3, RoleId = 3, FirstName = "Виктор", LastName = "Викторов", Birthday = "16.04.1982" });
            ListPerson.Add(new Person { Id = 4, RoleId = 3, FirstName = "Черевко", LastName = "Григорий", Birthday = "21.06.2005" });
        }

        public ObservableCollection<PersonDpo> GetListPersonDpo()
        {
            ListPersonDpo.Clear();
            foreach (var person in ListPerson)
            {
                PersonDpo p = new PersonDpo();
                p = p.CopyFromPerson(person);
                ListPersonDpo.Add(p);
            }
            return ListPersonDpo;
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListPerson)
            {
                if (max < r.Id) max = r.Id;
            }
            return max;
        }

        private RelayCommand addPerson;
        public RelayCommand AddPerson
        {
            get
            {
                return addPerson ??
                    (addPerson = new RelayCommand(obj =>
                    {
                        WindowNewEmployee wnPerson = new WindowNewEmployee
                        {
                            Title = "Новый сотрудник"
                        };

                        int maxIdPerson = MaxId() + 1;
                        PersonDpo per = new PersonDpo
                        {
                            Id = maxIdPerson,
                            Birthday = DateTime.Now.ToShortDateString()
                        };

                        wnPerson.DataContext = per;
                        per.SetDialogWindow(wnPerson);

                        if (wnPerson.ShowDialog() == true)
                        {
                            Role r = (Role)wnPerson.CbRole.SelectedItem;
                            if (r != null)
                            {
                                per.RoleName = r.NameRole;

                                ListPersonDpo.Add(per);

                                Person p = new Person();
                                p = p.CopyFromPersonDPO(per);
                                ListPerson.Add(p);

                                try
                                {
                                    SaveChanges(ListPerson);
                                }
                                catch (Exception e)
                                {
                                    Error = "Ошибка добавления \n" + e.Message;
                                }
                            }
                        }
                    }));
            }
        }

        private RelayCommand editPerson;
        public RelayCommand EditPerson
        {
            get
            {
                return editPerson ??
                    (editPerson = new RelayCommand(obj =>
                    {
                        WindowNewEmployee wnPerson = new WindowNewEmployee()
                        {
                            Title = "Редактирование данных сотрудника",
                        };

                        PersonDpo personDpo = SelectedPersonDpo;
                        PersonDpo tempPerson = personDpo.ShallowCopy();
                        wnPerson.DataContext = tempPerson;
                        tempPerson.SetDialogWindow(wnPerson);

                        if (wnPerson.ShowDialog() == true)
                        {
                            Role r = (Role)wnPerson.CbRole.SelectedItem;
                            if (r != null)
                            {
                                personDpo.RoleName = r.NameRole;
                                personDpo.FirstName = tempPerson.FirstName;
                                personDpo.LastName = tempPerson.LastName;
                                personDpo.Birthday = tempPerson.Birthday;

                                var per = ListPerson.FirstOrDefault(p => p.Id == personDpo.Id);
                                if (per != null)
                                {
                                    var newPerData = per.CopyFromPersonDPO(personDpo);
                                    per.FirstName = newPerData.FirstName;
                                    per.LastName = newPerData.LastName;
                                    per.Birthday = newPerData.Birthday;
                                    per.RoleId = newPerData.RoleId;

                                    try
                                    {
                                        SaveChanges(ListPerson);
                                    }
                                    catch (Exception e)
                                    {
                                        Error = "Ошибка редактирования \n" + e.Message;
                                    }
                                }
                            }
                        }
                    },
                    (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }

        private RelayCommand deletePerson;
        public RelayCommand DeletePerson
        {
            get
            {
                return deletePerson ??
                (deletePerson = new RelayCommand(obj =>
                {
                    PersonDpo person = SelectedPersonDpo;
                    MessageBoxResult result = MessageBox.Show(
                        "Удалить данные по сотруднику: \n" + person.LastName + " " + person.FirstName,
                        "Предупреждение",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                    {
                        ListPersonDpo.Remove(person);
                        var per = ListPerson.FirstOrDefault(p => p.Id == person.Id);
                        if (per != null)
                        {
                            ListPerson.Remove(per);
                            SaveChanges(ListPerson);
                        }
                    }
                },
                (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}