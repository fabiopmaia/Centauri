using Alpha.Data;
using Alpha.Entities;
using Alpha.Models;
using Alpha.Services;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;

using (var banco = new DataContext())
{
    DateTime data = new DateTime(2024, 06, 30);

    var dados = banco.Reservas.Join
        (
            banco.Participantes,
            res => res.Id_Matricula,
            par => par.Matricula,
            (res, par) => new { res, par }
        )
        .Where(p => p.res.DataRef == data &&
                    //p.res.Id_Vinculo > 0 &&
                    //p.res.Forma == 2 &&
                    p.res.Principal != null)
        .OrderBy(p => p.res.Id_Matricula)
        .ToList();

    List<Members> members = new List<Members>();

    foreach (var participante in dados)
    {
        byte link = participante.res.Id_Vinculo;
        double? paidMain = participante.res.ContribMain;
        double? paidAdd = participante.res.ContribAdd;
        byte? tipo = participante.res.Forma;
        short id = participante.res.Id_Matricula;
        double? main = participante.res.Principal;
        double? renda1 = participante.res.Renda;
        double? add = participante.res.Adicional;
        double? renda2 = participante.res.Renda1;
        DateTime nasc = participante.par.Nascimento;

        if (link == 1)
        {
            members.Add(new Members(id, paidMain, main, renda1, 0.0054, data, nasc, false, true, new ActiveRetired()));
        }

        if (link == 1 && add > 0.0 && add > 1.0)
        {
            members.Add(new Members(id, paidAdd, add, renda2, 0.0054, data, nasc, false, true, new ActiveRetired()));
        }

        if (tipo > 1 && tipo < 7 && main > 1.0)
        {
            members.Add(new Members(id, paidMain, main, renda1, 0.0054, data, nasc, false, true, new FixedValue()));
        }
        
        if (tipo > 1 && tipo < 7 && add > 1.0)
        {
            members.Add(new Members(id, paidAdd, add, renda2, 0.0054, data, nasc, false, true, new FixedValue()));
        }
    }

    foreach (var participante in members)
    {
        participante.Projetar();
    }

    var projetados = members
        .Where(p => p.Liability.Account.Count > 0)
        .OrderBy(p => p.Matricula)
        .ToList();

    CreateFile projecao = new CreateFile(data, projetados);
    projecao.GerarPlanilha();
}

