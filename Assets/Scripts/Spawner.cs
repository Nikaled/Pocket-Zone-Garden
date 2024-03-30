using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    [SerializeField] GameObject[] _spawnObjects;
    [SerializeField] GameObject _zombiePrefab;
    [SerializeField] int _enemyCount = 3;
    private Inventory _inventory;

    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }
    private List<GameObject> GenerateEnemyOnRandomPlace(BoundsInt bounds)
    {
        List<GameObject>  enemyList = new List<GameObject>();
        for (int i = 0; i < _enemyCount; i++)
        {
            float XPos = UnityEngine.Random.Range(bounds.xMin, bounds.xMax);
            float YPos = UnityEngine.Random.Range(bounds.yMin, bounds.yMax);
            enemyList.Add(Instantiate(_zombiePrefab, new Vector3(XPos, YPos, 0), Quaternion.identity));
        }
        return  enemyList;
    }
    private void Start()
    {
        _tilemap.CompressBounds();
        BoundsInt bounds = _tilemap.cellBounds;
       List<GameObject> Enemies =  GenerateEnemyOnRandomPlace(bounds);
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].GetComponent<Zombie>().OnDead += CreateRandomItem;
        }
    }
    private void CreateRandomItem(GameObject DeadZombie)
    {
        GameObject RandomItemPrefab = Instantiate(_spawnObjects[UnityEngine.Random.Range(0, _spawnObjects.Length)], DeadZombie.transform.position, Quaternion.identity);
        RandomItemPrefab.GetComponent<ItemInGame>().inventory = _inventory;
        DeadZombie.GetComponent<Zombie>().OnDead -= CreateRandomItem;
    }
}
