using UnityEngine;

public class LandMine : MonoBehaviour
{
    [Header("Configuração")]
    public int dano = 40; // Tira bastante vida!
    public GameObject efeitoExplosao; // O prefab da explosão (VFX)

    void OnTriggerEnter(Collider other)
    {
        // Verifica se é o Jogador (ou um inimigo burro que a pise)
        if (other.CompareTag("Player"))
        {
            Explodir(other.gameObject);
        }
    }

    void Explodir(GameObject alvo)
    {
        // 1. Tenta tirar vida ao alvo
        VidaScript vida = alvo.GetComponent<VidaScript>();

        if (vida != null)
        {
            vida.LevarDano(dano);
            Debug.Log("BOOM! Mina explodiu o jogador.");
        }

        // 2. Cria o efeito visual da explosão (se tiveres um)
        if (efeitoExplosao != null)
        {
            Instantiate(efeitoExplosao, transform.position, transform.rotation);
        }

        // 3. Destroi a mina
        Destroy(gameObject);
    }
}