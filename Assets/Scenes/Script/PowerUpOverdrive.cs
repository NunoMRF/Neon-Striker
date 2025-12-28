using UnityEngine;
using System.Collections;

public class PowerUpOverdrive : MonoBehaviour
{
    [Header("Configuração")]
    public float duracaoFuria = 10f; // Ficas 10 segundos em modo "Deus"

    // Esta função corre quando algo entra no "Trigger" do escudo
    void OnTriggerEnter(Collider other)
    {
        // Só ativa se for o Jogador a tocar
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AtivarModoFuria(other.gameObject));
        }
    }

    IEnumerator AtivarModoFuria(GameObject player)
    {
        Debug.Log("🔥 MODO FÚRIA ATIVADO! 🔥");

        // 1. TENTAR ESCONDER O POWER-UP VISUALMENTE
        // Desliga o sensor para não o apanhares duas vezes
        GetComponent<Collider>().enabled = false;

        // Tenta desligar os visuais (MeshRenderer).
        // Como este asset é complexo, se ele não desaparecer totalmente, avisa-me!
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers) r.enabled = false;

        // --- 2. ACEDER AOS TEUS SISTEMAS ---
        PlayerDashSystem dashSystem = player.GetComponent<PlayerDashSystem>();
        WeaponChargeSystem weaponSystem = player.GetComponent<WeaponChargeSystem>();

        // --- 3. GUARDAR OS VALORES NORMAIS (Para não estragar o jogo) ---
        float regenDashOriginal = 0.5f;
        float tempoCargaOriginal = 1.5f;

        if (dashSystem != null) regenDashOriginal = dashSystem.regeneracaoPorSegundo;
        if (weaponSystem != null) tempoCargaOriginal = weaponSystem.tempoParaCarregar;

        // --- 4. APLICAR OS VALORES DE FÚRIA (BATOTA!) ⚡ ---
        if (dashSystem != null)
        {
            // Regenera 20 de stamina por segundo (basicamente infinito)
            dashSystem.regeneracaoPorSegundo = 20f;
        }

        if (weaponSystem != null)
        {
            // Carrega o super tiro em 0.1 segundos (instantâneo)
            weaponSystem.tempoParaCarregar = 0.1f;
        }

        // --- 5. ESPERAR A DURAÇÃO ---
        yield return new WaitForSeconds(duracaoFuria);

        // --- 6. VOLTAR AO NORMAL ---
        Debug.Log("Modo Fúria terminou.");
        if (dashSystem != null) dashSystem.regeneracaoPorSegundo = regenDashOriginal;
        if (weaponSystem != null) weaponSystem.tempoParaCarregar = tempoCargaOriginal;

        // Destruir o objeto do power-up para sempre
        Destroy(gameObject);
    }
}