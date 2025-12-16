using UnityEngine;
using UnityEngine.AI; // Necessário para o NavMesh

public class SoldierAI : MonoBehaviour
{
    [Header("Alvos")]
    public Transform player;
    public Transform gunPoint; // A ponta da arma do Jones

    [Header("Configuração")]
    public float shootingRange = 15f; // Distância para começar a disparar
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    [Header("Munição")]
    public GameObject bulletPrefab;
    public AudioClip shootSound;
    private AudioSource audioSource;

    [Header("Animação")]
    public Animator animator; // Para controlar correr/parar

    // O Cérebro de movimento
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // Ajusta a velocidade do NavMesh para bater certo com a animação
        agent.speed = 3.5f;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // --- MÁQUINA DE ESTADOS ---

        if (distance <= shootingRange)
        {
            // ESTADO: ATACAR
            StopAndShoot();
        }
        else
        {
            // ESTADO: PERSEGUIR
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // Ativa a animação de correr (se tiveres um parâmetro "IsRunning")
        if (animator != null) animator.SetBool("IsRunning", true);
    }

    void StopAndShoot()
    {
        agent.isStopped = true; // Para de andar

        // Vira-se para o jogador suavemente
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Desliga a animação de correr
        if (animator != null) animator.SetBool("IsRunning", false);

        // Dispara
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Cria a bala
        if (bulletPrefab != null && gunPoint != null)
        {
            Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        }

        // Toca o som
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Se tiveres animação de tiro (Trigger "Shoot")
        if (animator != null) animator.SetTrigger("Shoot");
    }
}