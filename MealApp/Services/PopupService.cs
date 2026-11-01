using MealApp.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Services
{
    public static class PopupService
    {
        public static async Task<string?> ShowAsync(string title, string? message,
            string? icon, List<(string Text, string Value, bool IsDestructive)> options)
        {
            var popup = new CustomPopup(title, message, icon, options.ToList());
            await Shell.Current.CurrentPage.Navigation.PushModalAsync(popup, false);
            return await popup.ResultTask.Task;
        }

        public static Task<bool> ConfirmAsync(string title, string message, string confirmText = "Ja", string cancelText = "Nein", string icon = "⚠️")
        {
            return showConfirmAsync(title, message, icon, confirmText, cancelText);
        }

        public static async Task ShowInfoAsync(string title, string message, string icon = "ℹ️")
        {
            await ShowAsync(title, message, icon, new List<(string Text, string Value, bool IsDestructive)> { ("OK", "ok", false) });
        }

        public static async Task ShowErrorAsync(string title, string message)
        {
            await ShowAsync(title, message, "❌", new List<(string Text, string Value, bool IsDestructive)> { ("OK", "ok", false) });
        }

        private static async Task<bool> showConfirmAsync(string title, string message, string icon, string confirmText, string cancelText)
        {
            var result = await ShowAsync(title, message, icon, 
                new List<(string Text, string Value, bool IsDestructive)> 
                { 
                    (confirmText, "yes", true), 
                    (cancelText, "no", false) 
                });
            return result == "yes";
        }
    }
}
