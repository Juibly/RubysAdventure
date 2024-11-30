using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedCollectible : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        controller.ChangeSpeed(1);
        Destroy(gameObject);
    }
}