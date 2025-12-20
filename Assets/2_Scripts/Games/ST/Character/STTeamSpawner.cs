using UnityEngine;

namespace LUP.ST
{
    public class STTeamSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints; // 5개
        private bool spawned = false;

        public void Spawn(ShootingRuntimeData srd)
        {
            if (spawned) return;
            spawned = true;

            if (srd == null)
            {
                Debug.LogError("[STTeamSpawner] ShootingRuntimeData is null.");
                return;
            }

            if (spawnPoints == null || spawnPoints.Length < 5)
            {
                Debug.LogError("[STTeamSpawner] spawnPoints must have 5 elements.");
                return;
            }

            var team = srd.Team; // STCharacterData[5] 라는 전제
            if (team == null || team.Length < 5)
            {
                Debug.LogError("[STTeamSpawner] Team array missing/invalid.");
                return;
            }

            for (int i = 0; i < 5; i++)
            {
                var data = team[i];
                if (data == null || data.prefab == null)
                {
                    Debug.LogWarning($"[STTeamSpawner] Slot {i} data/prefab null.");
                    continue;
                }

                var sp = spawnPoints[i];
                var go = Instantiate(data.prefab, sp.position, sp.rotation);
                go.name = $"{data.name}_Slot{i}";
            }

            Debug.Log("[STTeamSpawner] Spawn complete.");
        }
    }
}
