using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace LUP.ES
{
    public class ResultDisplayCenter : MonoBehaviour
    {
        private EventBroker eventBroker;
        private ItemCenter itemCenter; //테스트 용
        public GameObject resultPanel;
        public GameObject ItemDisplayContent;
        public GameObject itemSlotPrefab;
        public Text resultHeader;
        //public Button lobbyButton;

        private Transform contentParent;
        private List<Item> items;
        private void Start()
        {
            resultPanel.SetActive(false);
            //lobbyButton.onClick.AddListener(LoadLobby);
            eventBroker = FindAnyObjectByType<EventBroker>();
            itemCenter = FindAnyObjectByType<ItemCenter>();
            contentParent = ItemDisplayContent.transform;
            if (eventBroker != null )
            {
                eventBroker.OnGameFinished += ShowResult;
            }
        }

        private void OnDestroy()
        {
            if (eventBroker != null)
            {
                eventBroker.OnGameFinished -= ShowResult;
            }
        }

        public void ShowInventoryItems(List<Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                GameObject newSlot = Instantiate(itemSlotPrefab, contentParent);

                ItemDisplaySlot slot = newSlot.GetComponent<ItemDisplaySlot>();

                if (slot != null)
                {
                    slot.SetItem(items[i]);
                }
            }
        }

        private void ShowResult(bool isSuccess)
        {
            Debug.Log("GameFinish");
            Time.timeScale = 0f;
            StringBuilder resultHeadrString = new StringBuilder();
            resultHeadrString.Append("탈출 ");
            resultPanel.SetActive(true);
            if (isSuccess)
            {
                resultHeadrString.Append("성공");
                Inventory inventory = FindAnyObjectByType<Inventory>();
                if (inventory != null)
                {
                    items = inventory.GetItems();
                    ShowInventoryItems(items);
                    ExtractionShooterStage extractionShooterStage = StageManager.Instance.GetCurrentStage() as ExtractionShooterStage;

                    foreach (Item item in items)
                    {
                        if (item == null)
                            continue;
                        if (item.ItemID == 1 || item.ItemID == 4 || item.ItemID == 7)
                            continue;
                        extractionShooterStage.ESInven.AddItem(item);
                    }
                }
                
            }
            else
                resultHeadrString.Append("실패");
            resultHeader.text = resultHeadrString.ToString();
        }
    }
}
