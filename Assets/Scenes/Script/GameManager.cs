using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Arrasta os Painéis aqui")]
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;

    public bool jogoAcabou = false;
    private bool jogoPausado = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (jogoAcabou) return;

        // Tecla P para Pausar
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (jogoPausado)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Tempo normal
        jogoPausado = false;

        Cursor.lockState = CursorLockMode.Locked; // Esconde o rato
        Cursor.visible = false;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Para o tempo
        jogoPausado = true;

        Cursor.lockState = CursorLockMode.None; // Mostra o rato
        Cursor.visible = true;
    }

    public void GameOver()
    {
        if (jogoAcabou) return;

        jogoAcabou = true;
        gameOverUI.SetActive(true);

        Time.timeScale = 0f; // Para tudo
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // O tempo tem de andar para carregar a cena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }
}