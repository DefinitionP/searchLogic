using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchLogic;

public class RequestFilter
{
    public ObservableCollection<IngredientView> RequiredIngredients { get; private set; }
    public ObservableCollection<IngredientView> IgnoredIngredients { get; private set; }
    public ObservableCollection<Tag> WantedTags { get; private set; }
    public ObservableCollection<Tag> UnwantedTags { get; private set; }

    public RequestFilter(ObservableCollection<IngredientView> required, ObservableCollection<IngredientView> ignored, ObservableCollection<Tag> wantedTags, ObservableCollection<Tag> unwantedTags)
    {
        RequiredIngredients = required;
        IgnoredIngredients = ignored;
        WantedTags = wantedTags;
        UnwantedTags = unwantedTags;
    }

    public RequestFilter()
    {
        RequiredIngredients = new ObservableCollection<IngredientView>();
        IgnoredIngredients = new ObservableCollection<IngredientView>();
        WantedTags = new ObservableCollection<Tag>();
        UnwantedTags = new ObservableCollection<Tag>();
    }

    public void RequireIngredient(IngredientView ingredient) // Запросить
    {
        int id = ingredient.ID;
        if (!RequiredIngredients.Any(x => x.ID == id))
        {
            RequiredIngredients.Add(ingredient);
        }
    }
    public void DisableIngredient(IngredientView ingredient)// Запретить
    {
        int id = ingredient.ID;
        if (!IgnoredIngredients.Any(x => x.ID == id))
        {
            IgnoredIngredients.Add(ingredient);
        }
    }
    public void UnrequireIngredient(IngredientView ingredient) // Отменить запрос
    {
        int id = ingredient.ID;
        if (RequiredIngredients.Any(x => x.ID == id))
        {
            RequiredIngredients.Remove(ingredient);
        }
    }
    public void EnableIngredient(IngredientView ingredient)// Отменить запрет
    {
        int id = ingredient.ID;
        if (IgnoredIngredients.Any(x => x.ID == id))
        {
            IgnoredIngredients.Remove(ingredient);
        }
    }



    public void RequireTag(Tag tag) // Запросить ТЕГ
    {
        if (!WantedTags.Contains(tag))
        {
            WantedTags.Add(tag);
        }
    }
    public void UnwantTag(Tag tag) // Запретить ТЕГ
    {
        if (!UnwantedTags.Contains(tag))
        {
            UnwantedTags.Add(tag);
        }
    }
    public void RemoveWantedTag(Tag tag) // Отменить запрос ТЕГА
    {
        if (WantedTags.Contains(tag))
        {
            WantedTags.Remove(tag);
        }
    }
    public void RemoveUnwantedTag(Tag tag) // Отменить запрет ТЕГА
    {
        if (UnwantedTags.Contains(tag))
        {
            UnwantedTags.Remove(tag);
        }
    }
    public void ClearAll()
    {
        RequiredIngredients.Clear();
        IgnoredIngredients.Clear();
        WantedTags.Clear();
        UnwantedTags.Clear();
    }
}
