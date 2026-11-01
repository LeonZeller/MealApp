using MealApp.Views;
using System.Threading.Tasks;

namespace MealApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(AddMealPage), typeof(AddMealPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }

        
    }
}
