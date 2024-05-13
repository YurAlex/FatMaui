using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatMaui.Model;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FatMaui.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string registerUsername;

        [ObservableProperty]
        private string registerEmail;

        [ObservableProperty]
        private string registerPassword;

        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private User currentUser;

        private NutritionDatabase _database;

        public bool IsLoggedIn => CurrentUser != null;
        public bool IsLoggedOut => !IsLoggedIn;

        public LoginViewModel()
        {
            _database = new NutritionDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NutritionData.db3"));
            var userId = Preferences.Get("userId", 0);

            if (userId != 0)
            {
                // Пользователь уже вошел в систему 
                LoadUserData(userId);
            }
        }

        private async void LoadUserData(int userId)
        {
            var user = await _database.GetUserByIdAsync(userId);
            CurrentUser = user;
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLoggedOut));// Уведомляем об изменении состояния входа в систему
        }

       
       


        [RelayCommand]
        private async void Login()
        {
            var user = await _database.GetUserByEmailAsync(Email);

            if (user != null && VerifyPassword(Password, user.PasswordHash))
            {
                Preferences.Set("userId", user.Id);
                CurrentUser = user; 
                OnPropertyChanged(nameof(IsLoggedIn));
                MessagingCenter.Send(this, "UserLoggedIn");
                StatusMessage = "Вход в систему успешен";
            }
            else
            {
                StatusMessage = "Ошибка входа в систему";
            }
        }


        [RelayCommand]
        private async void Register()
        {
            var existingUser = await _database.GetUserByEmailAsync(RegisterEmail);

            if (existingUser == null)
            {
                var newUser = new User
                {
                    Username = RegisterUsername,
                    Email = RegisterEmail,
                    PasswordHash = HashPassword(RegisterPassword)
                };
                await _database.SaveUserAsync(newUser);
                StatusMessage = "Регистрация успешна";
            }

            else
            {
                StatusMessage = "Пользователь с таким адресом электронной почты уже существует";
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        [RelayCommand]
        private void Logout()
        {
            Preferences.Clear();
            CurrentUser = null;
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLoggedOut));
            StatusMessage = "Гость";
        }
    }
}

