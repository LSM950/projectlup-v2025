using UnityEngine;
using UnityEngine.UI;
using Roguelike.Define;


namespace LUP.RL
{
    public class CharacterSelectionButtonPanel : MonoBehaviour
    {
        enum Buttontype
        {
            Btn_Select,
            Btn_Long,
            Btn_Middle,
            Btn_Short
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private Button SelectButton;

        private Button FilterLongBtn;
        private Button FilterMiddleBtn;
        private Button FilterShortBtn;

        private Image[] btnBackGrounds;

        private CharacterSelectionScrollPanel ScrollPanel;

        private CharacterAtkType currentSelectedFilterType = CharacterAtkType.None;

        [SerializeField]
        private Color normalColor = Color.red;
        [SerializeField]
        private Color highlightColor = Color.blue;


        private void Awake()
        {
            ScrollPanel = gameObject.GetComponentInParent<CharacterSelectionScrollPanel>();

            if (ScrollPanel == null)
                UnityEngine.Debug.LogError(("Cant' find CharacterSectionPanel!"));

            Button[] buttons = GetComponentsInChildren<Button>();

            SelectButton = buttons[0];

            FilterLongBtn = buttons[1];
            FilterMiddleBtn = buttons[2];
            FilterShortBtn = buttons[3];

            btnBackGrounds = new Image[buttons.Length];
            btnBackGrounds[(int)Buttontype.Btn_Select] = SelectButton.GetComponent<Image>();
            btnBackGrounds[(int)Buttontype.Btn_Long] = FilterLongBtn.GetComponent<Image>();
            btnBackGrounds[(int)Buttontype.Btn_Middle] = FilterMiddleBtn.GetComponent<Image>();
            btnBackGrounds[(int)Buttontype.Btn_Short] = FilterShortBtn.GetComponent<Image>();


            //Temp
            {

                FilterLongBtn.GetComponent<TextImageBtn>().Init();
                FilterMiddleBtn.GetComponent<TextImageBtn>().Init();
                FilterShortBtn.GetComponent<TextImageBtn>().Init();

            }

            //SelectButton.onClick.AddListener(() => ScrollPanel.OnCharacterSelectBtnClicked(10));
            //알리기만 하자, 상위단 패널이 현재 선택된게 무엇인지도 알게 하자
            SelectButton.onClick.AddListener(OnConfirmBtnSelected);

            FilterLongBtn.onClick.AddListener(() => OnFilterBtnClicked(CharacterAtkType.Long));
            FilterMiddleBtn.onClick.AddListener(() => OnFilterBtnClicked(CharacterAtkType.Middle));
            FilterShortBtn.onClick.AddListener(() => OnFilterBtnClicked(CharacterAtkType.Short));

        }

        void OnConfirmBtnSelected()
        {
            currentSelectedFilterType = CharacterAtkType.None;
            ReFreshColor();

            ScrollPanel.OnSelectedCharacter();
        }

        void OnFilterBtnClicked(CharacterAtkType filteringType)
        {
            if (currentSelectedFilterType == filteringType)
                currentSelectedFilterType = CharacterAtkType.None;

            else
            {
                currentSelectedFilterType = filteringType;
            }


            ScrollPanel.SetCharacterFilter(currentSelectedFilterType);
            SetFilterImageHighlighte(currentSelectedFilterType);
        }

        void SetFilterImageHighlighte(CharacterAtkType filteringType)
        {
            ReFreshColor();

            switch (filteringType)
            {
                case CharacterAtkType.None:
                    break;
                case CharacterAtkType.Long:
                    btnBackGrounds[(int)Buttontype.Btn_Long].color = highlightColor;

                    //Temp
                    FilterLongBtn.GetComponent<TextImageBtn>().SetActive(true);
                    break;
                case CharacterAtkType.Middle:
                    btnBackGrounds[(int)Buttontype.Btn_Middle].color = highlightColor;

                    //Temp
                    FilterMiddleBtn.GetComponent<TextImageBtn>().SetActive(true);
                    break;
                case CharacterAtkType.Short:
                    btnBackGrounds[(int)Buttontype.Btn_Short].color = highlightColor;

                    //Temp
                    FilterShortBtn.GetComponent<TextImageBtn>().SetActive(true);
                    break;
            }
        }

        void Start()
        {
            ReFreshColor();
        }

        void ReFreshColor()
        {
            for (int i = 1; i < btnBackGrounds.Length; i++)
            {
                btnBackGrounds[i].color = normalColor;


                //Temp
                btnBackGrounds[i].GetComponent<TextImageBtn>().SetActive(false);
            }
        }
    }
}

