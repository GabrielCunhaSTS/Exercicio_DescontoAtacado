using System.Globalization;

CultureInfo real = new CultureInfo("pt-BR");

string[,] catalogo = new string[,]{
    {"7891024110348", "2,88", "2,51", "12"},
    {"7891048038017", "4,40", "4,37", "3"},
    {"7896066334509", "5,19", "---", "---"},
    {"7891700203142", "2,39", "2,38", "6"},
    {"7894321711263", "9,79", "---", "---"},
    {"7896001250611", "9,89", "9,10", "10"},
    {"7793306013029", "12,79", "12,35", "3"},
    {"7896004400914", "4,20", "4,05", "6"},
    {"7898080640017", "6,99", "6,89", "12"},
    {"7891025301516", "12,99", "---", "---"},
    {"7891030003115", "3,12", "3,09", "4"},
};

string[,] compras = new string[,]{
    {"7891048038017", "1", "4,40"},
    {"7896004400914", "4", "16,80"},
    {"7891030003115", "1", "3,12"},
    {"7891024110348", "6", "17,28"},
    {"7898080640017", "24", "167,76"},
    {"7896004400914", "8", "33,60"},
    {"7891700203142", "8", "19,12"},
    {"7891048038017", "1", "4,40"},
    {"7793306013029", "3", "38,37"},
    {"7896066334509", "2", "10,38"},
};


int RegistrosCatalogo = catalogo.GetLength(0);
int RegistroCompras = compras.GetLength(0);
int qtdProdutos = 0;

string[] gtins = new string[RegistroCompras];
int[] quantidades = new int[RegistroCompras];

for (int i = 0; i < RegistroCompras; i++){
    string gtin = compras[i, 0];
    int qtd = int.Parse(compras[i, 1]);

    int posicaoProduto = -1;
    for (int j = 0; j < qtdProdutos; j++){
        if (gtins[j] == gtin){
            posicaoProduto = j;
            break;
        }
    }

    if (posicaoProduto == -1){
        gtins[qtdProdutos] = gtin;
        quantidades[qtdProdutos] = qtd;
        qtdProdutos++;
    }
    else{
        quantidades[posicaoProduto] += qtd;
    }
}

double subtotal = 0;
double totalDescontos = 0;
double[] descontos = new double[qtdProdutos];

for (int i = 0; i < qtdProdutos; i++){
    string gtin = gtins[i];
    int qtd = quantidades[i];

    double precoVarejo = 0;
    double precoAtacado = 0;
    int unidadesAtacado = 0;
    bool temAtacado = false;

    for (int j = 0; j < RegistrosCatalogo; j++){
        if (catalogo[j, 0] == gtin){
            precoVarejo = double.Parse(catalogo[j, 1], real);
            if (catalogo[j, 2] != "---" && catalogo[j, 3] != "---"){
                precoAtacado = double.Parse(catalogo[j, 2], real);
                unidadesAtacado = int.Parse(catalogo[j, 3]);
                temAtacado = true;
            }
            break;
        }
    }

    subtotal += precoVarejo * qtd;

    if (temAtacado && qtd >= unidadesAtacado){
        double valorSemDesconto = precoVarejo * qtd;
        double valorComDesconto = precoAtacado * qtd;
        double desconto = valorSemDesconto - valorComDesconto;

        descontos[i] = desconto;
        totalDescontos += desconto;
    }
    else{
        descontos[i] = 0;
    }
}

Console.Clear();
Console.WriteLine("--- Bem vindo(a) ao Antares atacados & varejos ---");
Console.WriteLine("--------------- Desconto no Atacado --------------\n");
Console.WriteLine("+-------- DESCONTOS --------+");
Console.WriteLine("| Cód Produto      Desconto |");
Console.WriteLine("+---------------------------+");
for (int i = 0; i < qtdProdutos; i++){
    if (descontos[i] > 0)
        Console.WriteLine($"| {gtins[i],-15}  R$ {descontos[i]:F2}  |");
}
Console.WriteLine("+---------------------------+\n");

Console.WriteLine("+---------- SUBTOTAIS ---------+");
Console.WriteLine($"| (+) Subtotal  =    R$ {subtotal:F2} |");
Console.WriteLine($"| (-) Descontos =      R$ {totalDescontos:F2} |");
Console.WriteLine($"| (=) Total     =    R$ {(subtotal - totalDescontos):F2} |");
Console.WriteLine("+------------------------------+");

Console.Write("\nDigite qualquer tecla para finalizar o programa!");
Console.ReadKey();
Console.Clear();
