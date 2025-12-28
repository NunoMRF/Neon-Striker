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

    [Header("Power Up")]
    private float damageMultiplier = 1f;
    private Coroutine damageCoroutine;

    [Header("Efeitos Visuais")]
    public GameObject hitEffect;
    public GameObject muzzleFlashPrefab;
    public Transform muzzlePointLocation;

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

        if (muzzleFlashPrefab != null && muzzlePointLocation != null)
        {
            GameObject flash = Instantiate(
                muzzleFlashPrefab,
                muzzlePointLocation.position,
                muzzlePointLocation.rotation
            );

            flash.transform.SetParent(muzzlePointLocation);
            Destroy(flash, 0.5f);
        }

        if (recoil != null)
            recoil.ApplyRecoil();

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            int finalDamage = Mathf.RoundToInt(damage * damageMultiplier);

            VidaScript vida = hit.transform.GetComponent<VidaScript>();
            if (vida != null)
            {
                vida.LevarDano(finalDamage);
            }

            TurretHealth turret = hit.transform.GetComponentInParent<TurretHealth>();
            if (turret != null)
            {
                turret.ReceberDano(finalDamage);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        UpdateAmmoUI();
    }

    // ---------------- POWER UP ----------------

    public void ActivateDamageBoost(float duration)
    {
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(DamageBoostRoutine(duration));
    }

    private IEnumerator DamageBoostRoutine(float duration)
    {
        damageMultiplier = 2f;
        yield return new WaitForSeconds(duration);
        damageMultiplier = 1f;
    }

    // ------------------------------------------

    IEnumerator Reload()
    {
        isReloading = true;

        if (reloadText != null)
            reloadText.enabled = true;

        Quaternion originalRotation = transform.localRotation;
        transform.localRotation = Quaternion.Euler(20f, 0f, 0f);

        yield return new WaitForSeconds(reloadTime);

        transform.localRotation = originalRotation;

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
