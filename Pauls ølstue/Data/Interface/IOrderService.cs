using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Collections.ObjectModel;

namespace Data.Interface
{
    public interface IOrderService
    {
        int PlaceOrder(FullOrder orders);
        int GetOrdreTotal();
        int GetOrdreTotalForUser(int userId);

        List<Order> GetOrdersForYear(int year);
        List<Order> GetOrdersYearForUser(int year, int userId);

    }
}