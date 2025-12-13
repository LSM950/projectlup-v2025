using UnityEngine;
namespace LUP.RL
{
    [CreateAssetMenu(fileName = "EnemyData",menuName = "EnemyData")]
    public class EnemyDefinition : ScriptableObject
    {
        public EnemyType type;
        public GameObject prefab;
    }
}
