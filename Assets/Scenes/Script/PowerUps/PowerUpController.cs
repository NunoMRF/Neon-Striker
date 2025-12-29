using UnityEngine;
using TMPro;
using System.Collections;

public class PowerUpController : MonoBehaviour
{
    [Header("UI (Opcional - depende da cena)")]
    public TextMeshProUGUI powerUpText;

    private Coroutine powerUpCoroutine;

    public void ActivateDamageBoostUI(float duration)
    {
        // Segurança: se não houver UI nesta cena, não faz nada
        if (powerUpText == null)
        {
            Debug.Log("PowerUp apanhado (Cena sem UI)");
            return;
        }

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
