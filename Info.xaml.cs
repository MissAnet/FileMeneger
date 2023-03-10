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
    /// Логика взаимодействия для Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public Info(FileInfo file)
        {
            InitializeComponent();

            lab_Size.Content = lab_Size.Content + (file.Length/1024).ToString();
            lab_Name.Content = lab_Name.Content + (file.FullName).ToString();
            lab_TimeCreate.Content = lab_TimeCreate.Content + (file.CreationTime).ToString();
            lab_WriteTime.Content = lab_WriteTime.Content + (file.LastWriteTime).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
