using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 51)]
public class ItemList : ScriptableObject
{
    public List<Sprite> ItemSprites;
}
