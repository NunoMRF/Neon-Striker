using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VidaScript : MonoBehaviour
{
    [Header("Configura��o de Tipo")]
    public bool Jogador = false; 

    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaAtual;

    [Header("UI (S� para o Jogador)")]
    public Slider barraDeVida;
    public TextMeshProUGUI textoVida;

    [Header("Refer�ncias")]
    public Animator animadorInimigo; 

    void Start()
    {
        vidaAtual = vidaMaxima;

        // Atualizar slider
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaAtual;
        }

        // Atualizar texto
        AtualizarTextoVida();
    }

    public void LevarDano(int dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        // Atualiza a UI se ela existir (no caso do Player)
        if (barraDeVida != null)
            barraDeVida.value = vidaAtual;

        AtualizarTextoVida();

        // Se a vida chegar a zero, chama a fun��o de morrer
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    public void Curar(int quantidade)
    {
        vidaAtual += quantidade;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        if (barraDeVida != null)
            barraDeVida.value = vidaAtual;

        AtualizarTextoVida();
    }

    void AtualizarTextoVida()
    {
        if (textoVida != null)
            textoVida.text = vidaAtual + " / " + vidaMaxima;
    }

    void Morrer()
    {
        if (Jogador)
        {
            // ... (C�digo do jogador igual) ...
            if (GameManager.instance != null) GameManager.instance.GameOver();
        }
        else
        {
            Debug.Log("Morte do inimigo iniciada!");

            // EM VEZ DE PROCURAR, USAMOS A REFER�NCIA DIRETA:
            if (animadorInimigo != null)
            {
                animadorInimigo.SetTrigger("Die");
                Debug.Log("Ordem de anima��o enviada!");
            }
            else
            {
                // Tenta procurar como plano B, caso te esque�as de arrastar
                Animator animAuto = GetComponentInChildren<Animator>();
                if (animAuto != null) animAuto.SetTrigger("Die");
            }

            // ... (Continua a desligar o AI, NavMesh e Rigidbody igual ao que tinhas) ...
            SoldierAI ai = GetComponent<SoldierAI>();
            if (ai != null) ai.enabled = false;

            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.enabled = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) { rb.isKinematic = true; rb.linearVelocity = Vector3.zero; }

            // Destr�i
            Destroy(gameObject, 4f);
        }
    }
}