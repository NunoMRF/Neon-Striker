using TMPro;
using UnityEngine;

public class DashUIController : MonoBehaviour
{
    public TextMeshProUGUI dashStatusText;

    public void SetReady()
    {
        dashStatusText.text = "READY";
        dashStatusText.color = Color.green;
    }

    public void SetCooldown(float time)
    {
        dashStatusText.text = "Cooldown: " + time.ToString("F1") + "s";
        dashStatusText.color = Color.red;
    }
}
