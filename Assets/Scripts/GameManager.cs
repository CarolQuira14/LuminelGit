using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Inventario")]
    public Dictionary<string, bool> inventory = new Dictionary<string, bool>();

    [Header("Objetos Persistentes")]
    public GameObject[] persistentObjects;
    public GameObject gameoverPanel;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            CleanUp();
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        MarkPersistence();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Nivel1")
        {
            ResetGameState();
        }
    }

    public void ResetGameState()
    {
        inventory.Clear(); // Limpia inventario
        Time.timeScale = 1f; // Asegura que el tiempo esté normal
        if (gameoverPanel != null)
            gameoverPanel.SetActive(false);

        Debug.Log("GameManager reiniciado para nueva partida.");
    }

    public void AddToInventory(string item)
    {
        if (!inventory.ContainsKey(item))
        {
            inventory.Add(item, true);
            Debug.Log($"Item {item} added to inventory.");
        }
        else
        {
            Debug.Log($"Item {item} is already in inventory.");
        }
    }

    private void MarkPersistence()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
                DontDestroyOnLoad(obj);
        }
    }

    private void CleanUp()
    {
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }

    public void GameOver()
    {
        if (gameoverPanel != null)
            gameoverPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        if (gameoverPanel != null)
            gameoverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}