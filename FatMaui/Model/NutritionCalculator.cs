using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatMaui.Model
{
    public class NutritionCalculator
    {
        private double weight;
        private double height;
        private int age;
        private string gender;
        private double proteinPercentage;
        private double fatPercentage;
        private double activityLevel;
        private string goal;

        public NutritionCalculator(double weight, double height, int age, 
            string gender, double proteinPercentage, double fatPercentage, 
            double activityLevel, string goal)
        {
            this.weight = weight;
            this.height = height;
            this.age = age;
            this.gender = gender;
            this.proteinPercentage = proteinPercentage;
            this.fatPercentage = fatPercentage;
            this.activityLevel = activityLevel;
            this.goal = goal;
        }


        public double CalculateBMR()
        {
            double bmr;
            if (gender == "male")
            {
                bmr = (10 * weight) + (6.25 * height) - (5 * age) + 5;
            }
            else
            {
                bmr = (10 * weight) + (6.25 * height) - (5 * age) - 161;
            }
            return bmr;
        }

        public double CalculateEnergyIntake()
        {
            double tdee = CalculateBMR() * activityLevel;
            if (goal == "weight loss")
            {
                tdee *= 0.8;
            }
            else if (goal == "weight gain")
            {
                tdee *= 1.2;
            }
            return tdee;
        }

        public double CalculateProteinIntake()
        {
            return (CalculateEnergyIntake() * proteinPercentage/100) / 4;
        }

        public double CalculateFatIntake()
        {
            return (CalculateEnergyIntake() * fatPercentage/100) / 9;
        }

        public double CalculateCarbIntake()
        {
            return (CalculateEnergyIntake() - (CalculateProteinIntake() * 4 + CalculateFatIntake() * 9)) / 4;
        }
    }

}
