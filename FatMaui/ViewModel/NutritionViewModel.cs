using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatMaui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;

namespace FatMaui.ViewModel
{
    public partial class NutritionViewModel : ObservableObject
    {
        [ObservableProperty]
        private double weight;
        [ObservableProperty]
        private double height;
        [ObservableProperty]
        private int age;
        [ObservableProperty]
        private string gender;
        [ObservableProperty]
        private double proteinPercentage;
        [ObservableProperty]
        private double fatPercentage;
        [ObservableProperty]
        private double activityLevel;
        [ObservableProperty]
        private string goal;

        [ObservableProperty]
        private double bmr;
        [ObservableProperty]
        private double energyIntake;
        [ObservableProperty]
        private double proteinIntake;
        [ObservableProperty]
        private double fatIntake;
        [ObservableProperty]
        private double carbIntake;

        [ObservableProperty]
        private User currentUser;

        private NutritionDatabase _database;

        

        public NutritionViewModel()
        {
            _database = new NutritionDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NutritionData.db3"));
            Task.Run(() => LoadData());
            
        }

        
        [RelayCommand]
        private async Task Calculate()
        {
            NutritionCalculator calculator = new NutritionCalculator(Weight, Height, Age, Gender, ProteinPercentage, FatPercentage, ActivityLevel, Goal);
            Bmr = calculator.CalculateBMR();
            EnergyIntake = calculator.CalculateEnergyIntake();
            ProteinIntake = calculator.CalculateProteinIntake();
            FatIntake = calculator.CalculateFatIntake();
            CarbIntake = calculator.CalculateCarbIntake();

            // Сохраняем данные в базу данных
            NutritionData data = new NutritionData
            {
                UserId = Preferences.Get("userId", 0),
                Date = DateTime.Now,
                Bmr = Bmr,
                EnergyIntake = EnergyIntake,
                ProteinIntake = ProteinIntake,
                FatIntake = FatIntake,
                CarbIntake = CarbIntake
            };
            await _database.SaveNutritionDataAsync(data);
        }

        internal async Task LoadData()
        {

            // Загружаем данные о питании из базы данных для текущего пользователя
            var userId = Preferences.Get("userId", 0);
            var data = await _database.GetNutritionDataAsync(userId);

            // Проверяем, есть ли данные
            if (data != null && data.Any())
            {
                // Берем последнюю запись
                var latestData = data.OrderByDescending(d => d.Date).First();

                // Устанавливаем значения свойств модели представления
                Bmr = latestData.Bmr;
                EnergyIntake = latestData.EnergyIntake;
                ProteinIntake = latestData.ProteinIntake;
                FatIntake = latestData.FatIntake;
                CarbIntake = latestData.CarbIntake;
                
            }
        }

    }
}
