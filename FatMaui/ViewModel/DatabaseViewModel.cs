using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatMaui.Model;
using SQLite;

namespace FatMaui.ViewModel
{
    public partial class DatabaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string usersData;

        private readonly NutritionDatabase _nutritionDatabase;

        public DatabaseViewModel(NutritionDatabase nutritionDatabase)
        {
            _nutritionDatabase = nutritionDatabase;
        }

        [RelayCommand]
        public async Task LoadUsersData()
        {
            // Здесь происходит загрузка данных всех пользователей из базы данных SQLite
            var users = await _nutritionDatabase.GetUsersAsync();
            // Преобразование данных в строку
            UsersData = FormatUsersData(users);
        }


        private string FormatUsersData(List<User> users)
        {
            var formattedData = new StringBuilder();
            foreach (var user in users)
            {
                formattedData.AppendLine($"Id: {user.Id}, Username: {user.Username}, Email: {user.Email}");
            }
            return formattedData.ToString();
        }
    }
}
