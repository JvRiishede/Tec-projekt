using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Data.Interface
{
    public interface IPropertyService
    {
        T GetProperty<T>(Properties property, T defaultValue);
        bool SetProperty<T>(Properties property, T defaultValue);
    }
}
