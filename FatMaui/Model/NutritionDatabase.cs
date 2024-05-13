using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SQLite;

namespace FatMaui.Model
{
    public class NutritionDatabase
    {
        SQLiteAsyncConnection _database;

        public NutritionDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<NutritionData>().Wait();
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<Meal>().Wait();
            _database.CreateTableAsync<Breakfast>().Wait();
            _database.CreateTableAsync<Dinner>().Wait();
            _database.CreateTableAsync<Lunch>().Wait();
            _database.CreateTableAsync<Snack>().Wait();
        }

        public Task<List<NutritionData>> GetNutritionDataAsync()
        {
            return _database.Table<NutritionData>().ToListAsync();
        }

        public Task<int> SaveNutritionDataAsync(NutritionData data)
        {
            return _database.InsertAsync(data);
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return _database.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<NutritionData>> GetNutritionDataAsync(int userId)
        {
            return await _database.Table<NutritionData>()
                                  .Where(data => data.UserId == userId)
                                  .ToListAsync();
        }



        // Таблица Product (Продукты)
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _database.Table<Product>().ToListAsync();
        }

        public Task<int> SaveProductAsync(Product product)
        {
            return _database.InsertAsync(product);
        }

        public Task<List<Product>> GetProductsByNameAsync(string name)
        {
            return _database.Table<Product>().Where(u => u.Name == name).ToListAsync();
        }
        public Task<List<Product>> GetProductsByIdAsync(int id)
        {
            return _database.Table<Product>().Where(u => u.Id == id).ToListAsync();
        }


        // Таблица Meal (Еда)
        public async Task<List<Meal>> GetMealAsync()
        {
            return await _database.Table<Meal>().ToListAsync();
        }

        public Task<int> SaveMealAsync(Meal meal)
        {
            return _database.InsertAsync(meal);
        }

        public async Task<Meal> GetMealIdByUserIdAndDataAsync(int userId, DateTime date)
        {
            return await _database.Table<Meal>().Where(u => u.UserId == userId && u.Date == date).FirstOrDefaultAsync();
        }



        // Таблица Breakfast(Завтрак)
        public Task<List<Breakfast>> GetBreakfastAsync()
        {
            return _database.Table<Breakfast>().ToListAsync();
        }
        public Task<List<Breakfast>> GetBreakfastByMealAsync(int mealId)
        {
            return _database.Table<Breakfast>().Where(u => u.MealId == mealId).ToListAsync();
        }
        public Task<int> SaveBreakfastAsync(Breakfast breakfast)
        {
            return _database.InsertAsync(breakfast);
        }
        public Task<int> DeleteBreakfastAsync(Breakfast breakfast)
        {
            return _database.DeleteAsync(breakfast);
        }


        // Таблица Lunch(Обед)
        public Task<List<Lunch>> GetLunchAsync()
        {
            return _database.Table<Lunch>().ToListAsync();
        }
        public Task<int> SaveLunchAsync(Lunch lunch)
        {
            return _database.InsertAsync(lunch);
        }
        public Task<List<Lunch>> GetLunchByMealAsync(int mealId)
        {
            return _database.Table<Lunch>().Where(u => u.MealId == mealId).ToListAsync();
        }
        public Task<int> DeleteLunchAsync(Lunch lunch)
        {
            return _database.DeleteAsync(lunch);
        }
       
        

        // Таблица Dinner(Ужин)
        public Task<List<Dinner>> GetDinnerAsync()
        {
            return _database.Table<Dinner>().ToListAsync();
        }
        public Task<int> SaveDinnerAsync(Dinner dinner)
        {
            return _database.InsertAsync(dinner);
        }
        public Task<List<Dinner>> GetDinnerByMealAsync(int mealId)
        {
            return _database.Table<Dinner>().Where(u => u.MealId == mealId).ToListAsync();
        }
        public Task<int> DeleteDinnerAsync(Dinner dinner)
        {
            return _database.DeleteAsync(dinner);
        }


        // Таблица Snack(Перекус)
        public Task<List<Snack>> GetSnackAsync()
        {
            return _database.Table<Snack>().ToListAsync();
        }
        public Task<int> SaveSnackAsync(Snack snack)
        {
            return _database.InsertAsync(snack);
        }
        public Task<List<Snack>> GetSnackByMealAsync(int mealId)
        {
            return _database.Table<Snack>().Where(u => u.MealId == mealId).ToListAsync();
        }
        public Task<int> DeleteSnackAsync(Snack snack)
        {
            return _database.DeleteAsync(snack);
        }
    }
}
