using UnityEngine;

namespace KrampStudio.BT.Misc
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ItemRoot : MonoBehaviour
    {
        [SerializeField] private ItemData _itemData = default;

        public ItemData Data
        {
            get { return _itemData; }
        }
    }
}
