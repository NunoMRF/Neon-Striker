using UnityEngine;

public class LaserAlarme : MonoBehaviour
{
    [Header("Configuração")]
    public int danoAoTocar = 10;
    public float duracaoDoAlarme = 10f;
    public AudioClip somAlarme;

    private AudioSource audioSource;
    private bool alarmeAtivo = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = true;

            // GARANTIA: O som normal começa em 3D (só se ouve perto)
            audioSource.spatialBlend = 1f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alarmeAtivo)
        {
            // 1. Tira Vida
            VidaScript vida = other.GetComponent<VidaScript>();
            if (vida != null) vida.LevarDano(danoAoTocar);

            // 2. Chama a Sirene Global
            DispararSirene();

            // 3. Mata o Laser Visual/Físico
            LaserIntermitente controle = GetComponent<LaserIntermitente>();
            if (controle != null)
            {
                controle.ForcarParagemTotal();
            }
        }
    }

    void DispararSirene()
    {
        if (audioSource != null && somAlarme != null)
        {
            alarmeAtivo = true;

            // --- O TRUQUE DE SOM GLOBAL ---
            // Muda o som para 2D (0) para se ouvir no mapa todo com o mesmo volume
            audioSource.spatialBlend = 0f;

            audioSource.clip = somAlarme;
            audioSource.Play();

            Debug.Log("🚨 ALARME GLOBAL! Toca por " + duracaoDoAlarme + "s");
            Invoke("PararSirene", duracaoDoAlarme);
        }
    }

    void PararSirene()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            // (Opcional) Se o laser voltasse a ligar, teríamos de voltar a pôr a spatialBlend a 1f
        }
    }
}