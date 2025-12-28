using UnityEngine;
using System.Collections;

public class PowerUpPickup : MonoBehaviour
{
    public float duration = 10f;
    public float respawnTime = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PowerUpController controller = other.GetComponent<PowerUpController>();

        if (controller != null)
        {
            controller.ActivatePowerUp(duration);
        }

        // Inicia o respawn ANTES de desligar
        StartCoroutine(RespawnRoutine());

        // Desliga o power up (como antes)
        gameObject.SetActive(false);
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        gameObject.SetActive(true);
    }
}
