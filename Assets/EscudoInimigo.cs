using UnityEngine;

public class EscudoInimigo : MonoBehaviour
{
    [Header("Configurações do Escudo")]
    public int vidaExtra = 50;
    public GameObject bolhaVisual;

    private VidaScript scriptVida;
    private int limiteParaQuebrar; // O valor de vida onde o escudo desaparece

    void Awake()
    {
        scriptVida = GetComponent<VidaScript>();

        if (scriptVida != null)
        {
            // Aumenta a vida máxima do inimigo
            scriptVida.vidaMaxima += vidaExtra;
        }
    }

    void Start()
    {
        if (scriptVida != null)
        {
            // Calcula o limite. Exemplo: Se tinha 100 e virou 150, 
            // quando a vida baixar de 100 (150 - 50), o escudo quebra.
            limiteParaQuebrar = scriptVida.vidaMaxima - vidaExtra;
        }
    }

    void Update()
    {
        // Só verifica se a bolha ainda estiver ativa
        if (bolhaVisual != null && bolhaVisual.activeSelf)
        {
            // Se a vida atual caiu para o nível normal (perdeu o extra)
            if (scriptVida.vidaAtual <= limiteParaQuebrar)
            {
                QuebrarEscudo();
            }
        }
    }

    void QuebrarEscudo()
    {
        bolhaVisual.SetActive(false); // Desliga o visual azul
        Debug.Log("O Escudo do Elite quebrou!");

        // Se quiser adicionar um som de vidro quebrando, seria aqui!
    }
}