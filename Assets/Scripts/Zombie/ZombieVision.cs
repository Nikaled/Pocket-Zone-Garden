using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieVision : MonoBehaviour
{
    public event Action<GameObject> PlayerInVision; 
    public event Action PlayerOutOfVision; 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            PlayerInVision?.Invoke(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            PlayerOutOfVision?.Invoke();
        }
    }
}
