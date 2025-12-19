using LUP.ES;
using UnityEngine;

namespace LUP.ES
{
    public class PlayerBushVisualBinder : MonoBehaviour
    {
        public BushState bushState;
        public PlayerVisualController visualController;

        private void Awake()
        {
            bushState.OnBushStateChanged += visualController.SetTransparent;
        }
    }
}