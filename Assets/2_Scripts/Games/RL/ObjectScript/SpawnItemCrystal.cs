using UnityEngine;
using Roguelike.Define;
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
            bIsStageCleared = true;

            if (sphereCollider != null && crystalRigidBody != null)
            {
                sphereCollider.isTrigger = true;

                crystalRigidBody.isKinematic = false;
                crystalRigidBody.useGravity = false;
            }
                
        }

        public void PopupBounce(float horizontalForceMin = 8f,float horizontalForceMax = 12f,float verticalForceMin = 15f,float verticalForceMax = 22f)
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
        }
    }
}

