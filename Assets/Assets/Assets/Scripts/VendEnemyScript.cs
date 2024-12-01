using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class VendEnemyScript : MonoBehaviour
{
    //public variables
    public float speed;
    public float changeTime;
    public bool vertical;
    public ParticleSystem SmokeEffect;
    public GameObject projectilePrefab;

    //private variables
    Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = 1;
    bool broken = true;
    int rotate;
    Vector2 moveDirection = new Vector2(1, 0);

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
        //something to make rotate change vertical, i tried but it kept not working how i wanted
        //thinking using a random number generator and if random is 0 vertical true and 1 vertical false but it may break it like having an if statement and a counter did earlier for me
    }

    private void Update()
    {
        if(!broken)
        {
            return;
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        timer -= Time.deltaTime;

        
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
            //if (rotate > 0) { vertical = true; }
            //else { vetical = false; }
        }
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", -direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();


        if (controller != null)
        {
            if (broken == true)
            {
                controller.ChangeHealth(-1);
            }
        }
    }

    //attack (call launch) script can be triggered when player walks in the colliders (named detection zone) for the vending machine
    //moved detection zones over to water out of way since they block the player from shooting projectiles for some reason
    // vector move direction to face whatever direction moving possibly ? 

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);


        animator.SetTrigger("Launch");
    }

    public void Fixv()

    {
        broken = false;
        animator.SetTrigger("Fixed");
        GetComponent<Rigidbody2D>().simulated = false;
        SmokeEffect.Stop();
        gameObject.tag = ("fixedBot");
    }
}
