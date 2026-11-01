using CommunityToolkit.Mvvm.ComponentModel;
using MealApp.Models;
using MealApp.Services;
using MealApp.ViewModels;
using System.Formats.Asn1;
using System.Threading.Tasks;

namespace MealApp.Views;

public partial class AddMealPage : ContentPage
{
    

    public AddMealPage(MealServices mealService, Meal? mealToEdit = null, bool isComponent = false)
    {
        InitializeComponent();
        viewModel = new AddMealViewModel(mealService, mealToEdit);
        viewModel.IsComponent = isComponent || (mealToEdit?.IsComponent ?? false);
        Title = viewModel.IsComponent ? "Neue Komponente" : "Neues Gericht";

        if (mealToEdit != null)
        {
            Title = viewModel.IsComponent ? "Komponente bearbeiten" : "Gericht bearbeiten";
        }

        BindingContext = viewModel;

        if (mealToEdit != null)
        {
            loadExistingIngredients(mealToEdit);
        }
        else
        {
            OnAddIngredientClicked(null!, EventArgs.Empty);
        }
    }

    

    private void loadExistingIngredients(Meal meal)
    {
        isLoading = true;

        foreach (var ingredient in meal.Ingredients)
        {
            OnAddIngredientClicked(null!, EventArgs.Empty);
            var lastEntry = ingredientEntries.Last();
            lastEntry.NameEntry.Text = ingredient.Name;
            lastEntry.AmountEntry.Text = ingredient.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture);
            lastEntry.UnitPicker.SelectedItem = ingredient.Unit;
        }

        isLoading = false;
    }

    private void OnAddIngredientClicked(object sender, EventArgs e)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
        },
            ColumnSpacing = 10
        };

        var nameEntry = new Entry { Placeholder = "Zutat" };
        var amountEntry = new Entry { Placeholder = "Menge", Keyboard = Keyboard.Numeric };
        var unitPicker = new Picker { Title = "Einheit", ItemsSource = viewModel.Units };

        grid.Add(nameEntry, 0, 0);
        grid.Add(amountEntry, 1, 0);
        grid.Add(unitPicker, 2, 0);

        var suggestionBox = new VerticalStackLayout
        {
            Spacing = 0,
            IsVisible = false,
            Margin = new Thickness(0, 2, 0, 0)
        };

        nameEntry.TextChanged += (s, args) => updateSuggestions(nameEntry, suggestionBox);
        nameEntry.Unfocused += async (s, args) =>
        {
            await Task.Delay(150);
            suggestionBox.IsVisible = false;
        };

        var wrapper = new VerticalStackLayout { Spacing = 0 };
        wrapper.Add(grid);
        wrapper.Add(suggestionBox);

        IngredientsStack.Add(wrapper);
        ingredientEntries.Add((nameEntry, amountEntry, unitPicker));
    }

    private void updateSuggestions(Entry entry, VerticalStackLayout box)
    {
        if (isLoading)
        {
            box.IsVisible = false;
            return;
        }

        box.Clear();

        var text = entry.Text?.Trim() ?? string.Empty;
        if (text.Length < 2)
        {
            box.IsVisible = false;
            return;
        }

        var components = viewModel.ComponentNames
            .Where(n => n.Contains(text, StringComparison.OrdinalIgnoreCase))
            .Select(n => (Name: n, IsComponent: true));

        var ingredients = viewModel.AllIngredientNames
            .Where(n => n.Contains(text, StringComparison.OrdinalIgnoreCase))
            .Where(n => !viewModel.ComponentNames.Contains(n, StringComparer.OrdinalIgnoreCase))
            .Select(n => (Name: n, IsComponent: false));

        var matches = components.Concat(ingredients).Take(5).ToList();

        if (matches.Count == 0)
        {
            box.IsVisible = false;
            return;
        }

        foreach (var match in matches)
        {
            var label = new Label
            {
                Text = match.IsComponent ? $"{match.Name}  🧩" : match.Name,
                FontSize = 14,
                TextColor = Color.FromArgb("#2E2723"),
                Padding = new Thickness(12, 10)
            };

            var border = new Border
            {
                Content = label,
                Stroke = Color.FromArgb("#E1E1E1"),
                StrokeThickness = 0.5,
                BackgroundColor = Colors.White,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 6 }
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                entry.Text = match.Name;
                box.IsVisible = false;
            };
            border.GestureRecognizers.Add(tap);

            box.Add(border);
        }

        box.IsVisible = true;
    }


    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result == null) return;
            
            var fileName = $"meal_{Guid.NewGuid()}.jpg";
            var newPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            using var sourceStream = await result.OpenReadAsync();
            using var desStream = File.OpenWrite(newPath);
            await sourceStream.CopyToAsync(desStream);

            viewModel.ImagePath = newPath;
            MealImage.Source = ImageSource.FromFile(newPath);
        }
        catch (PermissionException)
        {
            await PopupService.ShowErrorAsync("Berechtigung fehlt",
                "Bitte erlauben sie den Zugriff auf deine Fotos in den Einstellungen");
        }
        catch (FeatureNotSupportedException)
        {

            await PopupService.ShowErrorAsync("Nicht unterstützt",
                $"Dein Gerät unterstützt diese Funktion leider nicht.");
        }
        catch (IOException)
        {
            await PopupService.ShowErrorAsync("Speicher-Fehler",
                $"Das Bild konnte nicht gespeichert werden. Möglicherweise ist nicht genug Spiecherplatz frei.");
        }
        catch (Exception ex)
        {
            await PopupService.ShowErrorAsync("Fehler",
                $"Bild konnte nicht geladen werden: {ex.Message}");
        }
    }

    private void OnRemoveImageClicked(object sender, EventArgs e)
    {
        viewModel.ImagePath = string.Empty;

    }

    private async Task saveMeal()
    {
        var ingredients = ingredientEntries
        .Where(e => !string.IsNullOrWhiteSpace(e.NameEntry.Text))
        .Select(e => (
            Name: e.NameEntry.Text.Trim(),
            Amount: double.TryParse(e.AmountEntry.Text?.Trim(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out double amount) ? amount : 0,
            Unit: e.UnitPicker.SelectedItem?.ToString() ?? ""
        ))
        .ToList();

        await viewModel.SaveCommand.ExecuteAsync(ingredients);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await saveMeal();
    }
    
    private readonly AddMealViewModel viewModel;
    private readonly List<(Entry NameEntry, Entry AmountEntry, Picker UnitPicker)> ingredientEntries = new();

    private bool isLoading;
}