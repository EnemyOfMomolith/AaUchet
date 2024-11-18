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
using System.Windows.Shapes;

namespace AaUchet
{
    /// <summary>
    /// Логика взаимодействия для prepodPage.xaml
    /// </summary>
    public partial class prepodPage : Page
    {
        public prepodPage()
        {
            InitializeComponent();

            var currentPrepod = UchetUSPEntities.GetContext().Преподаватели.ToList();
            PrepodListView.ItemsSource = currentPrepod;


            DepartmentComboBox.SelectedIndex = 0;

            Update();
            PrepodListView.Items.Refresh();
        }
        private void Update()
        {
            var currentPrepod = UchetUSPEntities.GetContext().Преподаватели.ToList();

            var start_count = currentPrepod.Count;

            if (DepartmentComboBox.SelectedIndex == 0)
            {
                currentPrepod = currentPrepod;
            }
            if (DepartmentComboBox.SelectedIndex == 1)
            {
                currentPrepod = currentPrepod.Where(p => (p.КодКафедры == 2)).ToList();
            }
            if (DepartmentComboBox.SelectedIndex == 2)
            {
                currentPrepod = currentPrepod.Where(p => (p.КодКафедры == 3)).ToList();
            }
            if (DepartmentComboBox.SelectedIndex == 3)
            {
                currentPrepod = currentPrepod.Where(p => (p.КодКафедры == 4)).ToList();
            }
            if (DepartmentComboBox.SelectedIndex == 4)
            {
                currentPrepod = currentPrepod.Where(p => (p.КодКафедры == 5)).ToList();
            }
            if (DepartmentComboBox.SelectedIndex == 5)
            {
                currentPrepod = currentPrepod.Where(p => (p.КодКафедры == 6)).ToList();
            }

            currentPrepod = currentPrepod.Where(p => p.Имя.ToLower().Contains(SearchBox.Text.ToLower()) ||
            p.Фамилия.ToLower().Contains(SearchBox.Text.ToLower()) ||
            p.Отчество.ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            if (SortAscRadioButton.IsChecked.Value)
            {
                currentPrepod = currentPrepod.OrderBy(p => p.Стаж).ToList();

            }
            if (SortDescRadioButton.IsChecked.Value)
            {
                currentPrepod = currentPrepod.OrderByDescending(p => p.Стаж).ToList();
            }

            TBCount.Text = string.Format("показано преподавателей {0} из {1}", currentPrepod.Count, start_count);

            PrepodListView.ItemsSource = currentPrepod;




        }

        private void AddPrepod_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new AddEditPage(null));
            Update();
            PrepodListView.Items.Refresh();
        }

        private void editPrepod_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Преподаватели));
            Update();
            PrepodListView.Items.Refresh();
        }

        private void deletePrepod_Click(object sender, RoutedEventArgs e)
        {
            var currentPrepod = (sender as Button).DataContext as Преподаватели;

            if (currentPrepod.Стаж < 1)
            {
                MessageBox.Show("Нельзя удалить преподавателя со стажем меньше 1 года.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (MessageBox.Show("Вы точно хотите удалить?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                    UchetUSPEntities.GetContext().Преподаватели.Remove(currentPrepod);
                    UchetUSPEntities.GetContext().SaveChanges();
                    PrepodListView.ItemsSource = UchetUSPEntities.GetContext().Преподаватели.ToList();
                    Update();
            }

         
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
            PrepodListView.Items.Refresh();

        }

        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
            PrepodListView.Items.Refresh();

        }

        private void SortAscRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Update();
            PrepodListView.Items.Refresh();

        }

        private void SortDescRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Update();
            PrepodListView.Items.Refresh();

        }

        private void PrepodListView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                UchetUSPEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                PrepodListView.ItemsSource = UchetUSPEntities.GetContext().Преподаватели.ToList();
                Update();
            }
        }

        private void PrepodListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
            PrepodListView.Items.Refresh();
        }
    }
}
