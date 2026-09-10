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
               $"^FO20,15^FB280,2,0,L,0^A0N,36,36^FD{produtoNome.ToUpper()}^FS\n" +
               $"^FO20,115^A0N,30,30^FDFab: {fabricacao:dd/MM/yyyy}^FS\n" +
               $"^FO20,155^A0N,30,30^FDVenc: {vencimento:dd/MM/yyyy}^FS\n" +
               $"^FO20,195^A0N,22,22^FDMet. Armazenagem: {metodoArmazenagem}^FS\n" +
               $"^FO20,225^A0N,22,22^FDResp: {funcionarioNome}^FS\n" +
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
               $"^FO20,15^FB280,2,0,L,0^A0N,36,36^FD{produtoNome.ToUpper()}^FS\n" +
               $"^FO20,115^A0N,40,40^FD{precoTexto}^FS\n" +
               $"^FO20,175^A0N,26,26^FDFab: {fabricacao:dd/MM/yyyy}^FS\n" +
               $"^FO20,205^A0N,26,26^FDVenc: {vencimento:dd/MM/yyyy}^FS\n" +
               $"^FO20,235^A0N,22,22^FDMet. Armazenagem: {metodoArmazenagem}^FS\n" +
               "^XZ\n";
    }
}
