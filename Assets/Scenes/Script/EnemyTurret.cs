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

    [Header("Munição")]
    public GameObject bulletPrefab;
    void Start()
    {
        if (player == null)
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
        Vector3 dir = player.position - turretHead.position; // Ajustado para usar a posição da cabeça
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
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}