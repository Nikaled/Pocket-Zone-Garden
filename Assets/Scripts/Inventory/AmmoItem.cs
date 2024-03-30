using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : Item, IExpendable
{
    public event Action<int> ItemCountChanged;
    public int Count { get; set; }

    public void IncreaseItemInStack(int AddedCount)
    {
        Count += AddedCount;
        ItemCountChanged?.Invoke(Count);
    }

    public void DecreaseItemInStack(int RemovedCount)
    {
        Count -= RemovedCount;
        ItemCountChanged?.Invoke(Count);
    }
}
