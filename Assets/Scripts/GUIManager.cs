using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager
{
    private GameObject _loseWindow;
    public GUIManager(PlayerMovement _playerMovement, GameObject LoseWindow)
    {
        _playerMovement.HitPoints.OnDied += ShowLoseWindow;
            _loseWindow = LoseWindow;
        Time.timeScale = 1f;
    }
    private void ShowLoseWindow()
    {
        _loseWindow.SetActive(true);
        Time.timeScale = 0f;
    }

}
