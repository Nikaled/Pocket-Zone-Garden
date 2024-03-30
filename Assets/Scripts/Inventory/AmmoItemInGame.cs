using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItemInGame : ItemInGame
{
    public int CountOfItem;
    protected override void Start()
    {
        AmmoItem ammo = new AmmoItem();
        ammo.Count = CountOfItem;
        ammo.Icon = GetComponent<SpriteRenderer>().sprite;
        CreatedItem = ammo;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            var expendable = (IExpendable)CreatedItem;
                if (inventory.AlreadyHasAvailableExpandableSlot(expendable))
                {
                    inventory.AddExpendableItem(expendable, expendable.Count);
                    Destroy(gameObject);
                }
                else
                {
                    if (inventory.HasAvailableSlot())
                    {
                        inventory.AddItem(CreatedItem);
                        Destroy(gameObject);
                    }
                }
        }
    }
}
