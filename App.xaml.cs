using Microsoft.Maui.Platform;

namespace JsonEditor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);


#if WINDOWS
            window.Created += (s, e) =>
            {
                var handle = WinRT.Interop.WindowNative.GetWindowHandle(window.Handler.PlatformView);
                var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                appWindow.Closing += async (sender, args) =>
                {
                    args.Cancel = true;

                    if (App.Current.MainPage != null)
                    {
                        bool answer = await App.Current.MainPage.DisplayAlert(
                            "Вихід з програми",
                            "Чи дійсно ви хочете завершити роботу?",
                            "Так",
                            "Ні");

                        if (answer)
                        {
                            Application.Current.Quit();
                        }
                    }
                };
            };
#endif
            return window;
        }
    }
}