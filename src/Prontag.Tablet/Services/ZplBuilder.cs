using System.Globalization;

namespace Prontag.Tablet.Services;

public static class ZplBuilder
{
    public static string EtiquetaProduto(string produtoNome, DateOnly fabricacao, DateOnly vencimento, string funcionarioNome, string metodoArmazenagem)
    {
        return "^XA\n" +
               "^CI28\n" +
               "^MD14\n" +
               "^PW320\n" +
               "^LL320\n" +
               $"^FO10,35^FB300,2,0,C,0^A0N,28,28^FD{produtoNome.ToUpper()}^FS\n" +
               $"^FO20,120^A0N,30,30^FDFab: {fabricacao:dd/MM/yyyy}^FS\n" +
               $"^FO20,160^A0N,30,30^FDVenc: {vencimento:dd/MM/yyyy}^FS\n" +
               $"^FO20,200^A0N,22,22^FDArmaz: {metodoArmazenagem}^FS\n" +
               $"^FO20,230^A0N,22,22^FDResp: {funcionarioNome}^FS\n" +
               "^XZ\n";
    }

    public static string EtiquetaExpositor(string produtoNome, decimal? preco, string unidade, DateOnly fabricacao, DateOnly vencimento, string metodoArmazenagem)
    {
        var precoTexto = preco.HasValue
            ? $"{preco.Value.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}/{unidade}"
            : "Preço não informado";

        return "^XA\n" +
               "^CI28\n" +
               "^MD14\n" +
               "^PW320\n" +
               "^LL320\n" +
               $"^FO10,35^FB300,2,0,C,0^A0N,28,28^FD{produtoNome.ToUpper()}^FS\n" +
               $"^FO10,120^FB300,1,0,C,0^A0N,38,38^FD{precoTexto}^FS\n" +
               $"^FO20,180^A0N,26,26^FDFab: {fabricacao:dd/MM/yyyy}^FS\n" +
               $"^FO20,210^A0N,26,26^FDVenc: {vencimento:dd/MM/yyyy}^FS\n" +
               $"^FO20,240^A0N,22,22^FDArmaz: {metodoArmazenagem}^FS\n" +
               "^XZ\n";
    }
}
