using UnityEngine;
using System;

public static class FerramentasDebug
{
    /// <summary>
    /// Carrega uma configuração inicial do tabuleiro a partir de um arquivo de texto.
    /// </summary>
    public static string[,] CarregarMatrizInicial()
    {
        string[,] matrizFormas = new string[ConfiguracoesJogo.NumeroLinhas, ConfiguracoesJogo.NumeroColunas];

        TextAsset arquivoTexto = Resources.Load("level") as TextAsset;
        if (arquivoTexto == null)
        {
            Debug.LogError("Arquivo de nível não encontrado!");
            return matrizFormas;
        }

        string conteudoArquivo = arquivoTexto.text;
        string[] linhas = conteudoArquivo.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        int quantidadeLinhas = Mathf.Min(linhas.Length, ConfiguracoesJogo.NumeroLinhas);

        for (int linha = quantidadeLinhas - 1; linha >= 0; linha--)
        {
            string[] celulas = linhas[linha].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int quantidadeColunas = Mathf.Min(celulas.Length, ConfiguracoesJogo.NumeroColunas);

            for (int coluna = 0; coluna < quantidadeColunas; coluna++)
            {
                matrizFormas[linha, coluna] = celulas[coluna];
            }
        }

        return matrizFormas;
    }

    /// <summary>
    /// Rotaciona objeto para debug visual.
    /// </summary>
    public static void RotacionarParaDebug(GameObject objeto)
    {
        if (objeto != null)
            objeto.transform.Rotate(Vector3.forward * 80f);
    }

    /// <summary>
    /// Altera a transparência do objeto para debug visual.
    /// </summary>
    public static void AjustarTransparencia(GameObject objeto)
    {
        if (objeto != null)
        {
            SpriteRenderer sprite = objeto.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                Color novaCor = sprite.color;
                novaCor.a = 0.6f;
                sprite.color = novaCor;
            }
        }
    }

    /// <summary>
    /// Exibe posições das peças trocadas.
    /// </summary>
    public static void MostrarTrocaPecas(GameObject objetoA, GameObject objetoB)
    {
        if (objetoA == null || objetoB == null)
        {
            Debug.LogError("Um dos objetos passados para MostrarTrocaPecas é nulo.");
            return;
        }

        var pecaA = objetoA.GetComponent<ElementoTabuleiro>();
        var pecaB = objetoB.GetComponent<ElementoTabuleiro>();

        if (pecaA == null || pecaB == null)
        {
            Debug.LogError("Os objetos passados não possuem o componente ElementoTabuleiro.");
            return;
        }

        string mensagemDebug = $"Peça A: ({pecaA.Linha}, {pecaA.Coluna}) <-> Peça B: ({pecaB.Linha}, {pecaB.Coluna})";
        Debug.Log(mensagemDebug);
    }

    /// <summary>
    /// Mostra os conteúdos do tabuleiro atual no console.
    /// </summary>
    public static void ExibirEstadoTabuleiro(GerenciadorTabuleiro tabuleiro)
    {
        if (tabuleiro == null)
        {
            Debug.LogError("Tabuleiro não inicializado.");
            return;
        }

        string conteudoTabuleiro = ConstruirStringTabuleiro(tabuleiro);
        Debug.Log(conteudoTabuleiro);
    }

    /// <summary>
    /// Cria uma string com o conteúdo atual do tabuleiro.
    /// </summary>
    private static string ConstruirStringTabuleiro(GerenciadorTabuleiro tabuleiro)
    {
        string resultado = "";

        for (int linha = ConfiguracoesJogo.NumeroLinhas - 1; linha >= 0; linha--)
        {
            for (int coluna = 0; coluna < ConfiguracoesJogo.NumeroColunas; coluna++)
            {
                if (tabuleiro[linha, coluna] == null)
                {
                    resultado += "[VAZIO] | ";
                }
                else
                {
                    var peca = tabuleiro[linha, coluna].GetComponent<ElementoTabuleiro>();
                    resultado += $"Tipo:{peca.TipoPeca} ({linha},{coluna}) | ";
                }
            }
            resultado += Environment.NewLine;
        }

        return resultado;
    }
}
