using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Helper;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.ViewModel;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model
{
    public class PersonDpo : INotifyPropertyChanged
    {
        private string _roleName;
        private string firstName;
        private string lastName;
        private string birthday;
        private Window dialogWindow;

        public int Id { get; set; }

        public string RoleName
        {
            get { return _roleName; }
            set
            {
                _roleName = value;
                OnPropertyChanged("RoleName");
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public PersonDpo() { }

        public void SetDialogWindow(Window window)
        {
            dialogWindow = window;
        }

        public PersonDpo(int id, string roleName, string firstName, string lastName, string birthday)
        {
            this.Id = id;
            this.RoleName = roleName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                (saveCommand = new RelayCommand(obj =>
                {
                    if (dialogWindow != null)
                    {
                        dialogWindow.DialogResult = true;
                    }
                }));
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                (cancelCommand = new RelayCommand(obj =>
                {
                    if (dialogWindow != null)
                    {
                        dialogWindow.DialogResult = false;
                    }
                }));
            }
        }

        public PersonDpo ShallowCopy()
        {
            return (PersonDpo)this.MemberwiseClone();
        }

        public PersonDpo CopyFromPerson(Person person)
        {
            PersonDpo perDpo = new PersonDpo();
            RoleViewModel vmRole = new RoleViewModel();
            string role = string.Empty;

            foreach (var r in vmRole.ListRole)
            {
                if (r.Id == person.RoleId)
                {
                    role = r.NameRole;
                    break;
                }
            }

            if (role != string.Empty)
            {
                perDpo.Id = person.Id;
                perDpo.RoleName = role;
                perDpo.FirstName = person.FirstName;
                perDpo.LastName = person.LastName;
                perDpo.Birthday = person.Birthday;
            }

            return perDpo;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}