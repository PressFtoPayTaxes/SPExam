using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace SPExam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int[] array;
        delegate void FileDownloadingDelegate(Uri address, string fileName);

        private void ArrayButtonClick(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if (!int.TryParse(countTextBox.Text, out count))
                return;

            if (count <= 0)
                return;

            var threads = new Thread[count];
            array = new int[count];

            for (int i = 0; i < count; i++)
            {
                threads[i] = new Thread(WriteNumberInArray);
                threads[i].Start(i + 1);
            }
        }

        private void WriteNumberInArray(object numberToWrite)
        {
            lock (this)
            {
                var number = (int)numberToWrite;
                array[number - 1] = number;

                if (number == array.Length)
                {
                    ShowArray();
                }
            }
        }

        private void ShowArray()
        {
            string stringArray = "";

            foreach (var number in array)
            {
                stringArray += number;
                stringArray += " ";
            }

            MessageBox.Show("Получившийся массив: " + stringArray);
        }

        private async void DownloadButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(urlTextBox.Text))
                return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ShowDialog();

            string url = null;
            Dispatcher.Invoke(() => url = urlTextBox.Text);


            using (var client = new WebClient())
            {
                try
                {
                    FileDownloadingDelegate fileDelegate = client.DownloadFile;
                    fileDelegate.BeginInvoke(new Uri(url), dialog.FileName, ShowSuccessMessageBox, null);
                }
                catch (WebException exception)
                {
                    MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            using (var context = new DataContext())
            {
                context.FilesDownloadingInfo.Add(new FileDownloadingInfo
                {
                    FileUrl = urlTextBox.Text,
                    SaveDirectory = dialog.FileName
                });

                await context.SaveChangesAsync();
            }
        }

        private void ShowSuccessMessageBox(IAsyncResult asyncResult)
        {
            MessageBox.Show("Файл загружен успешно");
        }
    }
}
