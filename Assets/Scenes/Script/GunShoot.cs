using UnityEngine;
using System.Collections;
using TMPro;

public class GunShoot : MonoBehaviour
{
    [Header("Referências")]
    public Camera playerCamera;
    public GunRecoil recoil;

    [Header("Tiro e Dano")]
    public float range = 100f;
    public int damage = 20;
    public float fireRate = 0.15f;

    
    [Header("Efeitos Visuais")]
    public GameObject hitEffect;

    private float nextTimeToShoot = 0f;

    [Header("Munição")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        if (reloadText != null)
            reloadText.enabled = false;
    }

    void Update()
    {
        if (isReloading)
            return;

       
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

       
        if (currentAmmo <= 0)
            return;

        
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        
        if (recoil != null)
            recoil.ApplyRecoil();

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Atingiste: " + hit.transform.name);

            
            VidaScript vida = hit.transform.GetComponent<VidaScript>();
            if (vida != null)
            {
                vida.LevarDano(damage);
            }

            // 2. Tenta tirar vida à Turret (NOVO)
            TurretHealth turret = hit.transform.GetComponent<TurretHealth>();
            if (turret != null)
            {
                turret.ReceberDano(damage);
            }

            
            if (hitEffect != null)
            {
                
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        UpdateAmmoUI();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("A recarregar...");

        if (reloadText != null)
            reloadText.enabled = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

        if (reloadText != null)
            reloadText.enabled = false;

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}