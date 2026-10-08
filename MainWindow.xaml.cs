using System.Collections.ObjectModel;
using System.Windows;
using WpfCrudApp.Models;
using WpfCrudApp.Services;

namespace WpfCrudApp
{
    public partial class MainWindow : Window
    {
        private readonly PersonService _service = new();
        private ObservableCollection<Person> _people = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = _service.GetAll();
                _people = new ObservableCollection<Person>(list);
                PeopleGrid.ItemsSource = _people;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось загрузить данные.\n\nПроверьте:\n" +
                    $"1. Запущен ли SQLExpress\n" +
                    $"2. Выполнен ли скрипт CreateDatabase.sql\n" +
                    $"3. Правильность строки подключения в PersonService.cs\n\n" +
                    $"Ошибка: {ex.Message}",
                    "Ошибка подключения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newPerson = new Person
                {
                    FullName = "Новый человек",
                    Age = 18,
                    Email = "",
                    Phone = ""
                };

                int newId = _service.Insert(newPerson);
                newPerson.Id = newId;
                newPerson.CreatedAt = DateTime.UtcNow;

                _people.Add(newPerson);

                // Выделяем новую строку
                PeopleGrid.SelectedItem = newPerson;
                PeopleGrid.ScrollIntoView(newPerson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (PeopleGrid.SelectedItem is not Person selected)
            {
                MessageBox.Show("Выберите строку для обновления.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(selected.FullName))
            {
                MessageBox.Show("Поле «ФИО» не может быть пустым.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Фиксируем изменения из DataGrid (на случай, если ячейка ещё в режиме редактирования)
                PeopleGrid.CommitEdit(DataGridEditingUnit.Row, true);

                _service.Update(selected);

                MessageBox.Show("Запись успешно обновлена.",
                    "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (PeopleGrid.SelectedItem is not Person selected)
            {
                MessageBox.Show("Выберите строку для удаления.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить запись «{selected.FullName}» (Id = {selected.Id})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _service.Delete(selected.Id);
                _people.Remove(selected);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
