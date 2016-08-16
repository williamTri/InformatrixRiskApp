using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;

namespace InformatrixRiskApp.Models
{
    public class TransactionRepository : ITransactionRepository
    {
        static List<Transaction> AllTrxs = new List<Transaction>();

        public IEnumerable<Transaction> GetAll(string settled, string unsettled)
        {
            // Clear the list
            AllTrxs.Clear();

            try
            {
                foreach (string line in File.ReadLines(settled))
                {
                    String[] trx = (line.Split(','));
                    AllTrxs.Add(new Transaction("settled", trx));
                }
            }
            catch (System.IO.IOException e)
            {
                // Log errror message
                Environment.Exit(0);
            }

            try
            {
                foreach (string line in File.ReadLines(settled))
                {
                    String[] trx = (line.Split(','));
                    AllTrxs.Add(new Transaction("unsettled", trx));
                }
            }
            catch (System.IO.IOException e)
            {
                // Invalid input file format
                Environment.Exit(0);
            }
            return AllTrxs;
        }             
     }
}
