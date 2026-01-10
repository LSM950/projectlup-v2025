using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{

    public class InventoryEquipBtn : MonoBehaviour, IPanelContentAble
    {

        

        [SerializeField]
        private Button button;

        [SerializeField]
        private Image buttonBorder;

        [SerializeField]
        private Image ButtonBackGroundImage;

        [SerializeField]
        private Image ItemIcon;

        [SerializeField]
        private Image TierImage;
        [SerializeField]
        private Image TypeBorder;
        [SerializeField]
        private Image TypeIconImage;

        public bool Init()
        {

            if (button != null)
                return true;
            
            return false;
        }

        public void SetEquipButton(Color backGroundColor, Color buttonBorderColor,Sprite itemIcon, bool bIsEpickEquip, Color borderColor, Sprite typeIconImage)
        {
            ButtonBackGroundImage.color = backGroundColor;
            buttonBorder.color = buttonBorderColor;

            ItemIcon.sprite = itemIcon;

            if (bIsEpickEquip == false)
                TierImage.gameObject.SetActive(false);

            TypeBorder.color = borderColor;
            TypeIconImage.sprite = typeIconImage;


        }
    }
}

