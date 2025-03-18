using UnityEngine;

public class ElementoTabuleiro : MonoBehaviour
{
    public string TipoPeca { get; set; }
    public int Linha { get; set; }
    public int Coluna { get; set; }
    public TipoBonusEspecial TipoBonus { get; set; }

    /// <summary>
    /// Define as propriedades básicas do elemento.
    /// </summary>
    public void Definir(string tipo, int linha, int coluna)
    {
        TipoPeca = tipo;
        Linha = linha;
        Coluna = coluna;
    }

    /// <summary>
    /// Verifica se este elemento é do mesmo tipo que outro elemento fornecido.
    /// </summary>
    public bool EhMesmoTipo(ElementoTabuleiro outroElemento)
    {
        if (outroElemento == null)
            throw new System.ArgumentNullException("OutroElemento não pode ser null");

        return TipoPeca == outroElemento.TipoPeca;
    }

    /// <summary>
    /// Troca linha/coluna com outro elemento fornecido.
    /// </summary>
    public void Trocar(ElementoTabuleiro outroElemento)
    {
        if (outroElemento == null)
            throw new System.ArgumentNullException("OutroElemento não pode ser null");

        int tempLinha = outroElemento.Linha;
        int tempColuna = outroElemento.Coluna;

        outroElemento.Linha = this.Linha;
        outroElemento.Coluna = this.Coluna;

        this.Linha = tempLinha;
        this.Coluna = tempColuna;
    }
}
