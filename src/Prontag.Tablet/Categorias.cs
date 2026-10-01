namespace Prontag.Tablet;

public static class Categorias
{
    public static readonly string[] Expositor = new[]
    {
        "Salgados",
        "Quitandas de queijo",
        "Quitandas doces",
        "Confeitaria",
        "Sequilhos",
        "Bolos",
    }
    .OrderBy(x => x)
    .ToArray();

    public static readonly string[] Produtos = new[]
    {
        "Farinhas, Açúcar e Grãos",
        "Fermentos e Químicos",
        "Recheios, Bolos e Coberturas",
        "Laticínios",
        "Legumes, Frutas e Verduras",
        "Carnes e embutidos",
        "Bebidas",
        "Doces e geleias",
        "Materiais e Insumos de Produção",
    }
    .OrderBy(x => x)
    .ToArray();
}
}
