using UnityEngine;
using UnityEngine.UI;

public class WeaponChargeSystem : MonoBehaviour
{
    [Header("Configuração da Carga")]
    public float tempoParaCarregar = 1.5f;
    public float velocidadeTiro = 40f;

    [Header("Referências")]
    public GameObject balaFortePrefab;
    public Transform canoDaArma;
    public Image barraDeCargaUI;

    [Header("Efeitos Visuais (VFX)")]
    // ARRASTA AQUI o Particle System que criaste (se tiveres)
    public ParticleSystem particulasCarga;
    // ARRASTA AQUI a Luz que criaste
    public Light luzCarga;

    private float cargaAtual = 0f;
    private float intensidadeLuzOriginal; // Para guardar o brilho máximo da luz

    void Start()
    {
        if (barraDeCargaUI != null) barraDeCargaUI.fillAmount = 0f;

        // Configuração inicial dos efeitos
        if (luzCarga != null)
        {
            intensidadeLuzOriginal = luzCarga.intensity; // Guarda o valor que puseste no Inspector
            luzCarga.intensity = 0f; // Começa apagada
            luzCarga.enabled = false;
        }
        if (particulasCarga != null) particulasCarga.Stop();
    }

    void Update()
    {
        InputTiroCarregado();
    }

    void InputTiroCarregado()
    {
        // 1. AO CARREGAR NO BOTÃO (Início)
        if (Input.GetMouseButtonDown(1))
        {
            // Liga os efeitos
            if (particulasCarga != null) particulasCarga.Play();
            if (luzCarga != null) luzCarga.enabled = true;
        }

        // 2. ENQUANTO SEGURA O BOTÃO (A carregar)
        if (Input.GetMouseButton(1))
        {
            cargaAtual += Time.deltaTime;

            // Calcula a percentagem da carga (de 0 a 1)
            float progresso = Mathf.Clamp01(cargaAtual / tempoParaCarregar);

            // Atualiza a barra UI
            if (barraDeCargaUI != null) barraDeCargaUI.fillAmount = progresso;

            // Atualiza a intensidade da Luz (fica mais forte com o tempo)
            if (luzCarga != null)
            {
                // Mathf.Lerp faz a luz ir de 0 até ao máximo suavemente
                luzCarga.intensity = Mathf.Lerp(0f, intensidadeLuzOriginal, progresso);
            }
        }

        // 3. AO SOLTAR O BOTÃO (Disparo)
        if (Input.GetMouseButtonUp(1))
        {
            if (cargaAtual >= tempoParaCarregar)
            {
                DispararSuperTiro();
            }

            ResetarCarga();
        }
    }

    void ResetarCarga()
    {
        cargaAtual = 0f;
        if (barraDeCargaUI != null) barraDeCargaUI.fillAmount = 0f;

        // Desliga os efeitos
        if (particulasCarga != null) particulasCarga.Stop();
        if (luzCarga != null)
        {
            luzCarga.enabled = false;
            luzCarga.intensity = 0f;
        }
    }

    void DispararSuperTiro()
    {
        if (balaFortePrefab == null || canoDaArma == null) return;

        // Cria a bala ligeiramente à frente para não bater na própria arma
        Vector3 pontoSpawn = canoDaArma.position + canoDaArma.forward * 0.5f;
        GameObject bala = Instantiate(balaFortePrefab, pontoSpawn, canoDaArma.rotation);

        Rigidbody rb = bala.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = canoDaArma.forward * velocidadeTiro;
        }
        Destroy(bala, 5f);
    }
}