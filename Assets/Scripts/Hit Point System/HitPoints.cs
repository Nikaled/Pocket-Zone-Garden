using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPoints : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHitPoints;
    [SerializeField] Image _hitPointBar;
    public event Action OnDied;
    public int CurrentHP
    {
        get
        {
            return _currentHP;
        }
        set
        {
            _currentHP = value;
            _hitPointBar.fillAmount =(float) _currentHP/_maxHitPoints;
        }
    }
    private int _currentHP;

    void Start()
    {
        _currentHP = _maxHitPoints;
    }
    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if(CurrentHP <= 0)
        {
            OnDied?.Invoke();
        }
    }
    public void SetHitPoints(int hp)
    {
        CurrentHP = hp;
    }
}
