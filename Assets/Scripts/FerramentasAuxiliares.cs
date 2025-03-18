using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Classe com métodos auxiliares para o funcionamento do tabuleiro.
/// </summary>
public static class FerramentasAuxiliares
{
    /// <summary>
    /// Anima potenciais combinações com efeito de transparência.
    /// </summary>
    public static IEnumerator AnimarPotenciais(IEnumerable<GameObject> potenciaisCombinacoes)
    {
        float opacidadeAtual = 1f;
        while (opacidadeAtual > 0.3f)
        {
            AlterarOpacidade(potenciaisCombinacoes, opacidadeAtual);
            opacidadeAtual -= 0.1f;
            yield return new WaitForSeconds(ConfiguracoesJogo.TempoFrameAnimacaoOpacidade);
        }

        while (opacidadeAtual < 1f)
        {
            AlterarOpacidade(potenciaisCombinacoes, opacidadeAtual);
            opacidadeAtual += 0.1f;
            yield return new WaitForSeconds(ConfiguracoesJogo.TempoFrameAnimacaoOpacidade);
        }
    }

    private static void AlterarOpacidade(IEnumerable<GameObject> objetos, float valorOpacidade)
    {
        foreach (var objeto in objetos)
        {
            SpriteRenderer sprite = objeto.GetComponent<SpriteRenderer>();
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, valorOpacidade);
        }
    }

    /// <summary>
    /// Verifica se dois elementos são vizinhos diretos.
    /// </summary>
    public static bool SaoVizinhos(ElementoTabuleiro elem1, ElementoTabuleiro elem2)
    {
        bool alinhadoColuna = elem1.Coluna == elem2.Coluna && Mathf.Abs(elem1.Linha - elem2.Linha) == 1;
        bool alinhadoLinha = elem1.Linha == elem2.Linha && Mathf.Abs(elem1.Coluna - elem2.Coluna) == 1;

        return alinhadoLinha || alinhadoColuna;
    }

    /// <summary>
    /// Busca por combinações potenciais no tabuleiro.
    /// </summary>
    public static IEnumerable<GameObject> BuscarCombinacoesPotenciais(GerenciadorTabuleiro tabuleiro)
    {
        List<List<GameObject>> combinacoes = new List<List<GameObject>>();

        for (int linha = 0; linha < ConfiguracoesJogo.NumeroLinhas; linha++)
        {
            for (int coluna = 0; coluna < ConfiguracoesJogo.NumeroColunas; coluna++)
            {
                var combinacao = ChecarCombinacoesPossiveis(linha, coluna, tabuleiro);
                if (combinacao != null)
                    combinacoes.Add(combinacao);

                if (combinacoes.Count >= 3 || (linha > ConfiguracoesJogo.NumeroLinhas / 2 && combinacoes.Any()))
                    return combinacoes[UnityEngine.Random.Range(0, combinacoes.Count)];
            }
        }

        return null;
    }

    private static List<GameObject> ChecarCombinacoesPossiveis(int linha, int coluna, GerenciadorTabuleiro tabuleiro)
    {
        return ChecarHorizontalmente(linha, coluna, tabuleiro) ??
               ChecarVerticalmente(linha, coluna, tabuleiro);
    }

    private static List<GameObject> ChecarHorizontalmente(int linha, int coluna, GerenciadorTabuleiro tabuleiro)
    {
        if (coluna <= ConfiguracoesJogo.NumeroColunas - 2)
        {
            var elementoAtual = tabuleiro[linha, coluna].GetComponent<ElementoTabuleiro>();
            var proximoElemento = tabuleiro[linha, coluna + 1].GetComponent<ElementoTabuleiro>();

            if (elementoAtual.EhMesmoTipo(proximoElemento))
            {
                if (linha >= 1 && coluna >= 1)
                {
                    var diagonal = tabuleiro[linha - 1, coluna - 1].GetComponent<ElementoTabuleiro>();
                    if (elementoAtual.EhMesmoTipo(diagonal))
                        return new List<GameObject> { tabuleiro[linha, coluna], tabuleiro[linha, coluna + 1], tabuleiro[linha - 1, coluna - 1] };
                }

                if (linha < ConfiguracoesJogo.NumeroLinhas - 1 && coluna >= 1)
                {
                    var diagonal = tabuleiro[linha + 1, coluna - 1].GetComponent<ElementoTabuleiro>();
                    if (elementoAtual.EhMesmoTipo(diagonal))
                        return new List<GameObject> { tabuleiro[linha, coluna], tabuleiro[linha, coluna + 1], tabuleiro[linha + 1, coluna - 1] };
                }
            }
        }
        return null;
    }

    private static List<GameObject> ChecarVerticalmente(int linha, int coluna, GerenciadorTabuleiro tabuleiro)
    {
        if (linha <= ConfiguracoesJogo.NumeroLinhas - 2)
        {
            var elementoAtual = tabuleiro[linha, coluna].GetComponent<ElementoTabuleiro>();
            var proximoElemento = tabuleiro[linha + 1, coluna].GetComponent<ElementoTabuleiro>();

            if (elementoAtual.EhMesmoTipo(proximoElemento))
            {
                if (coluna >= 1 && linha >= 1)
                {
                    var diagonal = tabuleiro[linha - 1, coluna - 1].GetComponent<ElementoTabuleiro>();
                    if (elementoAtual.EhMesmoTipo(diagonal))
                        return new List<GameObject> { tabuleiro[linha, coluna], tabuleiro[linha + 1, coluna], tabuleiro[linha - 1, coluna - 1] };
                }

                if (coluna < ConfiguracoesJogo.NumeroColunas - 1 && linha >= 1)
                {
                    var diagonal = tabuleiro[linha - 1, coluna + 1].GetComponent<ElementoTabuleiro>();
                    if (elementoAtual.EhMesmoTipo(diagonal))
                        return new List<GameObject> { tabuleiro[linha, coluna], tabuleiro[linha + 1, coluna], tabuleiro[linha - 1, coluna + 1] };
                }
            }
        }
        return null;
    }
}
