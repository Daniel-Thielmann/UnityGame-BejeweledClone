using System.Collections.Generic;
using UnityEngine;

public class InformacoesPecasModificadas
{
    private List<GameObject> pecasAlteradas;

    public int MaiorDistancia { get; set; }

    /// <summary>
    /// Retorna uma lista única das peças modificadas
    /// </summary>
    public IEnumerable<GameObject> PecasUnicas
    {
        get
        {
            HashSet<GameObject> conjuntoTemporario = new HashSet<GameObject>();
            foreach (var peca in pecasAlteradas)
            {
                if (!conjuntoTemporario.Contains(peca))
                {
                    conjuntoTemporario.Add(peca);
                    yield return peca;
                }
            }
        }
    }

    public InformacoesPecasModificadas()
    {
        pecasAlteradas = new List<GameObject>();
    }

    public void RegistrarPeca(GameObject objeto)
    {
        bool objetoJaRegistrado = false;
        int contador = 0;

        while (!objetoJaRegistrado && contador < pecasAlteradas.Count)
        {
            if (pecasAlteradas[contador] == objeto)
            {
                objetoJaRegistrado = true;
            }
            contador++;
        }

        if (!objetoJaRegistrado)
        {
            pecasAlteradas.Add(objeto);
        }
    }
}
