using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchLogic;

public class DataIngredient
{
    public string Name { get; set; }
    public int ID { get; set; }
    public double GramsInPce { get; set; }
    public double GramsInCup { get; set; }
    public DataIngredient(string name, int id, double gramsInPce, double gramsInCup)
    {
        this.Name = name;
        this.ID = id;
        GramsInPce = gramsInPce;
        GramsInCup = gramsInCup;
    }
}
