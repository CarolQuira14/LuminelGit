using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalizarJuego : MonoBehaviour
{
    private GameObject player;
    private GameObject canvasPersistente;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        canvasPersistente = GameObject.Find("Canvas Persistente");


        player.SetActive(false);
        canvasPersistente.SetActive(false);

        StartCoroutine(WaitToCredits());
    }

    // Update is called once per frame
    private IEnumerator WaitToCredits()
    {
        yield return new WaitForSecondsRealtime(4);

        SceneManager.LoadScene("Creditos");
    }
}
