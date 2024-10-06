using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Character/Ability/LifeRecovery")]
    public class CharacterLifeRecovery : CharacterAbility
    {
        public GameObject fire;

        private int lifeRecoveryValue;
        private float lifeRecoveryTimer;

        private int purchaseFireCount;
        private float fireTimer;

        private readonly float howManySecondsRecoverty = 10;
        private readonly string fireName = "fire";

        public int PlanternLifeRecovery{ get; set; }

        protected override void Initialization()
        {
            base.Initialization();
            Reuse();
        }

        public override void Reuse()
        {
            base.Reuse();
            lifeRecoveryValue = GameManager.Instance.UserData.LifeRecovery;
            purchaseFireCount = ShopManager.Instance.PurchasePropCount(fireName);
            fire.SetActive(purchaseFireCount > 0);
            lifeRecoveryTimer = 0;
            fireTimer = 0;
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            if (character.IsDead || !GameManager.Instance.PlayerEnable)
                return;
            if (purchaseFireCount > 0)
            {
                fireTimer += Time.deltaTime;
                if (fireTimer >= 1f / purchaseFireCount)
                {
                    fireTimer = 0;
                    GameManager.Instance.DoDamage(1, DamageType.Fire);
                }
            }

            if (character.FacingDirection == FacingDirections.Left)
                lifeRecoveryValue = GameManager.Instance.UserData.LifeRecovery + GardenManager.Instance.BloverEffect.BloverResume;
            else
                lifeRecoveryValue = GameManager.Instance.UserData.LifeRecovery;
            lifeRecoveryValue += PlanternLifeRecovery;
            if (lifeRecoveryValue > 0)
            {
                lifeRecoveryTimer += Time.deltaTime;
                if (lifeRecoveryTimer >= howManySecondsRecoverty / lifeRecoveryValue)
                {
                    lifeRecoveryTimer = 0;
                    GameManager.Instance.AddHP(1);
                }
            }
        }
    }
}
