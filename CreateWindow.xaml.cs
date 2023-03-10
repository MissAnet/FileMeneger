using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        string path_;
        MainWindow mWind2 = new MainWindow();
        public CreateWindow(MainWindow mWind)
        {
            InitializeComponent();
            path_ = mWind.textBox_path.Text;
            mWind2 = mWind;
        }

        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {
            Create(path_);
        }

        //create
        public void Create(string path_)
        {
            if (textBox_TypeFile.Text != "")
            {
                string path = path_ + "/" + textBox_NameFile.Text + "." + textBox_TypeFile.Text;
                if (File.Exists(path))
                {
                    MessageBox.Show("Файл с таким именем уже суещствует");
                }
                else
                {
                    File.Create(path);
                }
            }
            else
            {
                string path = path_ + "/" + textBox_NameFile.Text;
                if (Directory.Exists(path))
                {
                    MessageBox.Show("Папка с таким именем уже суещствует");
                }
                else
                {
                    Directory.CreateDirectory(path);
                }
            }
            mWind2.loadFileDir(path_);
            Close();
        }
    }
}
