using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorDeTempo : MonoBehaviour
{
    public Text textoCronometro;
    public GerenciadorTabuleiro gerenciadorTabuleiro;

    private float tempoRestante;
    public bool jogoFinalizado = false;

    public float totalTempoJogo = 60f; // 60 segundos padrão

    private void Awake()
    {
        InicializarCronometro();
    }

    private void Update()
    {
        if (jogoFinalizado)
            return;

        AtualizarTempo();
        AtualizarTextoTempoRestante();

        if (tempoRestante <= 0 && !jogoFinalizado)
        {
            ExecutarFimDeJogo();
        }
    }

    private void InicializarCronometro()
    {
        tempoRestante = totalTempoJogo;
        jogoFinalizado = false;
    }

    private void AtualizarTempo()
    {
        tempoRestante -= Time.deltaTime;
    }

    private void AtualizarTextoTempoRestante()
    {
        if (textoCronometro != null)
        {
            int segundosExibicao = Mathf.CeilToInt(tempoRestante);
            textoCronometro.text = $"Tempo restante: {segundosExibicao}s";
        }
    }

    public bool JogoFinalizado()
    {
        return jogoFinalizado;
    }

    private void ExecutarFimDeJogo()
    {
        jogoFinalizado = true;
        int pontuacaoAtual = PlayerPrefs.GetInt("PontuacaoAtual", 0);

        if (gerenciadorTabuleiro != null)
        {
            var controladorJogo = FindObjectOfType<ControladorJogo>();
            if (controladorJogo != null)
                pontuacaoAtual = controladorJogo.ObterPontuacaoAtual();
        }

        PlayerPrefs.SetInt("PontuacaoFinal", pontuacaoAtual);
        SceneManager.LoadScene("TelaFinal");
    }
}
