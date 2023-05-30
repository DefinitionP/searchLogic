using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchLogic;

public class IngredientView
{
    public string Name { get; set; }
    public string ViewString { get; set; }
    public double Importance { get; set; }
    public double Quantity { get; set; }
    public string Measurement { get; set; }
    public int ID { get; set; }
    public double GramsInPce { get; set; }
    public double GramsInCup { get; set; }
    public IngredientView(string name, double importance, double quantity, string measurement, int id, double gramsInPce, double gramsInCup)
    {
        Name = name;
        Importance = importance;
        ID = id;
        Quantity = quantity;
        Measurement = measurement;
        GramsInPce = gramsInPce;
        GramsInCup = gramsInCup;
        ViewString = $"{Name} - {Quantity} {Measurement}";
    }
    public IngredientView(string name, double importance, int id, double gramsInPce, double gramsInCup)
    {
        Name = name;
        Importance = importance;
        ID = id;
        GramsInPce = gramsInPce;
        GramsInCup = gramsInCup;
        ViewString = $"{Name}";
    }
    public IngredientView(string name, int id)
    {
        Name = name;
        ID = id;
        ViewString = $"{Name}";
    }
}
