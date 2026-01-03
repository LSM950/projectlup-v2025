using LUP.DSG.Utils.Enums;
using UnityEngine;

namespace LUP.DSG
{
    public class AttackBuff : IStatusEffect
    {
        public EOperationType operationType;
        private ActionEffect buffdebuffEffect;
        private float playerAttack;
        public AttackBuff(EOperationType Type, float Amount, int Turns)
            : base(EStatusEffectType.AttackBuff,Type, Amount, Turns)
        {
            operationType = Type;
        }
        public override void Apply(Character C)
        {
            playerAttack = C.characterData.attack;
            float result = 0;
            Operation.TryEval(operationType, playerAttack, amount,out result);
            C.characterData.attack = result;
            
            if(operationType == EOperationType.Minus)
            {
                buffdebuffEffect = ActionEffect.Get_AttackDebuff;
            }
            else if(operationType == EOperationType.Plus)
            {
                buffdebuffEffect = ActionEffect.Get_AttackBuff;
            }
        }
        public override void Turn(Character C) { C.ActioneffectPool.PlayVFX(buffdebuffEffect, C.transform.position, C.transform.rotation, 1.5f); }
        public override void Remove(Character C)
        {
            C.characterData.attack = playerAttack;
        }
    }
}