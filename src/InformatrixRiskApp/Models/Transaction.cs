using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformatrixRiskApp.Models
{
    public class Transaction
    {
        internal string trxType;

        public string Customer { get; set; }
        public string TrxEvent { get; set; }
        public string Participant { get; set; }
        public decimal Stake { get; set; }
        public decimal Amount { get; set; }
        public string TrxType { get; set; }
        public string RiskyCust { get; set; }
        public string RiskyTrx { get; set; }

        public Transaction()
        {
        }

        public Transaction(string trxType, string[] data)
        {
            try
            {
                this.Customer = data[0];
                this.TrxEvent = data[1];
                this.Participant = data[2];
                this.Stake = System.Convert.ToDecimal(data[3]);
                this.Amount = System.Convert.ToDecimal(data[4]);
                this.TrxType = trxType;
            }
            catch
            {
                // Raise Error 'Invalid trx'
            }
        }

    }
}
