using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private InventorySlot[] _slots;
    [SerializeField] private GameObject _loseWindow;
    [SerializeField] private ItemList _itemList;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Inventory>().AsSingle().WithArguments(_slots).NonLazy();
        Container.Bind<GUIManager>().AsSingle().WithArguments(_loseWindow).NonLazy();
        Container.Bind<SaveLoadJSON>().AsSingle().WithArguments(_itemList, _saveButton, _loadButton).NonLazy();
    }
}
