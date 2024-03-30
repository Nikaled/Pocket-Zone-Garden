using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Zenject;
using static UnityEditor.Progress;
public class ItemInGame : MonoBehaviour
{
    public Inventory inventory;
    protected Item CreatedItem;

    [Inject]public void Construct(Inventory inventory)
    {
        this.inventory = inventory;
    }
    protected virtual void Start()
    {
        CreatedItem = new Item();
        CreatedItem.Icon = GetComponent<SpriteRenderer>().sprite;
    }
    protected  virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            if (inventory.HasAvailableSlot())
            {
                inventory.AddItem(CreatedItem);
                Destroy(gameObject);
            }

        }
    }
}
