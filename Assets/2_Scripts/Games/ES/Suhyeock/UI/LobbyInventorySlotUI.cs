using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class LobbyInventorySlotUI : MonoBehaviour
    {
        private WeaponType weaponType;
        public Image iconImage;
        public Text stackText;
        [HideInInspector]
        public Button slotButton;

        private int slotIndex;
        private LobbyInventoryUIController lobbyInventoryUIController;
        private ItemIconLoader itemIconLoader;

        public void Init(int slotIndex, LobbyInventoryUIController lobbyInventoryUIController, ItemIconLoader itemIconLoader, WeaponType weaponType)
        {
            this.slotIndex = slotIndex;
            this.lobbyInventoryUIController = lobbyInventoryUIController;
            this.itemIconLoader = itemIconLoader;
            this.weaponType = weaponType;

            if (slotButton == null) slotButton = GetComponent<Button>();

            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => OnClickSlot());
        }

        private void OnClickSlot()
        {
            if (lobbyInventoryUIController != null)
            {
                if (slotIndex == -1)
                {
                    lobbyInventoryUIController.OnWeaponSlotClicked(weaponType);
                    return;
                }
                switch (weaponType)
                {
                    case WeaponType.Melee:
                        lobbyInventoryUIController.OnMeleeInventorySlotClicked(slotIndex);
                        break;
                    case WeaponType.Ranged:
                        lobbyInventoryUIController.OnRangedInventorySlotClicked(slotIndex);
                        break;
                    case WeaponType.Throwing:
                        lobbyInventoryUIController.OnThrowingInventorySlotClicked(slotIndex);
                        break;
                    default:
                        break;
                }
            }
        }

        public void UpdateSlot(InventorySlot dataSlot)
        {
            if (dataSlot.IsEmpty)
            {
                iconImage.gameObject.SetActive(false);
                stackText.text = "";
            }
            else
            {
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = itemIconLoader.LoadIconSprite(dataSlot.item.baseItem.ID);

                stackText.text = dataSlot.item.count.ToString();
            }
        }
    }
}
