using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatMaui.Model;
using FatMaui.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FatMaui.ViewModel
{

    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime selectedDate;

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

        [ObservableProperty]
        private string id;
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private double proteins;
        [ObservableProperty]
        private double fats;
        [ObservableProperty]
        private double carbs;
        [ObservableProperty]
        private double calories;

        [ObservableProperty]
        private double weight;
        [ObservableProperty]
        private double productId;
        [ObservableProperty]
        private double mealId;

        [ObservableProperty]
        private MealProduct selectedMeallBreakfastProduct;
        public ICommand DeleteSelectedMeallBreakfastProductCommand => new Command(async () => await DeleteBr(SelectedMeallBreakfastProduct));

        [ObservableProperty]
        private MealProduct selectedMeallLunchProduct;
        public ICommand DeleteSelectedMeallLunchProductCommand => new Command(async () => await DeleteLu(SelectedMeallLunchProduct));

        [ObservableProperty]
        private MealProduct selectedMeallDinnerProduct;
        public ICommand DeleteSelectedMeallDinnerProductCommand => new Command(async () => await DeleteDi(SelectedMeallDinnerProduct));

        [ObservableProperty]
        private MealProduct selectedMeallSnackProduct;
        public ICommand DeleteSelectedMeallSnackProductCommand => new Command(async () => await DeleteSn(SelectedMeallSnackProduct));

        [ObservableProperty]
        private double totalBrProteins;
        [ObservableProperty]
        private double totalBrFats;
        [ObservableProperty]
        private double totalBrCarbs;
        [ObservableProperty]
        private double totalBrCalories;

        [ObservableProperty]
        private double totalLuProteins;
        [ObservableProperty]
        private double totalLuFats;
        [ObservableProperty]
        private double totalLuCarbs;
        [ObservableProperty]
        private double totalLuCalories;

        [ObservableProperty]
        private double totalDiProteins;
        [ObservableProperty]
        private double totalDiFats;
        [ObservableProperty]
        private double totalDiCarbs;
        [ObservableProperty]
        private double totalDiCalories;

        [ObservableProperty]
        private double totalSnProteins;
        [ObservableProperty]
        private double totalSnFats;
        [ObservableProperty]
        private double totalSnCarbs;
        [ObservableProperty]
        private double totalSnCalories;

        [ObservableProperty]
        private double totalProteins;
        [ObservableProperty]
        private double totalFats;
        [ObservableProperty]
        private double totalCarbs;
        [ObservableProperty]
        private double totalCalories;

        [ObservableProperty]
        public ObservableCollection<Breakfast> breakfast;

        [ObservableProperty]
        public ObservableCollection<MealProduct> meallBreakfastProduct;
        [ObservableProperty]
        public ObservableCollection<MealProduct> meallLunchProduct;
        [ObservableProperty]
        public ObservableCollection<MealProduct> meallDinnerProduct;
        [ObservableProperty]
        public ObservableCollection<MealProduct> meallSnackProduct;

        private NutritionDatabase _database;

        
        //public string TotalProteinsAndProteinIntake
        //{
        //    get
        //    {
        //        return $"Белки: {TotalProteins:F1} / {ProteinIntake:F1}";
        //    }
        //}

        public MainViewModel()
        {
            _database = new NutritionDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NutritionData.db3"));
            Task.Run(() => LoadData());
            SelectedDate = DateTime.Today;
            MeallBreakfastProduct = new ObservableCollection<MealProduct>();
            MeallLunchProduct = new ObservableCollection<MealProduct>();
            MeallDinnerProduct = new ObservableCollection<MealProduct>();
            MeallSnackProduct = new ObservableCollection<MealProduct>();
            Task.Run(() => LoadMealProduct());
            
        }

        internal async Task LoadMealProduct()
        {
            double BrProteins = 0.0; double BrFats = 0.0; double BrCarbs = 0.0; double BrCalories = 0.0;
            TotalBrProteins = 0.0; TotalBrFats = 0.0; TotalBrCarbs = 0.0; TotalBrCalories = 0.0;

            double LuProteins = 0.0; double LuFats = 0.0; double LuCarbs = 0.0; double LuCalories = 0.0;
            TotalLuProteins = 0.0; TotalLuFats = 0.0; TotalLuCarbs = 0.0; TotalLuCalories = 0.0;

            double DiProteins = 0.0; double DiFats = 0.0; double DiCarbs = 0.0; double DiCalories = 0.0;
            TotalDiProteins = 0.0; TotalDiFats = 0.0; TotalDiCarbs = 0.0; TotalDiCalories = 0.0;

            double SnProteins = 0.0; double SnFats = 0.0; double SnCarbs = 0.0; double SnCalories = 0.0;
            TotalSnProteins = 0.0; TotalSnFats = 0.0; TotalSnCarbs = 0.0; TotalSnCalories = 0.0;

            var userId = Preferences.Get("userId", 0);
            var date = DateTime.Today;
            var meal = await _database.GetMealIdByUserIdAndDataAsync(userId, date);

            MeallBreakfastProduct.Clear();
            MeallLunchProduct.Clear();
            MeallDinnerProduct.Clear();
            MeallSnackProduct.Clear();

            var breakfasts = await _database.GetBreakfastByMealAsync(meal.Id);
            foreach (var breakfast in breakfasts)
            {
                var products = await _database.GetProductsByIdAsync(breakfast.ProductId);
                foreach (var product in products)
                {
                    var mealProduct = new MealProduct
                    {
                        Id = breakfast.Id,
                        Name = product.Name,
                        Calories = ((product.Calories / 100) * breakfast.Weight),
                        Fats = ((product.Fats / 100) * breakfast.Weight),
                        Carbs = ((product.Carbs / 100) * breakfast.Weight),
                        Proteins = ((product.Proteins / 100) * breakfast.Weight),
                        Weight = breakfast.Weight
                    };
                    MeallBreakfastProduct.Add(mealProduct);

                    BrProteins = mealProduct.Proteins; 
                    BrFats = mealProduct.Fats; 
                    BrCarbs = mealProduct.Carbs; 
                    BrCalories = mealProduct.Calories;
                }
                TotalBrProteins += BrProteins; 
                TotalBrFats += BrFats; 
                TotalBrCarbs += BrCarbs; 
                TotalBrCalories += BrCalories;

                TotalProteins += TotalBrProteins; 
                TotalFats += TotalBrFats; 
                TotalCarbs += TotalBrCarbs; 
                TotalCalories += TotalBrCalories;
            }

            var lunchs = await _database.GetLunchByMealAsync(meal.Id);
            foreach (var lunch in lunchs)
            {
                var products = await _database.GetProductsByIdAsync(lunch.ProductId);
                foreach (var product in products)
                {
                    var mealProduct = new MealProduct
                    {
                        Id = lunch.Id,
                        Name = product.Name,
                        Calories = ((product.Calories / 100) * lunch.Weight),
                        Fats = ((product.Fats / 100) * lunch.Weight),
                        Carbs = ((product.Carbs / 100) * lunch.Weight),
                        Proteins = ((product.Proteins / 100) * lunch.Weight),
                        Weight = lunch.Weight
                    };
                    MeallLunchProduct.Add(mealProduct);

                    LuProteins = mealProduct.Proteins; 
                    LuFats = mealProduct.Fats; 
                    LuCarbs = mealProduct.Carbs; 
                    LuCalories = mealProduct.Calories;
                }
                TotalLuProteins += LuProteins; 
                TotalLuFats += LuFats; 
                TotalLuCarbs += LuCarbs; 
                TotalLuCalories += LuCalories;

                TotalProteins += TotalLuProteins; 
                TotalFats += TotalLuFats; 
                TotalCarbs += TotalLuCarbs; 
                TotalCalories += TotalLuCalories;
            }

            var dinners = await _database.GetDinnerByMealAsync(meal.Id);
            foreach (var dinner in dinners)
            {
                var products = await _database.GetProductsByIdAsync(dinner.ProductId);
                foreach (var product in products)
                {
                    var mealProduct = new MealProduct
                    {
                        Id = dinner.Id,
                        Name = product.Name,
                        Calories = ((product.Calories / 100) * dinner.Weight),
                        Fats = ((product.Fats / 100) * dinner.Weight),
                        Carbs = ((product.Carbs / 100) * dinner.Weight),
                        Proteins = ((product.Proteins / 100) * dinner.Weight),
                        Weight = dinner.Weight
                    };
                    MeallDinnerProduct.Add(mealProduct);

                    DiProteins = mealProduct.Proteins; 
                    DiFats = mealProduct.Fats; 
                    DiCarbs = mealProduct.Carbs; 
                    DiCalories = mealProduct.Calories;
                }
                TotalDiProteins += DiProteins; 
                TotalDiFats += DiFats; 
                TotalDiCarbs += DiCarbs; 
                TotalDiCalories += DiCalories;

                TotalProteins += TotalDiProteins; 
                TotalFats += TotalDiFats; 
                TotalCarbs += TotalDiCarbs; 
                TotalCalories += TotalDiCalories;
            }

            var snacks = await _database.GetSnackByMealAsync(meal.Id);
            foreach (var snack in snacks)
            {
                var products = await _database.GetProductsByIdAsync(snack.ProductId);
                foreach (var product in products)
                {
                    var mealProduct = new MealProduct
                    {
                        Id = snack.Id,
                        Name = product.Name,
                        Calories = ((product.Calories / 100) * snack.Weight),
                        Fats = ((product.Fats / 100) * snack.Weight),
                        Carbs = ((product.Carbs / 100) * snack.Weight),
                        Proteins = ((product.Proteins / 100) * snack.Weight),
                        Weight = snack.Weight
                    };
                    MeallSnackProduct.Add(mealProduct);

                    SnProteins = mealProduct.Proteins; 
                    SnFats = mealProduct.Fats; 
                    SnCarbs = mealProduct.Carbs; 
                    SnCalories = mealProduct.Calories;
                }
                TotalSnProteins += SnProteins; 
                TotalSnFats += SnFats; 
                TotalSnCarbs += SnCarbs; 
                TotalSnCalories += SnCalories;

                TotalProteins += TotalSnProteins;
                TotalFats += TotalSnFats; 
                TotalCarbs += TotalSnCarbs; 
                TotalCalories += TotalLuCalories;
            }
        }

        [RelayCommand]
        internal async Task DeleteBr(MealProduct mealProduct)
        {
            MeallBreakfastProduct.Remove(mealProduct);

            var breakfast = new Breakfast
            {
                Id = mealProduct.Id,
            };
            await _database.DeleteBreakfastAsync(breakfast);

            TotalBrProteins -= mealProduct.Proteins;
            TotalBrFats -= mealProduct.Fats;
            TotalBrCarbs -= mealProduct.Carbs;
            TotalBrCalories -= mealProduct.Calories;
        }

        [RelayCommand]
        internal async Task DeleteLu(MealProduct mealProduct)
        {
            MeallLunchProduct.Remove(mealProduct);

            var lunch = new Lunch
            {
                Id = mealProduct.Id,
            };
            await _database.DeleteLunchAsync(lunch);

            TotalLuProteins -= mealProduct.Proteins;
            TotalLuFats -= mealProduct.Fats;
            TotalLuCarbs -= mealProduct.Carbs;
            TotalLuCalories -= mealProduct.Calories;
        }

        [RelayCommand]
        internal async Task DeleteDi(MealProduct mealProduct)
        {
            MeallDinnerProduct.Remove(mealProduct);

            var dinner = new Dinner
            {
                Id = mealProduct.Id,
            };
            await _database.DeleteDinnerAsync(dinner);

            TotalDiProteins -= mealProduct.Proteins;
            TotalDiFats -= mealProduct.Fats;
            TotalDiCarbs -= mealProduct.Carbs;
            TotalDiCalories -= mealProduct.Calories;
        }

        [RelayCommand]
        internal async Task DeleteSn(MealProduct mealProduct)
        {
            MeallSnackProduct.Remove(mealProduct);

            var snack = new Snack
            {
                Id = mealProduct.Id,
            };
            await _database.DeleteSnackAsync(snack);

            TotalSnProteins -= mealProduct.Proteins;
            TotalSnFats -= mealProduct.Fats;
            TotalSnCarbs -= mealProduct.Carbs;
            TotalSnCalories -= mealProduct.Calories;
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
        [RelayCommand]
        public void AddBreakfast()
        {
            Preferences.Set("MealType", "Breakfast");
            Shell.Current.Navigation.PushModalAsync(new FatMaui.View.AddProductPage());
        }

        [RelayCommand]
        public async Task AddLunch()
        {
            Preferences.Set("MealType", "Lunch");
            await Shell.Current.Navigation.PushModalAsync(new FatMaui.View.AddProductPage());
        }

        [RelayCommand]
        public async Task AddDinner()
        {
            Preferences.Set("MealType", "Dinner");
            await Shell.Current.Navigation.PushModalAsync(new FatMaui.View.AddProductPage());
        }

        [RelayCommand]
        public async Task AddSnack()
        {
            Preferences.Set("MealType", "Snack");
            await Shell.Current.Navigation.PushModalAsync(new FatMaui.View.AddProductPage());
        }

        
    }
}
