using UnityEngine;
using UnityEngine.AI; // Necessário para usar o NavMeshAgent

public class AranhaAI : MonoBehaviour
{
    [Header("Configurações")]
    public float raioDeDeteccao = 10f; // A distância "X" metros para ela te ver
    public string tagDoPlayer = "Player"; // A tag que o seu personagem usa

    private NavMeshAgent agent;
    private Transform alvo; // O Transform do Player
    private Animator animator; // Para controlar a animação depois

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Tenta encontrar o Player automaticamente pela Tag
        GameObject objPlayer = GameObject.FindGameObjectWithTag(tagDoPlayer);
        if (objPlayer != null)
        {
            alvo = objPlayer.transform;
        }
    }

    void Update()
    {
        if (alvo == null) return; // Se não achou o player, não faz nada

        // Calcula a distância entre a Aranha e o Player
        float distancia = Vector3.Distance(transform.position, alvo.position);

        // Lógica de "Perseguição"
        if (distancia <= raioDeDeteccao)
        {
            // Se estiver perto: Ativa o movimento e vai até o player
            agent.isStopped = false;
            agent.SetDestination(alvo.position);
        }
        else
        {
            // Se estiver longe: Para o movimento
            agent.isStopped = true;
        }
    }

    // Ferramenta visual para você ver o raio na cena (Círculo Vermelho)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDeDeteccao);
    }
}