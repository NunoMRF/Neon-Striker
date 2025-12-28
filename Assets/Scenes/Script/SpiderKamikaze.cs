using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SpiderKamikaze : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 7f;
    public float distanciaParaDetonar = 2.5f; // Quão perto tem de chegar para iniciar a bomba

    [Header("Explosão")]
    public float tempoDePavio = 1.5f; // Tempo a piscar antes de explodir
    public float raioDaExplosao = 4f; // Alcance do dano final
    public int danoMaximo = 40;
    public GameObject efeitoExplosao; // O VFX (podes usar o da Mina ou Turret)

    [Header("Sons")]
    public AudioClip somArmar;   // "Bip bip bip"
    public AudioClip somExplosao; // "BOOM"

    private Transform jogador;
    private NavMeshAgent agente;
    private bool modoBomba = false; // Para saber se já começou a contagem
    private Renderer corpoAranha;
    private Color corOriginal;

    void Start()
    {
        // Encontra o jogador automaticamente pela Tag
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) jogador = p.transform;

        agente = GetComponent<NavMeshAgent>();
        agente.speed = velocidade;

        // Guarda a cor original para poder piscar
        corpoAranha = GetComponentInChildren<Renderer>();
        if (corpoAranha != null) corOriginal = corpoAranha.material.color;
    }

    void Update()
    {
        if (jogador == null || modoBomba) return;

        float distancia = Vector3.Distance(transform.position, jogador.position);

        // 1. PERSEGUIR
        if (distancia > distanciaParaDetonar)
        {
            agente.SetDestination(jogador.position);
        }
        // 2. INICIAR AUTO-DESTRUIÇÃO
        else
        {
            StartCoroutine(SequenciaAutoDestruicao());
        }
    }

    IEnumerator SequenciaAutoDestruicao()
    {
        modoBomba = true;
        agente.isStopped = true; // Pára de andar
        agente.ResetPath(); // Garante que fica quieta

        // Toca som de aviso
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && somArmar != null) audio.PlayOneShot(somArmar);

        // EFEITO VISUAL: Piscar Vermelho rápido durante o tempo do pavio
        float timer = 0f;
        while (timer < tempoDePavio)
        {
            // Pisca entre Vermelho e a Cor Original muito rápido
            if (corpoAranha != null)
            {
                corpoAranha.material.color = (timer % 0.2f < 0.1f) ? Color.red : corOriginal;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        Explodir();
    }

    void Explodir()
    {
        // 1. Instancia o efeito visual
        if (efeitoExplosao != null)
        {
            Instantiate(efeitoExplosao, transform.position, Quaternion.identity);
        }

        // 2. Verifica se o jogador AINDA está no raio de explosão
        if (jogador != null)
        {
            float distanciaFinal = Vector3.Distance(transform.position, jogador.position);

            if (distanciaFinal <= raioDaExplosao)
            {
                // Dá dano
                VidaScript vida = jogador.GetComponent<VidaScript>();
                if (vida != null) vida.LevarDano(danoMaximo);
                Debug.Log("Aranha explodiu no jogador!");
            }
        }

        // 3. Som de explosão (se tiveres um objeto de som separado ou AudioSource 3D no prefab da explosão)
        // Se usares AudioSource.PlayClipAtPoint, o som toca mesmo depois do objeto morrer
        if (somExplosao != null) AudioSource.PlayClipAtPoint(somExplosao, transform.position);

        // 4. Morre
        Destroy(gameObject);
    }
}