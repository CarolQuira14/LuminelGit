using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Additional : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    public void AccessHurtPlayer(int value)
    {
        playerController.TakeDamage(value);
    }
    public void AccessHealPlayer(int value)
    {
        playerController.Heal(value);
    }

}
