using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModels
{
    public class BaseViewmodel
    {
        protected readonly string ConnectionString;

        public BaseViewmodel()
        {
            ConnectionString = "Server=35.195.31.100;Database=PaulsData;Uid=root;Pwd=password;";
        }

    }
}
