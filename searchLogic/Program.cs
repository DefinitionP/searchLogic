using searchLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace searchLogic;
public class Program
{
    public static List<DataIngredient> Ingredients = new List<DataIngredient>();
    public static List<Recipe> Recipes = new List<Recipe>();
    public static List<Recipe> Recipes2 = new List<Recipe>();
    public static List<Tag> Tags = new List<Tag>();
    public static void Main()
    {
        Open();
        RequestFilter filter;
        filter = Request.Set();
        CheckMeasurement();
        CheckRequest(Recipes, filter);

        List<Recipe> output;
        output = Search.ApplyRequestFilter(filter, Recipes);
        CheckRequest(output, filter);
        PrintResult(output);
    }

    public static void CheckRequest(List<Recipe> recipes, RequestFilter filter)
    {
        Console.WriteLine();
        foreach (var element in filter.RequiredIngredients)
        {
            CheckName(element.Name, recipes);
        }
        foreach (var element in filter.IgnoredIngredients)
        {
            CheckName(element.Name, recipes);
        }
        foreach (var element in filter.WantedTags)
        {
            CheckTag(element.Name, recipes);
        }
        foreach (var element in filter.UnwantedTags)
        {
            CheckTag(element.Name, recipes);
        }
        Console.WriteLine();
    }
    public static void PrintResult (List<Recipe> output)
    {
        foreach (var rec in output)
        {
            Console.WriteLine(rec.Name);
            foreach (var ingr in rec.Ingredients)
            {
                Console.WriteLine($"\t{ingr.Name},");
            }
            //foreach (var tag in rec.Tags)
            //{
            //    Console.WriteLine($"\t{tag},");
            //}
        }
    }
    public static void Open()
    {
        FileStream file = File.OpenRead(@"C:\Users\LSM303D\source\repos\c00ker\Dishcovery\Resources\Raw\recipes1000menu[part 1].json");
        Recipes = JsonSerializer.Deserialize<List<Recipe>>(file);
        file = File.OpenRead(@"C:\Users\LSM303D\source\repos\c00ker\Dishcovery\Resources\Raw\recipes1000menu[part 2].json");
        Recipes2 = JsonSerializer.Deserialize<List<Recipe>>(file);
        file = File.OpenRead(@"C:\Users\LSM303D\source\repos\c00ker\Dishcovery\Resources\Raw\ingredients1000menu.json");
        Ingredients = JsonSerializer.Deserialize<List<DataIngredient>>(file);


        using Stream fileStream = File.OpenRead(@"C:\Users\LSM303D\source\repos\c00ker\Dishcovery\Resources\Raw\tagsNames.txt");
        using StreamReader reader = new StreamReader(fileStream);
        while (!reader.EndOfStream)
        {
            Tags.Add(new Tag(reader.ReadLine()));
        }


        foreach (var rec in Recipes2)
        {
            Recipes.Add(rec);
        }
        int ingCount = 0;
        List<DataIngredient> Ingredients2 = new List<DataIngredient>();
        foreach (var ingr in Ingredients)
        {
            Ingredients2.Add(ingr);
        }
        foreach (var rec in Recipes)
        {
            foreach (var ingr in rec.Ingredients)
            {
                for (int i = 0; i < Ingredients2.Count(); i++)
                {
                    var founded = Ingredients2[i];
                    if (founded.Name == ingr.Name)
                    {
                        ingCount++;
                        Ingredients2.Remove(founded);
                        i++;
                    }
                }
            }
        }

        Console.WriteLine($"Рецептов: {Recipes.Count()}");
        Console.WriteLine($"Тегов: {Tags.Count()}");
        Console.WriteLine($"Ингредиентов: {Ingredients.Count()} ({ingCount} в списке рецептов)");
    }
    public static IngredientView toIng (string name, double importance)
    {
        bool contains = false;
        int ind = -1;
        foreach (var ingr in Ingredients)
        {
            if (ingr.Name == name)
            {
                ind = Ingredients.IndexOf(ingr);
                contains = true;
                break;
            }
        }
        if (!contains || ind == -1)
        {
            Console.WriteLine("wrong name!");
            return null;
        }
        IngredientView output = new IngredientView(Ingredients[ind].Name, importance, Ingredients[ind].ID, Ingredients[ind].GramsInPce,
            Ingredients[ind].GramsInCup);
        return output;
    }
    public static IngredientView toIng(string name)
    {
        return toIng(name, 1.0);
    }
    public static Tag GetTag(string name)
    {
        foreach (var t in Tags)
        {
            if (t.Name.ToLower() == name.ToLower()) return t;
        }
        return null;
    }
    public static void CheckName (string nameIng, List<Recipe> Recipes)
    {
        int count = 0;
        foreach (var rec in Recipes)
        {
            foreach (var ingr in rec.Ingredients)
            {
                if (ingr.Name.ToLower() == nameIng.ToLower()) count++;
            }
        }
        Console.WriteLine($"Найдено {count} рецептов с ингредиентом \"{nameIng}\"");
    }
    public static void CheckTag(string tag, List<Recipe> Recipes)
    {
        int count = 0;
        foreach (var rec in Recipes)
        {
            foreach (var ntag in rec.Tags)
            {
                if (ntag.ToLower() == tag.ToLower()) count++;
                continue;
            }
        }
        Console.WriteLine($"Найдено {count} рецептов с тегом \"{tag}\"");
    }
    public static void CheckMeasurement ()
    {
        long COUNT = 0;
        foreach (var rec in Recipes)
        {
            foreach (var ingr in rec.Ingredients)
            {
                COUNT++;
            }
        }

        List<int> ints = new List<int>();
        List<string> mea = new List<string>();
        foreach (var rec in Recipes)
        {
            foreach (var ingr in rec.Ingredients)
            {
                bool contains = false;
                foreach (var test in mea)
                {
                    if (test == ingr.Measurement) { contains = true; ints[mea.IndexOf(test)]++; continue; }
                }
                if (!contains) { mea.Add(ingr.Measurement); ints.Add(1); }
            }
        }
        long sum = 0;
        foreach (var sr in mea)
        {
            sum += ints[mea.IndexOf(sr)];
            Console.WriteLine($"{sr} {ints[mea.IndexOf(sr)]}");
        }
        Console.WriteLine($"Всего различных вхождений рецептов {sum}");
        Console.WriteLine($"В исходном варианте {COUNT} \n");
    }


}