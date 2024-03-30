using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public event Action<List<GameObject>> ZombieInVision;
    public List<GameObject> ZombiesInVision = new List<GameObject>();
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Zombie>() != null)
        {
            ZombieInVision?.Invoke(ZombiesInVision);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Zombie>() != null)
        {
            ZombiesInVision.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Zombie>() != null)
        {
            ZombiesInVision.Remove(collision.gameObject) ;
        }
    }
}
