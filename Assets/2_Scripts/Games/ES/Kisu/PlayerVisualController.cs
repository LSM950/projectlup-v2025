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
            int excludeLayer = LayerMask.NameToLayer(excludeLayerName);

            foreach (Renderer r in allRenderers)
            {
                if (excludeLayer == -1 || r.gameObject.layer != excludeLayer)
                {
                    validRenderers.Add(r);
                }
            }
            renderersToFade = validRenderers.ToArray();
        }

        public void SetTransparent(bool isTransparent)
        {
            float targetAlpha = isTransparent ? 0.4f : 1.0f;

            foreach (Renderer r in renderersToFade)
            {
                Material mat = r.material;

                if(mat.HasProperty("_BaseColor"))
                {
                    Color c = mat.GetColor("_BaseColor");
                    c.a = targetAlpha;
                    mat.SetColor("_BaseColor", c);
                }
                else if (mat.HasProperty("_Color"))
                {
                    Color c = mat.GetColor("_Color");
                    c.a = targetAlpha;
                    mat.SetColor("_Color", c);
                }

                if (isTransparent)
                {
                    mat.SetFloat("_Surface", 1);
                    mat.SetFloat("_Blend", 0);
                    mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_Zwrite", 0);
                    mat.renderQueue = (int)RenderQueue.Transparent;
                    mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    mat.DisableKeyword("_SURFACE_TYPE_ALPHATEST_ON");
                }
                else
                {
                    mat.SetFloat("_Surface", 0);
                    mat.SetInt("_SrcBlend", (int)BlendMode.One);
                    mat.SetInt("_DstBlend", (int)BlendMode.Zero);
                    mat.SetInt("_ZWrite", 1);
                    mat.renderQueue = (int)RenderQueue.Geometry;
                    mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                }
            }
        }
    }
}