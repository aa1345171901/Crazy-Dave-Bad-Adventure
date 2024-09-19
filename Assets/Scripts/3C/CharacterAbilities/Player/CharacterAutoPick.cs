using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Character/Ability/CharacterAutoPick")]
    public class CharacterAutoPick : CharacterAbility
    {
        public float defualtPickRange = 1f;
        public LayerMask moneyClickLayer;
        private float pickRange;

        protected override void Initialization()
        {
            base.Initialization();
            Reuse();
        }

        public override void Reuse()
        {
            base.Reuse();
            pickRange = defualtPickRange * (100 + GameManager.Instance.UserData.Range) / 100f;
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            if (character.IsDead || GameManager.Instance.IsDaytime)
                return;
            var colliders = Physics2D.OverlapCircleAll(this.transform.position, pickRange, moneyClickLayer);
            foreach (var item in colliders)
            {
                if (item.isTrigger)
                {
                    var money = item.GetComponent<MoneyClick>();
                    if (money != null)
                        money.OnClick();
                }
            }
        }
    }
}
