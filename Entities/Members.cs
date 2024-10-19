using Alpha.Models;
using ClosedXML.Excel;

namespace Alpha.Entities
{
    internal class Members
    {
        public int Matricula { get; set; }
        public double? Contrib { get; set; }
        public double? Balance { get; set; }
        public double? Benefit { get; set; }
        public double Interest { get; set; }
        public DateTime Data { get; set; }
        public DateTime Birth { get; set; }
        public double? Payment { get; set; } = 0.0;
        public bool Withdraw { get; set; }
        public bool Anual { get; set; }

        public ILiability Liability;

        public Members(int matricula, double? contrib, double? balance, double? benefit, double interest, DateTime data, DateTime birth, bool withdraw, bool anual, ILiability liability)

        {
            Matricula = matricula;
            Contrib = contrib;
            Balance = balance;
            Benefit = benefit;
            Interest = interest;
            Data = data;
            Birth = birth;
            Withdraw = withdraw;
            Anual = anual;
            Liability = liability;
        }

        public void Projetar()
        {
            Liability.Calculate(Data, Contrib, Balance, Benefit, Interest, Birth, Withdraw, Anual);
        }
    }
}
