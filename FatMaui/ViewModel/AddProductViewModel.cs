using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatMaui.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace FatMaui.ViewModel
{
    public partial class AddProductViewModel : ObservableObject
    {
        [ObservableProperty]
        private int mealId;

        [ObservableProperty]
        private Product selectedProduct;

        [ObservableProperty]
        private double productWeight;


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
        private string newProductName;
        [ObservableProperty]
        private double newProductCalories;
        [ObservableProperty]
        private double newProductFats;
        [ObservableProperty]
        private double newProductCarbs;
        [ObservableProperty]
        private double newProductProteins;


        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private string setMealBreakfast;
        [ObservableProperty]
        private string setMealLunch;
        [ObservableProperty]
        private string setMealDinner;
        [ObservableProperty]
        private string setMealSnack;


        private NutritionDatabase _database;


        [ObservableProperty]
        public ObservableCollection<Product> products;

        public AddProductViewModel()
        {
            _database = new NutritionDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NutritionData.db3"));
            Products = new ObservableCollection<Product>();
            Task.Run(() => LoadProducts());
            Task.Run(() => CreateDailyMeal());

            var mealType = Preferences.Get("MealType", string.Empty);
            switch (mealType)
            {
                case "Breakfast": SetMealBreakfast = "Breakfast"; break;                 
                case "Lunch": SetMealLunch = "Lunch"; break;                 
                case "Dinner": SetMealDinner = "Dinner"; break;                   
                case "Snack": SetMealSnack = "Snack"; break;                    
            }
        }
        private async Task CreateDailyMeal()
        {
            var userId = Preferences.Get("userId", 0);
            var date = DateTime.Today;

            var meal = new Meal
            {
                UserId = userId,
                Date = date
            };
            var existingMeal = await _database.GetMealIdByUserIdAndDataAsync(userId, date);
            if (existingMeal != null)
            {
                MealId = existingMeal.Id;
            }
            else
            {
                await _database.SaveMealAsync(meal);
                MealId = meal.Id;
            }
        }
        [RelayCommand]
        private async Task AddProduct()
        {
            if (SetMealBreakfast == "Breakfast")
            {
                var breakfast = new Breakfast
                {
                    MealId = MealId,
                    Weight = ProductWeight,
                    ProductId = SelectedProduct.Id
                };
                await _database.SaveBreakfastAsync(breakfast);
            }
            else if (SetMealLunch == "Lunch")
            {
                var lunch = new Lunch
                {
                    MealId = MealId,
                    Weight = ProductWeight,
                    ProductId = SelectedProduct.Id
                };
                await _database.SaveLunchAsync(lunch);

            }
            else if (SetMealDinner == "Dinner")
            {
                var dinner = new Dinner
                {
                    MealId = MealId,
                    Weight = ProductWeight,
                    ProductId = SelectedProduct.Id
                };
                await _database.SaveDinnerAsync(dinner);

            }
            else if (SetMealSnack == "Snack")
            {
                var snack = new Snack
                {
                    MealId = MealId,
                    Weight = ProductWeight,
                    ProductId = SelectedProduct.Id
                };
                await _database.SaveSnackAsync(snack);

            }
        }
        [RelayCommand]
        private async Task SaveNewProduct()
        {
                var product = new Product
                {
                    Name = NewProductName,
                    Calories = NewProductCalories,
                    Fats = NewProductFats,
                    Carbs = NewProductCarbs,
                    Proteins = NewProductProteins
                };
                await _database.SaveProductAsync(product);
            StatusMessage = "Продукт добавлен";
            
          await LoadProducts();
        }
        [RelayCommand]
        internal async Task LoadProducts()
        {
            Products.Clear();
            var products = await _database.GetProductsAsync();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        [RelayCommand]
        private async Task Close()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

    }
}
