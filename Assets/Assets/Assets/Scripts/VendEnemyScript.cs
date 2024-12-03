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

    public void Fixv()

    {
        broken = false;
        animator.SetTrigger("Fixed");
        GetComponent<Rigidbody2D>().simulated = false;
        SmokeEffect.Stop();
        gameObject.tag = ("fixedBot");
    }
}
