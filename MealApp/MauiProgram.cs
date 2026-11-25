using MealApp.Models;
using MealApp.Views;
using MealApp.Services;
using Vapolia.WheelPickers;

namespace MealApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            builder.Services.AddSingleton<ICalendarService, CalendarService>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<MealServices>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<CalendarPage>();
            builder.Services.AddSingleton<ShoppingListPage>();
            builder.Services.AddSingleton<IShoppingService, ShoppingService>();           
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<BackupSettingsPage>();
            builder.UseMauiApp<App>().UseWheelPicker();
            return builder.Build();
        }

    }
}

    