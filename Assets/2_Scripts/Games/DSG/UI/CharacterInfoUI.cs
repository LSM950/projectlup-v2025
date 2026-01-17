using LUP.DSG.Utils.Enums;
using System.ComponentModel;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class CharacterInfoUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI levelText;
        [SerializeField]
        private Image attributeIcon;
        public void SetCharacterInfo(EAttributeType attribute, int level)
        {
            StringBuilder sb = new StringBuilder("LV." + level.ToString());
            levelText.text = sb.ToString();

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            FormationSystem system = stage.GetComponent<FormationSystem>();
            AttributeTypeImage typeIcon = system.GetTypeByAttributeImage(attribute);

            if (typeIcon.TypeIcon == null)
                return;

            attributeIcon.sprite = typeIcon.TypeIcon;
            attributeIcon.color = typeIcon.TypeColor;
        }
    }
}
