using FatMaui.ViewModel;

namespace FatMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            SetStartupPage();

            MessagingCenter.Subscribe<LoginViewModel>(this, "UserLoggedIn", (sender) =>
            {
                SetStartupPage();
            });
        }

        private void SetStartupPage()
        {
            if (IsUserLoggedIn())
            {
                CurrentItem = DiaryTab;
            }
            else
            {
                CurrentItem = LoginTab;
            }
        }

        private bool IsUserLoggedIn()
        {
            // Получите значение из Preferences
            var userId = Preferences.Get("userId", 0);

            // Если userId не равен 0, значит пользователь вошел в систему
            return userId != 0;
        }
    }
}

