using UnityEngine;

namespace LUP.PCR
{
    public class BuildingRestaurant : BuildingBase
    {
        protected IBuildState constructState;
        protected IBuildState completeState;
        protected IBuildState cookingState;

        public FoodType currFood = FoodType.None;
        private bool isCooking;
        // 필요 자원 만들어야 한다.

        

        private void Awake()
        {
            buildingEvents = new BuildingEvents();
            constructState = new UnderConstructionState();
            completeState = new CompletedState();
            cookingState = new CookingState();
        }

        private void Start()
        {

            buildingEvents.OnBuildingSelected += OpenBuildingUI;
            buildingEvents.OnBuildingDeselected += CloseBuildingUI;
        }

        private void Update()
        {
            if (!hasWork)
            {
                return;
            }

            // 추후에 가속 아이템 적용 가능하게 만들어야 한다.
            float deltaTime = Time.deltaTime;
            currBuildState?.Tick(deltaTime);
        }

        public override void Init(ProductionRuntimeData runtimeData)
        {
            this.runtimeData = runtimeData;


            // Constructing Building
            constructionInfo = runtimeData.GetConstructionInfo(buildingInfo.buildingId);
            if (constructionInfo == null)
            {
                ConstructionInfo newConstructionInfo = new ConstructionInfo(buildingInfo.buildingId, 0f);
                runtimeData.AddToList(runtimeData.ConstructionInfoList, newConstructionInfo);
                constructionInfo = newConstructionInfo;
            }

            if (ConstructScreen)
            {
                ConstructScreen.SetActive(false);
            }

            hasWork = true;
            buildingName = "Restaurant";

            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;
            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.RESTAURANT, buildingInfo.level);


            if (buildingInfo.isConstructing)
            {
                ChangeState(constructState);
            }
            else
            {
                ChangeState(completeState);
            }
        }

        public override void CompleteContruction()
        {
            // 레벨업
            buildingInfo.level++;
            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;
            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.WHEATFARM, buildingInfo.level);

            ChangeState(completeState);
        }

        public void CompleteCooking()
        {

        }

        public override void Upgrade()
        {
            ChangeState(constructState);
        }

        public override void DeliverToInventory()
        {

        }

    }

}

