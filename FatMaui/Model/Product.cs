using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatMaui.Model
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; } 
        public double Fats { get; set; }
        public double Carbs { get; set; }
        public double Proteins { get; set; }
        
    }
}
 