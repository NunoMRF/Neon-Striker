using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensibilidadeX = 120f;
    public float sensibilidadeY = 120f;

    public Transform playerBody;   // onde roda no eixo Y (o Player)

    float rotacaoX = 0f;           // emoção vertical da camera

    private void Start()
    {        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerBody == null)
            playerBody = transform.parent;

        // RESETAR rotações iniciais
        transform.localRotation = Quaternion.identity;
        playerBody.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeY * Time.deltaTime;

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -85f, 85f);

        // Rotação vertical da camera (apenas eixo X)
        transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);

        // Rotação horizontal do Player (apenas Y)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
