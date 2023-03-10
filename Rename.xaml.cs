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
    /// Логика взаимодействия для Rename.xaml
    /// </summary>
    public partial class Rename : Window
    {
        string _name;
        string _extension;
        string path_;
        public Rename(string name,string path,string extension)
        {
            InitializeComponent();
            _name = name;
            path_ = path;
            _extension = extension;
            textBox_TeName.Text = _name;
        }

        private void reName_Click(object sender, RoutedEventArgs e)
        {
                MessageBox.Show(path_ + _name + _extension);
                MessageBox.Show(path_ + textBox_TeName.Text + _extension);

                if (_extension != "")
                    File.Move(path_ + _name + _extension, path_ + textBox_TeName.Text + _extension);
                else
                    Directory.Move(path_ + _name, path_ + textBox_TeName.Text);
                Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
