using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootController
{
    public Inventory Inventory;
    private PlayerVision _playerVision;
    private PlayerMovement _playerMovement;
    private GameObject _currentZombieToShoot;
    private int _shootDamage;

    public PlayerShootController(Button shootButton, PlayerVision playerVision, PlayerMovement playerMovement, int ShootDamage)
    {
        _playerVision = playerVision;
        _playerVision.ZombieInVision += FindCorrectTargetToShoot;
        shootButton.onClick.AddListener(ShootButton);
        _playerMovement = playerMovement;
        _shootDamage = ShootDamage;
    }
    public event Action Shoot;
    public void ShootButton()
    {
        if (IsShootAvailable() && _currentZombieToShoot !=null)
        {
            Shoot?.Invoke();
            ShootZombie();
        }
        else
        {
            Debug.Log("No Ammo or enemies");
        }
    }
    private void ShootZombie()
    {
        _currentZombieToShoot.GetComponent<IDamageable>()?.TakeDamage(_shootDamage);
    }
    private bool IsShootAvailable()
    {
        return Inventory.IsAmmoInInventory();
    }
    private void FindCorrectTargetToShoot(List<GameObject> Zombies)
    {
        if(Zombies.Count == 0)
        {
            _currentZombieToShoot = null;
            return;
        }
       List<GameObject> CorrectZombies =  CheckZombieIsInVision(Zombies);
        if(CorrectZombies.Count == 0) {
            _currentZombieToShoot = null;
            return; }
        _currentZombieToShoot =  CalculateNearestZombie(CorrectZombies);
    }
    private List<GameObject> CheckZombieIsInVision(List<GameObject> Zombies)
    {
        List<GameObject> AvailableZombies = new List<GameObject>();
        if (_playerMovement.CharacterFlippedLeft == false)
        {
            for (int i = 0; i < Zombies.Count; i++)
            {
                if (Zombies[i] != null)
                {
                    if (Zombies[i].transform.position.x > _playerMovement.transform.position.x)
                    {
                        AvailableZombies.Add(Zombies[i]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < Zombies.Count; i++)
            {
                if (Zombies[i] != null)
                {
                    if (Zombies[i].transform.position.x < _playerMovement.transform.position.x)
                    {
                        AvailableZombies.Add(Zombies[i]);
                    }
                }
            }
        }
        return AvailableZombies;

    }
    private GameObject CalculateNearestZombie(List<GameObject> Zombies)
    {
        GameObject NearestZombie = Zombies[0];
        float Distance = Vector3.Distance(Zombies[0].transform.position, _playerMovement.transform.position);
        for (int i = 0; i < Zombies.Count; i++)
        {
            if (Zombies[i] != null)
            {
                 float ThisZombieDistance = Vector3.Distance(Zombies[i].transform.position, _playerMovement.transform.position);
                if (ThisZombieDistance < Distance)
                {
                    NearestZombie = Zombies[i];
                    Distance = ThisZombieDistance;
                }
            }
        }
        return NearestZombie;
    }
}

