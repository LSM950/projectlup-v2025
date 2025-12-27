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

        //private void LateUpdate()
        //{
        //    if (target == null) return;

        //    Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position + offset);

        //    Vector2 localPos;
        //    // 스크린 좌표를 캔버스의 로컬 좌표로 정확하게 변환해줍니다.
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //        canvasRect,
        //        screenPos,
        //        null, // Overlay 모드일 때는 null, Camera 모드일 때는 해당 카메라
        //        out localPos
        //    );

        //    rectTransform.anchoredPosition = localPos;

        //    //rectTransform.anchoredPosition = new Vector2((viewportPos.x * sizeDelta.x) - (sizeDelta.x * 0.5f), (viewportPos.y * sizeDelta.y) - (sizeDelta.y * 0.5f));


        //    //Vector3 worldPos = target.position;
        //    //Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        //    //rectTransform.position = screenPos + offset;
        //}

        //public void SetTarget(Canvas canvas, Transform newTarget)
        //{
        //    canvasRect = canvas.GetComponent<RectTransform>();
        //    target = newTarget;
        //    gameObject.SetActive(true);
        //}

        //public void ReleaseTarget()
        //{
        //    gameObject.SetActive(false);
        //    target = null;
        //}

        public void SetCharacterInfo(EAttributeType attribute, int level)
        {
            StringBuilder sb = new StringBuilder("LV." + level.ToString());
            levelText.text = sb.ToString();

            FormationSystem system = FindFirstObjectByType<FormationSystem>();
            AttributeTypeImage typeIcon = system.GetTypeByAttributeImage(attribute);

            if (typeIcon.TypeIcon == null)
                return;

            attributeIcon.sprite = typeIcon.TypeIcon;
            attributeIcon.color = typeIcon.TypeColor;
        }
    }
}
