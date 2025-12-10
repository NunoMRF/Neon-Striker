using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    [Header("Resistência")]
    public int vidaMaxima = 50;
    private int vidaAtual;
    private bool estaMorta = false; // Para não morrer 2 vezes seguidas

    [Header("Efeitos")]
    public GameObject explosaoVFX;
    public Animator anim; // O "cérebro" das animações

    void Start()
    {
        vidaAtual = vidaMaxima;

        // Se te esqueceres de arrastar, ele tenta apanhar sozinho
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public void ReceberDano(int dano)
    {
        if (estaMorta) return; 

        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            IniciarSequenciaDeMorte();
        }
    }

    void IniciarSequenciaDeMorte()
    {
        estaMorta = true;
        Debug.Log("A destruir Turret...");

        
        if (explosaoVFX != null)
        {
            Instantiate(explosaoVFX, transform.position, transform.rotation);
        }

        
        if (anim != null)
        {
            
            anim.Play("Turret_v1_deactivation");
        }

        
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        
        Destroy(gameObject, 3f);
    }
}