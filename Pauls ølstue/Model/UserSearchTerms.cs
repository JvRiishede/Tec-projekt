using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserSearchTerms
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public SortUser Sort { get; set; }
        public string SearchText { get; set; }
    }

    public enum SortUser
    {
        AscFornavn,
        DescFornavn,
        AscEfternavn,
        DescEfternavn,
        AscVærelseNr,
        DescVærelseNr,
        AscEmail,
        DescEmail,
        AscType,
        DescType
    }

}
