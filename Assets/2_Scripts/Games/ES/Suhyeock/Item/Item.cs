using UnityEngine;

namespace LUP.ES
{
    public class Item : IItemable
    {
        public BaseItemData baseItem;
        public int count;
    
        public Item(BaseItemData baseItem)
        {
            this.baseItem = baseItem;
        }

        public int ItemID => baseItem.ID;

        public string ItemName => baseItem.Name;

        public LUP.Define.ItemType Type => LUP.Define.ItemType.None;

        public int MaxStackSize => baseItem.MaxStackSize;

        public Sprite Icon => null;

        public string Description => baseItem.Description;

        public bool IsUsable => false;

        public int GetInt(string fieldName, int defaultValue = 0)
        {
            return defaultValue;
        }

        public float GetFloat(string fieldName, float defaultValue = 0f)
        {
            return defaultValue;
        }

        public string GetString(string fieldName, string defaultValue = "")
        {
            return defaultValue;
        }

        public bool GetBool(string fieldName, bool defaultValue = false)
        {
            return defaultValue;
        }

        public bool HasCustomField(string fieldName)
        {
            return false;
        }

        public void OnUse()
        {

        }
    }
}
