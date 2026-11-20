namespace MealApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var saved = Preferences.Get("AppTheme", "System");
            UserAppTheme = saved switch
            {
                "Light" => AppTheme.Light,
                "Dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }       
    }
}