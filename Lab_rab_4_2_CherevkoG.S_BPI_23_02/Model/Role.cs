using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Helper;


namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model
{
    public class Role : INotifyPropertyChanged
    {
        private string nameRole;
        private static Window dialogWindow; ///Ужас, чтобы поставить сюда static и исправить ошибку ушло 2 часа

        public int Id { get; set; }

        public string NameRole
        {
            get { return nameRole; }
            set
            {
                nameRole = value;
                OnPropertyChanged("NameRole");
            }
        }

        public Role() { }
        public void SetDialogWindow(Window window)
        {
            dialogWindow = window;
        }
        public Role(int id, string nameRole)
        {
            this.Id = id;
            this.NameRole = nameRole;
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
                    if (dialogWindow == null) MessageBox.Show("Окно не найдено!");
                    else dialogWindow.DialogResult = false;
                }));
            }
        }
        public Role ShallowCopy()
        {
            return (Role)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
