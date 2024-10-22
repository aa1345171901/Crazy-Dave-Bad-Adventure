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
        private int purpleGarlicCount;
        private float timer;

        private readonly float howManySecondsRecoverty = 10;
        private readonly string fireName = "fire";
        private readonly string purpleGarlicName = "purpleGarlic";

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
            purpleGarlicCount = ShopManager.Instance.PurchasePropCount(purpleGarlicName);
            fire.SetActive(purchaseFireCount > 0);
            lifeRecoveryTimer = 0;
            timer = 0;
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            if (character.IsDead || !GameManager.Instance.PlayerEnable)
                return;

            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timer = 0;
                if (purchaseFireCount > 0)
                {
                    var finalDamage = character.Health.health - purchaseFireCount <= 0 ? character.Health.health - 1: purchaseFireCount;
                    GameManager.Instance.DoDamage(finalDamage, DamageType.Fire);
                }
                if (purpleGarlicCount > 0)
                {
                    var finalDamage = character.Health.health - purpleGarlicCount <= 0 ? character.Health.health - 1 : purpleGarlicCount;
                    GameManager.Instance.DoDamage(finalDamage, DamageType.Fire);
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
                float unitTime = howManySecondsRecoverty / lifeRecoveryValue;
                int mul = 1;
                if (unitTime < 1)
                {
                    mul = Mathf.CeilToInt(1 / unitTime);
                    if (lifeRecoveryTimer >= 1)
                    {
                        lifeRecoveryTimer = 0;
                        GameManager.Instance.AddHP(1 * mul);
                    }
                }
                else if (lifeRecoveryTimer >= unitTime)
                {
                    lifeRecoveryTimer = 0;
                    GameManager.Instance.AddHP(1);
                }
            }
        }
    }
}
