using UnityEngine;

namespace LUP.PCR
{
    public class CookingState : IBuildState
    {
        public float cookTime;
        public float progressRatio;
        public int totalCount;
        public bool isCompledted;     
        public bool isStarted;

        private BuildingRestaurant restaurant;

        public void Enter(BuildingBase building)
        {
            Debug.Log("CookingState Enter");

            if (restaurant == null)
            {
                restaurant = building as BuildingRestaurant;
            }


        }
        public void Exit()
        {
            Debug.Log("CookingState Exit");

        }
        public void Tick(float deltaTime)
        {



        }
    }
}


