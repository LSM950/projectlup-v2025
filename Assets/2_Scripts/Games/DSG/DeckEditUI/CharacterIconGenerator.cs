using LUP.DSG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OpenCvSharp.ML.DTrees;


namespace LUP.DSG
{
    public class CharacterIconGenerator : MonoBehaviour
    {
        [Header("Preview Setup")]
        [SerializeField] private Camera previewCamera;
        [SerializeField] private Transform characterPivot;
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private RuntimeAnimatorController controller;

        public static event Action<int, Sprite> OnCharacterIconReady;

        private readonly HashSet<int> _inFlightCharacterIds = new HashSet<int>();

        private AnimatorOverrideController portraitOverride;

        public IEnumerator GenerateIconRoutine(DeckStrategyStage stage, int cacheKeyCharacterId, int modelId)
        {
            if (CharacterIconCache.TryGetByCharacterId(cacheKeyCharacterId, out _))
            {
                Debug.Log($"[CharacterIconGenerator] characterId {cacheKeyCharacterId} 는 이미 캐시에 있음, 스킵");
                yield break;
            }

            if (stage == null)
            {
                Debug.LogError("[CharacterIconGenerator] stage 가 null 입니다.");
                yield break;
            }

            GameObject prefab = stage.GetCharacterPrefab(modelId);
            if (prefab == null)
            {
                Debug.LogError($"[CharacterIconGenerator] modelId {modelId} 프리팹을 찾지 못했습니다.");
                yield break;
            }

            Debug.Log($"[CharacterIconGenerator] Instantiate prefab {prefab.name} for characterId={cacheKeyCharacterId}, modelId={modelId}");

            GameObject instance = Instantiate(prefab, characterPivot.position, characterPivot.rotation);

            instance.GetComponent<Animator>().runtimeAnimatorController = controller;

            Transform transform = instance.GetComponent<PortraitTransform>().portraitTransform;
            previewCamera.transform.position = new Vector3(previewCamera.transform.position.x, transform.position.y, previewCamera.transform.position.z);

            yield return new WaitForEndOfFrame();

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;

            Texture2D tex = new Texture2D(
                renderTexture.width,
                renderTexture.height,
                TextureFormat.RGBA32,
                false
            );
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            RenderTexture.active = currentRT;

            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            CharacterIconCache.SetByCharacterId(cacheKeyCharacterId, sprite);
            CharacterIconCache.SetByModelId(modelId, sprite);

            Destroy(instance);
        }
        public IEnumerator GenerateIconByModelRoutine(DeckStrategyStage stage, int modelId)
        {
            // modelId 기준으로 이미 캐시에 있으면 스킵
            if (CharacterIconCache.TryGetByModelId(modelId, out _))
            {
                yield break;
            }

            if (stage == null)
            {
                yield break;
            }

            GameObject prefab = stage.GetCharacterPrefab(modelId);
            if (prefab == null)
            {
                yield break;
            }

            var instance = Instantiate(prefab, characterPivot.position, characterPivot.rotation);

            var animator = instance.GetComponent<Animator>();
            if (animator != null)
                animator.runtimeAnimatorController = controller;

            var portraitTf = instance.GetComponent<PortraitTransform>();
            if (portraitTf != null)
            {
                previewCamera.transform.position =
                    new Vector3(previewCamera.transform.position.x,
                                portraitTf.portraitTransform.position.y,
                                previewCamera.transform.position.z);
            }

            yield return new WaitForEndOfFrame();

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;

            Texture2D tex = new Texture2D(
                renderTexture.width,
                renderTexture.height,
                TextureFormat.RGBA32,
                false
            );
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            RenderTexture.active = currentRT;

            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            CharacterIconCache.SetByModelId(modelId, sprite);
            Destroy(instance);
        }
       
    }
}