using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmmoCollectible : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        controller.ChangeAmmo(1);
        Destroy(gameObject);
    }
}