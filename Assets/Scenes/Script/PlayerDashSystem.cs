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
    public float forcaDash = 20f;
    public float distanciaDash = 10f;
    public float duracaoDash = 0.2f;
    public KeyCode teclaDash = KeyCode.LeftShift;

    [Header("Efeito Visual (Câmara)")]
    public Camera camaraJogador;
    public float fovDash = 90f;
    public float velocidadeEfeito = 10f;

    [Header("Interface")]
    public Image[] barrasStamina;

    private Rigidbody rb;
    private CharacterController cc;
    private bool isDashing = false;
    private float fovOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();

        if (camaraJogador == null)
            camaraJogador = Camera.main;

        if (camaraJogador != null)
            fovOriginal = camaraJogador.fieldOfView;

        // 🔥 LIGAÇÃO AUTOMÁTICA À TUA UI REAL
        if (barrasStamina == null || barrasStamina.Length == 0)
        {
            GameObject staminaHolder = GameObject.Find("Stamina_Holder");

            if (staminaHolder != null)
            {
                barrasStamina = staminaHolder.GetComponentsInChildren<Image>();
                Debug.Log("PlayerDashSystem: Stamina ligada a Stamina_Holder.");
            }
            else
            {
                Debug.LogWarning("PlayerDashSystem: Não encontrei Stamina_Holder!");
            }
        }
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

        staminaAtual = Mathf.Clamp(staminaAtual, 0f, staminaMaxima);
    }

    IEnumerator FazerDash()
    {
        isDashing = true;
        staminaAtual -= 1f;

        StartCoroutine(MudarFOV(fovDash));

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(transform.forward * forcaDash, ForceMode.Impulse);
            yield return new WaitForSeconds(duracaoDash);
        }
        else if (cc != null)
        {
            float tempo = 0f;
            Vector3 direcao = transform.forward;

            while (tempo < duracaoDash)
            {
                cc.Move(direcao * (distanciaDash / duracaoDash) * Time.deltaTime);
                tempo += Time.deltaTime;
                yield return null;
            }
        }

        StartCoroutine(MudarFOV(fovOriginal));
        isDashing = false;
    }

    IEnumerator MudarFOV(float alvo)
    {
        if (camaraJogador == null) yield break;

        while (Mathf.Abs(camaraJogador.fieldOfView - alvo) > 0.5f)
        {
            camaraJogador.fieldOfView = Mathf.Lerp(
                camaraJogador.fieldOfView,
                alvo,
                Time.deltaTime * velocidadeEfeito
            );
            yield return null;
        }

        camaraJogador.fieldOfView = alvo;
    }

    void AtualizarBarrasUI()
    {
        if (barrasStamina == null || barrasStamina.Length == 0)
            return;

        for (int i = 0; i < barrasStamina.Length; i++)
        {
            if (barrasStamina[i] == null) continue;

            float valor = staminaAtual - i;
            barrasStamina[i].fillAmount = Mathf.Clamp01(valor);
        }
    }
}
