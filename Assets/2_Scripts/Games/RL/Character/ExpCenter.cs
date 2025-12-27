//using UnityEngine;
//using System.Collections.Generic;
//namespace LUP.RL
//{
//    public class ExpCenter : MonoBehaviour

//    {
//        private Archer archer;
//        [SerializeField] private LevelDataTable levelTable;

//        public int PendingExp = 0;

//        private StageController stage;
//        private void Awake()
//        {
//            stage = UnityEngine.Object.FindFirstObjectByType<StageController>();

//        }
//        private void OnEnable()
//        {
//            Enemy.OnEnemyDied += AddPendingExp;

//            if (stage != null)
//                stage.onStageClear.AddListener(ApplyStageExp);
        
//        }
//        private void OnDisable()
//        {
//            Enemy.OnEnemyDied -= AddPendingExp;

//            if (stage != null)
//                stage.onStageClear.RemoveListener(ApplyStageExp);
//        }
//        public void BindPlayer(GameObject playerObj)
//        {
//            archer = playerObj.GetComponent<Archer>();
//            if(archer == null)
//            {
//                Debug.Log("null archer");
//            }
//        }
//        private void ApplyStageExp()
//        {
//            Debug.Log("ExpCenter : apply stageExp");
//            if (archer == null)
//            {
//                Debug.LogWarning("ExpCenter : Archer 아직 연결 안됨");
//                return;
//            }

//            if (PendingExp <= 0)
//                return;

//            archer.RuntimeData.xp += PendingExp;
//            PendingExp = 0;

//            while (true)
//            {
//                var data = levelTable.GetLevelData(archer.RuntimeData.level);

//                if (archer.RuntimeData.xp < data.RequiredExp)
//                    break;

//                archer.RuntimeData.xp -= data.RequiredExp;
//                archer.LevelUp();
//            }

//            //archer.RaiseExpChanged();
//        }
//        private void AddPendingExp(int exp)
//        {
//            PendingExp += exp;
//        }
//    }

//}