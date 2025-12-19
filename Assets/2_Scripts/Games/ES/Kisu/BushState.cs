using UnityEngine;

namespace LUP.ES
{
    public class BushState : MonoBehaviour
    {
        public bool IsInBush { get; private set; }
        public System.Action<bool> OnBushStateChanged;

        public void SetBush(bool value)
        {
            IsInBush = value;
            OnBushStateChanged?.Invoke(value);
        }
    }
}