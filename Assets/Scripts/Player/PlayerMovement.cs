using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick _joyStick;
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private GameObject _playerParts;
    public HitPoints HitPoints;
    public bool CharacterFlippedLeft { get; private set; }
    void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(_joyStick.Horizontal*_speed, _joyStick.Vertical*_speed);

        if(_joyStick.Horizontal < 0  && CharacterFlippedLeft == false)
        {
            FlipCharacter();
            CharacterFlippedLeft = true;
        }
        if(_joyStick.Horizontal > 0 && CharacterFlippedLeft == true)
        {
            FlipCharacter();
            CharacterFlippedLeft = false;
        }
    }
    private void FlipCharacter()
    {
        CharacterFlippedLeft = !CharacterFlippedLeft;
        Vector3 Scaler = _playerParts.transform.localScale;
        Scaler.x *= -1;
        _playerParts.transform.localScale = Scaler;
    }
}
