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
    enum EnumStatus : int
    {
        Active =1,
        InActive =2
    }

    enum EnumCurrency : int
    {
        Shekel = 1,
        Dolar = 2,
        Euro =3
    }
    public class DB
    {
        //private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Paying> GetPayings(int customerId= 0, int activityId = 0, int classId =0)
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
                                                  (i.ActivityId == activityId || activityId == 0) &&
                                                  i.Class == classId)
                    .OrderBy(i => i.Customers.Name)
                    .ThenBy(i => i.Name)
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

        public static async Task<bool> AddBrodcastHistory(double amount, int customerId,int payingClass, int sumRecord, int sumNewRecords = 0)
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
                        ValueDate = DateTime.Now,
                        Class = payingClass,
                        StatusId = (int)EnumStatus.Active,
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
        public static List<Paying> GetPayingsToReport(int dayInMonth , int customerId, int year, int month, int payingClass)
        {
            var dt = new DateTime(year, month, dayInMonth);
            using (var ctx = new MasavContext())
            {

                var payments = new List<Paying>();
                if (payingClass == 0)
                {
                    payments = ctx.Payings.AsNoTracking()
                                  .Include("CodeBank")
                                  .Where(p => p.CustomerId == customerId && p.ActivityId == 1
                                   && p.PaymentDate == dayInMonth
                                   && ((p.StartDate <= dt && p.EndDate >= dt)
                                   || (p.PaymentSum > 0)))
                                   .OrderBy(p => p.Name)
                                   .ToList();
                }
                else
                {
                    payments = ctx.Payings.AsNoTracking()
                        .Include("CodeBank")
                        .Where(p => p.CustomerId == customerId && p.ActivityId == 1
                                       && p.PaymentDate == dayInMonth && p.Class == payingClass
                                       && ((p.StartDate <= dt && p.EndDate >= dt)
                                       || (p.PaymentSum > 0)))
                                       .OrderBy(p => p.Name)
                                       .ToList();
                }
                var lastRate = ctx.CurrencyRates.OrderByDescending(r => r.Date).FirstOrDefault();
                if (lastRate != null)
                {
                    foreach (var item in payments)
                    {
                        if (item.CurrencyId == (int)EnumCurrency.Dolar)
                            item.Amount = item.Amount * lastRate.CurrencyRateInShekels;
                    }
                }
                return payments;
            }
        }

        public static bool IsNewPaying(Paying paying)
        {
            //בדיקה עבור יצירת PDF 
            //שאז כבר נוצרה רשומה בטבלת היסטוריית תשלומים
            using (var ctx = new MasavContext())
            {
               var res = ctx.PaymentHistories.AsNoTracking().Where(p => p.PaidId == paying.Id && p.StatusId == (int)EnumStatus.Active);
                if (res != null && res.Count() == 1)
                    return true;
            }
            return false;
        }

        public static bool IsNewPayingToPaymentHistory(Paying paying)
        {
            //בדיקה עבור עדכון טבלת PaymentHistory 
            //שעדיין לא נוצרה רשומה בטבלת היסטוריית תשלומים
            using (var ctx = new MasavContext())
            {
                var res = ctx.PaymentHistories.AsNoTracking().FirstOrDefault(p => p.PaidId == paying.Id && p.StatusId == (int)EnumStatus.Active);
                if (res == null)
                    return true;
            }
            return false;
        }

        public static double? GetLastRate()
        {
            using (var ctx = new MasavContext())
            {
                return ctx.CurrencyRates.OrderByDescending(r => r.Date).FirstOrDefault()?.CurrencyRateInShekels;
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

        public static async Task UpdateCurrencyRateAsync(string rate)
        {
            using (var ctx = new MasavContext())
            {
                double rateD;
                var res = Double.TryParse(rate, out rateD);
                if (res == true)
                {
                    var cr = new CurrencyRates()
                    {
                        CurrencyId = (int)EnumCurrency.Dolar,
                        Date = DateTime.Now,
                        CurrencyRateInShekels = rateD,
                    };
                    ctx.CurrencyRates.Add(cr);
                    await ctx.SaveChangesAsync();
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

        public static async Task<AddPaymentHistoryRes> AddPaymentHistory(int dayInMonth, int customerId, int payingClass, int year,
                                                             int month, bool isOverride= false)
        {
            var res = new AddPaymentHistoryRes();
            try
            {
                var dt = new DateTime(year, month, dayInMonth);
               
                using (var ctx = new MasavContext())
                {
                    var lastRate = ctx.CurrencyRates.OrderByDescending(r => r.Date).FirstOrDefault();
                    if (isOverride)
                    {
                        await OveerideLastPaymentToSpecificCustomer(customerId);
                    }
                    var payments = new List<Paying>();
                    if (payingClass == 0)
                    {
                        payments = ctx.Payings.Where(p => p.CustomerId == customerId && p.ActivityId == 1
                                       && p.PaymentDate == dayInMonth
                                       && ((p.StartDate <= dt && p.EndDate >= dt)
                                       || (p.PaymentSum > 0))).ToList();
                    }
                    else
                    {
                        payments = ctx.Payings.Where(p => p.CustomerId == customerId && p.ActivityId == 1
                                           && p.PaymentDate == dayInMonth && p.Class == payingClass
                                           && ((p.StartDate <= dt && p.EndDate >= dt)
                                           || (p.PaymentSum > 0))).ToList();
                    }
                    double? tempAmount = 0;
                    foreach (var item in payments)
                    {
                        if (item.IsNew || IsNewPayingToPaymentHistory(item))
                        {
                            item.IsNew = false;
                            ctx.Payings.AddOrUpdate(item);
                            res.SumNewRecord++;
                        }

                        if (lastRate != null && item.CurrencyId == (int)EnumCurrency.Dolar)
                        {
                            tempAmount = item.Amount * lastRate.CurrencyRateInShekels;
                        }
                        else tempAmount = item.Amount;

                        var ph = new PaymentHistory()
                        {
                            CustomerId = customerId,
                            PaidId = item.Id,
                            PaymentDate = DateTime.Now,
                            PaymentAmount = tempAmount,
                            StatusId = (int)EnumStatus.Active,
                        };
                        ctx.PaymentHistories.Add(ph);
                        await ctx.SaveChangesAsync();
                        res.AmountSum += (double)tempAmount;
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

        private static async Task OveerideLastPaymentToSpecificCustomer(int customerId)
        {
            try
            {
                using (var ctx = new MasavContext())
                {
                    //שליפת השידור האחרון עבור הלקוח
                    var customerBroadcast = ctx.BroadcastHistories.Where(b => b.CustomerId == customerId
                                                            && b.StatusId == (int)EnumStatus.Active)
                                                            .OrderByDescending(b => b.BroadcastDate);

                    if (customerBroadcast.FirstOrDefault() != null)
                    {
                        var latestBroadcast = customerBroadcast.First();
                        //להפוך לבוטל את התשלומים שבוצעו
                        var lastPayments = ctx.PaymentHistories.ToList().Where(p => p.CustomerId == customerId
                                                            && p.StatusId == (int)EnumStatus.Active
                                                            && p.PaymentDate.Value.Date == latestBroadcast.BroadcastDate.Value.Date);
                        foreach (var paymentHistory in lastPayments)
                        {
                            paymentHistory.StatusId = (int)EnumStatus.InActive;
                            ctx.PaymentHistories.AddOrUpdate(paymentHistory);
                        }
                        await ctx.SaveChangesAsync();

                        //להפוך לבוטל את ההיסטוריית שידור
                        latestBroadcast.StatusId = (int)EnumStatus.InActive;
                        ctx.BroadcastHistories.AddOrUpdate(latestBroadcast);
                        await ctx.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in OveerideLastPaymentToSpecificCustomer, customerId: {customerId} , ex: {ex.Message}");
            }
        }

        public static AddPaymentHistoryRes GetSumResultOfPayment(int dt, int customerId)
        {
            var res = new AddPaymentHistoryRes();
            try
            {
                using (var ctx = new MasavContext())
                {
                    var lastRate = ctx.CurrencyRates.OrderByDescending(r => r.Date).FirstOrDefault();
                    var payments = ctx.Payings.AsNoTracking()
                        .Where(p => p.CustomerId == customerId && p.ActivityId == 1 && p.PaymentDate == dt).ToList();
                    foreach (var item in payments)
                    {
                        if (lastRate != null && item.CurrencyId == (int)EnumCurrency.Dolar)
                        {
                            item.Amount = item.Amount * lastRate.CurrencyRateInShekels;
                        }
                        res.AmountSum += (double)item.Amount;
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
                    .Include("Institution")
                    .Where(i => (i.ActivityId == activityId || activityId == 0))
                    .OrderBy(i => i.Name)
                    .ToList();
                return list;
            }
        }

        public static List<int> GetClassesToCustomer(int customerId)
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.Payings.AsNoTracking()
                    .Where(i => i.CustomerId == customerId)
                    .GroupBy(i => i.Class)
                    .Select(n => n.Key)
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
                    .Include("Status")
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
            codeBank = codeBank.Replace(" ", "");
            codeBank = codeBank.TrimStart('0');
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

        public static List<Institution> GetInstitutionsList()
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.Institutions.AsNoTracking()
                    .ToList();
                return list;
            }
        }

        public static List<PaymentHistory> GetPaymentHistory(int customerId = 0)
        {
            using (var ctx = new MasavContext())
            {
                var list = ctx.PaymentHistories
                    .Include("Paying")
                    .Include("Customers")
                    .AsNoTracking()
                    .Where(p => (p.CustomerId == customerId ||customerId == 0) && p.StatusId == (int)EnumStatus.Active)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToList();
                foreach (var item in list)
                {
                    item.Paying = ctx.Payings.FirstOrDefault(p => p.Id == item.PaidId);
                }
                return list;
            }
        }
    }

    public class AddPaymentHistoryRes
    {
        public double AmountSum { get; set; }
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
