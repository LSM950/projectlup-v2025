using LUP.DSG;
using LUP.DSG.Utils.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
public enum ActionEffect
{
    None,

    Attack_Melee_OneHanded,
    Attack_Melee_TwoHanded,
    Attack_Magic,
    Attack_Gun_Rifle,
    Attack_Throw,
    Attack_Skill_Test,

    GetHit_Melee_OneHanded,
    GetHit_Melee_TwoHandedd,
    GetHit_Magic,
    GetHit_Gun_Rifle,
    GetHit_Throw,
    GetHit_Skill_Test,

    Get_Burn,
    Get_Poison,
    Get_AttackBuff,
    Get_AttackDebuff,

    ThrowBullet,
    MagicBullet
}

[System.Serializable]
public struct EffectParticlePair
{
    public ActionEffect name;
    public GameObject particlePrefab;
}

public class EffectPool : MonoBehaviour
{
    [SerializeField] private EffectParticlePair[] effectpairs;
    private Dictionary<ActionEffect, Queue<GameObject>> vfxPool = new Dictionary<ActionEffect, Queue<GameObject>>();
    public Vector3 offset = new Vector3(0, 0.05f, 0);
    private void Awake()
    {
        foreach(var pair in effectpairs)
        {
            var q = new Queue<GameObject>();
            vfxPool[pair.name] = q;
        }

        StartCoroutine(TryLoading());
    }

    private IEnumerator TryLoading()
    {
        foreach(var pair in effectpairs)
        {
            GameObject eff;
            eff = Instantiate(pair.particlePrefab);

            if(eff != null)
            {

                eff.SetActive(true);
                ParticleSystem ps = eff.GetComponent<ParticleSystem>();

                ps.Play();
                yield return null;
                ps.Stop();

                eff.SetActive(false);
                vfxPool[pair.name].Enqueue(eff);
            }
        }
    }

    public void PlayVFX(ActionEffect effectname, Vector3 position, Quaternion rotation, float lifeTime = 1.0f)
    {
        GameObject eff;
        if (vfxPool[effectname].Count > 0)
        {
            eff = vfxPool[effectname].Dequeue();
        }
        else
        {
            eff = Instantiate(System.Array.Find(effectpairs, s => s.name == effectname).particlePrefab);
        }

        if (eff == null)
            return;

        eff.transform.SetPositionAndRotation(position + offset, rotation);
        eff.SetActive(true);

        StartCoroutine(ReturnVFX(eff, effectname, lifeTime));
    }

    private IEnumerator ReturnVFX(GameObject go, ActionEffect id, float  lifeTime = 1.0f)
    {
        yield return new WaitForSeconds(lifeTime);
        go.SetActive(false);
        vfxPool[id].Enqueue(go);
    }

    public ActionEffect GetAttackEffectByGetHITEffect(ActionEffect attackeffect)
    {
        switch(attackeffect)
        {
            case ActionEffect.Attack_Melee_OneHanded:
                return ActionEffect.GetHit_Melee_OneHanded;
            case ActionEffect.Attack_Melee_TwoHanded:
                return ActionEffect.GetHit_Melee_TwoHandedd;
            case ActionEffect.Attack_Gun_Rifle:
                return ActionEffect.GetHit_Gun_Rifle;
            case ActionEffect.Attack_Magic:
                return ActionEffect.GetHit_Magic;
            case ActionEffect.Attack_Throw:
                return ActionEffect.GetHit_Throw;

            default:
                return ActionEffect.None;
        }    
    }
}
