using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using JsonEditor.Models;
using JsonEditor.Utilities;
using Microsoft.Maui.Storage;

namespace JsonEditor
{
    public partial class MainPage : ContentPage
    {
        private readonly EventManager _eventManager;

        public MainPage()
        {
            InitializeComponent();
            _eventManager = new EventManager();
        }

        private async void OnOpenClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Оберіть JSON файл",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".json" } },
                    })
                });

                if (result != null)
                {
                    FilePathLabel.Text = result.FileName;
                    await _eventManager.LoadFromFile(result.FullPath);
                    UpdateListView();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Не вдалося відкрити файл: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                await _eventManager.SaveToFile();
                await DisplayAlert("Успіх", "Файл успішно збережено!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Не вдалося зберегти: {ex.Message}", "OK");
            }
        }

        private void UpdateListView(List<SPF_Event> source = null)
        {
            var data = source ?? _eventManager.GetAll();
            EventsList.ItemsSource = new ObservableCollection<SPF_Event>(data);
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            string term = SearchEntry.Text;
            string criteria = SortPicker.SelectedItem?.ToString();
            var results = _eventManager.Search(term, criteria);
            UpdateListView(results);
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            if (!_eventManager.IsFileLoaded)
            {
                await DisplayAlert("Увага", "Будь ласка, спершу відкрийте або створіть JSON файл!", "OK");
                return; 
            }

            var editor = new EventEditorPage(null);
            editor.OnSave += (newEvent) =>
            {
                _eventManager.Add(newEvent);
                UpdateListView();
            };
            await Navigation.PushAsync(editor);
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var eventToEdit = button.CommandParameter as SPF_Event;
            var editor = new EventEditorPage(eventToEdit);
            editor.OnSave += (updatedEvent) =>
            {
                _eventManager.Update(updatedEvent);
                UpdateListView();
            };
            await Navigation.PushAsync(editor);
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var eventToDelete = button.CommandParameter as SPF_Event;

            bool answer = await DisplayAlert("Видалення", $"Видалити '{eventToDelete.Event_name}'?", "Так", "Ні");
            if (answer)
            {
                _eventManager.Delete(eventToDelete.Event_id);
                UpdateListView();
            }
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Інфо", "Лабораторна робота №3\nТема: JSON Editor\nСтудент: Директоренко Ярослав\nгрупа К-26", "OK");
        }

        private async void ExitButton_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Вихід з програми",
                "Чи дійсно ви хочете завершити роботу з програмою?",
                "Так", "Ні");

            if (confirm)
            {
                Application.Current.Quit();
            }
        }
    }
        

}