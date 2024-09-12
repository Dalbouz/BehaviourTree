using Dawud.BT.General;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ItemManager : SingletonRoot<ItemManager>
{
    [SerializeField] private List<GameObject> _pickupableItems = new List<GameObject>();

    public List<GameObject> PickupableItems
    {
        get { return _pickupableItems; }
    }
}
