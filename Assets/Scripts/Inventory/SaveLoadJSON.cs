using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerData
{
    public Vector3 position;
    public int HitPoints;
}
public class SerializabileItem
{
    public string ItemType;
    public int Count;
    public int SpriteIndexInItemList;
}
public class SerializabileSlots
{
    public SerializabileSlots(int Length)
    {
        ItemTypes = new string[Length];
        Count = new int[Length];
        SpriteIndexInItemList = new int[Length];
    }
    public string[] ItemTypes;
    public int[] Count;
    public int[] SpriteIndexInItemList;
}

public class SaveLoadJSON
{
    PlayerData playerData;
    string saveInventoryFilePath;
    string savePlayerDataFilePath;
    private Inventory _inventory;
    private ItemList _itemList;
    private PlayerMovement _playerMovement;
    private HitPoints _playerHitPoints;

    public SaveLoadJSON(Inventory inventory, ItemList itemList, Button SaveButton, Button LoadButton, PlayerMovement playerMovement)
    {
        _inventory = inventory;
        _itemList = itemList;
        _playerMovement = playerMovement;
        _playerHitPoints = _playerMovement.HitPoints;
        saveInventoryFilePath = Application.persistentDataPath + "/InventoryData.json";
        savePlayerDataFilePath = Application.persistentDataPath + "/PlayerData.json";
        SaveButton.onClick.AddListener(SaveGame);
        LoadButton.onClick.AddListener(LoadGame);
        playerData = new PlayerData();
    }
    private void SaveGame()
    {
        SaveItemSlots();
        SavePlayerData();
    }
    private void LoadGame()
    {
        LoadSlots();
        LoadPlayerData();
    }
    private void LoadPlayerData()
    {
        if (File.Exists(savePlayerDataFilePath))
        {
            string loadPlayerData = File.ReadAllText(savePlayerDataFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
            _playerMovement.transform.position = playerData.position;
            _playerHitPoints.SetHitPoints(playerData.HitPoints);
        }
    }
    private void SavePlayerData()
    {
        playerData.position = _playerMovement.transform.position;
        playerData.HitPoints = _playerHitPoints.CurrentHP;
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(savePlayerDataFilePath, savePlayerData);
    }
    private void SaveItemSlots()
    {
        List<SerializabileItem> SetItemSlots = new();
        for (int i = 0; i < _inventory.InventorySlots.Length; i++)
        {
            if(_inventory.InventorySlots[i].CurrentItem !=null)
            SetItemSlots.Add(SaveItem(_inventory.InventorySlots[i].CurrentItem, i));
        }
        SerializabileSlots  SerSlots = new SerializabileSlots(SetItemSlots.Count);
        for (int i = 0; i < SetItemSlots.Count; i++)
        {
            SerSlots.ItemTypes[i] = SetItemSlots[i].ItemType;
            SerSlots.Count[i] = SetItemSlots[i].Count;
            SerSlots.SpriteIndexInItemList[i] = SetItemSlots[i].SpriteIndexInItemList;
        }
        SaveSlots(SerSlots);
    }
    public void SaveSlots(SerializabileSlots SerSlots)
    {
        string savePlayerData = JsonUtility.ToJson(SerSlots);
        File.WriteAllText(saveInventoryFilePath, savePlayerData);
    }
    public void LoadSlots()
    {
        if (File.Exists(saveInventoryFilePath))
        {
            _inventory.ClearInventory();
            string loadPlayerData = File.ReadAllText(saveInventoryFilePath);
            SerializabileSlots SerSlots = JsonUtility.FromJson<SerializabileSlots>(loadPlayerData);
            List<SerializabileItem> SetItemSlots = new();
            for (int i = 0; i < SerSlots.Count.Length; i++)
            {
                SerializabileItem SerItem = new SerializabileItem();
                SerItem.SpriteIndexInItemList = SerSlots.SpriteIndexInItemList[i];
                SerItem.Count = SerSlots.Count[i];
                SerItem.ItemType = SerSlots.ItemTypes[i];
                SetItemSlots.Add(SerItem);
            }

            for (int i = 0; i < SetItemSlots.Count; i++)
            {
                _inventory.AddItem(PrepareToDeserealizeitem(SetItemSlots[i]));
            }


        }
    }
    private SerializabileItem PrepareToSerializeItem(Item item)
    {
        SerializabileItem SlotIoSerialize = new SerializabileItem();
        SlotIoSerialize.ItemType = item.GetType().ToString();
        if(item is IExpendable expendable)
        {
            SlotIoSerialize.Count = expendable.Count;
        }
        SlotIoSerialize.SpriteIndexInItemList = SetObjectIndexBySprite(item.Icon);
        return SlotIoSerialize;
    }
    private Item PrepareToDeserealizeitem(SerializabileItem SerItem)
    {
        Type itemType = Type.GetType(SerItem.ItemType);
        var item = Activator.CreateInstance(itemType);
        if(item is IExpendable expendable)
        {
            expendable.Count = SerItem.Count;
        }
        if(item is Item NewItem)
        {
            NewItem.Icon = SetSpriteByIndex(SerItem.SpriteIndexInItemList);
        }
        Item ItemToReturn = (Item)item;
        return ItemToReturn;

    }
    private SerializabileItem SaveItem(Item itemToSave, int index)
    {
         Item item = itemToSave;
        var s_item = PrepareToSerializeItem(item);
        return s_item;
    }
    private int SetObjectIndexBySprite(Sprite sprite)
    {
        for (int i = 0; i < _itemList.ItemSprites.Count; i++)
        {
            if (_itemList.ItemSprites[i] == sprite)
                return i;
        }
        return -1;
    }
    private Sprite SetSpriteByIndex(int index)
    {
        for (int i = 0; i < _itemList.ItemSprites.Count; i++)
        {
            if (i == index)
                return _itemList.ItemSprites[i];
        }
        return null;
    }


    public void DeleteSaveFile()
    {
        if (File.Exists(saveInventoryFilePath))
        {
            File.Delete(saveInventoryFilePath);

            Debug.Log("Save file deleted!");
        }
        else
            Debug.Log("There is nothing to delete!");
        if (File.Exists(savePlayerDataFilePath))
        {
            File.Delete(savePlayerDataFilePath);

            Debug.Log("Save file deleted!");
        }
        else
            Debug.Log("There is nothing to delete!");
    }
}