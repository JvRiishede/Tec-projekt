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

        List<ItemSold> GetTopMostSold();

    }
}
