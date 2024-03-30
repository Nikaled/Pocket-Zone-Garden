using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExpendable 
{
    int Count { get; set; }
    void IncreaseItemInStack(int IncreaseCount);
    void DecreaseItemInStack(int DecreaseCount);
    event Action<int> ItemCountChanged;
}
