using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;

public class HealItem : MonoBehaviour
{


    [SerializeField]
    int healStrength = 1;
    


    void OnTriggerEnter2D(Collider2D collider)
    {
        Collide(collider);
    }


    void Collide(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerController>().Heal(healStrength);
            Destroy(gameObject);
        }
    }
}
