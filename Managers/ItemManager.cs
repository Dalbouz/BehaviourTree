using Dawud.BT.General;
using Dawud.BT.Misc;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ItemManager : SingletonRoot<ItemManager>
{
    [SerializeField] private List<ItemGeneric> _pickupableItems = new List<ItemGeneric>();

    public List<ItemGeneric> PickupableItems
    {
        get { return _pickupableItems; }
    }
}
