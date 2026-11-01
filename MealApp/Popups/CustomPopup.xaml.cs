namespace MealApp.Popups;

public partial class CustomPopup : ContentPage
{
	public TaskCompletionSource<string?> ResultTask { get; } = new();
	public CustomPopup(string title, string? message, string? icon, 
		List<(string Text, string Value, bool IsDestructive)> options)
	{
		InitializeComponent();

		TitleLabel.Text = title;

        if (string.IsNullOrEmpty(message))
            MessageLabel.IsVisible = false;
        else
            MessageLabel.Text = message;

        if (string.IsNullOrEmpty(icon))
            IconLabel.IsVisible = false;
        else
            IconLabel.Text = icon;

        foreach (var option in options)
        {
            var button = new Button
            {
                Text = option.Text,
                BackgroundColor = option.IsDestructive ? Color.FromArgb("#C53030") : Color.FromArgb("#C0533A"),
                TextColor = Colors.White,
                CornerRadius = 8,
                Padding = new Thickness(14, 12),
                MinimumHeightRequest = 44
            };
            button.Clicked += async (s, e) =>
            {
                ResultTask.SetResult(option.Value);
                await Navigation.PopModalAsync();
            };
            ButtonContainer.Children.Add(button);
        }
    }
}