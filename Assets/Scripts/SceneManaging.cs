using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManaging : MonoBehaviour
{
    private Animator camAnim;
    private Animator transitionAnim;
    private GameObject transition;
    void Start()
    {
        camAnim = GetComponent<Animator>();
    }
    public void LoadScene(string sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void StopCasaAnim()
    {
        camAnim.SetBool("casita", false);
    }

    public void AnimationTransEnded(){
        transition = GameObject.Find("Transcicsion");
        transitionAnim = transition.GetComponent<Animator>();
        transitionAnim.SetTrigger("finalizado");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been closed");
    }
}
