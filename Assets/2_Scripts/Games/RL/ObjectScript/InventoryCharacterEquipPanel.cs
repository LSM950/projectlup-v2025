using LUP.RL;
using Roguelike.Util;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryCharacterEquipPanel : MonoBehaviour, IPanelContentAble
{
    private Vector2 parentViewportSize;
    private CharacterPreviewAnimation inventoryCharacterPreviewAnimImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    void Start()
    {
        inventoryCharacterPreviewAnimImage = gameObject.GetComponentInChildren<CharacterPreviewAnimation>();
    }

    public bool Init()
    {
        showPanel();
        return true;
    }

    void showPanel()
    {
        parentViewportSize = gameObject.transform.parent.transform.parent.GetComponent<RectTransform>().rect.size;
        Vector2 ItemBoxSize = new Vector2(parentViewportSize.x, parentViewportSize.y * 0.6f);
        gameObject.GetComponent<RectTransform>().sizeDelta = ItemBoxSize;

        SetPastGameData();
    }

    void SetPastGameData()
    {
        LobbyGameCenter lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();
        string characterName = lobbyGameCenter.GetselectedCharacter().Name;

        SetInventoryCharacterPrieViewAnimImage(characterName);

    }

    public void SetInventoryCharacterPrieViewAnimImage(string characterName)
    {
        inventoryCharacterPreviewAnimImage.ChangeSpriteSheet(characterName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
