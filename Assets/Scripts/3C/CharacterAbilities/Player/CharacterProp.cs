using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    public class CharacterProp : CharacterAbility
    {
        public Transform elfRoot;

        List<Transform> elfPos = new List<Transform>();

        /// <summary>
        /// 不攻击同一个目标
        /// </summary>
        public List<GameObject> targets = new List<GameObject>();

        protected override void Initialization()
        {
            base.Initialization();
            
            for (int i = 0; i < elfRoot.childCount; i++)
            {
                elfPos.Add(elfRoot.GetChild(i));
            }
        }

        public Transform GetElfRandomPos()
        {
            if (elfPos.Count == 0)
                return null;
            var index = Random.Range(0, elfPos.Count);
            var pos = elfPos[index];
            elfPos.RemoveAt(index);
            return pos;
        }
    }
}
