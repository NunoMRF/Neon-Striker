using UnityEngine;
using TMPro;
using System.Collections;

public class PowerUpController : MonoBehaviour
{
    public TextMeshProUGUI powerUpText;

    private Coroutine powerUpCoroutine;

    public void ActivateDamageBoostUI(float duration)
    {
        if (powerUpCoroutine != null)
            StopCoroutine(powerUpCoroutine);

        powerUpCoroutine = StartCoroutine(DamageBoostRoutine(duration));
    }

    private IEnumerator DamageBoostRoutine(float duration)
    {
        float timeLeft = duration;

        powerUpText.gameObject.SetActive(true);

        while (timeLeft > 0)
        {
            powerUpText.text = "DANO x2 - " + Mathf.CeilToInt(timeLeft) + "s";
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        powerUpText.gameObject.SetActive(false);
    }
}
