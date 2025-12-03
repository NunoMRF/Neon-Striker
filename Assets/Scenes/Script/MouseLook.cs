using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 120f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        // Lock do cursor ao centro e invisível
        Cursor.lockState = CursorLockMode.Locked;

        // Se o playerBody não estiver preenchido, assume o parent
        if (playerBody == null)
            playerBody = transform.parent;
    }

    void Update()
    {
        // Movimento do rato
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Movimento da Câmera (olhar para cima/baixo)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rodar o corpo do player (esquerda/direita)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
