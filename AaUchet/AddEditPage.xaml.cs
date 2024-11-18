using Microsoft.Win32;
using System;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AaUchet
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Преподаватели _currentPrepod = new Преподаватели();
        public AddEditPage(Преподаватели selectedPrepod)
        {
            InitializeComponent();
            if (selectedPrepod != null)
            {
                _currentPrepod = selectedPrepod;
                ComboType.SelectedIndex = _currentPrepod.КодКафедры - 2;
            }

            DataContext = _currentPrepod;
        }

        private void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {

                FileInfo fileInfo = new FileInfo(myOpenFileDialog.FileName);


                if (fileInfo.Length < 2 * 1024 * 1024)
                {
                    _currentPrepod.Фото = myOpenFileDialog.FileName;
                    _photo.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
                }
                else
                {

                    MessageBox.Show("Размер файла превышает 2 мегабайта. Выберите другой файл.");
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder err = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentPrepod.Имя))
                err.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(_currentPrepod.Фамилия))
                err.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(_currentPrepod.Отчество))
                err.AppendLine("Укажите отчество");
            if (string.IsNullOrWhiteSpace(_currentPrepod.Должность))
                err.AppendLine("Укажите должность");
                if(DateTime.TryParse(tb_experience.Text, out DateTime newDate))
                {
                    _currentPrepod.ГодПоступления = newDate;
                }
                else{
                    err.AppendLine("Укажите дату выхода на работу");
                }
            if (string.IsNullOrWhiteSpace(_currentPrepod.Адрес))
                err.AppendLine("Укажите адрес");
            if (GenderComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите пол.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ComboType.SelectedItem == null)
                err.AppendLine("Укажите Кафедру ");
            else
            {
                _currentPrepod.КодКафедры = ComboType.SelectedIndex + 2;
            }

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString());
                return;
            }

            if (_currentPrepod.КодПреподавателя == 0)
                _currentPrepod.КодПреподавателя = UchetUSPEntities.GetContext().Преподаватели.Select(p=>p.КодПреподавателя).Max()+1;
                UchetUSPEntities.GetContext().Преподаватели.Add(_currentPrepod);

            try
            {
                _currentPrepod.Пол = GenderComboBox.Text;
                _currentPrepod.Телефон = "7(937)964-02-58";
                _currentPrepod.ГодРождения = DateTime.Parse("12.12.2000");
                _currentPrepod.Город = "test";
                UchetUSPEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                manager.MainFrame.GoBack();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
