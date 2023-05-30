using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace searchLogic;

public static class Search
{
    
    public static int MinPriorityValue = 0; 
    public static int MaxPriorityValue = 1;
    static private RequestFilter filter { get; set; }
    public static List<Recipe> ApplyRequestFilter(RequestFilter request, List<Recipe> input)
    {
        filter = request;
        InGrams(); // Переводим единицы измерения требуемых ингредиентов
        PrintInfo();
        int q = input.Count();
        input = DeleteForbidden(input); // Удаляем запрещённое
        Console.WriteLine($"{q - input.Count} рецептов с запрещёнными тегами и ингредиентами удалено");
        q = input.Count();
        input = DeleteUnnecessary(input); // Удаляем лишнее
        Console.WriteLine($"{q - input.Count} лишних рецептов удалено");
        // Ветка поиска по тегам
        if (filter.RequiredIngredients.Count == 0)
        {
            Console.WriteLine("Произведена сортировка по тегам");
            input = TagSort(input);
            return input;
        }
        else
        {
            q = input.Count();
            input = QuantitySelection(input); // Удаляем рецепты с нехваткой любого из ингредиентов
            Console.WriteLine($"{q - input.Count} рецептов удалено по причине нехватки ингредиентов");
            input = Sorting(input);

            Console.WriteLine("\nАлгоритм сортировки выполнен");
            Console.WriteLine($"{input.Count()} рецептов найдено \n");
            return input;
        }
    }



