using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Alpha.Entities
{
    internal class ActiveRetired : ILiability
    {
        public List<double?> Account { get; set; } = new List<double?>();
        public List<double?> AnnualFlow { get; set; } = new List<double?>();
        public List<int> Idade { get; set; } = new List<int>();

        public void Calculate(DateTime baseDate, double? contrib, double? balance, double? benefit, double interest, DateTime birth, bool withdraw, bool anual)
        {
            double? payment = 0.0;

            DateTime forecastDate = new DateTime(baseDate.Year, baseDate.Month, 1);
            DateTime endDate = forecastDate.AddYears(80);
            double? saldo = balance;

            int idade = 0;
            int tabua = 86;

            benefit = 0.0;
            int prazo = 195;
            if (contrib is null) contrib = 0.0;

            double? desoneracao = contrib * 1.0;

            double? contribParti = contrib;
            double? contribPatro = contrib;

            bool retired = false;

            while (saldo > benefit && idade < tabua && forecastDate < endDate)
            {
                forecastDate = forecastDate.AddMonths(1);

                if (retired == false)
                {
                    Math.Round((double)(saldo *= (interest + 1)), 2);
                    Math.Round((double)(saldo += contribParti + contribPatro), 2);

                    Math.Round((double)(payment += (contribParti + contribPatro - desoneracao) * -1), 2);
                }
                else
                {
                    benefit = Math.Round((double)saldo/prazo, 2);
                    prazo--;

                    Math.Round((double)(saldo -= benefit), 2);
                    if (forecastDate.Month != 12) saldo *= (interest + 1);

                    Math.Round((double)(payment += benefit), 2);
                }

                if (anual == false && forecastDate.Month != 12)
                {
                    idade = forecastDate.Year - birth.Year;

                    AnnualFlow.Add(payment);
                    Account.Add(saldo);
                    Idade.Add(idade);
                }
                if (forecastDate.Month == 12)
                {
                    if (retired == true)
                    {
                        benefit = Math.Round((double)(saldo / prazo), 2);
                        prazo--;

                        Math.Round((double)(saldo -= benefit), 2);
                        Math.Round((double)(saldo *= (interest + 1)), 2);

                        payment += benefit;
                    }

                    idade = forecastDate.Year - birth.Year;
                    if (idade >= 58) retired = true;

                    AnnualFlow.Add(payment);
                    Account.Add(saldo);
                    Idade.Add(idade);

                    payment = 0;
                }
            }

            if (withdraw == true && saldo > 1.0)
            {
                AnnualFlow.Add(payment);
                Account.Add(saldo);
            }
        }

    }
}
