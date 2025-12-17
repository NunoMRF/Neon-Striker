using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Configura��o")]
    public float velocidade = 20f;
    public int dano = 10; // Quanto de vida tira?

    void Start()
    {
        // 1. Encontra o Jogador na cena (pela Tag)
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");

        if (jogador != null)
        {
            // 2. CORREÇÃO DE MIRA: Olha para o Jogador
            // Adicionamos Vector3.up * 1.2f para apontar para o PEITO e não para os pés
            Vector3 pontoDeMira = jogador.transform.position + (Vector3.up * 1.2f);

            transform.LookAt(pontoDeMira);
        }

        // 3. Agora que já está virada para o sítio certo, dá o impulso
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * velocidade;
        }

        // 4. Destruir passados 3 segundos
        Destroy(gameObject, 3f);
    }
    void OnTriggerEnter(Collider other)
    {
        // Se bater no JOGADOR
        if (other.CompareTag("Player"))
        {
            Debug.Log("Foste atingido!");

            // Procura o script de vida no Player
            VidaScript vidaPlayer = other.GetComponent<VidaScript>();

            // Se encontrou, d� dano
            if (vidaPlayer != null)
            {
                vidaPlayer.LevarDano(dano);
            }

            // A bala destr�i-se ap�s acertar
            Destroy(gameObject);
        }
        // Se bater no cen�rio (ch�o, paredes), destr�i-se tamb�m
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}