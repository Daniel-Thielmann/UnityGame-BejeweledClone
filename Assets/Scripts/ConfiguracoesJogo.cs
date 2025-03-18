using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Classe estática contendo valores fixos do jogo.
/// </summary>
public static class ConfiguracoesJogo
{
    // Dimensões do tabuleiro
    public static readonly int NumeroLinhas = 12;
    public static readonly int NumeroColunas = 12;

    // Configurações gerais para animações
    public static readonly float TempoEsperaParaChecagem = 2f;
    public static readonly float TempoEntreFramesOpacidade = 0.05f;

    public static float DuracaoExplosao = 0.5f;
    public static float TempoMinimoAnimacaoMovimento = 0.3f;
    public static readonly float DuracaoAnimacao = 0.3f;


    // Regras gerais para combinação das peças
    public static readonly int MinimoPecasParaCombinar = 3;
    public static readonly int MinimoPecasParaBonus = 4;

    // Pontuações obtidas ao realizar combinações
    public static readonly int PontuacaoCombinar3 = 60;
    public static readonly int PontosExtrasPorCombos = 60;

    // Configurações do tabuleiro
    public static readonly int TotalLinhas = 12;
    public static readonly int TotalColunas = 12;

    // Pontuações específicas
    public static readonly int PontosMatchNormal = 60;
    public static readonly int PontosMatchExtra = 120;

    // Delays e durações para animações específicas
    public static readonly float DuracaoAnimacaoOpacidade = 0.2f;
    public static readonly float TempoAnimacaoTrocaPecas = 0.1f;

    public static readonly float TempoFrameAnimacaoOpacidade = 0.05f;

}
