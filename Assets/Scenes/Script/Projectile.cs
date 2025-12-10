using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 40f;   
    public float lifeTime = 2f; 
    public int damage = 10;    

    void Start()
    {
      
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            
            VidaScript vida = other.GetComponent<VidaScript>();

            // Se encontrou o script, aplica o dano
            if (vida != null)
            {
                vida.LevarDano(damage); // Chama a função do teu colega
                Debug.Log("Acertei no Player! Vida restante: " + vida.vidaAtual);
            }

            // A bala destrói-se sempre que acerta no jogador
            Destroy(gameObject);
        }
        
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}