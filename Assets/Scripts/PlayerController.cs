using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    [Header("Player Settings")]
    public int health = 3;
    public GameManager gameManager;
    private bool isDead = false;
 
    //[SerializeField] private AudioClip damageSound;

    /*[Header("Movement Settings")]
    private Vector2 target;
    [SerializeField] private float moveSpeed = 200f;
    public Camera mainCamera;*/

    void Start()
    {
        /*mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        target = transform.position;*/
        anim = GetComponent<Animator>();
     }

    /* void Update()
     {
         Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

         if (Input.GetMouseButtonDown(0) && !isDead)
         {
             target = new Vector2(mousePosition.x, mousePosition.y);
         }

         transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
     }*/
    public void Heal(int amount)
    {
        anim.SetBool("Heal", true);
        health += amount;
        AudioManager.Instance.PlayEfecto("SonidoHeal");
        if (health > 3)
        {
            health = 3;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetBool("Hurt", true);
        AudioManager.Instance.PlayEfecto("SonidoHurt");
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }
    public void StopHurtAnimation()
    {
        anim.SetBool("Hurt", false);
    }
    public void StopHealAnimation()
    {
        anim.SetBool("Heal", false);
    }
    void Die()
    {
        Debug.Log("Player has died.");
        gameManager.GameOver();
        health = 3; // Reset health for the next game
        isDead = false; // Reset death state for the next game
    }
}
