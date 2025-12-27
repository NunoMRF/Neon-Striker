using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDashSystem : MonoBehaviour
{
    [Header("Configuração da Stamina")]
    public float staminaAtual = 3f;
    public float staminaMaxima = 3f;
    public float regeneracaoPorSegundo = 0.5f;

    [Header("Configuração do Dash")]
    public float forcaDash = 20f;      // Para Rigidbody
    public float distanciaDash = 10f;  // Para CharacterController
    public float duracaoDash = 0.2f;   // Duração do movimento
    public KeyCode teclaDash = KeyCode.LeftShift;

    [Header("Efeito Visual (Câmara)")]
    public Camera camaraJogador;       // ARRASTA A CÂMARA PARA AQUI
    public float fovDash = 90f;        // O FOV durante o dash (Efeito "Tunel")
    public float velocidadeEfeito = 10f; // Quão rápido a câmara estica/encolhe

    [Header("Interface")]
    public Image[] barrasStamina;

    private Rigidbody rb;
    private CharacterController cc;
    private bool isDashing = false;
    private float fovOriginal; // Guarda o valor normal da tua câmara

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();

        // Se te esqueceres de meter a câmara, ele tenta encontrar a MainCamera sozinho
        if (camaraJogador == null) camaraJogador = Camera.main;

        // Guarda o FOV que tinhas no início (ex: 60) para poder voltar a ele
        if (camaraJogador != null) fovOriginal = camaraJogador.fieldOfView;
    }

    void Update()
    {
        RegenerarStamina();
        AtualizarBarrasUI();

        if (Input.GetKeyDown(teclaDash) && staminaAtual >= 1f && !isDashing)
        {
            StartCoroutine(FazerDash());
        }
    }

    void RegenerarStamina()
    {
        if (staminaAtual < staminaMaxima)
        {
            staminaAtual += regeneracaoPorSegundo * Time.deltaTime;
        }
        if (staminaAtual > staminaMaxima) staminaAtual = staminaMaxima;
    }

    IEnumerator FazerDash()
    {
        isDashing = true;
        staminaAtual -= 1f;

        // --- 1. INÍCIO DO EFEITO VISUAL ---
        // Começa a esticar a câmara em paralelo (coroutine separada)
        StartCoroutine(MudarFOV(fovDash));

        // --- 2. MOVIMENTO FÍSICO ---
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(transform.forward * forcaDash, ForceMode.Impulse);
            yield return new WaitForSeconds(duracaoDash);
        }
        else if (cc != null)
        {
            float tempoPassado = 0f;
            Vector3 direcaoDash = transform.forward;

            while (tempoPassado < duracaoDash)
            {
                cc.Move(direcaoDash * (distanciaDash / duracaoDash) * Time.deltaTime);
                tempoPassado += Time.deltaTime;
                yield return null;
            }
        }

        // --- 3. FIM DO DASH ---
        // Volta a câmara ao normal
        StartCoroutine(MudarFOV(fovOriginal));

        isDashing = false;
    }

    // Esta função trata de suavizar a mudança da câmara
    IEnumerator MudarFOV(float alvo)
    {
        if (camaraJogador == null) yield break;

        // Enquanto o FOV não estiver quase igual ao alvo...
        while (Mathf.Abs(camaraJogador.fieldOfView - alvo) > 0.5f)
        {
            // ...aproxima o valor suavemente (Lerp)
            camaraJogador.fieldOfView = Mathf.Lerp(camaraJogador.fieldOfView, alvo, Time.deltaTime * velocidadeEfeito);
            yield return null;
        }
        // Garante que fica exatamente no valor final
        camaraJogador.fieldOfView = alvo;
    }

    void AtualizarBarrasUI()
    {
        for (int i = 0; i < barrasStamina.Length; i++)
        {
            float valor = staminaAtual - i;
            barrasStamina[i].fillAmount = Mathf.Clamp01(valor);
        }
    }
}