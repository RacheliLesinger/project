using MasavBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasavBL
{
    class Program
    {
        static void Main(string[] args)
        {
            //InitData();
        }

        static void InitData()
        {
            /*
using (var context = new MasavContext())
{

    var activities1 = new List<Activity>
    {
new Activity{Name = "פעיל"},
new Activity{Name = "מוקפא"},
new Activity{Name = "מחוק"}
};
    activities1.ForEach(s => context.Activities.Add(s));
    context.SaveChanges();

 var currencies = new List<Currency>
{
    new Currency {Name = "שקל"},
    new Currency {Name = "דולר"},
    new Currency {Name = "יורו"},
};
    currencies.ForEach(s => context.Currencies.Add(s));
    context.SaveChanges();

 var codeBanks = new List<CodeBank>
{
    new CodeBank {Name ="13-בנק אגוד לישראל "},
    new CodeBank {Name ="11-בנק דיסקונט לישראל "},
    new CodeBank {Name ="31-בנק הבינלאומי הראשון לישראל "},
    new CodeBank {Name ="12-בנק הפועלים "},
    new CodeBank {Name ="4-בנק יהב לעובדי המדינה "},
    new CodeBank {Name ="54-בנק ירושלים "},
    new CodeBank {Name ="10-בנק לאומי לישראל "},
    new CodeBank {Name ="30-בנק למסחר  (לבנק מונה מפרק זמני)"},
    new CodeBank {Name ="20-בנק מזרחי טפחות "},
    new CodeBank {Name ="46-בנק מסד "},
    new CodeBank {Name ="17-בנק מרכנתיל דיסקונט "},
    new CodeBank {Name ="68-מוניציפל בנק "},
    new CodeBank {Name ="-פועלים שירותי נאמנות  (חב' לנאמנות של פועלים)"},
    new CodeBank {Name ="50-מרכז סליקה בנקאי "},
    new CodeBank {Name ="59-שירותי בנק אוטומטיים"},

};
    codeBanks.ForEach(b => context.CodeBanks.Add(b));
    context.SaveChanges();

    var customers = new List<Customer>
{
new Customer{Name = "מטב באר שבע", CreatedDate = DateTime.Parse("2000-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-10"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="העליה 12", Contact = "אברהם לוי", ActivityId = 1},
new Customer{Name = "מטב ירושלים ", CreatedDate = DateTime.Parse("2001-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-10"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="כהנמן 10", Contact = "גיל ששון", ActivityId = 1},
new Customer{Name = "מטב תל אביב ", CreatedDate = DateTime.Parse("2002-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-10"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="ז'בוטינסקי 38", Contact = "יורם זק", ActivityId = 1},
new Customer{Name = "מטב חיפה ", CreatedDate = DateTime.Parse("2010-01-01") ,PaymentDate1 = DateTime.Parse("1999-06-10"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address=" 13 ז'בוטינסקי" ,Contact = "בני דוד", ActivityId = 1},
new Customer{Name = "גני ילדים בית יעקב ירושלים", CreatedDate = DateTime.Parse("2018-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-10"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="נלסון גליק 27", Contact = "חיים לוי", ActivityId = 1},
new Customer{Name = "וועד בית - בית הדפוס 8", CreatedDate = DateTime.Parse("2017-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-05"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="בית הדפוס 8 ירושלים", Contact = "יובל בשן", ActivityId = 1},
new Customer{Name = "וועד בית - הפלמח 12", CreatedDate = DateTime.Parse("2010-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-05"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="הפלמח ירושלים", Contact = "יובל בשן", ActivityId = 1},
new Customer{Name = "וועד בית -הגדוד העברי 36", CreatedDate = DateTime.Parse("2012-12-12") ,PaymentDate1 = DateTime.Parse("1999-01-05"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="אהרונוביץ 6 בני ברק", Contact = "יובל בשן", ActivityId = 1},
new Customer{Name = "בני עקיבא חיפה", CreatedDate = DateTime.Parse("2018-03-01") ,PaymentDate1 = DateTime.Parse("1999-01-05"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="העליה 56", Contact = "יובל בשן", ActivityId = 1},
new Customer{Name = "וועד בית - בית הדפוס 8", CreatedDate = DateTime.Parse("2010-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-05"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="נחל רביבים 13 בית שמש", Contact = "אורי יזרעלי", ActivityId = 1},
new Customer{Name = "קשר יהודי", CreatedDate = DateTime.Parse("2019-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-05"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="תאודור לוי ירושלים", Contact = "יוחאי הכט", ActivityId = 1},
new Customer{Name = "גני ילדים עץ הדעת", CreatedDate = DateTime.Parse("2010-01-01") ,PaymentDate1 = DateTime.Parse("1999-01-05"), PaymentDate2 = DateTime.Parse("1999-01-01"),Address="יד בנימין", Contact = "מירי גוטליב", ActivityId = 1}
};
    customers.ForEach(c => context.Customers.Add(c));
    context.SaveChanges();

var payings = new List<Paying>
{
new Paying{CustomerId = 13 , Name = "ראובן ששון", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 20, Amount = 100,  BankAccountNumber = "20",BankBranchNumber = "10", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =1, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 13 , Name = "חיים כהן", PaymentDate = DateTime.Parse("1999-01-05"), PaymentSum = 10, Amount =250, BankAccountNumber = "20",BankBranchNumber = "5", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =1, ActivityId =1, CurrencyId =2} ,
new Paying{CustomerId = 2 , Name = "שי לוי", PaymentDate = DateTime.Parse("1999-01-02"), PaymentSum = 10, Amount = 300, BankAccountNumber = "20",BankBranchNumber = "6", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =2, ActivityId =2, CurrencyId =1},
new Paying{CustomerId = 2 , Name = "הודיה גורביץ", PaymentDate = DateTime.Parse("1999-01-01"), PaymentSum = 20, Amount = 450,  BankAccountNumber = "20",BankBranchNumber = "1000", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =4, ActivityId =2,CurrencyId =1},
new Paying{CustomerId = 2 , Name = "שמעון ברק", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 200, Amount = 500,  BankAccountNumber = "20",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =3, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 2 , Name = "גדליה רוב", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 50, Amount = 350,  BankAccountNumber = "20",BankBranchNumber = "77", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =4, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "יובל רכניץ", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 50, Amount = 200,  BankAccountNumber = "20",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 5 , Name = "תהילה דקל", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 10, Amount = 500,  BankAccountNumber = "20",BankBranchNumber = "44", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =9, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "אחי נתן", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 7, Amount = 1000,  BankAccountNumber = "20",BankBranchNumber = "66", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =7, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 5 , Name = "אילה הכט", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 12, Amount = 1000,  BankAccountNumber = "20",BankBranchNumber = "22", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =1, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "תמר פולק", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 24, Amount = 2400,  BankAccountNumber = "20",BankBranchNumber = "12", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 3 , Name = "נועה כהן", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 36, Amount = 600,  BankAccountNumber = "20",BankBranchNumber = "30", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =8, ActivityId =3, CurrencyId =1},
new Paying{CustomerId = 3 , Name = "רחלי ניוביץ", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 700,  BankAccountNumber = "20",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =6, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 3 , Name = "יוסי לוי", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 20, Amount = 360,  BankAccountNumber = "20",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 3 , Name = "יריב פישמן", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 36, Amount = 30,  BankAccountNumber = "20",BankBranchNumber = "12", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =7, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 4 , Name = "חיה אזולאי", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 24, Amount = 300,  BankAccountNumber = "20",BankBranchNumber = "18", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =7, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 4 , Name = "נטלי דדון", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 24, Amount = 500,  BankAccountNumber = "20",BankBranchNumber = "22", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =1, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "הילה שי", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 24, Amount = 70,  BankAccountNumber = "999",BankBranchNumber = "223", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "חיים דוד", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 12, Amount = 1000,  BankAccountNumber = "777",BankBranchNumber = "9099", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =6, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "אהרון רזאל", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 10, Amount = 240,  BankAccountNumber = "444",BankBranchNumber = "878", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "שי לוי", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 10, Amount = 240,  BankAccountNumber = "87",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =11, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 5 , Name = "מרים אביטל", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 36, Amount = 2000,  BankAccountNumber = "987",BankBranchNumber = "656", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =10, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "יעל בן חיים", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 20, Amount = 200,  BankAccountNumber = "20",BankBranchNumber = "212", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 5 , Name = "מיכל יהושע", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 500, Amount = 1000,  BankAccountNumber = "77",BankBranchNumber = "666", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =10, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 6 , Name = "יעל כין", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 360, Amount = 500,  BankAccountNumber = "432",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =10, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 6 , Name = "אפרץ אורבך", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 1000, Amount = 550,  BankAccountNumber = "20",BankBranchNumber = "765", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =4, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 6 , Name = "ציון כהן", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 36, Amount = 360,  BankAccountNumber = "99",BankBranchNumber = "432", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =4, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 6 , Name = "לאה קיין", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 450,  BankAccountNumber = "20",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =3, ActivityId =3, CurrencyId =1},
new Paying{CustomerId = 7 , Name = "מנוחה זקס", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 250, Amount = 100,  BankAccountNumber = "777",BankBranchNumber = "098", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =3, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 7 , Name = "רעות ריין", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 10, Amount = 560,  BankAccountNumber = "432",BankBranchNumber = "564", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =3, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 7 , Name = "דינה פוגל", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 30, Amount = 550,  BankAccountNumber = "897",BankBranchNumber = "55", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =3, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 7 , Name = "יפה לוי", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 40, Amount = 240,  BankAccountNumber = "324",BankBranchNumber = "6630", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 8 , Name = "רחלי ברנשטיין", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 50, Amount = 100,  BankAccountNumber = "20",BankBranchNumber = "521", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 8 , Name = "נעמה חיים", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 20, Amount = 500,  BankAccountNumber = "78",BankBranchNumber = "833", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =11, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 8 , Name = "טניה דוד", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 500,  BankAccountNumber = "20",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =12, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 8 , Name = "פייגי לסקר", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 100,  BankAccountNumber = "56",BankBranchNumber = "943", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =12, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 8 , Name = "זאב זבוטינסקי", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 500, Amount = 500,  BankAccountNumber = "20",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =11, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 9 , Name = "נח הרצל", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 500, Amount = 120,  BankAccountNumber = "200",BankBranchNumber = "222", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 9 , Name = "דני יורם", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 300, Amount = 390,  BankAccountNumber = "100",BankBranchNumber = "11", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =10, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 10 , Name = "נועה לוין", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 1000,  BankAccountNumber = "34",BankBranchNumber = "5544", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =9, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 10 , Name = "מנחם מועלם", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 20, Amount = 150,  BankAccountNumber = "45",BankBranchNumber = "33", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =8, ActivityId =1, CurrencyId =1},
new Paying{CustomerId = 11 , Name = "אלחנן דגני", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 50, Amount = 135,  BankAccountNumber = "321",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =8, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 11 , Name = "מני גבריאל", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 125,  BankAccountNumber = "20",BankBranchNumber = "333", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =6, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 11 , Name = "עומרי לוי", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 105,  BankAccountNumber = "45",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =9, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 12 , Name = "לאה ארביב", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 10, Amount = 50,  BankAccountNumber = "87",BankBranchNumber = "55", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =7, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 12 , Name = "יצחק שילוח", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 20, Amount = 100,  BankAccountNumber = "974",BankBranchNumber = "444", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =6, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 11 , Name = "חוי ראקוב", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 150,  BankAccountNumber = "2323",BankBranchNumber = "777", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =2, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 10 , Name = "ניבה ברנר", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 100, Amount = 45,  BankAccountNumber = "6543",BankBranchNumber = "764", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =1, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 8 , Name = "ברוך שטיין", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 50, Amount = 785,  BankAccountNumber = "766",BankBranchNumber = "9885", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =1, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 9 , Name = "מיכל בן הרוש", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 36, Amount = 35,  BankAccountNumber = "20",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =1, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 9 , Name = "אסף שטיין", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 30, Amount = 90,  BankAccountNumber = "109",BankBranchNumber = "543", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 13 , Name = "נעמי פרידמן", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 36, Amount = 300,  BankAccountNumber = "208",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =2, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 12 , Name = "לי דקל", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 15, Amount = 300,  BankAccountNumber = "20",BankBranchNumber = "917", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =2, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 12 , Name = "טובה גוטליב", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 30, Amount = 195,  BankAccountNumber = "20",BankBranchNumber = "715", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 12 , Name = "דבי סופר", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 30, Amount = 200,  BankAccountNumber = "195",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =2, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 11 , Name = "נתי אהרון", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 10, Amount = 200,  BankAccountNumber = "20",BankBranchNumber = "615", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =10, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 11 , Name = "רויטל יגודה", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 20, Amount = 1000,  BankAccountNumber = "100",BankBranchNumber = "88", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =4, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 8 , Name = "בני אהרוני", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 10, Amount = 2000,  BankAccountNumber = "875",BankBranchNumber = "872", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =4, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 7 , Name = "רויטל אליסיאן", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 20, Amount = 360,  BankAccountNumber = "208",BankBranchNumber = "765", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =5, ActivityId =1, CurrencyId =2},
new Paying{CustomerId = 5 , Name = "חנן זולדן", PaymentDate = DateTime.Parse("1999-01-10"), PaymentSum = 10, Amount = 500,  BankAccountNumber = "309",BankBranchNumber = "098", StartDate = DateTime.Parse("1999-01-10"), EndDate = DateTime.Parse("2020-01-10"), CodeBankId =8, ActivityId =1, CurrencyId =2}
};
payings.ForEach(s => context.Payings.Add(s));
context.SaveChanges();

var users = new List<User>
{
new User{ UserName = "Admin1", Password="1234",Role="Admin" },
new User{ UserName = "Admin2", Password="1111",Role="Admin" },
new User{ UserName = "Admin3", Password="aaa", Role="Admin" },
new User{ UserName = "official1",Password="aaa", Role="Official" },
new User{ UserName = "official2",Password="1234", Role="Official" },
new User{ UserName = "official3",Password="aaa", Role="Official" },
new User{ UserName = "u7",Password="1qaz", Role="Official" },
};
users.ForEach(u => context.Users.Add(u));
context.SaveChanges();

}
*/
        }
    }
}
