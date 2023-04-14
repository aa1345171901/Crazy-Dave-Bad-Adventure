using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/AI/Ability/CollisionAttack")]
    public class CollisionAttack : CharacterAbility
    {
        public Trigger2D trigger2D;
        public int Damage = 1;

        public int realDamage;

        protected override void Initialization()
        {
            base.Initialization();
            Reuse();
        }

        public override void Reuse()
        {
            base.Reuse();
            int waveIndex = LevelManager.Instance.IndexWave + 1;
            if (waveIndex < 4)
            {
                this.realDamage = Damage;
            }
            else if (waveIndex < 9)
            {
                this.realDamage = (int)(Damage * (waveIndex / 4f));
            }
            else if (waveIndex < 13)
            {
                this.realDamage = (int)((Damage + 1) * (waveIndex / 3f));
            }
            else if (waveIndex < 17)
            {
                this.realDamage = (int)((Damage + 2) * (waveIndex / 1.5f));
            }
            else if (waveIndex < 21)
            {
                this.realDamage = (Damage + 3) * waveIndex;
            }
            else
            {
                this.realDamage = (int)((Damage + 4) * waveIndex * 1.5f);
            }
        }

        private void Update()
        {
            if (trigger2D.IsTrigger && trigger2D.Target == GameManager.Instance.Player.gameObject)
            {
                GameManager.Instance.DoDamage(realDamage);
            }
        }
    }
}
