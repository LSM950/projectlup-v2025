using LUP.ES;
using LUP.RL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class RoguelikeStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
        public List<RoguelikeStaticData> DataList;

        public LobbyGameCenter lobbyGameCenter;
        public InGameCenter inGameCenter;

        public Inventory inventory;

       

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.RL;

            inventory.filename = "RLinventory.json";
        }

        void Start()
        {
           
        }

        void Update()
        {

        }

        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();
            
            // Inventory 생성 및 파일명 설정
            string inventoryFilename = inventory.filename;

            if (JsonDataHelper.FileExists(inventoryFilename))
            {
                // 기존 인벤토리 로드
                inventory = JsonDataHelper.LoadData<Inventory>(inventoryFilename);
                if (inventory != null)
                {
                    inventory.filename = inventoryFilename;
                    inventory.InitializeFromJson();  // Dictionary 복원
                    Debug.Log("[MainStage] 인벤토리 로드 완료");
                }
                else
                {
                    Debug.LogWarning("[MainStage] 인벤토리 로드 실패, 새로 생성");
                    inventory = new Inventory();
                    inventory.filename = inventoryFilename;
                }
            }
            else
            {
                // 새 인벤토리 생성
                inventory = new Inventory();
                inventory.filename = inventoryFilename;
                Debug.Log("[MainStage] 새 인벤토리 생성");
            }

            if (lobbyGameCenter != null)
            {
                lobbyGameCenter.InitializeCenter();
            }

            if (inGameCenter != null)
            {
                inGameCenter.InitializeCenter();
            }

            yield return null;
        }

        public override IEnumerator OnStageStay()
        {
            yield return base.OnStageStay();
            //일단 납두기
            yield return null;
        }

        public override IEnumerator OnStageExit()
        {
            yield return base.OnStageExit();
            //구현부


            yield return null;
        }

        protected override void LoadResources()
        {
            //resource = ResourceManager.Instance.Load...
        }

        protected override void GetDatas()
        {
            List<BaseStaticDataLoader> loaders = base.GetStaticData(this, 1);
            List<BaseRuntimeData> runtimeDatas = base.GetRuntimeData(this, 1);

            if (loaders != null && loaders.Count > 0)
            {
                foreach (var loader in loaders)
                {
                    if (loader is RoguelikeStaticDataLoader rlLoader)
                    {
                        DataList = rlLoader.GetDataList();
                    }
                }
            }

            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is RoguelikeRuntimeData rlRuntimeData)
                    {
                        RuntimeData = rlRuntimeData;
                    }
                }
            }
        }

        protected override void SaveDatas()
        {
            List<BaseRuntimeData> runtimeDataList = new List<BaseRuntimeData>();

            if (RuntimeData != null)
            {
                runtimeDataList.Add(RuntimeData);
            }

            base.SaveRuntimeDataList(runtimeDataList);
        }
    }
}

