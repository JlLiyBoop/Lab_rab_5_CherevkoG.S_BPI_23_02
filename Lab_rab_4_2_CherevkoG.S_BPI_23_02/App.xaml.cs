using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Helper;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.View;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02
{

    public partial class App : Application
    {

        private RelayCommand changeThemeCommand;
        public RelayCommand ChangeThemeCommand
        {
            get
            {
                return changeThemeCommand ??
                (changeThemeCommand = new RelayCommand(obj =>
                {
                    if (obj is string themeId)
                    {
                        switch (int.Parse(themeId))
                        {
                            case 0:
                                ThemesController.SetTheme(ThemesController.ThemeType.Light);
                                break;
                            case 1:
                                ThemesController.SetTheme(ThemesController.ThemeType.Dark);
                                break;
                        }
                    }
                }));
            }
        }
    }
}
