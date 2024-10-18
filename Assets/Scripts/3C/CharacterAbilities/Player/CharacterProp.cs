using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    public class CharacterProp : CharacterAbility
    {
        public Transform elfRoot;

        List<Transform> elfPos = new List<Transform>();

        public List<GameObject> targets = new List<GameObject>();

        protected override void Initialization()
        {
            base.Initialization();
            
            for (int i = 0; i < elfRoot.childCount; i++)
            {
                elfPos.Add(elfRoot.GetChild(i));
            }
        }

        public Transform GetRandomPos()
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
