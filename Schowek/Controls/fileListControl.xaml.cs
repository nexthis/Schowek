using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Schowek.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy fileListControl.xaml
    /// </summary>
    /// 
    /// <TODO>
    /// LAzy Load foldery tylko do drugiego zagnieżdzenia 
    /// </TODO>
    public partial class fileListControl : UserControl
    {
        public fileListControl()
        {
            InitializeComponent();
        }
        public StringCollection allFile = null;
        public fileListControl(StringCollection files)
        {
            InitializeComponent();
            ladFileList(files);
            allFile = files;
        }

        double fileLengh = 0;

        private void ladFileList(StringCollection files)
        {
            foreach (var file in files)
            {
                if (Directory.Exists(file))
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = new DirectoryInfo(file).Name;
                    item.Tag = file;
                    item.Selected += treeItem_Selected;
                    GetFolder(file, ref item);
                    GetFiles(file, ref item);


                    fileLengh += DirSize (new DirectoryInfo(file));

                    fileListTreeView.Items.Add(item);
                }
                else
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = GetFile(file);

                    fileLengh += new FileInfo(file).Length;

                    fileListTreeView.Items.Add(item);
                }
            }
            fileSizeTextBlock.Text = GetFileSize(fileLengh);
        }

        public string GetFileSize(double fileLength)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = fileLength;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        private void GetFiles (string path,ref TreeViewItem viewItem)
        {
            foreach (var files in Directory.GetFiles(path))
            {
                viewItem.Items.Add(GetFile(files)); //Path.GetFileName(files) 
            }
        }

        private void GetFolder(string path, ref TreeViewItem viewItem)
        {
            foreach (var files in Directory.GetDirectories(path))
            {
                var iteam = new TreeViewItem();
                iteam.Header = new DirectoryInfo(files).Name;
                iteam.Tag = files;
                iteam.Selected += treeItem_Selected;
                GetFolder(files, ref iteam);
                GetFiles(files, ref iteam);
                viewItem.Items.Add(iteam);
            }
        }

        async void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            MainWindow.AppWindow.ShowSnackbar();
            try
            {
                MainWindow.OffClipboard = true;
                StringCollection collection = new StringCollection();
                // MessageBox.Show(fileListTreeView.SelectedItem.ToString());
                if (fileListTreeView.SelectedItem.GetType() == typeof(StackPanel))
                {
                    collection.Add((string)(fileListTreeView.SelectedItem as StackPanel).Tag);
                }
                else
                {
                    collection.Add((string)(fileListTreeView.SelectedItem as TreeViewItem).Tag);
                }

                Clipboard.SetFileDropList(collection);
                await Task.Delay(100);
                MainWindow.OffClipboard = false;
            }
            catch
            {

            }
        }


        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private StackPanel GetFile(string path)
        {
            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            var image = new Image();
            image.Width = 16;
            image.Height = 16;
            image.Source = Icon(path);
            stack.Tag = path;
            var text = new TextBlock();
            text.Margin = new Thickness(8, 0, 0, 0);
            text.Text = Path.GetFileName(path);
            stack.Children.Add(image);
            stack.Children.Add(text);
            return stack;
        }


        public System.Windows.Media.ImageSource Icon(string path)
        {

            using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(path))
            {
               var icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                            sysicon.Handle,
                            System.Windows.Int32Rect.Empty,
                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                return icon;
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppWindow.listBox.Items.Remove(this);
            System.GC.Collect();
        }


    }
}
