using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schowek.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy imageControl.xaml
    /// </summary>
    public partial class imageControl : UserControl
    {
        Stream stream;
         BitmapSource paseToClipboardBitmap;
        public string paseToClipboardFilePath;
        public string paseToClipboardURL;


        public imageControl(BitmapSource bitmap,string html)
        {
            InitializeComponent();
            img.Source = bitmap;
            paseToClipboardBitmap = bitmap;
            paseToClipboardURL=Regex.Match(html, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;

        }
        ~imageControl()
        {
            
        }

        public imageControl(string filePath)
        {
            InitializeComponent();
            stream = File.Open(filePath, FileMode.Open);
            BitmapImage imgsrc = new BitmapImage();
            imgsrc.BeginInit();
            imgsrc.StreamSource = stream;
            imgsrc.CacheOption = BitmapCacheOption.OnLoad;
            imgsrc.EndInit();
            img.Source = imgsrc; //new BitmapImage(new Uri(filePath, UriKind.Absolute)); ;
            paseToClipboardFilePath = filePath;
            var fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = System.IO.Path.GetDirectoryName(filePath);
            fileSystemWatcher.Filter = System.IO.Path.GetFileName(filePath);
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Deleted += deletedFile;
            stream.Close();
        }

        private void deletedFile(object sender, FileSystemEventArgs e)
        {
            //stream.Close();
            this.Dispatcher.Invoke(() =>
            {

                MainWindow.AppWindow.listBox.Items.Remove(this);

                var name = this.GetValue(NameProperty).ToString();
                try
                {
                    NameScope.GetNameScope(this).UnregisterName(name);
                }
                catch
                {

                }

            });

        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //if(stream != null)  stream.Close();

            MainWindow.AppWindow.listBox.Items.Remove(this);

            System.GC.Collect();

        }


        private async void RippleEffect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.AppWindow.ShowSnackbar();
            try
            {
                MainWindow.OffClipboard = true;
                if (paseToClipboardBitmap != null)
                {
                    Clipboard.SetImage(paseToClipboardBitmap);
                }
                else
                {
                    StringCollection collection = new StringCollection();
                    collection.Add(paseToClipboardFilePath);
                    Clipboard.SetFileDropList(collection);
                }
                await Task.Delay(100);
                MainWindow.OffClipboard = false;
            }
            catch
            {

            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(paseToClipboardURL);
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }


}