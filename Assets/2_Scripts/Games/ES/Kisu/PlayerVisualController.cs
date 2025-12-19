using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace LUP.ES
{
    public class PlayerVisualController : MonoBehaviour
    {
        private Renderer[] renderersToFade;

        [Tooltip("투명화 대상에서 제외할 렌더러의 레이어 이름입니다 (예: Minimap)")]
        public string excludeLayerName = "MVPDisplayModel";

        private void Awake()
        {
            Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
            List<Renderer> validRenderers = new List<Renderer>();

            // 제외할 레이어 ID를 찾습니다.
            int excludeLayer = LayerMask.NameToLayer(excludeLayerName);

            // 렌더러 필터링 (미니맵 제외)
            if (excludeLayer == -1)
            {
                Debug.LogWarning($"Layer '{excludeLayerName}' not found. Check spelling.");
                renderersToFade = allRenderers;
            }
            else
            {
                foreach (Renderer r in allRenderers)
                {
                    if (r.gameObject.layer != excludeLayer)
                    {
                        validRenderers.Add(r);
                    }
                }
                renderersToFade = validRenderers.ToArray();
            }

            // 머티리얼을 투명 모드로 설정 (Awake에서 한 번만 설정)
            foreach (Renderer r in renderersToFade)
            {
                // .material을 사용해 런타임 인스턴스에 접근합니다.
                Material mat = r.material;

                // URP Lit 머티리얼을 Transparent 모드로 설정
                mat.SetFloat("_Surface", 1);
                mat.SetFloat("_Blend", 1);
                mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                mat.SetFloat("_ZWrite", 0);
                mat.renderQueue = (int)RenderQueue.Transparent;
                mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
        }

        public void SetTransparent(bool isTransparent)
        {
            float targetAlpha = isTransparent ? 0.4f : 1.0f;

            for (int i = 0; i < renderersToFade.Length; i++)
            {
                Material mat = renderersToFade[i].material;

                // 머티리얼 전체를 교체하는 대신, 색상(알파 값)만 변경합니다.
                Color c = mat.color;

                // 부드러운 전환을 원하면 Lerp 등으로 감싸서 Update()에서 실행할 수 있습니다.
                c.a = targetAlpha;

                mat.color = c;
            }
        }
    }
}