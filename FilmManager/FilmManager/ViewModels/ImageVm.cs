using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FilmManagerCore;

namespace FilmManager.ViewModels
{
    public class ImageVm : Notifier
    {
        static Dictionary<string, ImageSource> _imagesCache = new Dictionary<string, ImageSource>();
        ImageSource _source;
        bool _loadingError;
        bool _isOpened;
        string _text;
        BitmapImage _currentImage;

        public ImageSource Source
        {
            get
            {
                return _source;
            }

            set
            {
                if (_source != value)
                {
                    _source = value;
                    OnPropertyChanged(nameof(Source));
                    OnPropertyChanged(nameof(Loading));
                }
            }
        }

        public bool LoadingError
        {
            get
            {
                return _loadingError;
            }

            set
            {
                if (_loadingError = value)
                {
                    _loadingError = value;
                    OnPropertyChanged(nameof(LoadingError));
                    OnPropertyChanged(nameof(Loading));
                }
            }
        }

        public bool Loading
        {
            get
            {
                return _source != null && _source is BitmapImage && ((BitmapImage)_source).IsDownloading;
            }
        }

        public bool IsOpened
        {
            get
            {
                return _isOpened;
            }

            set
            {
                if (_isOpened != value)
                {
                    _isOpened = value;
                    OnPropertyChanged(nameof(IsOpened));
                }

            }
        }

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }

            }
        }

        public void LoadImage(string url)
        {
            OnPropertyChanged(nameof(LoadingError));
            IsOpened = true;
            if (_imagesCache.ContainsKey(url))
                Source = _imagesCache[url];
            else
            {
                try
                {
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        _currentImage = new BitmapImage();
                        _currentImage.BeginInit();
                        
                        _currentImage.CacheOption = BitmapCacheOption.OnLoad;
                        _currentImage.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                        _currentImage.DownloadFailed += Bitmap_DownloadFailed;
                        _currentImage.DownloadCompleted += Bitmap_DownloadCompleted;
                        _currentImage.EndInit();
                        
                        
                        Source = _currentImage;
                        OnPropertyChanged(nameof(Loading));
                    }));
                    
                }
                catch (Exception)
                {
                    LoadingError = true;
                }
                finally
                {
                    OnPropertyChanged(nameof(Loading));
                }
            }
        }

        void Bitmap_DownloadCompleted(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Loading));
            var url = _currentImage.UriSource?.ToString();
            if (!string.IsNullOrWhiteSpace(url))
                _imagesCache[url] = Source;
        }

        void Bitmap_DownloadFailed(object sender, ExceptionEventArgs e)
        {
            if (Source == _currentImage)
            {
                LoadingError = true;
            }
        }

        public async Task LoadImageAsync(string url)
        {
            await Task.Run(() => LoadImage(url));
        }

        public void Hide()
        {
            IsOpened = false;
            LoadingError = false;
            Source = null;
        }
    }
}
