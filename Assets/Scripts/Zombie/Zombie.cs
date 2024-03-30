using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Zombie : MonoBehaviour
{
    public event Action<GameObject> OnDead;
    [SerializeField] float _speedModifier;
    [SerializeField] float _attackInterval;
    [SerializeField] int _damagePerAttack;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] ZombieVision _zombieVision;
    [SerializeField] GameObject _zombieParts;
    [SerializeField] private HitPoints _hitPointSystem;
    private bool _characterFlippedLeft;
    private bool _attackIsOnCooldown;

    private void OnEnable()
    {
        _hitPointSystem.OnDied += UnitDied;
        _zombieVision.PlayerInVision += MoveToPlayer;
        _zombieVision.PlayerOutOfVision += StopMoving;
    }
    private void OnDisable()
    {
        _hitPointSystem.OnDied -= UnitDied;
        _zombieVision.PlayerInVision -= MoveToPlayer;
        _zombieVision.PlayerOutOfVision -= StopMoving;
    }
    private void MoveToPlayer(GameObject player)
    {
        if(player.transform.position.x < transform.position.x && _characterFlippedLeft == false)
        {
            FlipCharacter();
            _characterFlippedLeft = true;
        }
        if (player.transform.position.x > transform.position.x && _characterFlippedLeft == true)
        {
            FlipCharacter();
            _characterFlippedLeft = false;
        }
        Vector3 toOther = player.transform.position - transform.position;
        toOther.Normalize();
        _rigidBody.velocity = toOther;
    }
    private void StopMoving()
    {
        _rigidBody.velocity = new Vector2(0, 0);
    }
    private void FlipCharacter()
    {
        _characterFlippedLeft = !_characterFlippedLeft;
        Vector3 Scaler = _zombieParts.transform.localScale;
        Scaler.x *= -1;
        _zombieParts.transform.localScale = Scaler;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            if(_attackIsOnCooldown == false)
            {
                StartCoroutine(AttackAnimation(collision.gameObject));
            }
        }
    }
    private IEnumerator AttackAnimation(GameObject player)
    {
        Attack(player);
        _attackIsOnCooldown = true;
        yield return new WaitForSeconds(_attackInterval);
        _attackIsOnCooldown = false;
    }
    private void Attack(GameObject player)
    {
        if(player.GetComponent<IDamageable>() != null)
        {
            player.GetComponent<IDamageable>().TakeDamage(_damagePerAttack);
        }
    }
    private void UnitDied()
    {
        OnDead.Invoke(gameObject);
        Destroy(gameObject);
    }
}
