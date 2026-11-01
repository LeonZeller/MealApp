using MealApp.Models;
using MealApp.Services;
using MealApp.ViewModels;
using System.Threading.Tasks;

namespace MealApp.Views;

public partial class ShoppingListPage : ContentPage
{
    public ShoppingListPage(CalendarService calendarService, ShoppingService shoppingService)
    {
        InitializeComponent();
        this.shoppingService = shoppingService;
        viewModel = new ShoppingListViewModel(calendarService, shoppingService);
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadCommand.ExecuteAsync(null);
    }

    private async void OnItemChecked(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value) return;

        var checkBox = (CheckBox)sender;
        var item = (ShoppingItem)checkBox.BindingContext;
        await viewModel.CheckItem(item);
    }

    private async void OnDeleteItemSwiped(object sender, EventArgs e)
    {
        var swipeItem = (SwipeItem)sender;
        var item = (ShoppingItem)swipeItem.BindingContext;

        if (item.IsCustom && item.CustomItem != null)
        {
            await viewModel.DeleteCustomItem(item.CustomItem);
        }
    }

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShoppingItemDetailPage(shoppingService));
    }

    private async void OnEditItemSwiped(object sender, EventArgs e)
    {
        var swipeItem = (SwipeItem)sender;
        var item = (ShoppingItem)swipeItem.BindingContext;

        if (item.IsCustom && item.CustomItem != null)
        {
            await Navigation.PushAsync(new ShoppingItemDetailPage(shoppingService));
        }
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    private readonly ShoppingListViewModel viewModel;
    private readonly ShoppingService shoppingService;

    
}