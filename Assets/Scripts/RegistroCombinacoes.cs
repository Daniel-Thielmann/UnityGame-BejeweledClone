using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Mantém o registro das peças que formam combinações.
/// </summary>
public class RegistroCombinacoes
{
    private List<GameObject> pecasCombinadas;

    public RegistroCombinacoes()
    {
        pecasCombinadas = new List<GameObject>();
    }

    /// <summary>
    /// Retorna uma lista única de peças que formam combinação.
    /// </summary>
    public IEnumerable<GameObject> PecasCombinadasUnicas
    {
        get
        {
            HashSet<GameObject> unicas = new HashSet<GameObject>(pecasCombinadas);
            return unicas;
        }
    }

    /// <summary>
    /// Registra uma única peça na lista.
    /// </summary>
    public void RegistrarPeca(GameObject novaPeca)
    {
        if (pecasCombinadas.IndexOf(novaPeca) == -1)
            pecasCombinadas.Add(novaPeca);
    }

    /// <summary>
    /// Registra várias peças simultaneamente.
    /// </summary>
    public void RegistrarVariasPecas(IEnumerable<GameObject> pecas)
    {
        foreach (GameObject peca in pecas)
        {
            RegistrarPeca(peca);
        }
    }

    private void RegistrarVarias(IEnumerable<GameObject> itens)
    {
        foreach (var item in itens)
        {
            RegistrarPeca(item);
        }
    }

    public TipoBonusEspecial BonusGerado { get; set; } = TipoBonusEspecial.Nenhum;

    /// <summary>
    /// Retorna true se tiver algum bônus especial ativado.
    /// </summary>
    public bool TemBonusEspecial()
    {
        return BonusGerado != TipoBonusEspecial.Nenhum;
    }
}
