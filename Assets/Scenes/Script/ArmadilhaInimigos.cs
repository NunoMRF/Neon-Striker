using UnityEngine;

public class ArmadilhaInimigos : MonoBehaviour
{
    [Header("Configurações")]
    public GameObject inimigoPrefab; // O molde do inimigo (arraste o arquivo azul aqui)
    public Transform[] pontosDeSpawn; // Lista dos lugares onde eles nascem

    private bool armadilhaAtivada = false;

    // Esta função será chamada quando o Player tocar no Laser/Trigger
    void OnTriggerEnter(Collider other)
    {
        // Verifica se foi o Player e se a armadilha ainda não disparou
        if (other.CompareTag("Player") && !armadilhaAtivada)
        {
            AtivarEmboscada();
        }
    }

    void AtivarEmboscada()
    {
        armadilhaAtivada = true;
        Debug.Log("ALERTA! Inimigos chegando!");

        // Passa por cada ponto de spawn e cria um inimigo lá
        foreach (Transform ponto in pontosDeSpawn)
        {
            Instantiate(inimigoPrefab, ponto.position, ponto.rotation);
        }

        // Opcional: Destruir o gatilho/laser depois de usado
        // Destroy(gameObject); 
    }
}