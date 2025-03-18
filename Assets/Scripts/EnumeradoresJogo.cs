using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Tipos disponíveis de bônus especiais no jogo
/// </summary>
[Flags]
public enum TipoBonusEspecial
{
    Nenhum = 0,
    LimparLinhaEColuna = 1,
    LimparLinhaEColunaInteira = 2
}

/// <summary>
/// Classe utilitária para operações relacionadas a bônus especiais.
/// </summary>
public static class FerramentasBonus
{
    /// <summary>
    /// Verifica se o bônus especial inclui limpeza completa de linha e coluna
    /// </summary>
    /// <param name="bonus"></param>
    /// <returns></returns>
    public static bool TemBonusLimpezaCompleta(TipoBonusEspecial bonus)
    {
        return (bonus & TipoBonusEspecial.LimparLinhaEColunaInteira) != 0;
    }
}

/// <summary>
/// Estado atual do jogo
/// </summary>
public enum EstadoAtualJogo
{
    Indefinido,
    IniciouSelecao,
    EmAnimacao
}
