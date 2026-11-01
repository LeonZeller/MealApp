using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealApp.Services;
using SQLite;

namespace MealApp.Models
{
    [Table("Meals")]
    public class Meal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IngredientsJson { get; set; } = "[]";
        public int PreperationTime { get; set; }
        public string PreperationStepsJson { get; set; } = "[]";
        public bool Vegan { get; set; }
        public string ImagePath { get; set; }
        public bool IsComponent { get; set; }

        [Ignore]
        public List<string> PreperationSteps
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(PreperationStepsJson) ?? new ();
            set => PreperationStepsJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        [Ignore]
        public List<Ingredient> Ingredients
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<Ingredient>>(IngredientsJson) ?? new List<Ingredient>();
            set => IngredientsJson = System.Text.Json.JsonSerializer.Serialize(value);        
        }
        public Meal()
        {
            
        }
        public Meal(string name, List<Ingredient> ingredients, 
            int preperationTime, List<string> preperationSteps, 
            bool vegan, string imagePath = "", bool isComponent = false)
        {
            Name = name;
            Ingredients = ingredients;
            PreperationTime = preperationTime;
            PreperationSteps = preperationSteps;
            Vegan = vegan;
            ImagePath = imagePath;
            IsComponent = isComponent;
        }
    }
    
}
