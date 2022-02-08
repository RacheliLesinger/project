using MasavBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using log4net;
using System.Net.Http;

namespace MasavBL
{
    public class DB
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<Paying> GetPayings(int customerId= 0, int activityId = 0)
        {
            using(var ctx = new MasavContext())
            {
                var list = ctx.Payings.AsNoTracking()
                    .Include("CodeBank")
                    .Include("Currency")
                    .Include("Customers")
                    .Include("Activity")
                    .Include("PaymentHistory")
                    .Where(i => (i.CustomerId == customerId || customerId == 0) &&
                                                  (i.ActivityId == activityId || activityId == 0))
                    .ToList();

                return list;
            }
        }

        public static bool UpdatePayings(Paying paying)
        {
            try
            {
                using (var ctx = new MasavContext())
                {
                    ctx.Payings.AddOrUpdate(paying);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in UpdatePayings {paying.Id} , ex: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateCustomers(Customer customer)
        {
            try
            {
                using (var ctx = new MasavContext())
                {
                    ctx.Customers.AddOrUpdate(customer);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in UpdateCustomer {customer.Id} , ex: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateBroadcast(BroadcastHistory broadcast)
        {
            try
            {
                using (var ctx = new MasavContext())
                {
                    ctx.BroadcastHistories.AddOrUpdate(broadcast);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in UpdateBroadcast {broadcast.Id} , ex: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> AddBrodcastHistory(int amount, int customerId, int sumRecord, int sumNewRecords = 0)
        {
            try
            {
                using (var ctx = new MasavContext())
                {
                    var bh = new BroadcastHistory()
                    {
                        BroadcastDate = DateTime.Now,
                        BroadcastAmount = amount,
                        CustomerId = customerId,
                        SumRecords = sumRecord,
                        SumNewRecords = sumNewRecords,
                        ValueDate = DateTime.Now
                    };
                    ctx.BroadcastHistories.Add(bh);
                    await ctx.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in AddBrodcastHistory, customerId: {customerId} , ex: {ex.Message}");
                return false;
            }
            return true;
        }
        public static List<Paying> GetPayingsToReport(int dayInMonth , int customerId, int year, int month)
        {
            var dt = new DateTime(year, month, dayInMonth);
            using (var ctx = new MasavContext())
            {
                return ctx.Payings.AsNoTracking()
                    .Include("CodeBank")
                    .Where(p => p.CustomerId == customerId && p.ActivityId == 1 && p.PaymentDate == dayInMonth
                                   &&( (p.StartDate <= dt && p.EndDate >= dt)
                                   ||(p.PaymentSum > 0 )))
                    .ToList();
            }
        }

        public static async Task GetCurrencyRateAsync()
        {
            //Examples:
            var from = "EUR";
            var to = "USD";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://free.currencyconverterapi.com");
                    var response = await client.GetAsync($"/api/v6/convert?q={from}_{to}&compact=y");
                    var stringResult = await response.Content.ReadAsStringAsync();
                    //var dictResult = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(stringResult);
                    //return dictResult[$"{from}_{to}"]["val"];
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    //return "Error calling API. Please do manual lookup.";
                }
            }

        }



        public static string GetPayingName(int payingId)
        {
            using (var ctx = new MasavContext())
            {
                return ctx.Payings.AsNoTracking().FirstOrDefault(c => c.Id == payingId)?.Name;
            }
        }

        public static string GetPayingIdentityNumber(int payingId)
        {
            using (var ctx = new MasavContext())
            {
                return ctx.Payings.AsNoTracking().FirstOrDefault(c => c.Id == payingId)?.IdentityNumber;
            }
        }

        public static string GetCustomerName(int customerId)
        {
            using (var ctx = new MasavContext())
            {
                return ctx.Customers.AsNoTracking().FirstOrDefault(c => c.Id == customerId)?.Name;
            }
        }

        public static string GetCustomerCode(int customerId)
        {
            using (var ctx = new MasavContext())
            {
                return ctx.Customers.AsNoTracking().FirstOrDefault(c => c.Id == customerId)?.Code;
            }
        }

        public static Institution GetInstitutionByCustomerId(int customerId)
        {
            using (var ctx = new MasavContext())
            {
                return ctx.Customers.AsNoTracking()
                    .Include("Institution")
                    .FirstOrDefault(c => c.Id == customerId)?.Institution;
            }
        }

        public static async Task<AddPaymentHistoryRes> AddPaymentHistory(int dayInMonth, int customerId, int year, int month)
        {
            var res = new AddPaymentHistoryRes();
            try
            {
                var dt = new DateTime(year, month, dayInMonth);
                using (var ctx = new MasavContext())
                {
                    var payments = ctx.Payings.Where(p => p.CustomerId == customerId && p.ActivityId == 1 && p.PaymentDate == dayInMonth
                                       && ((p.StartDate <= dt && p.EndDate >= dt)
                                   || (p.PaymentSum > 0))).ToList();
                    foreach (var item in payments)
                    {
                        if (!item.IsNew)
                        {
                            item.IsNew = true;
                            ctx.Payings.AddOrUpdate(item);
                        }
                        var ph = new PaymentHistory()
                        {
                            CustomerId = customerId,
                            PaidId = item.Id,
                            PaymentDate = DateTime.Now,
                            PaymentAmount = item.Amount
                        };
                        ctx.PaymentHistories.Add(ph);
                        await ctx.SaveChangesAsync();
                        res.AmountSum += (int)item.Amount;
                        res.SumRecord++;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in AddPaymentHistory, customerId: {customerId} date in month: {dayInMonth} , ex: {ex.Message}");
            }
            return res;
        }

        public static AddPaymentHistoryRes GetSumResultOfPayment(int dt, int customerId)
        {
            var res = new AddPaymentHistoryRes();
            try
            {
                using (var ctx = new MasavContext())
                {
                    var payments = ctx.Payings.AsNoTracking()
                        .Where(p => p.CustomerId == customerId && p.ActivityId == 1 && p.PaymentDate == dt).ToList();
                    foreach (var item in payments)
                    { 
                        res.AmountSum += (int)item.Amount;
                        res.SumRecord++;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in GetSumResultOfPayment, customerId: {customerId} date in month: {dt} , ex: {ex.Message}");
            }
            return res;
        }

        public static List<Customer> GetCustomers(int activityId = 0)
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.Customers.AsNoTracking()
                    .Include("Activity")
                    .Where(i => (i.ActivityId == activityId || activityId == 0))
                    .ToList();
                return list;
            }
        }

        public static List<BroadcastHistory> GetBroadcastHistoryList(int customerId = 0)
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.BroadcastHistories.AsNoTracking()
                    .Include("Customers")
                    .Where(i => (i.CustomerId == customerId || customerId == 0))
                    .ToList();
                return list;
            }
        }

        public static List<Customer> GetCustomersList()
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.Customers.AsNoTracking()
                    .ToList();
                return list;
            }
        }

        public static List<Currency> GetCurrencyList()
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.Currencies.AsNoTracking()
                    .ToList();
                return list;
            }
        }

        public static List<CodeBank> GetBanksList()
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.CodeBanks.AsNoTracking()
                    .ToList();
                return list;
            }
        }

        public static int GetBankIdByCode(string codeBank)
        {
            using (var ctx = new MasavContext())
            {
                var val = ctx.CodeBanks.AsNoTracking()
                    .FirstOrDefault(b => b.Code == codeBank);
                if(val != null)
                  return val.Id;
                return 0;
            }
        }

        public static List<Activity> GetActivitiesList()
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.Activities.AsNoTracking()
                    .ToList();
                return list;
            }
        }

        public static List<PaymentHistory> GetPaymentHistory(int customerId, DateTime fromDate, DateTime endDate)
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.PaymentHistories.AsNoTracking()
                    .Where(p => p.CustomerId == customerId && p.PaymentDate >= fromDate && p.PaymentDate <= endDate )
                    .OrderBy(p => p.PaidId)
                    .ToList();
                return list;
            }
        }
    }

    public class AddPaymentHistoryRes
    {
        public int AmountSum { get; set; }
        public int SumNewRecord { get; set; }
        public int SumRecord { get; set; }

        public AddPaymentHistoryRes()
        {
            this.AmountSum = 0;
            this.SumNewRecord = 0;
            this.SumRecord = 0;
        }
    }
}
