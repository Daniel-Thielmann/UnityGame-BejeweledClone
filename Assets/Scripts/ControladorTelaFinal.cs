using UnityEngine;
using UnityEngine.UI;

public class ControladorTelaFinal : MonoBehaviour
{
    public Text textoPontuacaoFinal;

    void Start()
    {
        ExibirPontuacaoFinal();
    }

    private void ExibirPontuacaoFinal()
    {
        int pontosObtidos = PlayerPrefs.GetInt("PontuacaoFinal", 0);
        textoPontuacaoFinal.text = $"Sua pontuação final: {pontosObtidos}";
    }
}
