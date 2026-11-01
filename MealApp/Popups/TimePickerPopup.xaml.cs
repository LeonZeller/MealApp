using Vapolia.WheelPickers;

namespace MealApp.Popups;

public class  TimePickerResult
{
    public TimeSpan Time { get; set; }
    public int Persons { get; set; }
}

public partial class TimePickerPopup : ContentPage
{
    public TaskCompletionSource<TimePickerResult?> ResultTask { get; } = new();
    public List<List<string>> WheelItems { get; }

    public TimePickerPopup(TimeSpan initialTime)
    {
        var hours = Enumerable.Range(0, 24).Select(h => h.ToString("00")).ToList();
        var minutes = Enumerable.Range(0, 12).Select(m => (m * 5).ToString("00")).ToList();

        WheelItems = new List<List<string>> { hours, minutes };

        BindingContext = this;
        InitializeComponent();

        int closestMinuteIndex = (int)Math.Round(initialTime.Minutes / 5.0);
        if (closestMinuteIndex >= minutes.Count) closestMinuteIndex = minutes.Count - 1;

        selectedHourIndex = initialTime.Hours;
        selectedMinuteIndex = closestMinuteIndex;

        TimeWheelPicker.SelectedItemsIndex = new List<int> { selectedHourIndex, selectedMinuteIndex };
    }

    private void OnSelectionChanged(object sender, WheelChangedEventArgs e)
    {
        if (e.WheelIndex == 0)
            selectedHourIndex = e.SelectedItemIndex;
        else if (e.WheelIndex == 1)
            selectedMinuteIndex = e.SelectedItemIndex;
    }

    private async void OnOkClicked(object sender, EventArgs e)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"Hour: {selectedHourIndex}, Min: {selectedMinuteIndex}, Persons: {persons}");

            ResultTask.SetResult(new TimePickerResult
            {
                Time = new TimeSpan(selectedHourIndex, selectedMinuteIndex * 5, 0),
                Persons = persons
            });
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"FEHLER: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"STACK: {ex.StackTrace}");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        ResultTask.SetResult(null);
        await Navigation.PopModalAsync();
    }

    private void OnPersonsMinusClicked(object sender, EventArgs e)
    {
        if (persons > 1)
        {
            persons--;
            PersonsLabel.Text = persons.ToString();
        }
    }

    private void OnPersonsPlusClicked(object sender, EventArgs e)
    {
        if (persons < 20)
        {
            persons++;
            PersonsLabel.Text = persons.ToString();
        }
    }

    private int selectedHourIndex;
    private int selectedMinuteIndex;
    private int persons = 1;

}