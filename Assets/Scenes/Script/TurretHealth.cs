using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    [Header("Resistência")]
    public int vidaMaxima = 50;
    private int vidaAtual;

    [Header("Efeitos Visuais")]
    public GameObject explosaoFinalPrefab; // A tua explosão gigante

    [Header("Efeitos Sonoros")]
    public AudioClip somExplosao; // NOVO: O ficheiro de som (arrasta do WarFX)

    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log("CABUM! Torre destruída.");

        // 1. Toca o som da explosão no local onde a torre está
        // Usamos PlayClipAtPoint porque o objeto vai ser destruído logo a seguir
        if (somExplosao != null)
        {
            AudioSource.PlayClipAtPoint(somExplosao, transform.position);
        }

        // 2. Cria a explosão visual (Fogo/Fumo)
        if (explosaoFinalPrefab != null)
        {
            Instantiate(explosaoFinalPrefab, transform.position, Quaternion.identity);
        }

        // 3. Destrói a torre imediatamente
        Destroy(gameObject);
    }
}