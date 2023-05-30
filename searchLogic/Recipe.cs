namespace searchLogic;

public class Recipe
{
    public string Name { get; set; }
    public string Link { get; set; }
    public string ImageLink { get; set; }
    public int Rating { get; set; }
    public int ServingsNumber { get; set; }
    public int Calories { get; set; }
    public Time CookingTime { get; set; }
    public List<string> Tags { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public Recipe(string link, string name, string imagelink, int rating, int servingsNumber, int calories, Time cookingTime, List<Ingredient> ingredients)
    {
        Name = name;
        Link = link;
        ImageLink = imagelink;
        Rating = rating;
        ServingsNumber = servingsNumber;
        Calories = calories;
        CookingTime = cookingTime;
        Tags = new List<string>();
        Ingredients = ingredients;
    }
}
