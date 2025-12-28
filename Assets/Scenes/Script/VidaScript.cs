using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VidaScript : MonoBehaviour
{
    [Header("Configuração de Tipo")]
    public bool Jogador = false;

    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaAtual;

    [Header("UI (Só para o Jogador)")]
    public Slider barraDeVida;
    public TextMeshProUGUI textoVida;

    [Header("Referências")]
    public Animator animadorInimigo;

    // Variável interna para controlar a cor da barra
    private Image imagemDoSlider;

    void Start()
    {
        vidaAtual = vidaMaxima;

        // Configuração inicial do Slider
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaAtual;

            // Tenta encontrar a imagem de preenchimento (Fill) dentro do Slider
            if (barraDeVida.fillRect != null)
            {
                imagemDoSlider = barraDeVida.fillRect.GetComponent<Image>();
            }
        }

        // Atualizar texto e cor inicial
        AtualizarTextoVida();
        AtualizarCorDaBarra();
    }

    void Update()
    {
        // Garante que a barra visual acompanha a vida (caso mudes a vida noutro script)
        if (barraDeVida != null)
        {
            // Se a vida máxima mudar (por causa do Escudo Azul), atualiza o limite do slider
            if (barraDeVida.maxValue != vidaMaxima)
            {
                barraDeVida.maxValue = vidaMaxima;
            }

            barraDeVida.value = vidaAtual;
            AtualizarCorDaBarra();
        }
    }

    public void LevarDano(int dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        AtualizarUI();

        // Se a vida chegar a zero, chama a função de morrer
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    public void Curar(int quantidade)
    {
        vidaAtual += quantidade;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        AtualizarUI();
    }

    // Função centralizada para atualizar tudo na UI
    void AtualizarUI()
    {
        if (barraDeVida != null)
            barraDeVida.value = vidaAtual;

        AtualizarTextoVida();
        AtualizarCorDaBarra();
    }

    void AtualizarTextoVida()
    {
        if (textoVida != null)
            textoVida.text = vidaAtual + " / " + vidaMaxima;
    }

    // --- A NOVA LÓGICA DA COR ---
    void AtualizarCorDaBarra()
    {
        if (imagemDoSlider == null) return; // Se não tiver barra, sai

        if (vidaAtual > 100)
        {
            // MODO ESCUDO: Azul Ciano (Brilhante)
            imagemDoSlider.color = Color.cyan;
        }
        else
        {
            // MODO NORMAL: Verde
            imagemDoSlider.color = Color.green;
        }
    }

    void Morrer()
    {
        if (Jogador)
        {
            if (GameManager.instance != null) GameManager.instance.GameOver();
        }
        else
        {
            Debug.Log("Morte do inimigo iniciada!");

            if (animadorInimigo != null)
            {
                animadorInimigo.SetTrigger("Die");
            }
            else
            {
                Animator animAuto = GetComponentInChildren<Animator>();
                if (animAuto != null) animAuto.SetTrigger("Die");
            }

            SoldierAI ai = GetComponent<SoldierAI>();
            if (ai != null) ai.enabled = false;

            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.enabled = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) { rb.isKinematic = true; rb.linearVelocity = Vector3.zero; }

            Destroy(gameObject, 4f);
        }
    }
}