using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes;
using Model;

namespace Data.Interface
{
    public interface IVareService
    {
        List<Vare> GetAllVare();
        Vare GetVare(int id);
        int CreateVare(string Navn, decimal pris);
        bool UpdateVare(Vare vare);
        bool DeleteVare(int id);
        List<Vare> GetPagedVare(ProductSearchTerms terms);
        int GetVareTotal(string searchText = "");
        int GetVareTotalForUser(int userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="id"></param>
        /// <returns>Tidsstempel, Sum af varer</returns>
        List<Tuple<DateTime, decimal>> GetVarerForYear(int year, int id);
        int GetTotalSold(int id);
        List<ItemSold> GetTopMostSold();

    }
}
