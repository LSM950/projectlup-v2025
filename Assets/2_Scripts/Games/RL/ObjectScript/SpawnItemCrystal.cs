using Roguelike.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace LUP.RL
{
    public class SpawnItemCrystal : MonoBehaviour
    {
        public float flyingSpeed = 10f;

        [HideInInspector]
        public int itemID = 0;

        [HideInInspector]
        public int amount = 0;

        public RLDropItemType itemType;

        [HideInInspector]
        public bool bIsStageCleared = false;

        [HideInInspector]
        public Transform target;

        private ItemSpawner spawnPool;

        public SphereCollider sphereCollider;
        public Rigidbody crystalRigidBody;

        public float flyingPower = 80;
        public float dropPower = 30;

        private bool bIsDropeed = false;

        // Update is called once per frame
        void Update()
        {
            if (bIsStageCleared == false)
                return;

            if (target != null)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    flyingSpeed * Time.deltaTime
                );
            }
        }

        public void SetSpawnItemInfo(RLDropItemType type, int ItemID, int gainedAmount, Transform playerPos, ItemSpawner spawner)
        {
            itemType = type;
            itemID = ItemID;
            target = playerPos;

            spawnPool = spawner;

            amount = gainedAmount;

            if(sphereCollider != null && crystalRigidBody != null)
            {
                sphereCollider.isTrigger = false;

                crystalRigidBody.isKinematic = true;
                crystalRigidBody.useGravity = true;
            }

            bIsDropeed = false;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                spawnPool.ReturnCrystal(itemType, this.gameObject);
            }
        }

        public void CallRoomCleared()
        {
            

            if (sphereCollider != null && crystalRigidBody != null)
            {
                sphereCollider.isTrigger = true;

                crystalRigidBody.isKinematic = false;
                crystalRigidBody.useGravity = false;
            }

            if(bIsDropeed)
            {
                StartCoroutine(FlyAndActive(0.2f));
            }

            else
            {
                bIsStageCleared = true;
            }
                
        }

        public void PopupBounce(float horizontalForceMin = 20f,float horizontalForceMax = 30f,float verticalForceMin = 80f,float verticalForceMax = 90f)
        {
            if (crystalRigidBody == null)
                return;

            crystalRigidBody.isKinematic = false;
            crystalRigidBody.useGravity = true;

            //crystalRigidBody.linearVelocity = Vector3.zero;
            crystalRigidBody.angularVelocity = Vector3.zero;

            Vector3 horizontalDir = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            ).normalized;

            float horizontalForce = Random.Range(horizontalForceMin, horizontalForceMax);
            float verticalForce = Random.Range(verticalForceMin, verticalForceMax);

            crystalRigidBody.AddForce(horizontalDir * horizontalForce, ForceMode.Impulse);
            crystalRigidBody.AddForce(Vector3.up * verticalForce, ForceMode.Impulse);

            StartCoroutine(WaitUntilPeakAndCall(0.5f));
        }

        IEnumerator WaitUntilPeakAndCall(float delay)
        {
            yield return new WaitForSeconds(delay);

            KillVelocityNDrop();
        }

        IEnumerator FlyAndActive(float delay)
        {
            crystalRigidBody.useGravity = false;

            crystalRigidBody.AddForce(Vector3.up * flyingPower, ForceMode.Impulse);
            yield return new WaitForSeconds(delay);

            KillVelocityNStop();
        }

        void KillVelocityNDrop()
        {
            crystalRigidBody.linearVelocity = new Vector3(crystalRigidBody.linearVelocity.x, 0.0f, crystalRigidBody.linearVelocity.y);
            crystalRigidBody.AddForce(Vector3.down * dropPower, ForceMode.Impulse);

            bIsDropeed = true;
        }

        void KillVelocityNStop()
        {
            crystalRigidBody.linearVelocity = Vector3.zero;

            StartCoroutine(WaitForFloating(0.4f));
        }

        IEnumerator WaitForFloating(float delay)
        {
            yield return new WaitForSeconds(delay);

            bIsStageCleared = true;
        }
    }
}

