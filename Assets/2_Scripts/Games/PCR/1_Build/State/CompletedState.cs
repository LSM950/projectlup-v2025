using UnityEngine;

namespace LUP.PCR
{
    public class CompletedState : IBuildState
    {
        public void Enter(BuildingBase building)
        {
            Debug.Log("CompletedState Enter");
        }
        public void Exit()
        {
            Debug.Log("CompletedState Exit");
        }
        public void Tick(float deltaTime)
        {

        }

    }
}
