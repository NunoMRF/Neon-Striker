using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VidaScript : MonoBehaviour
{
    [Header("Vida do Jogador")]
    public int vidaMaxima = 100;
    public int vidaAtual;

    [Header("UI")]
    public Slider barraDeVida;
    public TextMeshProUGUI textoVida; // NOVO

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

        if (barraDeVida != null)
            barraDeVida.value = vidaAtual;

        AtualizarTextoVida();

        if (vidaAtual <= 0)
            Morrer();
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
        Debug.Log("Jogador morreu!");
        // Mais tarde: respawn, animação, etc.
    }
}
