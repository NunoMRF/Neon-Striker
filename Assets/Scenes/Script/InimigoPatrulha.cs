using UnityEngine;
using UnityEngine.AI;

public class InimigoPatrulha : MonoBehaviour
{
    [Header("Componentes")]
    public NavMeshAgent agent;
    public Transform player;
    public Animator anim; // Arrastar o Animator do Jones para aqui

    [Header("Patrulha (Passear)")]
    public float raioPatrulha = 10f; // Quão longe ele vai andar sozinho
    public float tempoEspera = 2f;   // Tempo parado antes de mudar de sitio
    private float timerPatrulha;

    [Header("Combate")]
    public float raioDeteccao = 15f; // Distância para te ver
    public float raioAtaque = 5f;    // Distância para disparar

    // Estados internos
    private bool playerDetetado = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        // Começa logo a patrulhar
        MoverParaPontoAleatorio();
    }

    void Update()
    {
        if (player == null) return;
        float distanciaPlayer = Vector3.Distance(transform.position, player.position);

        // LÓGICA DE VELOCIDADE (NOVO)
        // Se detetou o jogador: Corre (Speed = 6)
        // Se está a patrulhar: Anda (Speed = 2.5)
        if (playerDetetado)
        {
            agent.speed = 6f;
        }
        else
        {
            agent.speed = 2.5f;
        }

        // --- LÓGICA DE DECISÃO ---

        // 1. Está perto o suficiente para ATACAR?
        if (distanciaPlayer <= raioAtaque)
        {
            EstadoAtaque();
        }
        // 2. Está perto o suficiente para PERSEGUIR?
        else if (distanciaPlayer <= raioDeteccao)
        {
            EstadoPerseguicao();
        }
        // 3. Não vê ninguém? PATRULHA.
        else
        {
            EstadoPatrulha();
        }
    }

    // --- COMPORTAMENTOS ---

    void EstadoPatrulha()
    {
        playerDetetado = false;
        agent.isStopped = false;

        // Animação: Não está a atacar, e corre se tiver velocidade
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsRunning", agent.velocity.magnitude > 0.1f);

        // Se chegou ao destino da patrulha
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            timerPatrulha += Time.deltaTime;
            anim.SetBool("IsRunning", false); // Fica parado à espera

            if (timerPatrulha >= tempoEspera)
            {
                MoverParaPontoAleatorio();
                timerPatrulha = 0;
            }
        }
    }

    void EstadoPerseguicao()
    {
        playerDetetado = true;
        agent.isStopped = false;

        // Corre atrás do jogador
        agent.SetDestination(player.position);

        // Animação
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsRunning", true);
    }

    void EstadoAtaque()
    {
        // Para o boneco para disparar
        agent.isStopped = true;

        // Olha para o jogador (rotação suave)
        Vector3 direcao = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direcao.x, 0, direcao.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Animação
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsAttacking", true);
    }

    // --- UTILITÁRIOS ---

    void MoverParaPontoAleatorio()
    {
        // Escolhe um ponto aleatório dentro de uma esfera imaginária
        Vector3 randomDirection = Random.insideUnitSphere * raioPatrulha;
        randomDirection += transform.position;

        NavMeshHit hit;
        // Encontra o ponto válido mais próximo no chão azul (NavMesh)
        if (NavMesh.SamplePosition(randomDirection, out hit, raioPatrulha, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    // Desenha as bolas coloridas para tu veres as distâncias
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, raioPatrulha); // Área onde ele anda
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeteccao); // Visão
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioAtaque);   // Tiro
    }
}