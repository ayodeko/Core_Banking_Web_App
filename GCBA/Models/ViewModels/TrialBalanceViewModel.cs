namespace GCBA.Models.ViewModels
{
    public class TrialBalanceViewModel
    {
        public string SubCategory { get; set; }
        public string MainCategory { get; set; }
        public string AccountName { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
    }
}