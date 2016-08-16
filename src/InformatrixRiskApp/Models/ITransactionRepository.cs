using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformatrixRiskApp.Models
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAll(string settlefilepath, string unsettlefilepath);
    }
}
