using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace LUP.ES
{
    public class WallHider : MonoBehaviour
    {
        public Transform target; // 플레이어 Transform
        public LayerMask wallLayer; // 벽 레이어 마스크

        private Camera mainCam;
        private List<Renderer> currentlyHidden = new List<Renderer>(); // 현재 투명화된 벽 Renderer 목록

        void Start()
        {
            mainCam = Camera.main;
        }

        void Update()
        {
            if (target == null || mainCam == null) return;

            Vector3 origin = mainCam.transform.position;
            Vector3 direction = target.position - mainCam.transform.position;
            float distance = direction.magnitude;

            // 이전에 가려졌던 벽들을 원래대로 복원 (투명화 해제)
            RestoreHiddenObjects();

            // 디버그 시각화에 사용할 레이 색상 초기화 (기본: 녹색)
            Color rayColor = Color.green;

            RaycastHit[] hits = Physics.RaycastAll(
               origin,
               direction.normalized,
               distance,
               wallLayer
           );

            // 카메라에서 플레이어까지 레이캐스트 발사. wallLayer에 해당하는 오브젝트만 감지
            if (hits.Length > 0)
            {
                // 벽을 감지했습니다. 레이 색상을 빨간색으로 변경
                rayColor = Color.red;

                foreach (RaycastHit hit in hits)
                {
                    Renderer wallRenderer = hit.collider.GetComponent<Renderer>();

                    if (wallRenderer != null && !currentlyHidden.Contains(wallRenderer))
                    {
                        MakeTransparent(wallRenderer);
                        currentlyHidden.Add(wallRenderer);
                    }
                }
            }

            //  Scene 뷰에 레이를 그립니다. (Game 뷰에서는 보이지 않습니다)
            // 레이가 벽에 맞았다면 빨간색, 벽을 통과했다면 녹색으로 보입니다.
            Debug.DrawRay(mainCam.transform.position, direction, rayColor);
        }

        // --- 투명화 및 복원 로직 (이 부분은 재질 설정에 따라 동작) ---

        void MakeTransparent(Renderer renderer)
        {
            // 주의: 벽 재질의 Rendering Mode가 Fade나 Transparent로 설정되어 있어야 합니다.
            Color color = renderer.material.color;
            color.a = 0.3f; // 30% 불투명
            renderer.material.color = color;
        }

        void RestoreHiddenObjects()
        {
            foreach (Renderer renderer in currentlyHidden)
            {
                if (renderer != null)
                {
                    Color color = renderer.material.color;
                    color.a = 1.0f; // 완전 불투명
                    renderer.material.color = color;
                }
            }
            currentlyHidden.Clear();
        }
    }
}