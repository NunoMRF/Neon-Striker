using UnityEngine;

public class BalaCarregada : MonoBehaviour
{
    public int dano = 30;

    void OnTriggerEnter(Collider other)
    {
        // 1. Ignora o próprio jogador e outras balas
        if (other.CompareTag("Player") || other.CompareTag("Projectile")) return;

        // 2. Tenta encontrar a vida de um SOLDADO
        VidaScript vidaSoldado = other.GetComponentInParent<VidaScript>();

        if (vidaSoldado != null)
        {
            vidaSoldado.LevarDano(dano);
            Debug.Log("Acertei num Soldado!");
            Destroy(gameObject);
            return; // Sai da função, já fizemos o trabalho
        }

        // 3. Tenta encontrar a vida de uma TURRET
        TurretHealth vidaTurret = other.GetComponentInParent<TurretHealth>();

        if (vidaTurret != null)
        {
            // ATENÇÃO: Confirma se no script 'TurretHealth' a função se chama 'LevarDano'
            // Se der erro vermelho, vê a explicação abaixo!
            vidaTurret.ReceberDano(dano);

            Debug.Log("Acertei numa Turret!");
            Destroy(gameObject);
        }
        else
        {
            // Bateu em paredes ou chão
            Destroy(gameObject);
        }
    }
}