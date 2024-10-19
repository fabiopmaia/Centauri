using Alpha.Entities;
using Alpha.Models;
using ClosedXML;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.CustomProperties;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace Alpha.Services
{
    internal class CreateFile
    {
        public DateTime BaseDate { get; private set; }
        public List<Members> Participantes { get; private set; }

        public CreateFile(DateTime baseDate, List<Members> participantes)
        {
            BaseDate = baseDate;
            Participantes = participantes;
        }

        public void GerarPlanilha()
        {
            using (var planilha = new XLWorkbook())
            {
                var guia = planilha.AddWorksheet("Planilha1");

                string path = @"D:\Dev\aceprev.png";
                //guia.AddPicture(path).MoveTo(guia.Cell(1, 1)).Scale(0.5);

                guia.Cell(1, 2).Value = Participantes.Count;
                guia.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                guia.Cell(1, 3).Value = "< Participantes projetados.";

                DateTime forecastDate = new DateTime(BaseDate.Year, 12, 1);

                guia.Cell(3, 2).Value = "Matrícula";
                guia.Cell(3, 3).Value = "Valor";
                guia.Cell(3, 4).Value = forecastDate;

                for (int t = 5; t < 64; t++)
                {
                    forecastDate = forecastDate.AddYears(1);
                    guia.Cell(3, t).Value = forecastDate;
                }

                int i = 4, k = 4;
                foreach (var participante in Participantes)
                {
                    guia.Cell(i, 1).Value = "Reserva";
                    guia.Cell(i, 2).Value = participante.Matricula;
                    guia.Cell(i, 3).Value = participante.Balance;
                    
                    foreach (double account in participante.Liability.Account)
                    {
                        guia.Cell(i, k).Value = account;
                        k++;
                    }

                    guia.Range(i, 3, i, 63).Style.NumberFormat.Format = "#,##0.00";
                    guia.Range(i, 3, i, 63).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    i++;
                    k = 4;

                    guia.Cell(i, 1).Value = "Fluxo";
                    guia.Cell(i, 2).Value = participante.Matricula;
                    guia.Cell(i, 3).Value = participante.Benefit;

                    foreach (double? annualflow in participante.Liability.AnnualFlow)
                    {
                        guia.Cell(i, k).Value = annualflow;
                        k++;
                    }

                    guia.Range(i, 3, i, 63).Style.NumberFormat.Format = "#,##0.00";
                    guia.Range(i, 3, i, 63).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    i++;
                    k = 4;

                    guia.Cell(i, 1).Value = "Idade";
                    guia.Cell(i, 2).Value = participante.Matricula;

                    foreach (double age in participante.Liability.Idade)
                    {
                        guia.Cell(i, k).Value = age;
                        k++;
                    }

                    guia.Range(i, 3, i, 63).Style.NumberFormat.Format = "#,##0";
                    guia.Range(i, 3, i, 63).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    guia.Range(i - 2, 2, i, 2).Column(1).Merge();
                    guia.Range(i - 2, 2, i, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    guia.Range(i - 2, 2, i, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    i += 2;
                    k = 4;
                }

                //-----Formatting the worksheet --------------------------|
                guia.SetShowGridLines(false);

                guia.Columns(1, 63).Width = 10;

                guia.Range(3, 1, 3, 63).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                guia.Range(3, 1, 3, 63).Style.Font.Bold = true;
                guia.Range(3, 4, 3, 63).Style.NumberFormat.Format = "MM/yyyy";

                int lr = guia.LastRowUsed().RowNumber();

                guia.Range(1, 1, lr, 63).Style
                .Font.SetFontName("Calibri")
                .Font.SetFontSize(10);

                //-----Constructs the formula of sumif-------------------| 
                for (int t = 4; t < 64; t++)
                {
                    guia.Cell(2, t).FormulaR1C1 = "=SUMIF(R4C1:R" + lr + "C1,\"Fluxo\",R4C" + t + ":R" + lr + "C" + t + ")";
                }

                guia.Range(2, 4, 2, 64).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                guia.Range(2, 4, 2, 64).Style.Font.Bold = true;
                guia.Range(2, 4, 2, 64).Style.Font.FontSize = 9;
                guia.Range(2, 4, 2, 64).Style.NumberFormat.Format = "#,##0.00";
                //-------------------------------------------------------|

                guia.Range(4, 1, lr, 01).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                guia.Range(4, 2, lr, 02).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                guia.Range(4, 2, 4, 63).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                guia.Range(4, 2, 4, 63).Style.Border.TopBorderColor = XLColor.DarkBlue;
                guia.Range(4, 2, lr, 2).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                guia.Range(4, 2, lr, 2).Style.Border.LeftBorderColor = XLColor.DarkBlue;
                guia.Range(4, 63, lr, 63).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                guia.Range(4, 63, lr, 63).Style.Border.RightBorderColor = XLColor.DarkBlue;
                guia.Range(lr, 2, lr, 63).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                guia.Range(lr, 2, lr, 63).Style.Border.BottomBorderColor = XLColor.DarkBlue;
                guia.Range(4, 2, lr, 63).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                guia.Range(4, 2, lr, 63).Style.Border.InsideBorderColor = XLColor.LightGray;
                //---------------------------------------------------|


                planilha.SaveAs(@"C:\Users\fabio\Downloads\Projeção.xlsx");

                Process.Start(new ProcessStartInfo(@"C:\Users\fabio\Downloads\Projeção.xlsx") { UseShellExecute = true });
            }
        }
    }
}
