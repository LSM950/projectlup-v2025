using UnityEngine;

namespace LUP.ES
{
    public class Item : IItemable
    {
        public BaseItemData baseItem;
        public int count;
        LUP.Define.ItemType type;
    
        public Item(BaseItemData baseItem)
        {
            this.baseItem = baseItem;
            switch (baseItem.itemType)
            {
                case ItemType.None:
                    type = LUP.Define.ItemType.None;
                    break;
                case ItemType.Weapon:
                    type = LUP.Define.ItemType.Weapon;
                    break;
                case ItemType.Armor:
                    type = LUP.Define.ItemType.Armor;
                    break;
                case ItemType.Consumable:
                    type = LUP.Define.ItemType.Consumable;
                    break;
                case ItemType.Material:
                    type = LUP.Define.ItemType.Material;
                    break;
                default:
                    break;
            }
        }

        public int ItemID => baseItem.ID;

        public string ItemName => baseItem.Name;

        public LUP.Define.ItemType Type => type;

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
