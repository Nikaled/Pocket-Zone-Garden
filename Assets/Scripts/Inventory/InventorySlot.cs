using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Zenject;
public class InventorySlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _slotIcon;
    [SerializeField] private TextMeshProUGUI _itemInSlotCount;
    [SerializeField] private Image ItemCountImage;
    [SerializeField] private GameObject DeleteButton;
    [SerializeField] private GameObject ItemContextMenuParent;
    public Item CurrentItem { get; private set; }
    private Inventory _inventory;
    public bool IsEmpty { get; private set; } = true;


    [Inject] public void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }
    public void AddItem(Item item)
    {
        CurrentItem = item;
        IsEmpty = false;
        _slotIcon.enabled = true;
        _slotIcon.sprite = CurrentItem.Icon;
        if(item is IExpendable expendable)
        {
            ItemCountImage.enabled = true;
            _itemInSlotCount.text = expendable.Count.ToString();
            expendable.ItemCountChanged += ShowCurrentCount;
        }
        else
        {
            ItemCountImage.enabled = false;
            _itemInSlotCount.text = null;
        }
    }
    public void RemoveItem()
    {
        IsEmpty = true;
        if (CurrentItem is IExpendable)
        {
            IExpendable ExItem = (IExpendable)CurrentItem;
            ExItem.ItemCountChanged -= ShowCurrentCount;
        }
        ItemCountImage.enabled = false;
        _itemInSlotCount.text = null;
        _slotIcon.enabled = false;
        CurrentItem = null;
    }
    private void ShowCurrentCount(int CurrentCount)
    {
        _itemInSlotCount.text = CurrentCount.ToString();
        if(CurrentCount == 0)
        {
            _inventory.RemoveItem(this);
        }
    }
    public void DeleteButtonFunction()
    {
        _inventory.RemoveItem(this);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(IsEmpty) return;
        if(ItemContextMenuParent !=null) DeleteButton.transform.parent = ItemContextMenuParent.transform;
        DeleteButton.SetActive(true);
    }
}
