using MealApp.Models;
using MealApp.Services;
using MealApp.ViewModels;

namespace MealApp.Views;

public partial class ShoppingItemDetailPage : ContentPage
{
	public ShoppingItemDetailPage(ShoppingService shoppingService, CustomShoppingItem? itemToEdit = null)
	{
		InitializeComponent();
		BindingContext = new ShoppingItemDetailViewModel(shoppingService, itemToEdit);
    }
}