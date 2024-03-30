using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] Button _shootButton;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerVision _playerVision;
    [SerializeField] int _shootDamage = 2;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PlayerShootController>().AsSingle().WithArguments(_shootButton, _playerVision, _shootDamage).NonLazy();
        Container.Bind<PlayerMovement>().FromInstance(_playerMovement).AsSingle().NonLazy();
    }
}
