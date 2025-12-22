using UnityEngine;

namespace LUP.PCR
{
    public interface IBuildState
    {
        void Enter(BuildingBase building);
        void Exit();
        void Tick(float deltaTime);
    }
}

