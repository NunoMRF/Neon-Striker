using UnityEngine;
using System.Collections;

public class LaserIntermitente : MonoBehaviour
{
    [Header("Tempos")]
    public float tempoLigado = 5f;
    public float tempoDesligado = 3f;
    public float atrasoInicial = 0f;

    [Header("Componentes")]
    public Renderer visualDoLaser;
    public Collider colisorDoLaser;
    public AudioSource somDoLaser; // (Som do zumbido, não do alarme)
    public Light luzDoLaser;

    void Start()
    {
        // Tenta encontrar componentes se estiverem vazios
        if (visualDoLaser == null) visualDoLaser = GetComponent<Renderer>(); // Ou GetComponentInChildren se for filho
        if (colisorDoLaser == null) colisorDoLaser = GetComponent<Collider>();
        if (luzDoLaser == null) luzDoLaser = GetComponentInChildren<Light>();

        // Inicia o ciclo
        StartCoroutine(CicloDoLaser());
    }

    IEnumerator CicloDoLaser()
    {
        yield return new WaitForSeconds(atrasoInicial);
        while (true)
        {
            MudarEstado(true); // LIGA
            yield return new WaitForSeconds(tempoLigado);

            MudarEstado(false); // DESLIGA
            yield return new WaitForSeconds(tempoDesligado);
        }
    }

    // --- NOVA FUNÇÃO QUE O ALARME VAI CHAMAR ---
    public void ForcarParagemTotal()
    {
        StopAllCoroutines(); // Pára o relógio (impede que volte a ligar)
        MudarEstado(false);  // Desliga luzes e colisor agora mesmo
    }

    void MudarEstado(bool estado)
    {
        if (visualDoLaser != null) visualDoLaser.enabled = estado;
        if (colisorDoLaser != null) colisorDoLaser.enabled = estado;
        if (luzDoLaser != null) luzDoLaser.enabled = estado;

        // Controla o som do zumbido (se existir)
        if (somDoLaser != null && somDoLaser.clip != null)
        {
            if (estado && !somDoLaser.isPlaying) somDoLaser.Play();
            else if (!estado) somDoLaser.Stop();
        }
    }
}