using UnityEngine;
using TMPro;
using UnityEngine.UI; // 1. UI 네임스페이스 추가

namespace LUP.ES
{
    public class ExtractionPoint : MonoBehaviour, IInteractable
    {
        [Header("설정")]
        [SerializeField] private float extractionTime = 10.0f;

        [Header("색상")]
        [SerializeField] private Color startFillColor = Color.green;
        [SerializeField] private Color successFillColor = Color.green;

        [Header("참조")]
        public EventBroker eventBroker;
        private InteractionUIController interactionUI;

        // 2. 머터리얼 대신 Image 참조 추가
        [Header("UI 요소")]
        [SerializeField] private Image progressCircle;
        [SerializeField] private TextMeshProUGUI timerText;

        public bool InterruptsOnMove => false;

        private bool isExtracting = false;
        private bool isExtracted = false;
        private float currentTime = 0.0f;

        public bool CanInteract() => !isExtracted;

        void Start()
        {
            interactionUI = GetComponent<InteractionUIController>();

            // 초기 상태 설정
            if (progressCircle != null)
            {
                progressCircle.fillAmount = 0f;
                progressCircle.color = startFillColor;
            }

            HideTimerTextObject();
        }

        public bool TryStartInteraction(float deltaTime)
        {
            if (isExtracted) return true;

            if (!isExtracting)
            {
                isExtracting = true;
                currentTime = extractionTime;

                ShowTimerTextObject();
                HideInteractionPrompt();
            }

            currentTime -= deltaTime;

            float progress = Mathf.Clamp01(1f - (currentTime / extractionTime));

            if (progressCircle != null)
            {
                progressCircle.fillAmount = progress;
                progressCircle.color = Color.Lerp(startFillColor, successFillColor, progress);
            }

            UpdateInteractionTimerText(Mathf.Max(0, currentTime));

            if (currentTime <= 0.0f)
            {
                Interact();
                return true;
            }

            return false;
        }

        public void Interact()
        {
            isExtracted = true;
            isExtracting = false;

            if (progressCircle != null)
            {
                progressCircle.fillAmount = 1f;
                progressCircle.color = successFillColor;
            }

            HideTimerTextObject();
            Debug.Log("탈출 성공!");

            if (eventBroker != null)
                eventBroker.ReportGameFinish(true);
        }

        public void ResetInteraction()
        {
            isExtracting = false;
            currentTime = 0.0f;

            if (progressCircle != null)
                progressCircle.fillAmount = 0f;

            HideTimerTextObject();
        }

        public void ShowInteractionPrompt() { if (!isExtracted) interactionUI.ShowInteractionPrompt(); }
        public void HideInteractionPrompt() { interactionUI.HideInteractionPrompt(); }
        public void ShowInteractionTimerUI() { }
        public void HideInteractionTimerUI() { }

        private void UpdateInteractionTimerText(float remainTime)
        {
            if (timerText != null) timerText.text = remainTime.ToString("F1");
        }

        private void ShowTimerTextObject() 
        { if (timerText != null) timerText.gameObject.SetActive(true); }
        private void HideTimerTextObject() 
        { if (timerText != null) timerText.gameObject.SetActive(false); }
    }
}