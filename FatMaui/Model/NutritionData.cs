using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FatMaui.Model
{
    public class NutritionData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public double Bmr { get; set; }
        public double EnergyIntake { get; set; }
        public double ProteinIntake { get; set; }
        public double FatIntake { get; set; }
        public double CarbIntake { get; set; }
    }
}
