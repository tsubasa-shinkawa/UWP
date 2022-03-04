using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UWPMediaPlayer2.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UWPMediaPlayer2.ViewModels
{
    public class MediaPlayerViewModel : ObservableObject
    {
        #region プロパティ

        #region ファイル名
        private string _FileName;
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName
        {
            get => _FileName;
            set => SetProperty(ref _FileName, value);
        }
        #endregion

        #region MediaPlayerのSource
        private IMediaPlaybackSource _source;
        /// <summary>
        /// MediaPlayerのSource
        /// </summary>
        public IMediaPlaybackSource Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }
        #endregion

        #region ページ
        private MediaPlayerPage _MediaPlayerPage;
        /// <summary>
        /// ページ
        /// </summary>
        public MediaPlayerPage MediaPlayerPage
        {
            get => _MediaPlayerPage;
            set => SetProperty(ref _MediaPlayerPage, value);
        }

        #endregion

        #endregion

        #region コマンド

        #region ファイル選択
        private ICommand _FileSelectCommand;
        /// <summary>
        /// ファイル選択
        /// </summary>
        public ICommand FileSelectCommand =>
            _FileSelectCommand ?? (_FileSelectCommand = new RelayCommand(ExecuteFileSelectCommand));

        private async void ExecuteFileSelectCommand()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.FileTypeFilter.Add(".mp4");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                FileName = file.Path;

                // ファイルを選択していたら開く。
                using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
                {
                    Source = MediaSource.CreateFromStream(stream, file.Path);

                    MediaPlayerPage.MediaPlayer.Play();
                    MediaPlayerPage.MediaPlayer.PlaybackSession.PlaybackStateChanged -= MediaPlayerPage.PlaybackSession_PlaybackStateChanged;
                    MediaPlayerPage.MediaPlayer.PlaybackSession.PlaybackStateChanged += MediaPlayerPage.PlaybackSession_PlaybackStateChanged;
                }
            }
            else
            {
            }
        }
        #endregion

        #region 再開
        private ICommand _PlayCommand;
        public ICommand PlayCommand => _PlayCommand ?? (_PlayCommand = new RelayCommand(ExecutePlayCommand));

        private void ExecutePlayCommand()
        {
            if (MediaPlayerPage.MediaPlayer is null)
            {
                return;
            }

            MediaPlayerPage.MediaPlayer.Play();
        }
        #endregion

        #region 停止
        private ICommand _StopCommand;
        public ICommand StopCommand => _StopCommand ?? (_StopCommand = new RelayCommand(ExecuteStopCommand));

        private void ExecuteStopCommand()
        {
            if (MediaPlayerPage.MediaPlayer is null)
            {
                return;
            }

            MediaPlayerPage.MediaPlayer.Pause();
        }
        #endregion

        #region 再生位置コマンド
        private ICommand _PositionCommand;

        public ICommand PositionCommand => _PositionCommand ?? (_PositionCommand = new RelayCommand<object>(ExecutePositionCommand));

        private void ExecutePositionCommand(object o)
        {
            SetPosition(o);
        }
        #endregion

        #endregion

        #region コンストラクタ
        public MediaPlayerViewModel()
        {
            //Source = MediaSource.CreateFromUri(new Uri(DefaultSource));
            //PosterSource = DefaultPoster;
        }
        #endregion

        /// <summary>
        /// 再生位置を設定する
        /// </summary>
        /// <param name="value"></param>
        private void SetPosition(object obj)
        {
            if(!(obj is string str)
               || !double.TryParse(str, out double value))
            {
                return;
            }

            if (MediaPlayerPage.MediaPlayer is null)
            {
                return;
            }

            var session = MediaPlayerPage.MediaPlayer.PlaybackSession;
            session.Position += TimeSpan.FromSeconds(value);
        }

        public void DisposeSource()
        {
            var mediaSource = Source as MediaSource;
            mediaSource?.Dispose();
            Source = null;
        }
    }
}
