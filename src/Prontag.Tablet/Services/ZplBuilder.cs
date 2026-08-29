using System.Globalization;

namespace Prontag.Tablet.Services;

public static class ZplBuilder
{
    public static string EtiquetaProduto(string produtoNome, DateOnly fabricacao, DateOnly vencimento, string funcionarioNome)
    {
        return "^XA\n" +
               "^CI28\n" +
               "^MD14\n" +
               "^PW824\n" +
               "^LL240\n" +
               $"^FO20,35^A0N,32,32^FD{produtoNome.ToUpper()}^FS\n" +
               $"^FO20,90^A0N,24,24^FDFab: {fabricacao:dd/MM/yyyy}^FS\n" +
               $"^FO20,130^A0N,24,24^FDVenc: {vencimento:dd/MM/yyyy}^FS\n" +
               $"^FO20,175^A0N,24,24^FDResp: {funcionarioNome}^FS\n" +
               "^XZ\n";
    }

    public static string EtiquetaExpositor(string produtoNome, decimal? preco, DateOnly fabricacao, DateOnly vencimento)
    {
        var precoTexto = preco.HasValue
            ? preco.Value.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))
            : "Preço não informado";

        return "^XA\n" +
               "^CI28\n" +
               "^MD14\n" +
               "^PW824\n" +
               "^LL240\n" +
               $"^FO20,35^A0N,32,32^FD{produtoNome.ToUpper()}^FS\n" +
               $"^FO20,90^A0N,28,28^FD{precoTexto}^FS\n" +
               $"^FO20,145^A0N,20,20^FDFab: {fabricacao:dd/MM/yyyy}^FS\n" +
               $"^FO20,175^A0N,20,20^FDVenc: {vencimento:dd/MM/yyyy}^FS\n" +
               "^XZ\n";
    }
}
