using System;

public class Ingredient
{
    public string Name { get; set; }
    public List<string> Unit { get; set; }

    public Ingredient(string name, List<string> unit)
    {
        Name = name;
        Unit = new List<string>("Kg", "g", "mg", "Tl", "El", "Prise");
    }
}
