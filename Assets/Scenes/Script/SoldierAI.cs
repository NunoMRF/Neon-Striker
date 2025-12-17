using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour
{
    [Header("Alvos")]
    public Transform player;
    public Transform gunPoint;

    [Header("Referências")]
    public Animator animator; // O Animator do filho (arrasta aqui)

    [Header("Combate")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float nextFireTime;

    [Header("Inteligência (AI)")]
    public float raioDetecao = 15f;   // Distância para ele te ver e atacar (Círculo Vermelho)
    public float raioPatrulha = 10f;  // Distância que ele anda sozinho (Círculo Verde)
    public float tempoDeEspera = 3f;  // Tempo parado antes de mudar de sítio

    private NavMeshAgent agent;
    private Vector3 pontoInicial; // O "centro" da zona de patrulha dele
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Guarda a posição onde ele nasceu como sendo a "casa" dele
        pontoInicial = transform.position;
        timer = tempoDeEspera;

        // Tenta encontrar o player automaticamente se não tiveres arrastado
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                // Tenta apontar para o peito (AlvoInimigo), senão vai aos pés
                Transform alvoPeito = p.transform.Find("AlvoInimigo");
                if (alvoPeito != null) player = alvoPeito;
                else player = p.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // 1. Calcular a distância entre Inimigo e Jogador
        float distanciaAoJogador = Vector3.Distance(transform.position, player.position);

        // 2. DECISÃO: O jogador está perto?
        if (distanciaAoJogador <= raioDetecao)
        {
            EstadoAtacar(distanciaAoJogador);
        }
        else
        {
            EstadoPatrulhar();
        }
    }

    // --- COMPORTAMENTO DE ATAQUE ---
    void EstadoAtacar(float distancia)
    {
        // Corre atrás do jogador
        agent.SetDestination(player.position);

        // Se estiver perto o suficiente para disparar (ex: 10 metros)
        if (distancia <= 10f)
        {
            agent.isStopped = true; // Pára de andar
            RotateTowards(player);

            if (animator != null) animator.SetBool("IsRunning", false);

            // Disparar com cadência
            if (Time.time >= nextFireTime)
            {
                if (bulletPrefab != null)
                    Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);

                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            agent.isStopped = false; // Continua a correr se o jogador fugir
            if (animator != null) animator.SetBool("IsRunning", true);
        }
    }

    // --- COMPORTAMENTO DE PATRULHA ---
    void EstadoPatrulhar()
    {
        agent.isStopped = false;

        // Verifica se chegou ao ponto de destino da patrulha
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Chegou! Fica parado um bocadinho
            if (animator != null) animator.SetBool("IsRunning", false);

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                EscolherNovoPonto();
                timer = tempoDeEspera;
            }
        }
        else
        {
            // Ainda está a caminho do ponto
            if (animator != null) animator.SetBool("IsRunning", true);
        }
    }

    void EscolherNovoPonto()
    {
        // Escolhe um ponto aleatório dentro do raio de patrulha
        Vector3 randomPoint = pontoInicial + Random.insideUnitSphere * raioPatrulha;

        NavMeshHit hit;
        // Verifica se esse ponto é válido no chão (NavMesh)
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // AJUDA VISUAL NO EDITOR
    void OnDrawGizmosSelected()
    {
        // Círculo Vermelho: Distância para te ver
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDetecao);

        // Círculo Verde: Área de patrulha (só mostra certo quando dás Play e o pontoInicial é definido, 
        // mas aqui mostra relativo à posição atual para ajudar)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, raioPatrulha);
    }
}