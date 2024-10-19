namespace Alpha.Entities
{
    internal class FixedValue : ILiability
    {
        public List<double?> Account { get; set; } = new List<double?>();
        public List<double?> AnnualFlow { get; set; } = new List<double?>();
        public List<int> Idade { get; set; } = new List<int>();

        public void Calculate(DateTime baseDate, double? contrib, double? balance, double? benefit, double interest, DateTime Birth, bool withdraw, bool anual)
        {
            double? payment = 0.0;

            DateTime forecastDate = new DateTime(baseDate.Year, baseDate.Month, 1);
            DateTime endDate = forecastDate.AddYears(60);
            double? saldo = balance;

            int idade = 0;
            int tabua = 86;
            if (withdraw == false) tabua = 150;

            while (saldo > benefit && idade < tabua && forecastDate < endDate)
            {
                forecastDate = forecastDate.AddMonths(1);

                saldo -= benefit;
                if (forecastDate.Month != 12) saldo *= (interest + 1);

                payment += benefit;

                if (anual = false)
                {
                    AnnualFlow.Add(saldo);

                    Account.Add(payment);

                    idade = forecastDate.Year - Birth.Year;
                    Idade.Add(idade);
                }

                if (forecastDate.Month == 12)
                {
                    idade = forecastDate.Year - Birth.Year;

                    if (saldo < benefit) benefit = saldo;

                    saldo -= benefit;
                    saldo *= (interest + 1);

                    payment += benefit;

                    AnnualFlow.Add(payment);
                    Account.Add(saldo);
                    Idade.Add(idade);

                    payment = 0;
                }
            }

            if (withdraw == true || saldo < benefit)
            {
                payment += saldo;
                saldo = 0;
            }

            AnnualFlow.Add(payment);
            Account.Add(saldo);
        }
    }
}
