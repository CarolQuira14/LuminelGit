using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{
    public UnityEngine.UI.Image rellenoBarra;
    private PlayerController playerController;
    private float vidaMax;
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (playerController != null)
        {
            vidaMax = playerController.health;
        }
    }
    void Update()
    {
        rellenoBarra.fillAmount = playerController.health / vidaMax;
    }
}
