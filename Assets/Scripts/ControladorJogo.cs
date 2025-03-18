using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class ControladorJogo : MonoBehaviour
{
    public Text textoDebug, textoPontuacao;
    public bool exibirDebug = false;

    public GerenciadorTabuleiro tabuleiro;

    private int pontuacaoAtual;

    public readonly Vector2 posicaoBase = new Vector2(-2.37f, -4.27f);
    public readonly Vector2 tamanhoElemento = new Vector2(0.7f, 0.7f);

    private EstadoAtualJogo estadoAtual = EstadoAtualJogo.Indefinido;
    private GameObject elementoSelecionado = null;
    private Vector2[] posicoesSpawn;
    public GameObject[] prefabsElementos;

    private IEnumerator BuscarCombinacoesPotenciais() { yield return null; }

    public GameObject[] prefabsExplosoes;
    public GameObject[] prefabsBonus;
    public static readonly float DuracaoAnimacao = 0.3f;


    private IEnumerator coroutineVerificaPotenciais;
    private IEnumerator coroutineAnimacaoPotenciais;

    IEnumerable<GameObject> combinacoesPotenciais;

    public ControladorDeTempo controladorTempo;

    void Awake()
    {
        if (textoDebug == null)
            textoDebug = FindObjectOfType<Text>(); // Busca um objeto Text na cena

        if (textoDebug != null)
            textoDebug.enabled = exibirDebug;

        InicializarValidacoes();
    }

    void Start()
    {
        InicializarTiposElementosEBonus();
        InicializarElementosEPosicoesSpawn();

        if (controladorTempo != null)
        {
            controladorTempo.gerenciadorTabuleiro = tabuleiro;
        }
    }

    void Update()
    {
        if (controladorTempo != null && controladorTempo.jogoFinalizado)
            return;

        if (estadoAtual == EstadoAtualJogo.Indefinido)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    elementoSelecionado = hit.collider.gameObject;
                    estadoAtual = EstadoAtualJogo.IniciouSelecao;
                }
            }
        }
        else if (estadoAtual == EstadoAtualJogo.IniciouSelecao)
        {
            if (Input.GetMouseButton(0))
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && elementoSelecionado != hit.collider.gameObject)
                {

                    if (!FerramentasAuxiliares.SaoVizinhos(elementoSelecionado.GetComponent<ElementoTabuleiro>(),
                        hit.collider.gameObject.GetComponent<ElementoTabuleiro>()))
                    {
                        estadoAtual = EstadoAtualJogo.Indefinido;
                    }
                    else
                    {
                        estadoAtual = EstadoAtualJogo.EmAnimacao;
                        AjustarOrdemVisual(elementoSelecionado, hit.collider.gameObject);
                        StartCoroutine(VerificarCombinacoesECollapse(hit));
                    }
                }
            }
        }
    }

    private void InicializarValidacoes()
    {
        if (textoDebug == null) Debug.LogError("TextoDebug não atribuído.");
        if (textoPontuacao == null) Debug.LogError("textoPontuacao não atribuído.");
        if (prefabsElementos == null || prefabsElementos.Length == 0) Debug.LogError("PrefabsElementos não atribuídos.");
    }

    public int ObterPontuacaoAtual()
    {
        return pontuacaoAtual;
    }




    private void AjustarOrdemVisual(GameObject objetoA, GameObject objetoB)
    {
        SpriteRenderer spriteA = objetoA.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteB = objetoB.GetComponent<SpriteRenderer>();

        if (spriteA.sortingOrder <= spriteB.sortingOrder)
        {
            spriteA.sortingOrder = spriteB.sortingOrder + 1;
        }
    }

    private void IniciarChecagemCombinacoesPotenciais()
    {
        coroutineVerificaPotenciais = BuscarCombinacoesPotenciais();
        StartCoroutine(coroutineVerificaPotenciais);
    }

    private void PararChecagemPotenciais()
    {
        if (coroutineAnimacaoPotenciais != null)
            StopCoroutine(coroutineAnimacaoPotenciais);
        if (coroutineVerificaPotenciais != null)
            StopCoroutine(coroutineVerificaPotenciais);
    }

    private void InicializarTiposElementosEBonus()
    {
        foreach (var elemento in prefabsElementos)
        {
            elemento.GetComponent<ElementoTabuleiro>().TipoPeca = elemento.name;
        }

        foreach (var bonus in prefabsBonus)
        {
            bonus.GetComponent<ElementoTabuleiro>().TipoPeca = prefabsElementos
                .Where(e => e.GetComponent<ElementoTabuleiro>().TipoPeca.Contains(bonus.name.Split('_')[1].Trim()))
                .Single().name;
        }
    }

    private void InicializarVariaveis()
    {
        pontuacaoAtual = 0;
        AtualizarPontuacao();
    }
    private void DestruirTodosElementos()
    {
        for (int linha = 0; linha < ConfiguracoesJogo.NumeroLinhas; linha++)
        {
            for (int coluna = 0; coluna < ConfiguracoesJogo.NumeroColunas; coluna++)
            {
                if (tabuleiro[linha, coluna] != null)
                    Destroy(tabuleiro[linha, coluna]);
            }
        }
    }
    private GameObject ObterElementoAleatorio()
    {
        if (prefabsElementos == null || prefabsElementos.Length == 0)
        {
            Debug.LogError("PrefabsElementos está vazio. Certifique-se de que há elementos atribuídos no Inspector.");
            return null; // Retorna nulo para evitar erro
        }

        return prefabsElementos[Random.Range(0, prefabsElementos.Length)];
    }

    private bool TemMatchHorizontal(int linha, int coluna, GameObject elemento)
    {
        return tabuleiro[linha, coluna - 1].GetComponent<ElementoTabuleiro>().EhMesmoTipo(elemento.GetComponent<ElementoTabuleiro>()) &&
               tabuleiro[linha, coluna - 2].GetComponent<ElementoTabuleiro>().EhMesmoTipo(elemento.GetComponent<ElementoTabuleiro>());
    }
    private bool TemMatchVertical(int linha, int coluna, GameObject elemento)
    {
        return tabuleiro[linha - 1, coluna].GetComponent<ElementoTabuleiro>().EhMesmoTipo(elemento.GetComponent<ElementoTabuleiro>()) &&
               tabuleiro[linha - 2, coluna].GetComponent<ElementoTabuleiro>().EhMesmoTipo(elemento.GetComponent<ElementoTabuleiro>());
    }
    private void InstanciarNovoElemento(int linha, int coluna, GameObject novoElemento)
    {
        GameObject go = Instantiate(novoElemento,
            posicaoBase + new Vector2(coluna * tamanhoElemento.x, linha * tamanhoElemento.y), Quaternion.identity);

        go.GetComponent<ElementoTabuleiro>().Definir(novoElemento.GetComponent<ElementoTabuleiro>().TipoPeca, linha, coluna);
        tabuleiro[linha, coluna] = go;
    }
    private void AjustarPosicoesSpawn()
    {
        for (int coluna = 0; coluna < ConfiguracoesJogo.NumeroColunas; coluna++)
        {
            posicoesSpawn[coluna] = posicaoBase
                + new Vector2(coluna * tamanhoElemento.x, ConfiguracoesJogo.NumeroLinhas * tamanhoElemento.y);
        }
    }
    private void IncrementarPontuacao(int valor)
    {
        pontuacaoAtual += valor;
        AtualizarPontuacao();
    }

    private void AtualizarPontuacao()
    {
        if (textoPontuacao != null)
            textoPontuacao.text = $"Pontos: {pontuacaoAtual}";
    }



    public void InicializarElementosEPosicoesSpawn()
    {
        InicializarVariaveis();

        if (tabuleiro != null)
            DestruirTodosElementos();

        tabuleiro = new GerenciadorTabuleiro();
        posicoesSpawn = new Vector2[ConfiguracoesJogo.NumeroColunas];

        for (int linha = 0; linha < ConfiguracoesJogo.NumeroLinhas; linha++)
        {
            for (int coluna = 0; coluna < ConfiguracoesJogo.NumeroColunas; coluna++)
            {
                GameObject novoElemento;

                do
                {
                    novoElemento = ObterElementoAleatorio();
                }
                while ((coluna >= 2 && TemMatchHorizontal(linha, coluna, novoElemento)) ||
                       (linha >= 2 && TemMatchVertical(linha, coluna, novoElemento)));

                InstanciarNovoElemento(linha, coluna, novoElemento);
            }
        }

        AjustarPosicoesSpawn();
    }
    /// <summary>
    /// Procura combinações e realiza animações e colapsos correspondentes.
    /// </summary>
    /// <summary>
    /// Procura combinações e realiza animações e colapsos correspondentes.
    /// </summary>
    private IEnumerator VerificarCombinacoesECollapse(RaycastHit2D hitInfo)
    {
        var segundoObjeto = hitInfo.collider.gameObject;
        tabuleiro.TrocarElementos(elementoSelecionado, segundoObjeto);

        elementoSelecionado.transform.DOMove(segundoObjeto.transform.position, ConfiguracoesJogo.DuracaoAnimacao);
        segundoObjeto.transform.DOMove(elementoSelecionado.transform.position, ConfiguracoesJogo.DuracaoAnimacao);
        yield return new WaitForSeconds(ConfiguracoesJogo.DuracaoAnimacao);

        var combinacoesElementoA = tabuleiro.ObterCombinacoes(elementoSelecionado);
        var combinacoesElementoB = tabuleiro.ObterCombinacoes(segundoObjeto);

        var todasCombinacoes = combinacoesElementoA.PecasCombinadasUnicas
            .Union(combinacoesElementoB.PecasCombinadasUnicas)
            .Distinct()
            .ToList(); // Convertendo para List para evitar múltiplas execuções de LINQ.

        if (todasCombinacoes.Count < ConfiguracoesJogo.MinimoPecasParaCombinar)
        {
            // Volta os elementos para suas posições originais
            elementoSelecionado.transform.DOMove(segundoObjeto.transform.position, ConfiguracoesJogo.DuracaoAnimacao);
            segundoObjeto.transform.DOMove(elementoSelecionado.transform.position, ConfiguracoesJogo.DuracaoAnimacao);
            yield return new WaitForSeconds(ConfiguracoesJogo.DuracaoAnimacao);

            tabuleiro.DesfazerTroca();
            yield break; // Sai da função
        }

        bool gerarBonus = todasCombinacoes.Count >= ConfiguracoesJogo.MinimoPecasParaBonus &&
                          !FerramentasBonus.TemBonusLimpezaCompleta(combinacoesElementoA.BonusGerado) &&
                          !FerramentasBonus.TemBonusLimpezaCompleta(combinacoesElementoB.BonusGerado);

        ElementoTabuleiro elementoBonus = null;
        if (gerarBonus)
        {
            var elementoComBonus = combinacoesElementoA.PecasCombinadasUnicas.Any() ? elementoSelecionado : segundoObjeto;
            elementoBonus = elementoComBonus.GetComponent<ElementoTabuleiro>();
        }

        int repeticoes = 1;
        while (todasCombinacoes.Count >= ConfiguracoesJogo.MinimoPecasParaCombinar)
        {
            IncrementarPontuacao((todasCombinacoes.Count - 2) * ConfiguracoesJogo.PontosMatchNormal);

            if (repeticoes >= 2)
                IncrementarPontuacao(ConfiguracoesJogo.PontosMatchExtra);

            foreach (var item in todasCombinacoes)
            {
                tabuleiro.RemoverElemento(item); // Corrigido para `RemoverPeca`
                RemoverElementoCena(item);
            }

            if (gerarBonus && elementoBonus != null)
                CriarBonus(elementoBonus);

            gerarBonus = false;

            var colunasAfetadas = todasCombinacoes.Select(go => go.GetComponent<ElementoTabuleiro>().Coluna).Distinct().ToList();

            var infoColapso = tabuleiro.ColapsarColunas(colunasAfetadas);
            var infoNovasPecas = GerarNovasPecasNasColunas(colunasAfetadas);

            int maiorDistancia = Mathf.Max(infoColapso.MaiorDistancia, infoNovasPecas.MaiorDistancia);

            AnimarMovimentos(infoNovasPecas.PecasUnicas, maiorDistancia);
            AnimarMovimentos(infoColapso.PecasUnicas, maiorDistancia);

            yield return new WaitForSeconds(ConfiguracoesJogo.TempoMinimoAnimacaoMovimento * maiorDistancia);

            todasCombinacoes = infoColapso.PecasUnicas
                .SelectMany(p => tabuleiro.ObterCombinacoes(p).PecasCombinadasUnicas)
                .Union(infoNovasPecas.PecasUnicas.SelectMany(p => tabuleiro.ObterCombinacoes(p).PecasCombinadasUnicas))
                .Distinct()
                .ToList();

            repeticoes++;
        }

        estadoAtual = EstadoAtualJogo.Indefinido;
        IniciarChecagemCombinacoesPotenciais();
    }


    private void CriarBonus(ElementoTabuleiro elemento)
    {
        GameObject prefabBonus = ObterPrefabBonus(elemento.TipoPeca);
        GameObject bonus = Instantiate(prefabBonus,
            posicaoBase + new Vector2(elemento.Coluna * tamanhoElemento.x, elemento.Linha * tamanhoElemento.y),
            Quaternion.identity);

        tabuleiro[elemento.Linha, elemento.Coluna] = bonus;
        var elementoBonus = bonus.GetComponent<ElementoTabuleiro>();
        elementoBonus.Definir(elemento.TipoPeca, elemento.Linha, elemento.Coluna);
        elementoBonus.TipoBonus |= TipoBonusEspecial.LimparLinhaEColuna;
    }

    private InformacoesPecasModificadas GerarNovasPecasNasColunas(IEnumerable<int> colunasSemPecas)
    {
        InformacoesPecasModificadas infoPecas = new InformacoesPecasModificadas();

        foreach (int coluna in colunasSemPecas)
        {
            var espacosVazios = tabuleiro.ObterEspacosVaziosNaColuna(coluna);
            foreach (var espaco in espacosVazios)
            {
                var prefab = ObterElementoAleatorio();
                var novaPeca = Instantiate(prefab, posicoesSpawn[coluna], Quaternion.identity);

                novaPeca.GetComponent<ElementoTabuleiro>().Definir(prefab.GetComponent<ElementoTabuleiro>().TipoPeca, espaco.PosicaoLinha, espaco.PosicaoColuna);

                if (ConfiguracoesJogo.NumeroLinhas - espaco.PosicaoLinha > infoPecas.MaiorDistancia)
                    infoPecas.MaiorDistancia = ConfiguracoesJogo.NumeroLinhas - espaco.PosicaoLinha;

                tabuleiro[espaco.PosicaoLinha, espaco.PosicaoColuna] = novaPeca;
                infoPecas.RegistrarPeca(novaPeca);
            }
        }
        return infoPecas;
    }

    private void AnimarMovimentos(IEnumerable<GameObject> objetos, int distancia)
    {
        foreach (var obj in objetos)
        {
            obj.transform.DOMove(
                posicaoBase + new Vector2(obj.GetComponent<ElementoTabuleiro>().Coluna * tamanhoElemento.x,
                    obj.GetComponent<ElementoTabuleiro>().Linha * tamanhoElemento.y),
                ConfiguracoesJogo.TempoMinimoAnimacaoMovimento * distancia);
        }
    }



    /// <summary>
    /// Remove objeto da cena com efeito visual.
    /// </summary>
    private void RemoverElementoCena(GameObject elemento)
    {
        GameObject explosao = ObterPrefabExplosao();
        var novaExplosao = Instantiate(explosao, elemento.transform.position, Quaternion.identity);
        Destroy(novaExplosao, ConfiguracoesJogo.DuracaoExplosao);

        tabuleiro.RemoverElemento(elemento);
    }


    /// <summary>
    /// Obtém explosão aleatória.
    /// </summary>
    private GameObject ObterPrefabExplosao()
    {
        return prefabsExplosoes[Random.Range(0, prefabsExplosoes.Length)];
    }

    /// <summary>
    /// Obtém bônus com base no tipo do elemento.
    /// </summary>
    private GameObject ObterPrefabBonus(string tipo)
    {
        string cor = tipo.Split('_')[1].Trim();
        foreach (var prefab in prefabsBonus)
        {
            if (prefab.GetComponent<ElementoTabuleiro>().TipoPeca.Contains(cor))
                return prefab;
        }
        throw new System.Exception("Tipo incorreto");
    }

    private void ResetarOpacidadeCombinacoesPotenciais()
    {
        if (combinacoesPotenciais == null) return;

        foreach (var item in combinacoesPotenciais)
        {
            if (item == null) continue;

            var sprite = item.GetComponent<SpriteRenderer>();
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
    }
}