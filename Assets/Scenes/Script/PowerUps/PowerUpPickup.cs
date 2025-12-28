using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public float duration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Power-up apanhado: DANO x2");

            // DANO
            GunShoot gun = other.GetComponentInChildren<GunShoot>();
            if (gun != null)
            {
                gun.ActivateDamageBoost(duration);
            }

            // UI
            PowerUpController ui = other.GetComponent<PowerUpController>();
            if (ui != null)
            {
                ui.ActivateDamageBoostUI(duration);
            }

            gameObject.SetActive(false);
        }
    }
}
