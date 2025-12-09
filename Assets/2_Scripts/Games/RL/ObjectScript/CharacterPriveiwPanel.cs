using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPriveiwPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject characterPreviewObject;
    public CharacterStatBox characterStatBox;

    private CharacterPreviewAnimation previewCharacterAnimImage;
    private TextMeshProUGUI previewCharacaterNameText;

    private void Awake()
    {
        previewCharacterAnimImage = characterPreviewObject.GetComponent<CharacterPreviewAnimation>();
        previewCharacaterNameText = characterPreviewObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacterPreview(RLCharacterData characterData)
    {
        //previewCharacterImage.sprite = characterData.GetDisplayableImage();

        string charaterName = characterData.Name;

        previewCharacterAnimImage.ChangeSpriteSheet(charaterName);

        previewCharacaterNameText.SetText(characterData.GetDisplayableName());

        characterStatBox.UpdateStatBox(characterData);
    }
}
