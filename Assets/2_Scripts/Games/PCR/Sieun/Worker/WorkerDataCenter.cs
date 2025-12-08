using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerDataCenter : MonoBehaviour
    {
        [SerializeField] PCRDataCenter pcrDataCenter;
        [SerializeField] AGridMap aGrid;
        [SerializeField] private const int maxWorkerCount = 50;
        [SerializeField] private List<WorkerAI> workers = new List<WorkerAI>(maxWorkerCount);
        [HideInInspector] public TileInfo[,] tileInfoes;

        // 외부에서 워커를 등록하는 함수
        public void RegisterWorker(WorkerAI newWorker)
        {
            if(!workers.Contains(newWorker))
            {
                workers.Add(newWorker);
                newWorker.InitBTReferences();
            }
        }


        private void Awake()
        {
            pcrDataCenter = GetComponentInChildren<PCRDataCenter>();
        }

        private void Start()
        {
            aGrid.InitMap(pcrDataCenter.tileInfoes);

            int count = workers.Count;

            for (int i = 0; i < count; i++)
            {
                if (workers[i] != null)
                {
                    workers[i].InitBTReferences();
                }
            }

        }

        private void Update()
        {
            int count = workers.Count;

            for (int i = 0; i < count; i++)
            {
                if(workers[i] != null)
                {
                    if (i >= workers.Count)
                    {
                        break;
                    }

                    workers[i].UpdateBT();
                }

            }
        }
    }
}

