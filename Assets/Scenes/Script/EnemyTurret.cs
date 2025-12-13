using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Alvos")]
    public Transform player;
    public Transform turretHead;   // A parte que roda
    public Transform firePoint;    // A ponta do canhão 

    [Header("Laser")]
    public LineRenderer laserLine;

    [Header("Configuração de Combate")]
    public float range = 25f;
    public float turnSpeed = 5f;
    public float fireRate = 1f;    // Tiros por segundo
    private float fireCountdown = 0f;

    [Header("Munição e Som")]
    public GameObject bulletPrefab;
    public AudioClip somTiro; // NOVO: O ficheiro de som do tiro
    private AudioSource audioSource; // NOVO: O componente que toca o som

    void Start()
    {
        // NOVO: Vai buscar o "altifalante" da torre
        audioSource = GetComponent<AudioSource>();

        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // Garante que o laser começa desligado
        if (laserLine) laserLine.enabled = false;
    }

    void Update()
    {
        if (player == null || turretHead == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= range)
        {
            LockOnTarget();
            LaserAndShoot();
        }
        else
        {
            // Se o jogador fugir, desliga o laser
            if (laserLine) laserLine.enabled = false;
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = player.position - turretHead.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(turretHead.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        turretHead.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void LaserAndShoot()
    {
        // 1. Lógica do Laser
        if (laserLine && firePoint)
        {
            if (!laserLine.enabled) laserLine.enabled = true;

            laserLine.SetPosition(0, firePoint.position); // Começa na arma
            laserLine.SetPosition(1, player.position + Vector3.up * 1.5f); // Termina no peito do jogador
        }

        // 2. Lógica do Tiro
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        // Cria a bala
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // NOVO: Toca o som do tiro
        if (audioSource != null && somTiro != null)
        {
            audioSource.PlayOneShot(somTiro);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}