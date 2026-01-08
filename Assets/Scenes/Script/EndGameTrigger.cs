using UnityEngine;
using TMPro;

public class EndGameTrigger : MonoBehaviour
{
    public GameObject endGameText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endGameText.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
