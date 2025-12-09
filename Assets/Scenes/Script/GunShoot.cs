using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [Header("Raycast")]
    public Camera playerCamera;
    public float range = 100f;
    public int damage = 20;

    [Header("Efeitos")]
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect; // opcional – podes deixar vazio

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Ativar o muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Atingiste: " + hit.transform.name);

            // Aplicar dano se tiver script de vida
            VidaScript vida = hit.transform.GetComponent<VidaScript>();
            if (vida != null)
            {
                vida.LevarDano(damage);
            }

            // Criar efeito no ponto de impacto
            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
