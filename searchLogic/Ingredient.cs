namespace searchLogic;

public class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Measurement { get; set; }
    public string OnePeaceName { get; set; }
    public string TasteInfo { get; set; }
    public double GramsInPce { get; set; }
    public double GramsInCup { get; set; }
    public int ID { get; set; }
    public double Proteins { get; set; }
    public double Fats { get; set; }
    public double Carbs { get; set; }
    public int Calories { get; set; }
    public Ingredient(string name, double quantity, string measurement, string onePeaceName, string tasteInfo, double gramsInPce, double gramsInCup, int iD, double proteins, double fats, double carbs, int calories)
    {
        Name = name;
        Quantity = quantity;
        Measurement = measurement;
        OnePeaceName = onePeaceName;
        TasteInfo = tasteInfo;
        GramsInPce = gramsInPce;
        GramsInCup = gramsInCup;
        ID = iD;
        Proteins = proteins;
        Fats = fats;
        Carbs = carbs;
        Calories = calories;
    }
}