    public static List<Recipe> Sorting(List<Recipe> Recipes)
    {
        Dictionary<Recipe, int> dict = new Dictionary<Recipe, int>();
        foreach (var rec in Recipes) 
        {
            int count = 0;
            foreach (var ingr in rec.Ingredients)
            {
                if (ContainsIngr(ingr, filter.RequiredIngredients)) count++;
            }
            dict.Add(rec, count);   
        }
        dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        // Отсортировать сгруппированные по вхождениям ингредиентов участки списка по: -> тегам -> рейтингу


        List<Recipe> output;
        output = dict.Keys.ToList();
        return output;
    }
    public static List<Recipe> TagSort (List<Recipe> Recipes)
    {
        Dictionary<Recipe, int> dict = new Dictionary<Recipe, int>();
        foreach (var rec in Recipes)
        {
            int val = 0;
            foreach (var tag in rec.Tags)
            {
                if (ContainsTag(tag, filter.WantedTags)) val++; 
            }
            dict.Add(rec, val);
        }
        //int LastValue = -1, lastInd = -1;
        //for (int i = 0; i < dict.Count(); i++)
        //{
        //    if (dict.Value)

        //}

        // Сортировка рецептов по числу вхождения искомых тегов
        dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        List<Recipe> output;
        output = dict.Keys.ToList();
        return output;
    }
    public static List<Recipe> QuantitySelection(List<Recipe> Recipes)
    {
        for (int i = 0; i < Recipes.Count(); i++)
        {
            var rec = Recipes[i];
            foreach (var iNeed in rec.Ingredients)
            {
                foreach (var iHave in filter.RequiredIngredients)
                {
                    if (CompareIngr(iNeed, iHave))
                    {
                        // Если ингредиента хватает в любом случае, приоритет не учитывается
                        if (iNeed.Quantity <= iHave.Quantity) goto nextIngr;
                        // Случаи, когда кол-во ингредиента не учитывается
                        if (iNeed.Measurement == "по вкусу") goto nextIngr;
                        if (iHave.Importance == MinPriorityValue) goto nextIngr;
                        // Случай, когда ингредиент с максимальным приоритетом
                        if (iHave.Importance == MaxPriorityValue && iHave.Quantity >= iNeed.Quantity) goto nextIngr;
                        // Случай, когда приоритет принадлежит (0, 1)
                        if (CountPriorityValue(iHave, iNeed)) goto nextIngr;


                        //Console.WriteLine($"\t Рецепт {rec.Name} удалён по причине нехватки ингредиента {iNeed.Name} - " +
                            //$"нужно {iNeed.Quantity} {iNeed.Measurement}");
                        Recipes.RemoveAt(i); 
                        i--; 
                        goto next;
                    }
                }
            nextIngr:
                continue;
            }
        next:
            continue;
        }

        return Recipes;
    }
    public static bool CountPriorityValue (IngredientView iHave, Ingredient iNeed) // Возврат максималього допуска по количеству
    /* Принцип работы параметра "Importance", он же "приоритет":
        Параметр показывает, на сколько алгоритм может отклоняться от имеющегося количества ингредиента
        в большую сторону. Значение высчитывается относительно количества требуемого ингредиента*/
    {
        double value = iNeed.Quantity * iHave.Importance;
        return (value <= iHave.Quantity);
    }
    public static void InGrams ()
    {
        foreach (var iHave in filter.RequiredIngredients)
        {
            double density = 1;
            if (iHave.GramsInCup != 0) density = iHave.GramsInCup / 200.0;
            switch (iHave.Measurement)
            {
                case ("кг"):
                    iHave.Quantity *= 1000;
                    break;
                case ("гр"):
                    break;
                case ("шт"):
                    iHave.Quantity *= iHave.GramsInPce;
                    break;
                case ("мл"):
                    iHave.Quantity *= density;
                    break;
            }
            iHave.Measurement = "гр";
        }
    }
    static List<Recipe> DeleteUnnecessary(List<Recipe> Recipes) // Удаление рецептов, не соответствующих запросу
    {
        if (filter.RequiredIngredients.Count != 0 && filter.WantedTags.Count != 0) // Если поиск по тегам и ингредиентам
        {
            //int max = Recipes.Count();
            for (int i = 0; i < Recipes.Count(); i++) // Удаление рецепта, который не содержит нужных ингредиентов и тегов
            {
                var recipe = Recipes[i];
                if (!Match(recipe.Ingredients, filter.RequiredIngredients) && !Match(recipe.Tags, filter.WantedTags))
                {
                    i--;
                    Recipes.Remove(recipe);
                }
            }
            return Recipes;
        }
        if (filter.RequiredIngredients.Count != 0 && filter.WantedTags.Count == 0) // Если поиск только по ингредиентам
        {
            for (int i = 0; i < Recipes.Count(); i++) // Удаление рецепта, который не содержит нужных ингредиентов
            {
                var recipe = Recipes[i];
                if (!Match(recipe.Ingredients, filter.RequiredIngredients))
                {
                    i--;
                    Recipes.Remove(recipe);
                }
            }
            return Recipes;
        }
        if (filter.RequiredIngredients.Count != 0 && filter.WantedTags.Count == 0) // Если поиск только по тегам
        {
            for (int i = 0; i < Recipes.Count(); i++) // Удаление рецепта, который не содержит нужных тегов
            {
                var recipe = Recipes[i];
                if (!Match(recipe.Tags, filter.WantedTags))
                {
                    i--;
                    Recipes.Remove(recipe);
                }
            }
        }
        return Recipes;
    }
    static List<Recipe> DeleteForbidden (List<Recipe> Recipes) 
    {
        for (int i = 0; i < Recipes.Count(); i++)
        {
            var recipe = Recipes[i];
            foreach (var ingridient in recipe.Ingredients) // Удаление рецепта, если он содержит запрещённые ингредиенты
            {
                if (ContainsIngr(ingridient, filter.IgnoredIngredients))
                {
                    i--;
                    Recipes.Remove(recipe);
                    goto next; 
                }
            }
            foreach (var tag in recipe.Tags) // Удаление рецепта, если он содержит запрещённые теги
            {
                if (ContainsTag(tag, filter.UnwantedTags))
                {
                    i--;
                    Recipes.Remove(recipe);
                    goto next;
                }
            }
        next:
            continue;
        }
        return Recipes;
    }
    static bool Match (List<Ingredient> ingredients, ObservableCollection<IngredientView> views)
    {
        foreach (var ing in ingredients)
        {
            if (ContainsIngr(ing, views)) return true;
        }
        return false;
    }
    static bool Match(List<Ingredient> ingredients, List<IngredientView> views)
    {
        foreach (var ing in ingredients)
        {
            if (ContainsIngr(ing, views)) return true;
        }
        return false;
    }
    static bool Match(List<string> tags, ObservableCollection<Tag> views)
    {
        foreach (var tag in tags)
        {
            if (ContainsTag(tag, views)) return true;
        }
        return false;
    }
    static bool ContainsTag (string t1, ObservableCollection<Tag> l1)
    {
        foreach (var tag in l1)
        {
            if (tag.Name == t1) return true;
        }
        return false;
    }
    static bool ContainsIngr (Ingredient i1, ObservableCollection<IngredientView> l1)
    {
        foreach (var i2 in l1)
        {
            if (CompareIngr(i1, i2))
            {
                return true;
            }
        }
        return false;
    }
    static bool ContainsIngr(Ingredient i1, List<IngredientView> l1)
    {
        foreach (var i2 in l1)
        {
            if (CompareIngr(i1, i2))
            {
                return true;
            }
        }
        return false;
    }
    static bool CompareIngr (Ingredient i1, IngredientView i2)
    {
        if (i1.ID == i2.ID) return true;
        else return false;
    } 
    static void PrintInfo()
    {
        Console.WriteLine("\nЗапрос:");
        foreach (var element in filter.RequiredIngredients)
        {
            Console.WriteLine($"\tДобавить ингредиент \"{element.Name}\"");
        }
        foreach (var element in filter.IgnoredIngredients)
        {
            Console.WriteLine($"\tЗапретить ингредиент \"{element.Name}\"");
        }
        foreach (var element in filter.WantedTags)
        {
            Console.WriteLine($"\tДобавить тег \"{element.Name}\"");
        }
        foreach (var element in filter.UnwantedTags)
        {
            Console.WriteLine($"\tЗапретить тег \"{element.Name}\"");
        }
        Console.WriteLine();
    }
}
