using UnityEngine;
using TMPro;
using System.Collections;

public class PowerUpController : MonoBehaviour
{
    public TextMeshProUGUI powerUpText;

    private Coroutine powerUpCoroutine;

    public void ActivatePowerUp(float duration)
    {
        if (powerUpCoroutine != null)
            StopCoroutine(powerUpCoroutine);

        powerUpCoroutine = StartCoroutine(PowerUpRoutine(duration));
    }

    private IEnumerator PowerUpRoutine(float duration)
    {
        powerUpText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        powerUpText.gameObject.SetActive(false);
    }
}
