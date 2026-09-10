using System.Globalization;

namespace Prontag.Tablet.Services;

public static class ZplBuilder
{
    public static string EtiquetaProduto(string produtoNome, DateOnly fabricacao, DateOnly vencimento, string funcionarioNome)
    {
        return "^XA\n" +
               "^CI28\n" +
               "^MD14\n" +
               "^PW320\n" +
               "^LL320\n" +
               $"^FO20,20^A0N,40,40^FD{produtoNome.ToUpper()}^FS\n" +
               $"^FO20,75^A0N,28,28^FDFab: {fabricacao:dd/MM/yyyy}^FS\n" +
               $"^FO20,115^A0N,28,28^FDVenc: {vencimento:dd/MM/yyyy}^FS\n" +
               $"^FO20,155^A0N,22,22^FDResp: {funcionarioNome}^FS\n" +
               "^XZ\n";
    }

    public static string EtiquetaExpositor(string produtoNome, decimal? preco, string unidade, DateOnly fabricacao, DateOnly vencimento)
    {
        var precoTexto = preco.HasValue
            ? $"{preco.Value.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}/{unidade}"
            : "Preço não informado";

        return "^XA\n" +
               "^CI28\n" +
               "^MD14\n" +
               "^PW320\n" +
               "^LL320\n" +
               $"^FO20,20^A0N,40,40^FD{produtoNome.ToUpper()}^FS\n" +
               $"^FO20,85^A0N,44,44^FD{precoTexto}^FS\n" +
               $"^FO20,160^A0N,20,20^FDFab: {fabricacao:dd/MM/yyyy}^FS\n" +
               $"^FO20,190^A0N,20,20^FDVenc: {vencimento:dd/MM/yyyy}^FS\n" +
               "^XZ\n";
    }
}
