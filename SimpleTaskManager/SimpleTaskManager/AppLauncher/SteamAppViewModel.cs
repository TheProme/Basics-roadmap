using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskManager.AppLauncher
{
    public class SteamAppViewModel : ApplicationViewModel
    {
        private int _gameID;

        public int GameID
        {
            get { return _gameID; }
            set { _gameID = value; OnPropertyChanged(); }
        }

    }
}
