using UnityEngine;

public class PowerUpEscudo : MonoBehaviour
{
    [Header("Configuração")]
    public int novaVidaMaxima = 150;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VidaScript scriptVida = other.GetComponent<VidaScript>();

            if (scriptVida != null)
            {
                // 1. Atualiza os valores internos
                scriptVida.vidaMaxima = novaVidaMaxima;
                scriptVida.vidaAtual = novaVidaMaxima;

                // 2. O TRUQUE: Simula um dano de 0 para forçar a barra a atualizar!
                scriptVida.LevarDano(0);

                Debug.Log("ESCUDO AZUL: Vida 150 e UI atualizada!");

                // 3. Desliga e destroi
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject);
            }
        }
    }
}