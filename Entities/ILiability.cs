namespace Alpha.Entities
{
    internal interface ILiability
    {
        //public List<double?> MainAccount { get; set; }
        //public List<double?> AddAccount { get; set; }

        //public List<double?> MainAnnualFlow { get; set; }
        //public List<double?> AddAnnualFlow { get; set; }

        public List<int> Idade { get; set; }

        public List<double?> Account { get; set; }
        public List<double?> AnnualFlow { get; set; }

        public void Calculate(DateTime baseDate, double? contrib, double? balance, double? benefit, double Interest, DateTime Birth, bool Withdraw, bool anual);
    }
}
