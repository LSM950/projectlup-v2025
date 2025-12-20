using UnityEngine;
using UnityEngine.UI;

namespace LUP.ST
{
    public class TeamCompositionUI : MonoBehaviour
    {
        public Button[] slotButtons;            // 상단 슬롯 선택 버튼(5개)
        public Image[] lobbyTeamImages;         // 로비 씬 팀 슬롯 이미지(5개, confirm 후 반영)

        public Button[] characterButtons;       // 하단 캐릭터 버튼(10개 = 5종 * 2)
        public STCharacterData[] characterDatas; // 캐릭터 종류 데이터(5개)

        public Button confirmButton;
        public Button cancelButton;

        private int selectedSlot = -1;

        // 슬롯에는 "캐릭터 종류 데이터"를 저장
        private STCharacterData[] teamCandidate = new STCharacterData[5];
        private STCharacterData[] oldTeam = new STCharacterData[5];

        private ShootingStage stage;
        private ShootingRuntimeData SRD;

        void Start()
        {
            stage = GameObject.FindFirstObjectByType<ShootingStage>();
            if (stage == null)
            {
                Debug.LogError("ShootingStage not found in this scene.");
                return;
            }

            SRD = stage.RuntimeData as ShootingRuntimeData;
            if (SRD == null)
            {
                Debug.LogError("ShootingRuntimeData not found or wrong type.");
                return;
            }

            // 초기 oldTeam/teamCandidate 동기화(예시: 0~4 한명씩)
            for (int i = 0; i < 5; i++)
                teamCandidate[i] = oldTeam[i] = characterDatas[i];

            // 슬롯 버튼 이벤트 연결
            for (int i = 0; i < slotButtons.Length; i++)
            {
                int idx = i;
                slotButtons[i].onClick.AddListener(() => OnSlotSelected(idx));
            }

            // 캐릭터 버튼 이벤트 연결 (10개)
            for (int i = 0; i < characterButtons.Length; i++)
            {
                int btnIdx = i;
                characterButtons[i].onClick.AddListener(() => OnCharacterSelected(btnIdx));
            }

            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);

            RefreshUI();
        }

        void OnSlotSelected(int slotIdx)
        {
            selectedSlot = slotIdx;
            RefreshUI();
        }

        void OnCharacterSelected(int btnIdx)
        {
            if (selectedSlot == -1) return; // 슬롯 먼저 선택 필요

            // 10개 버튼 → 5종 매핑
            int typeId = btnIdx / 2; // 0~4
            if (typeId < 0 || typeId >= characterDatas.Length) return;

            STCharacterData picked = characterDatas[typeId];
            if (picked == null) return;

            // 같은 캐릭터 최대 2명 제한
            if (CountInTeam(picked, teamCandidate) >= 2) return;

            // 같은 걸로 교체면 변화 없음
            if (teamCandidate[selectedSlot] == picked) return;

            teamCandidate[selectedSlot] = picked;
            RefreshUI();
        }

        int CountInTeam(STCharacterData data, STCharacterData[] team)
        {
            int count = 0;
            for (int i = 0; i < team.Length; i++)
                if (team[i] == data) count++;
            return count;
        }

        void OnConfirm()
        {
            for (int i = 0; i < 5; i++)
            {
                lobbyTeamImages[i].sprite = teamCandidate[i]?.thumbnail;
                oldTeam[i] = teamCandidate[i]; // 저장
            }

            SRD.SetTeam(teamCandidate);

            selectedSlot = -1;
            RefreshUI();
        }

        void OnCancel()
        {
            for (int i = 0; i < 5; i++)
                teamCandidate[i] = oldTeam[i];

            selectedSlot = -1;
            RefreshUI();
        }

        void RefreshUI()
        {
            // 타입별 현재 사용 개수(0~2)
            int[] typeCounts = new int[characterDatas.Length];
            for (int i = 0; i < teamCandidate.Length; i++)
            {
                STCharacterData d = teamCandidate[i];
                if (d == null) continue;

                // characterDatas 안에서 인덱스 찾기(5개라서 O(n)도 충분)
                int typeId = System.Array.IndexOf(characterDatas, d);
                if (typeId >= 0) typeCounts[typeId]++;
            }

            // 슬롯 하이라이트 및 이미지 반영
            for (int i = 0; i < slotButtons.Length; i++)
            {
                slotButtons[i].GetComponent<Image>().color =
                    (i == selectedSlot) ? Color.yellow : Color.white;

                var img = slotButtons[i].GetComponentInChildren<Image>();
                if (img != null)
                    img.sprite = teamCandidate[i]?.thumbnail;
            }

            // 캐릭터 버튼(10개) 표시 및 상호작용 제한
            for (int btnIdx = 0; btnIdx < characterButtons.Length; btnIdx++)
            {
                int typeId = btnIdx / 2;   // 0~4
                int token = btnIdx % 2;    // 0/1 (같은 타입 2개 버튼)

                if (typeId < 0 || typeId >= characterDatas.Length)
                {
                    characterButtons[btnIdx].interactable = false;
                    characterButtons[btnIdx].image.color = Color.gray;
                    continue;
                }

                STCharacterData d = characterDatas[typeId];
                characterButtons[btnIdx].image.sprite = d?.thumbnail;

                // 슬롯 선택 전에는 선택 불가(기존 UX 유지)
                if (selectedSlot == -1)
                {
                    characterButtons[btnIdx].interactable = false;
                    characterButtons[btnIdx].image.color = Color.gray;
                    continue;
                }

                int count = typeCounts[typeId];

                // 토큰 비활성 규칙:
                // count==0: 둘 다 가능
                // count==1: token0 비활성(= 하나 썼다 표시), token1만 가능
                // count==2: 둘 다 비활성
                bool disabled = (count >= 2) || (count == 1 && token == 0);

                characterButtons[btnIdx].interactable = !disabled;
                characterButtons[btnIdx].image.color = disabled ? Color.gray : Color.white;
            }
        }
    }
}
