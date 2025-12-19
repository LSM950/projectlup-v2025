using LUP.ES;
using UnityEngine;

namespace LUP.ES
{
    public class BushZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<BushState>()?.SetBush(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<BushState>()?.SetBush(false);
        }
    }
}