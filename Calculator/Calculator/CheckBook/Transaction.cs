﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Calculator.CheckBook
{
    public class CbDb : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }

    public class Transaction : BaseVM
    {
        public int Id { get; set; }

        /*
        public IEnumerable<Transaction> SimilarTransactions {
            get {
                return from t in VM.Transactions
                       where t.Payee == this.Payee
                       select t;
            }
        }
        */
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged(); }
        }

        private string _Payee;
        public string Payee
        {
            get { return _Payee; }
            set { _Payee = value; OnPropertyChanged(); }
        }

        public int AccountId { get; set; }

        private Account _Account;
        public virtual Account Account
        {
            get { return _Account; }
            set { _Account = value; OnPropertyChanged(); }
        }

        private double _Amount;
        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; OnPropertyChanged(); OnPropertyChanged("Currency2"); }
        }

        public double Currency2
        {
            get
            {
                if (ExchangeRateSing.Instance.Rates == null) return 0;
                return Amount * ExchangeRateSing.Instance.Rates.CAD;
            }
        }

        private string _Tag;
        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; OnPropertyChanged(); }
        }


    }
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Institution { get; set; }
        public bool Business{ get; set; }
        public virtual IList<Transaction> Transactions { get; set; }

    }

    public class CheckBookVM : BaseVM
    {
        public CheckBookVM()
        {
        }

        CbDb _Db = new CbDb();

        private int _RowsPerPage = 5;
        private int _CurrentPage = 1;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; OnPropertyChanged(); OnPropertyChanged("CurrentTransactions"); }
        }

        private ObservableCollection<Transaction> _Transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get { return _Transactions; }
            set { _Transactions = value; OnPropertyChanged(); OnPropertyChanged("Accounts"); }
        }
        public IEnumerable<Account> Accounts
        {
            get { return _Db.Accounts.Local; }
        }

        public IEnumerable<Transaction> CurrentTransactions
        {
            get { return Transactions.Skip((_CurrentPage - 1) * _RowsPerPage).Take(_RowsPerPage); }
        }

        public DelegateCommand MoveFront
        {
            get
            {
                return new DelegateCommand
                {
                    ExecuteFunction = _ => CurrentPage--,
                    CanExecuteFunction = _ => CurrentPage * _RowsPerPage > Transactions.Count
                };
            }
        }

        public DelegateCommand MoveNext
        {
            get
            {
                return new DelegateCommand
                {
                    ExecuteFunction = _ => CurrentPage++,
                    CanExecuteFunction = _ => CurrentPage * _RowsPerPage < Transactions.Count
                };
            }
        }

        public DelegateCommand Save
        {
            get
            {
                return new DelegateCommand
                {
                    ExecuteFunction = _ => _Db.SaveChangesAsync(),// .SaveChanges(),
                    CanExecuteFunction = _ => _Db.ChangeTracker.HasChanges()
                };
            }
        }

        public DelegateCommand NewTransaction
        {
            get
            {
                return new DelegateCommand
                {
                    ExecuteFunction = _ =>
                    {
                        Transactions.Add(new Transaction { });
                        CurrentPage = Transactions.Count / _RowsPerPage + 1;
                    }
                };
            }
        }

        private Rates _CurrentRates;
        public Rates CurrentRates
        {
            get { return _CurrentRates; }
            set { _CurrentRates = value; OnPropertyChanged(); }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }
        private string _Picture;

        public string Picture
        {
            get { return _Picture; }
            set { _Picture = value; OnPropertyChanged(); }
        }


        public async void Fill()
        {
            Transactions = _Db.Transactions.Local;
            _Db.Accounts.ToList();
            _Db.Transactions.ToList();
            //new ObservableCollection<Transaction>();

            var wnd = new LoginWindow();
            var token = await wnd.Login();

            var http = new HttpClient();

            dynamic me = Newtonsoft.Json.JsonConvert.DeserializeObject(await http.GetStringAsync("https://graph.facebook.com/me?access_token=" + token));
            
            Name = me.name;
            Picture = "https://graph.facebook.com/" + me.id + "/picture";

            var results = await http.GetAsync("http://openexchangerates.org/api/latest.json?app_id=2f23629162b444b580bc03970c41caad");
            var currencies = await results.Content.ReadAsAsync<ExchageRate>();
            CurrentRates = currencies.rates;
            ExchangeRateSing.Instance.Rates = currencies.rates;
            OnPropertyChanged("CurrentTransactions"); OnPropertyChanged("Transactions");
        }
    }

    public class ExchangeRateSing
    {
        private ExchangeRateSing() { }
        private static ExchangeRateSing _Instance;
        public static ExchangeRateSing Instance
        {
            get { if (_Instance == null)_Instance = new ExchangeRateSing(); return _Instance; }
        }

        private Rates _Rates;
        public Rates Rates
        {
            get { return _Rates; }
            set { _Rates = value; }
        }

    }
    public class ExchageRate
    {
        public string disclaimer { get; set; }
        public string license { get; set; }
        public int timestamp { get; set; }
        public string _base { get; set; }
        public Rates rates { get; set; }
    }

    public class Rates
    {
        public float AED { get; set; }
        public float AFN { get; set; }
        public float ALL { get; set; }
        public float AMD { get; set; }
        public float ANG { get; set; }
        public float AOA { get; set; }
        public float ARS { get; set; }
        public float AUD { get; set; }
        public float AWG { get; set; }
        public float AZN { get; set; }
        public float BAM { get; set; }
        public float BBD { get; set; }
        public float BDT { get; set; }
        public float BGN { get; set; }
        public float BHD { get; set; }
        public float BIF { get; set; }
        public float BMD { get; set; }
        public float BND { get; set; }
        public float BOB { get; set; }
        public float BRL { get; set; }
        public float BSD { get; set; }
        public float BTC { get; set; }
        public float BTN { get; set; }
        public float BWP { get; set; }
        public float BYR { get; set; }
        public float BZD { get; set; }
        public float CAD { get; set; }
        public float CDF { get; set; }
        public float CHF { get; set; }
        public float CLF { get; set; }
        public float CLP { get; set; }
        public float CNY { get; set; }
        public float COP { get; set; }
        public float CRC { get; set; }
        public float CUC { get; set; }
        public float CUP { get; set; }
        public float CVE { get; set; }
        public float CZK { get; set; }
        public float DJF { get; set; }
        public float DKK { get; set; }
        public float DOP { get; set; }
        public float DZD { get; set; }
        public float EEK { get; set; }
        public float EGP { get; set; }
        public float ERN { get; set; }
        public float ETB { get; set; }
        public float EUR { get; set; }
        public float FJD { get; set; }
        public float FKP { get; set; }
        public float GBP { get; set; }
        public float GEL { get; set; }
        public float GGP { get; set; }
        public float GHS { get; set; }
        public float GIP { get; set; }
        public float GMD { get; set; }
        public float GNF { get; set; }
        public float GTQ { get; set; }
        public float GYD { get; set; }
        public float HKD { get; set; }
        public float HNL { get; set; }
        public float HRK { get; set; }
        public float HTG { get; set; }
        public float HUF { get; set; }
        public float IDR { get; set; }
        public float ILS { get; set; }
        public float IMP { get; set; }
        public float INR { get; set; }
        public float IQD { get; set; }
        public float IRR { get; set; }
        public float ISK { get; set; }
        public float JEP { get; set; }
        public float JMD { get; set; }
        public float JOD { get; set; }
        public float JPY { get; set; }
        public float KES { get; set; }
        public float KGS { get; set; }
        public float KHR { get; set; }
        public float KMF { get; set; }
        public float KPW { get; set; }
        public float KRW { get; set; }
        public float KWD { get; set; }
        public float KYD { get; set; }
        public float KZT { get; set; }
        public float LAK { get; set; }
        public float LBP { get; set; }
        public float LKR { get; set; }
        public float LRD { get; set; }
        public float LSL { get; set; }
        public float LTL { get; set; }
        public float LVL { get; set; }
        public float LYD { get; set; }
        public float MAD { get; set; }
        public float MDL { get; set; }
        public float MGA { get; set; }
        public float MKD { get; set; }
        public float MMK { get; set; }
        public float MNT { get; set; }
        public float MOP { get; set; }
        public float MRO { get; set; }
        public float MTL { get; set; }
        public float MUR { get; set; }
        public float MVR { get; set; }
        public float MWK { get; set; }
        public float MXN { get; set; }
        public float MYR { get; set; }
        public float MZN { get; set; }
        public float NAD { get; set; }
        public float NGN { get; set; }
        public float NIO { get; set; }
        public float NOK { get; set; }
        public float NPR { get; set; }
        public float NZD { get; set; }
        public float OMR { get; set; }
        public float PAB { get; set; }
        public float PEN { get; set; }
        public float PGK { get; set; }
        public float PHP { get; set; }
        public float PKR { get; set; }
        public float PLN { get; set; }
        public float PYG { get; set; }
        public float QAR { get; set; }
        public float RON { get; set; }
        public float RSD { get; set; }
        public float RUB { get; set; }
        public float RWF { get; set; }
        public float SAR { get; set; }
        public float SBD { get; set; }
        public float SCR { get; set; }
        public float SDG { get; set; }
        public float SEK { get; set; }
        public float SGD { get; set; }
        public float SHP { get; set; }
        public float SLL { get; set; }
        public float SOS { get; set; }
        public float SRD { get; set; }
        public float STD { get; set; }
        public float SVC { get; set; }
        public float SYP { get; set; }
        public float SZL { get; set; }
        public float THB { get; set; }
        public float TJS { get; set; }
        public float TMT { get; set; }
        public float TND { get; set; }
        public float TOP { get; set; }
        public float TRY { get; set; }
        public float TTD { get; set; }
        public float TWD { get; set; }
        public float TZS { get; set; }
        public float UAH { get; set; }
        public float UGX { get; set; }
        public float USD { get; set; }
        public float UYU { get; set; }
        public float UZS { get; set; }
        public float VEF { get; set; }
        public float VND { get; set; }
        public float VUV { get; set; }
        public float WST { get; set; }
        public float XAF { get; set; }
        public float XAG { get; set; }
        public float XAU { get; set; }
        public float XCD { get; set; }
        public float XDR { get; set; }
        public float XOF { get; set; }
        public float XPF { get; set; }
        public float YER { get; set; }
        public float ZAR { get; set; }
        public float ZMK { get; set; }
        public float ZMW { get; set; }
        public float ZWL { get; set; }
    }

}
