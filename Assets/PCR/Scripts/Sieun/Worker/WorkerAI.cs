using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LUP.PCR
{
    //RequireComponent(typeof(Animator))]
    public class WorkerAI : MonoBehaviour
    {
        public WorkerBlackboard LocalBlackboard { get; private set; }

        // 이벤트 (UI 등 외부에서 구독용)
        public event Action<WorkerAI> OnEatCompleted;    
        public event Action<WorkerAI> OnTaskStarted;     // [작업 시작]
        public event Action<WorkerAI> OnTaskFinished;    // [작업 완료]
        
        BTNode root;
        Worker worker;
        
        public float hunger = 0.5f;

        // bool isWorking, bool isEating 등은 State 클래스에서 가져오게 할까 고민중
        // hasNewTask, isTaskPaused 등도 State 클래스에서 가져오게 할까 고민중
        public bool isWorking = false; 
        public bool isEating = false;
        public bool hasNewTask = false;
        public bool hasPausedWork = false;

        public Vector3 originSpot;
      
        public void StartTask()
        {
            isWorking = true;
            OnTaskStarted?.Invoke(this);
        }

        public void FinishTask()

        {
            isWorking = false;
            OnTaskFinished?.Invoke(this);

        }


        //public void MoveTo(TileInfo buiding)
        //{
        //   // transform.position = Vector3.MoveTowards(transform.position, tile.pos, Time.deltaTime * 2f);
        //}

        //public bool IsAt(BuildingData tile)
        //{
        //    //return Vector3.Distance(transform.position, tile.place) < 0.1f;
        //}

        private void Awake()
        {
            worker = GetComponent<Worker>();

            // [핵심 2] 블랙보드 인스턴스 생성
            LocalBlackboard = new WorkerBlackboard();
        }

        private void Start()
        {
            //정적 데이터(참조) 등록
            LocalBlackboard.SetValue(BBKeys.Self, worker);
            LocalBlackboard.SetValue(BBKeys.OwnerAI, this); // ActionNode에서 이벤트 호출 시 필요

            // BT 트리 구성
            SettingBT();
        }
        
        void SettingBT()
        {
            // 모든 Leaf Node 생성자에 LocalBlackboard를 전달 (주입)
            // CompositeNode(Sequence/Selector)는 블랙보드가 필요 없으므로 리스트만 전달

            // Sequence: 배고픔 처리
            BTNode hungerSequence = new SequenceNode(new List<BTNode>
         {
             new IsHealthLowChecker(LocalBlackboard),
             new GoToEatingPlace(LocalBlackboard),
             new EatFood(LocalBlackboard),
            // new GoBackToOriginPosition()
         });

            // Sequence: 하던 일 재개
            BTNode resumeTaskSequence = new SequenceNode(new List<BTNode>
        {
            new IsPausedTaskChecker(LocalBlackboard),
            new GoToPausedTaskLocation(LocalBlackboard),
            new ResumePausedTask(LocalBlackboard)
        });

            // Sequence: 새 일 시작
            BTNode newTaskSequence = new SequenceNode(new List<BTNode>
        {
            new IsNewTaskChecker(LocalBlackboard),
            new GoToNewTaskLocation(LocalBlackboard),
            new StartNewTask(LocalBlackboard)
        });

            // Selector: 작업/휴식
            BTNode taskSelector = new SelectorNode(new List<BTNode>
           {
               resumeTaskSequence,
               newTaskSequence,
               new GoToLounge(LocalBlackboard)
           });

            // Root Selector: 배고픔 → 작업/휴식
            root = new SelectorNode(new List<BTNode>
            {
                hungerSequence,
                taskSelector
            });

        }

        private void Update()
        {
            if (root == null) return;

            // [핵심 5] 동적 데이터 동기화 (Sync)
            // WorkerAI의 변수 값이 바뀌면 -> 블랙보드도 즉시 업데이트됨
            // BT 노드들은 변수를 직접 안 보고 블랙보드의 Key만 봄
            LocalBlackboard.SetValue(BBKeys.Hunger, hunger);
            LocalBlackboard.SetValue(BBKeys.IsWorking, isWorking);
            LocalBlackboard.SetValue(BBKeys.HasNewTask, hasNewTask);
            LocalBlackboard.SetValue(BBKeys.HasPausedTask, hasPausedWork);

            root.Evaluate();
        }



    }

}