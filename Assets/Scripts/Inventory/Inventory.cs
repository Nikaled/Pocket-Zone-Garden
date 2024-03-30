using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Inventory
{
   public InventorySlot[] InventorySlots;
    private PlayerShootController _playerShootManager;
    private AmmoItem _currentAmmo;
    public Inventory (InventorySlot[] inventorySlots, PlayerShootController playerShootManager)
    {
        InventorySlots = inventorySlots;
        _playerShootManager = playerShootManager;
        ClearInventory();
        _playerShootManager.Shoot += DecreaseAmmo;
        _playerShootManager.Inventory = this;
    }
    public void AddItem(Item item)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i].IsEmpty == true)
            {
                InventorySlots[i].AddItem(item);
                if(_currentAmmo == null)
                {
                    if(item is AmmoItem)
                    {
                        _currentAmmo = (AmmoItem)item;
                    }
                }
                break;
            }
        }
    }
    public void AddExpendableItem(IExpendable item, int Count)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if(InventorySlots[i].IsEmpty == false)
            {
                if (InventorySlots[i].CurrentItem.GetType() == item.GetType())
                {
                    IExpendable ExItem = (IExpendable)InventorySlots[i].CurrentItem;
                    ExItem.IncreaseItemInStack(Count);
                    break;
                }
            }
        }
    }
    public void RemoveItem(InventorySlot slot)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i] == slot)
            {
                if (InventorySlots[i].CurrentItem == _currentAmmo)
                {
                    _currentAmmo = null;
                }
                InventorySlots[i].RemoveItem();
                break;
            }
        }
    }
    public bool IsAmmoInInventory()
    {
        if( _currentAmmo == null)
        {
            SetCurrentAmmoStack();
            if(_currentAmmo == null)
            {
                return false;
            }
        }
        return true;
    }
    private void SetCurrentAmmoStack()
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i].CurrentItem is AmmoItem)
            {
                _currentAmmo = (AmmoItem)InventorySlots[i].CurrentItem;
                break;
            }
        }
    }
    private void DecreaseAmmo()
    {
        _currentAmmo.DecreaseItemInStack(1);
        if(_currentAmmo.Count == 0)
        {
            _currentAmmo = null;
            SetCurrentAmmoStack();
        }
    }
    public void ClearInventory()
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlots[i].RemoveItem();
        }
    }

    public bool HasAvailableSlot()
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i].IsEmpty == true)
            {
                return true;
            }
        }
        return false;
    }
    public bool AlreadyHasAvailableExpandableSlot(IExpendable item)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i].IsEmpty == false)
            {
                if (InventorySlots[i].CurrentItem.GetType() == item.GetType())
                {
                    return true;
                }
            }
        }
        return false;
    }
}
