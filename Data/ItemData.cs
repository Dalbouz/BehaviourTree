using UnityEngine;
using Dawud.BT.Enums;

namespace Dawud.BT.Misc
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName ="CustomItem_", menuName = "BTCustom/Item")]
    public class ItemData : ScriptableObject
    {
        public ItemEnum Type = default;
        public int Value = default;
        public bool Pickupable = false;
    }
}
