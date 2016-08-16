using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InformatrixRiskApp.Models;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace InformatrixRiskApp.Controllers
{
    [Route("infomatrix/[controller]")]
    public class TransactionsController : Controller
    {
        InputFilesPath riskFiles { get; }

        public TransactionsController(IOptions<InputFilesPath> filesConfig)
        {
            // Get files path from appsettings
            riskFiles = filesConfig.Value;
        }

        public IActionResult Index()
        {
            // Read transactions
            ITransactionRepository trxItems = new TransactionRepository();
            IEnumerable<Transaction> trx = trxItems.GetAll(riskFiles.SettledFile, riskFiles.UnsettledFile);

            // Get Settled transactions
            var settledTrxs = from t in trx
                              where t.TrxType == "settled"
                              select t;

            // Counting trx per customer
            var custTrx = from t in settledTrxs
                          group t by t.Customer
                      into grp
                          select new
                          {
                              Customer = grp.Key,
                              Count = grp.Select(x => x.Amount).Count()
                          };

            // Count the number of successful trx per customer
            var succCustTrx = from t in settledTrxs
                              where t.Amount > 0
                              group t by t.Customer
                              into grp
                              select new
                              {
                                  Customer = grp.Key,
                                  Count = grp.Select(x => x.Amount).Count()
                              };

            // Select risky customers
            var riskyCustomers = from t in succCustTrx
                                 join t2 in custTrx on t.Customer equals t2.Customer
                                 let r = (double)t.Count / (double)t2.Count
                                 where (r > .6)
                                 select new
                                 {
                                     Customer = t.Customer,
                                     Risk = r
                                 };

            string[] riskCust = riskyCustomers.Select(p => p.Customer).ToArray();

            // Add Customer Risk analysis
            List<Transaction> trxRisk = new List<Transaction>();

            foreach (Transaction t in trx)
            {
                // If customer is in riskCust array is risky
                if (Array.IndexOf(riskCust, t.Customer) > -1)
                {
                    trxRisk.Add(new Transaction()
                    {
                        Amount = t.Amount,
                        Customer = t.Customer,
                        Participant = t.Participant,
                        Stake = t.Stake,
                        TrxEvent = t.TrxEvent,
                        TrxType = t.TrxType,
                        RiskyCust = "risk customer",
                        RiskyTrx = ""
                    });
                }
                else
                {
                    trxRisk.Add(new Transaction()
                    {
                        Amount = t.Amount,
                        Customer = t.Customer,
                        Participant = t.Participant,
                        Stake = t.Stake,
                        TrxEvent = t.TrxEvent,
                        TrxType = t.TrxType,
                        RiskyCust = "",
                        RiskyTrx = ""
                    });
                }
            }

            // Order Settled trxs per customer
            trxRisk.Sort((x, y) => string.Compare(x.Customer, y.Customer));

            return View(trxRisk);
        }
    }
}
