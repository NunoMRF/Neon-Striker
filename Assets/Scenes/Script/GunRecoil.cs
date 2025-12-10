using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Recuo")]
    public float recoilAmount = 0.05f;     // Quanto recua para trás
    public float recoilSpeed = 10f;        // Velocidade a recuar
    public float returnSpeed = 15f;        // Velocidade a voltar ao normal

    private Vector3 originalPosition;
    private Vector3 currentRecoilOffset;
    private bool isRecoiling = false;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isRecoiling)
        {
            // Aproxima-se do recuo máximo
            currentRecoilOffset = Vector3.Lerp(
                currentRecoilOffset,
                new Vector3(0, 0, -recoilAmount),
                Time.deltaTime * recoilSpeed
            );

            // Quando chega perto do recuo máximo, começa a voltar
            if (currentRecoilOffset.z < -recoilAmount * 0.9f)
                isRecoiling = false;
        }
        else
        {
            // Voltar à posição original
            currentRecoilOffset = Vector3.Lerp(
                currentRecoilOffset,
                Vector3.zero,
                Time.deltaTime * returnSpeed
            );
        }

        // Aplicar recuo à arma
        transform.localPosition = originalPosition + currentRecoilOffset;
    }

    public void ApplyRecoil()
    {
        isRecoiling = true;
    }
}
