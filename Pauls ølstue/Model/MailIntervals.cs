using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum MailIntervals
    {
        [Display(Name = "Først dag i måneden")]
        FirstDayInMonth = 0,
        [Display(Name = "Hver dag i måneden")]
        EachDayInMonth = 1,
        [Display(Name = "Sidste dag i måneden")]
        LastDayInMonth = 2
    }
}
