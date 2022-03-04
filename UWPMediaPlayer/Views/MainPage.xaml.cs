using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace UWPMediaPlayer.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ViewModels.MainPageViewModel ViewModel { get; private set; } = new ViewModels.MainPageViewModel();

        public MainPage()
        {
            this.InitializeComponent();

            ViewModel.Initialize(this);
            this.DataContext = ViewModel;
        }
        
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(_mediaPlayerElement.MediaPlayer is null)
            {
                return;
            }

            _mediaPlayerElement.MediaPlayer.Pause();
        }

        /// <summary>
        /// 10＜＜
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SetPosition(-10);
        }

        /// <summary>
        /// ＞＞10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SetPosition(10);
        }

        /// <summary>
        /// ファイル選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.FileTypeFilter.Add(".mp4");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                tbFile.Text = file.Path;

                // ファイルを選択していたら開く。
                using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
                {
                    _mediaPlayerElement.Source = MediaSource.CreateFromStream(stream, file.Path);
                    var mediaPlayer = _mediaPlayerElement.MediaPlayer;
                    mediaPlayer.Play();

                    SetPlaybackRate();
                }
            }
            else
            {
            }
        }

        private async Task<StorageFile> GetStorageFile(string filepath)
        {
            return await StorageFile.GetFileFromPathAsync(filepath);
        }

        /// <summary>
        /// 再開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (_mediaPlayerElement.MediaPlayer is null)
            {
                return;
            }

            _mediaPlayerElement.MediaPlayer.Play();

        }

        /// <summary>
        /// 再生速度コンボボックスのアイテムチェンジ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPlaybackRate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPlaybackRate();
        }

        /// <summary>
        /// 再生速度を設定する
        /// </summary>
        private void SetPlaybackRate()
        {
            if (_mediaPlayerElement.MediaPlayer is null)
            {
                return;
            }

            ComboBoxItem item = cmbPlaybackRate.SelectedItem as ComboBoxItem;
            if (item is null
                || !(item.Content is string str))
            {
                return;
            }

            var session = _mediaPlayerElement.MediaPlayer.PlaybackSession;

            if (double.TryParse(str, out double value))
            {
                session.PlaybackRate = value;
            }
        }

        /// <summary>
        /// 再生位置を設定する
        /// </summary>
        /// <param name="value"></param>
        private void SetPosition(double value)
        {
            if (_mediaPlayerElement.MediaPlayer is null)
            {
                return;
            }

            var session = _mediaPlayerElement.MediaPlayer.PlaybackSession;
            session.Position += TimeSpan.FromSeconds(value);
        }

        /// <summary>
        /// 30＜＜
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetPosition(-30);
        }

        /// <summary>
        /// 90＜＜
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            SetPosition(-90);
        }

        /// <summary>
        /// ＞＞30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SetPosition(30);
        }

        /// <summary>
        /// ＞＞90
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            SetPosition(90);
        }
    }
}
