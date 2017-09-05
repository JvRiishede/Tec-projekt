using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductSearchTerms
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public SortProducts Sort { get; set; }
        public string SearchText { get; set; }
    }

    public enum SortProducts
    {
        AscNavn,
        DescNavn,
        AscTime,
        DescTime,
        AscPris,
        DescPris
    }
}
