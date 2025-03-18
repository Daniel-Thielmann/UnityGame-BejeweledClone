using UnityEngine;

/// <summary>
/// Guarda informações sobre a posição atual de um elemento no tabuleiro.
/// </summary>
public class DadosElemento
{
    public int PosicaoColuna { get; set; }
    public int PosicaoLinha { get; set; }

    public DadosElemento()
    {
        PosicaoLinha = 0;
        PosicaoColuna = 0;
    }

    public DadosElemento(int linha, int coluna)
    {
        PosicaoLinha = linha;
        PosicaoColuna = coluna;
    }
}
