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
    public GameObject hitEffect; // O efeito quando a bala bate na parede/inimigo

    // --- NOVO: Variáveis para o Clarão do Tiro ---
    public GameObject muzzleFlashPrefab; // O efeito do WarFX (RIFLE1)
    public Transform muzzlePointLocation; // O objeto vazio na ponta da arma
    // ---------------------------------------------

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

        // --- NOVO: Criar o Muzzle Flash (Clarão) ---
        if (muzzleFlashPrefab != null && muzzlePointLocation != null)
        {
            // Cria o efeito na ponta da arma
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePointLocation.position, muzzlePointLocation.rotation);

            // (Opcional) Faz o efeito ser "filho" da arma para se mexer com ela se estiveres a andar
            flash.transform.SetParent(muzzlePointLocation);

            // Destrói o efeito passados 0.5 segundos para não encher o jogo de lixo
            Destroy(flash, 0.5f);
        }
        // -------------------------------------------

        if (recoil != null)
            recoil.ApplyRecoil();

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Atingiste: " + hit.transform.name);

            // Tenta tirar vida a inimigos normais
            VidaScript vida = hit.transform.GetComponent<VidaScript>();
            if (vida != null)
            {
                vida.LevarDano(damage);
            }

            // Tenta tirar vida à Turret
            // Usei GetComponentInParent para garantir que acerta mesmo que batas num collider filho
            TurretHealth turret = hit.transform.GetComponentInParent<TurretHealth>();
            if (turret != null)
            {
                turret.ReceberDano(damage);
            }

            // Cria o efeito de impacto (onde a bala bateu)
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

        // Ativa o texto de "Reloading..." se existir
        if (reloadText != null)
            reloadText.enabled = true;

        // --- INÍCIO DA ANIMAÇÃO ---

        // 1. Guarda a posição original da arma para não a perdermos
        Quaternion anguloOriginal = transform.localRotation;

        // 2. Roda a arma 45 graus para baixo (simula baixar a arma para meter o pente)
        // Usamos 'localRotation' para ser relativo à câmara
        transform.localRotation = Quaternion.Euler(20f, 0f, 0f);

        // --------------------------

        // Espera o tempo definido (ex: 1.5 segundos) com a arma em baixo
        yield return new WaitForSeconds(reloadTime);

        // --- FIM DA ANIMAÇÃO ---

        // 3. Volta a pôr a arma na posição de tiro
        transform.localRotation = anguloOriginal;

        // -----------------------

        // Enche a munição
        currentAmmo = maxAmmo;
        isReloading = false;

        // Esconde o texto
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