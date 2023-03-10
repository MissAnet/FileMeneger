using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Reflection;
using System.IO.Packaging;
using System.IO.Pipes;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path_ = "";
        string path_copy = "";
        string name_copy = "";
        bool isFile = false;

        public MainWindow()
        {
            InitializeComponent();
                
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadFileDir(path_);
        }

        public void loadFileDir(string path)
        {
            DirectoryInfo directoryInfo;
            try
            {
                listV_Main.Items.Clear();
                textBox_path.Text = path;

                if (path == "")
                {
                    string[] disks = Environment.GetLogicalDrives();
                    for (int i = 0; i < disks.Length; i++)
                    {
                        listV_Main.Items.Add(disks[i]);
                    }
                }
                else
                {
                    directoryInfo = new DirectoryInfo(path);

                    DirectoryInfo[] directories = directoryInfo.GetDirectories();
                    FileInfo[] files = directoryInfo.GetFiles();


                    for (int i = 0; i < files.Length; i++)
                    {
                        listV_Main.Items.Add(files[i].Name);
                    }
                    for (int i = 0; i < directories.Length; i++)
                    {
                        listV_Main.Items.Add(directories[i].Name);
                    }
                }
            }
            catch 
            {
                MessageBox.Show("Не удалось загрузить" , "Ошибка");
            }
        }

        //doubleclick / open
        private void listV_Main_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (listV_Main.SelectedItem != null)
                {
                    FileAttributes fileAttributes;
                    string path = "";
                    if ((listV_Main.SelectedItem.ToString()).EndsWith(@"\"))
                        path = (listV_Main.SelectedItem.ToString()).Substring(0, (listV_Main.SelectedItem.ToString()).LastIndexOf(@"\")) + "/";

                    if (path != "")
                        fileAttributes = File.GetAttributes(path);
                    else
                        fileAttributes = File.GetAttributes(path_ + listV_Main.SelectedItem.ToString());

                    if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        if ((listV_Main.SelectedItem.ToString()).EndsWith(@"\"))
                            path_ = path_ + (listV_Main.SelectedItem.ToString()).Substring(0, (listV_Main.SelectedItem.ToString()).LastIndexOf(@"\")) + "/";
                        else
                            path_ = path_ + listV_Main.SelectedItem.ToString() + "/";
                        textBox_path.Text = path_;
                        loadFileDir(path_);
                    }
                    else
                    {
                        try
                        {
                            Process.Start(path_ + listV_Main.SelectedItem.ToString());
                        }
                        catch
                        {
                            MessageBox.Show("Не удалось открыть файл.", "Ошибка");
                        }
                    }
                }
            }
            catch { loadFileDir(path_); }
        }

        //goback
        private void btn_GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (path_.EndsWith("/"))
                path_ = path_.Remove(path_.Length - 1);
            if (path_.EndsWith(":"))
                path_ = "";
            try
            {
                path_ = path_.Substring(0, path_.LastIndexOf("/"));
                path_ += "/";
                loadFileDir(path_);
            }
            catch
            {
                loadFileDir("");
            }
        }

        //hot-keyboard
        private void listV_Main_KeyDown(object sender, KeyEventArgs e)
        {
            //ctrl + c
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.C)
            {
                MetodCopy();
            }
            //ctrl + v
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                MetodPaste();
            }
        }

        //delete
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (listV_Main.SelectedItem != null && path_ != "")
            {
                FileAttributes fileAttributes = File.GetAttributes(path_ + listV_Main.SelectedItem.ToString());
                if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    Directory.Delete(path_ + listV_Main.SelectedItem.ToString(),true);
                    loadFileDir(path_);
                }
                else
                {
                    File.Delete(path_ + listV_Main.SelectedItem.ToString());
                    loadFileDir(path_);
                }
            }
        }

        //create
        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {
            CreateWindow createWindow = new CreateWindow(this);
            createWindow.ShowDialog();
        }

        //info
        private void btn_ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            FileAttributes fileAttributes = File.GetAttributes(path_ + listV_Main.SelectedItem.ToString());

                if (listV_Main.SelectedItem != null & (fileAttributes & FileAttributes.Directory) != FileAttributes.Directory)
            {
                FileInfo file = new FileInfo(path_ + listV_Main.SelectedItem.ToString());
                Info info = new Info(file);
                info.Show();
            }
        }


        //copy
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            MetodCopy();
        }

        //paste
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            MetodPaste();
        }

        //методы
        public void MetodCopy()
        {
            try
            {
                if (path_ != "")
                {
                    path_copy = path_;
                    name_copy = listV_Main.SelectedItem.ToString();

                    FileAttributes fileAttributes = File.GetAttributes(path_ + listV_Main.SelectedItem.ToString());

                    if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                        isFile = false;
                    else isFile = true;

                    //буфер
                    System.Collections.Specialized.StringCollection collection = new System.Collections.Specialized.StringCollection();
                    collection.Add(path_ + listV_Main.SelectedItem.ToString());
                    Clipboard.SetFileDropList(collection);
                }
                else
                { 
                    MessageBox.Show("Тут никак"); 
                }
            }
            catch { }
        }
        public void MetodPaste()
        {
            if (path_ != "")
            {
                try
                {
                    if (isFile == true)
                        File.Move(path_copy + "/" + name_copy, path_ + "/" + name_copy);
                    else
                        Directory.Move(path_copy + "/" + name_copy, path_ + "/" + name_copy);
                    loadFileDir(path_);
                }
                catch { };
            }
        }

        //архив
        private void btn_Archiving_Click(object sender, RoutedEventArgs e)
        {
            if (path_ != "")
            {
                string path = path_ + listV_Main.SelectedItem.ToString();
                try
                {
                    FileAttributes fileAttributes = File.GetAttributes(path_ + listV_Main.SelectedItem.ToString());

                    if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        ZipFile.CreateFromDirectory(path, path + ".zip");
                        Directory.Delete(path, true);
                    }
                    else
                    {
                        path = path.Substring(0, path.LastIndexOf("."));
                        ZipFile.CreateFromDirectory(path, path + ".zip");
                    }
                    loadFileDir(path_);
                }
                catch { };
            }
        }

        //rename
        private void rename_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (path_ != "")
                {
                    FileAttributes fileAttributes = File.GetAttributes(path_ + listV_Main.SelectedItem.ToString());

                    if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                        isFile = false;
                    else isFile = true;
                    string extension = "";
                    string name = "";
                    if (isFile)
                    {
                        extension = System.IO.Path.GetExtension(path_ + listV_Main.SelectedItem.ToString());
                        name = listV_Main.SelectedItem.ToString().Substring(0, listV_Main.SelectedItem.ToString().LastIndexOf("."));
                    }
                    else
                        name = listV_Main.SelectedItem.ToString();

                    Rename rename = new Rename(name, path_, extension);
                    rename.Show();

                    loadFileDir(path_);
                }
            }
            catch { }
        }
    }
}