using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes;
using Model;

namespace Data.Interface
{
    public interface IDrinkService
    {
        Drink GetDrink(int id);
        List<Drink> GetDrinks();
        int CreateDrink(string navn);
        bool UpdateDrink(Drink drink);
        bool AddVareToDrink(int drinkId, int[] ids);
        bool RemoveVareFromDrink(int drinkId, int[] ids);
        bool DeleteDrink(int id);
        List<Drink> GetPagedDrinks(ProductSearchTerms terms);
        int GetDrinksTotal(string searchText = "");
        int GetDrinksTotalForUser(int userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="id"></param>
        /// <returns>Tidsstempel, Sum af varer</returns>
        List<Tuple<DateTime, decimal>> GetDrinksForYear(int year, int id);
        int GetTotalSold(int id);
        List<ItemSold> GetTopMostSold();
    }
}
