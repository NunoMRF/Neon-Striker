using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VidaScript : MonoBehaviour
{
    [Header("Configuração de Tipo")]
    public bool Jogador = false; // Define se é Player (Game Over) ou Inimigo (Destroy)

    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaAtual;

    [Header("UI (Só para o Jogador)")]
    public Slider barraDeVida;
    public TextMeshProUGUI textoVida;

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
            // Lógica do Jogador (Game Over)
            Debug.Log("O Jogador Morreu!");
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
        else
        {
            // Lógica do Inimigo (Animação + Delay)
            Debug.Log("Inimigo abatido!");

            // 1. Toca a animação de morte
            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Die");
            }

            // 2. Desliga o cérebro do soldado (para ele parar de disparar)
            SoldierAI ai = GetComponent<SoldierAI>();
            if (ai != null) ai.enabled = false;

            // 3. Desliga o movimento (NavMesh) para ele não deslizar morto
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.enabled = false;

            // 4. Desliga o Collider (para as balas não baterem no cadáver)
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            // 5. Destrói o corpo passados 4 segundos (dá tempo de ver a animação)
            Destroy(gameObject, 4f);
        }
    }
}