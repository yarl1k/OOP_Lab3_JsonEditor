using System;
using JsonEditor.Models;
using JsonEditor.Utilities;

namespace JsonEditor
{
    public partial class EventEditorPage : ContentPage
    {
        private SPF_Event _tempEvent;
        public event Action<SPF_Event> OnSave;

        public EventEditorPage(SPF_Event existingEvent)
        {
            InitializeComponent();

            if (existingEvent != null)
            {
                Title = "Редагування";
                _tempEvent = new SPF_Event
                {
                    Event_id = existingEvent.Event_id,
                    Event_name = existingEvent.Event_name,
                    Department = existingEvent.Department,
                    Event_date = existingEvent.Event_date,
                    Event_time = existingEvent.Event_time,
                    Event_location = existingEvent.Event_location
                };
            }
            else
            {
                Title = "Новий запис";
                _tempEvent = new SPF_Event();
            }
            BindingContext = _tempEvent;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var missing = new List<string>();

            if (string.IsNullOrWhiteSpace(_tempEvent.Event_name))
                missing.Add("назву");
            if (string.IsNullOrWhiteSpace(_tempEvent.Department))
                missing.Add("департамент");
            if (string.IsNullOrWhiteSpace(_tempEvent.Event_date))
                missing.Add("дату");
            if (string.IsNullOrWhiteSpace(_tempEvent.Event_time))
                missing.Add("час");
            if (string.IsNullOrWhiteSpace(_tempEvent.Event_location))
                missing.Add("місце");

            if (missing.Count > 0)
            {
                var msg = "Заповніть: " + string.Join(", ", missing);
                await DisplayAlert("Помилка", msg, "OK");
                return;
            }

            OnSave?.Invoke(_tempEvent);
            await Navigation.PopAsync();
        }

        private void OnDateChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string digits = new string(entry.Text.Where(char.IsDigit).ToArray());

            if (digits.Length > 8)
                digits = digits.Substring(0, 8);

            string formatted = digits.Length switch
            {
                <= 4 => digits,
                <= 6 => $"{digits.Substring(0, 4)}.{digits.Substring(4)}",
                _ => $"{digits.Substring(0, 4)}.{digits.Substring(4, 2)}.{digits.Substring(6)}"
            };

            if (formatted.Length == 10)
            {
                int year = int.Parse(formatted.Substring(0, 4));
                int month = int.Parse(formatted.Substring(5, 2));
                int day = int.Parse(formatted.Substring(8, 2));

                int currentYear = DateTime.Now.Year;

                bool valid =
                    year >= 1900 && year <= currentYear &&
                    month is >= 1 and <= 12 &&
                    day >= 1 && day <= DateTime.DaysInMonth(year, month);

                if (!valid)              
                    formatted = "";
            }

            if (entry.Text != formatted)
                entry.Text = formatted;
        }

        private void OnTimeChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string digits = new string(entry.Text.Where(char.IsDigit).ToArray());

            if (digits.Length > 4)
                digits = digits.Substring(0, 4);

            string formatted = digits.Length switch
            {
                <= 2 => digits,
                _ => $"{digits.Substring(0, 2)}:{digits.Substring(2)}"
            };

            if (formatted.Length == 5) 
            {
                int h = int.Parse(formatted.Substring(0, 2));
                int m = int.Parse(formatted.Substring(3, 2));

                if (h > 23 || m > 59)   
                    formatted = "";
            }

            if (entry.Text != formatted)
                entry.Text = formatted;
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}