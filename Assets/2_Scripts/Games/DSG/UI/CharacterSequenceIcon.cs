using LUP.DSG.Utils.Enums;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace LUP.DSG
{
    public class CharacterSequenceIcon : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Image background;
        [SerializeField]
        private UnityEngine.UI.Image portrait;
        [SerializeField]
        private UnityEngine.UI.Image attributeIcon;
        [SerializeField]
        private TextMeshProUGUI level;

        public void SetIconData(EAttributeType type, int characterLevel, bool isEnemy)
        {
            level.text = "Lv." + characterLevel;

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            FormationSystem system = stage.GetComponent<FormationSystem>();
            AttributeTypeImage typeIcon = system.GetTypeByAttributeImage(type);

            attributeIcon.sprite = typeIcon.TypeIcon;
            attributeIcon.color = typeIcon.TypeColor;


            UnityEngine.Color color = isEnemy ? UnityEngine.Color.red : UnityEngine.Color.blue;
            color.a = 0.6f;
            background.color = color;
        }
    }
}