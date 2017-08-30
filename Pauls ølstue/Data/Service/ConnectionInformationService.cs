using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;

namespace Data.Service
{
    public class ConnectionInformationService : IConnectionInformationService
    {
        public string ConnectionString { get; }

        public ConnectionInformationService(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
