using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendEnemyScript : MonoBehaviour
{
    public ParticleSystem SmokeEffect;
    Animator animator;
    bool broken;

    void Start()
    {
        broken = true;
        animator = GetComponent<Animator>();
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
        SmokeEffect.Stop();
    }
}
