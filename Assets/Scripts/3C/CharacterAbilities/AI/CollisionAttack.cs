using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/AI/Ability/CollisionAttack")]
    public class CollisionAttack : MonoBehaviour
    {
        public Trigger2D trigger2D;
        public int Damage = 1;

        private void Update()
        {
            if (trigger2D.IsTrigger)
            {
                GameManager.Instance.DoDamage(Damage);
            }
        }
    }
}
