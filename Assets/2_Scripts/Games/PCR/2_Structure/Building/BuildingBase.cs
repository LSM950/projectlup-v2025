using System;
using UnityEngine;

namespace LUP.PCR
{

    public abstract class BuildingBase : StructureBase
    {
        protected ProductionRuntimeData runtimeData;

        protected BuildingInfo buildingInfo;
        protected ConstructionInfo constructionInfo;

        public BuildingStaticData buildingStaticData;
        public PCRConstructionStaticData currentConstructionData;
        public BuildingEvents buildingEvents;

        public string buildingName;
        public PCRResourceCenter resourceCenter;
        public GameObject ConstructScreen;
        public ConstructionOverlayView constructionOverlay;


        protected IBuildState currBuildState;
        public abstract void Init(ProductionRuntimeData runtimeData);
        public abstract void CompleteContruction();

        public abstract void Upgrade();

        public abstract void DeliverToInventory();

        public void OpenBuildingUI()
        {
            Debug.Log("OpenUI");
        }

        public void CloseBuildingUI()
        {
            Debug.Log("CloseUI");
        }

        public void ChangeState(IBuildState state)
        {
            currBuildState?.Exit();
            currBuildState = state;
            currBuildState.Enter(this);
        }

        public BuildingInfo GetBuildingInfo()
        {
            return buildingInfo;
        }

        public void SetBuildingInfo(BuildingInfo buildingInfo)
        {
            this.buildingInfo = buildingInfo;
        }

        public ConstructionInfo GetConstructionInfo()
        {
            return constructionInfo;
        }

        public void SetConstructionInfo(ConstructionInfo constructionInfo)
        {
            this.constructionInfo = constructionInfo;
        }

        // TODO: 임시 입구 설정. 입구가 왼쪽도 있어야하고 오른쪽도 있어야하는 느낌..
        public void SetEntrance(Vector2Int pivotPos)
        {
            entrancePos = pivotPos;
        }

    }
}