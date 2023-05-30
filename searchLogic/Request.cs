using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static searchLogic.Program;

namespace searchLogic
{
    public class Request
    {
        public static RequestFilter Set()
        {
            RequestFilter filter = new RequestFilter();



            //filter.RequireIngredient(toIng("Свиная голова"));
            //filter.RequireIngredient(toIng("Горох желтый"));
            //filter.RequireTag(GetTag("Веселые и смешные рецепты"));
            //filter.RequireTag(GetTag("Алкоголь"));
            //filter.RequireTag(GetTag("Веселые и смешные рецепты"));
            //filter.DisableIngredient(toIng("Вода", 0));

            //(string name, double importance, double quantity, string measurement, int id, double gramsInPce, double gramsInCup)
            IngredientView pigHead = new IngredientView("Свиная голова", 0, 1, "шт", 1558, 8000, 0);
            IngredientView flour = new IngredientView("Мука пшеничная", 0, 10, "мл", 915, 0, 130);
            IngredientView wtf = new IngredientView("Молоко коровье цельное", 0, 1, "кг", 278, 0, 200);
            IngredientView shit = new IngredientView("Масло сливочное 82%", 0, 1000, "гр", 383, 0, 0);



            //filter.RequireIngredient(pigHead);
            filter.RequireIngredient(flour);
            filter.RequireIngredient(wtf);
            filter.RequireIngredient(shit);

            return filter;
        }
    } 
}
