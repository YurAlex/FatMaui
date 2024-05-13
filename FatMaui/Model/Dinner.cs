using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatMaui.Model
{
    public class Dinner
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MealId { get; set; }
        public int ProductId { get; set; }
        public double Weight { get; set; }
    }

}
