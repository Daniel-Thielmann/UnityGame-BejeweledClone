using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Classe responsável pela gestão dos elementos no tabuleiro.
/// </summary>
public class GerenciadorTabuleiro
{
    private GameObject[,] tabuleiro;

    private GameObject[,] elementos = new GameObject[ConfiguracoesJogo.NumeroLinhas, ConfiguracoesJogo.NumeroColunas];
    private GameObject elementoBackupA;
    private GameObject elementoBackupB;

    public GameObject this[int linha, int coluna]
    {
        get => elementos[linha, coluna];
        set => elementos[linha, coluna] = value;
    }



    public void TrocarElementos(GameObject elementoA, GameObject elementoB)
    {
        elementoBackupA = elementoA;
        elementoBackupB = elementoB;

        var infoA = elementoA.GetComponent<ElementoTabuleiro>();
        var infoB = elementoB.GetComponent<ElementoTabuleiro>();

        int tempLinha = infoA.Linha;
        int tempColuna = infoA.Coluna;

        infoA.Linha = infoB.Linha;
        infoA.Coluna = infoB.Coluna;

        infoB.Linha = tempLinha;
        infoB.Coluna = tempColuna;
    }

    public void DesfazerTroca()
    {
        if (elementoBackupA == null || elementoBackupB == null)
            throw new Exception("Backup inválido!");

        TrocarElementos(elementoBackupA, elementoBackupB);
    }

    public RegistroCombinacoes ObterCombinacoes(GameObject elemento)
    {
        RegistroCombinacoes combinacoes = new RegistroCombinacoes();

        var combinacoesHorizontais = BuscarCombinacoesHorizontais(elemento);
        if (TemBonusLinhaColuna(combinacoesHorizontais))
        {
            combinacoesHorizontais = ObterLinhaCompleta(elemento);
            combinacoes.BonusGerado |= TipoBonusEspecial.LimparLinhaEColuna;
        }

        combinacoes.RegistrarVariasPecas(combinacoesHorizontais);

        var combinacoesVerticais = BuscarCombinacoesVerticais(elemento);
        if (TemBonusLinhaColuna(combinacoesVerticais))
        {
            combinacoesVerticais = ObterColunaCompleta(elemento);
            combinacoes.BonusGerado |= TipoBonusEspecial.LimparLinhaEColuna;
        }

        combinacoes.RegistrarVariasPecas(combinacoesVerticais);

        return combinacoes;
    }

    private bool TemBonusLinhaColuna(IEnumerable<GameObject> pecas)
    {
        foreach (var peca in pecas)
        {
            if (FerramentasBonus.TemBonusLimpezaCompleta(peca.GetComponent<ElementoTabuleiro>().TipoBonus))
                return true;
        }
        return false;
    }

    private IEnumerable<GameObject> ObterLinhaCompleta(GameObject peca)
    {
        int linha = peca.GetComponent<ElementoTabuleiro>().Linha;
        List<GameObject> resultado = new List<GameObject>();

        for (int coluna = 0; coluna < ConfiguracoesJogo.NumeroColunas; coluna++)
            resultado.Add(elementos[linha, coluna]);

        return resultado;
    }

    private IEnumerable<GameObject> ObterColunaCompleta(GameObject peca)
    {
        int coluna = peca.GetComponent<ElementoTabuleiro>().Coluna;
        List<GameObject> resultado = new List<GameObject>();

        for (int linha = 0; linha < ConfiguracoesJogo.NumeroLinhas; linha++)
            resultado.Add(elementos[linha, coluna]);

        return resultado;
    }

    public InformacoesPecasModificadas ColapsarColunas(IEnumerable<int> colunas)
    {
        InformacoesPecasModificadas infoPecas = new InformacoesPecasModificadas();

        foreach (int coluna in colunas)
        {
            for (int linha = 0; linha < ConfiguracoesJogo.NumeroLinhas - 1; linha++)
            {
                if (elementos[linha, coluna] == null)
                {
                    for (int superior = linha + 1; superior < ConfiguracoesJogo.NumeroLinhas; superior++)
                    {
                        if (elementos[superior, coluna] != null)
                        {
                            elementos[linha, coluna] = elementos[superior, coluna];
                            elementos[superior, coluna] = null;

                            elementos[linha, coluna].GetComponent<ElementoTabuleiro>().Linha = linha;
                            elementos[linha, coluna].GetComponent<ElementoTabuleiro>().Coluna = coluna;

                            int distancia = superior - linha;
                            if (distancia > infoPecas.MaiorDistancia)
                                infoPecas.MaiorDistancia = distancia;

                            infoPecas.RegistrarPeca(elementos[linha, coluna]);
                            break;
                        }
                    }
                }
            }
        }

        return infoPecas;
    }

    public IEnumerable<DadosElemento> ObterEspacosVaziosNaColuna(int coluna)
    {
        List<DadosElemento> espacosVazios = new List<DadosElemento>();
        for (int linha = 0; linha < ConfiguracoesJogo.NumeroLinhas; linha++)
            if (elementos[linha, coluna] == null)
                espacosVazios.Add(new DadosElemento(linha, coluna));

        return espacosVazios;
    }

    private IEnumerable<GameObject> BuscarCombinacoesHorizontais(GameObject peca)
    {
        List<GameObject> combinacao = new List<GameObject> { peca };
        var elementoBase = peca.GetComponent<ElementoTabuleiro>();

        // Checa para a esquerda
        for (int c = elementoBase.Coluna - 1; c >= 0; c--)
        {
            var atual = elementos[elementoBase.Linha, c];
            if (atual.GetComponent<ElementoTabuleiro>().EhMesmoTipo(elementoBase))
                combinacao.Add(atual);
            else break;
        }

        // Checa para a direita
        for (int c = elementoBase.Coluna + 1; c < ConfiguracoesJogo.NumeroColunas; c++)
        {
            var atual = elementos[elementoBase.Linha, c];
            if (atual.GetComponent<ElementoTabuleiro>().EhMesmoTipo(elementoBase))
                combinacao.Add(atual);
            else break;
        }

        return combinacao.Count >= 3 ? combinacao : new List<GameObject>();
    }

    private IEnumerable<GameObject> BuscarCombinacoesVerticais(GameObject peca)
    {
        List<GameObject> combinacao = new List<GameObject> { peca };
        var elementoBase = peca.GetComponent<ElementoTabuleiro>();

        // Checa para cima
        for (int l = elementoBase.Linha + 1; l < ConfiguracoesJogo.NumeroLinhas; l++)
        {
            var atual = elementos[l, elementoBase.Coluna];
            if (atual.GetComponent<ElementoTabuleiro>().EhMesmoTipo(elementoBase))
                combinacao.Add(atual);
            else break;
        }

        // Checa para baixo
        for (int l = elementoBase.Linha - 1; l >= 0; l--)
        {
            var atual = elementos[l, elementoBase.Coluna];
            if (atual.GetComponent<ElementoTabuleiro>().EhMesmoTipo(elementoBase))
                combinacao.Add(atual);
            else break;
        }

        return combinacao.Count >= 3 ? combinacao : new List<GameObject>();
    }

    public void RemoverElemento(GameObject elemento)
    {
        if (elemento == null) return;

        ElementoTabuleiro dados = elemento.GetComponent<ElementoTabuleiro>();
        if (dados != null)
        {
            elementos[dados.Linha, dados.Coluna] = null;
        }

        GameObject.Destroy(elemento); // Corrigindo a chamada do Destroy
    }


}
