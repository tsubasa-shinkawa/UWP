using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UWPMediaPlayer.ViewModels
{
    public class MainPageViewModel : Helpers.Observable
    {
        private string _FileName;

        public string FileName
        {
            get { return _FileName; }
            set { OnPropertyChanged(nameof(FileName)); }
        }


        public ICommand TestCommand { get; set; }


        public Views.MainPage View { get; private set; } = null;

        public MainPageViewModel()
        {
        }

        public void Initialize(Views.MainPage mainPage)
        {
            View = mainPage;
        }


    }
}
