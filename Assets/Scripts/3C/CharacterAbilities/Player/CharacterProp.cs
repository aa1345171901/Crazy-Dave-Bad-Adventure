using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    public class CharacterProp : CharacterAbility
    {
        public Transform elfRoot;

        List<Transform> elfPos = new List<Transform>();
        int gunNum;

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

        public override void Reuse()
        {
            base.Reuse();
            gunNum = 0;
            elfPos.Clear();
            for (int i = 0; i < elfRoot.childCount; i++)
            {
                elfPos.Add(elfRoot.GetChild(i));
            }
        }

        public int GetGunLevel()
        {
            int level = 1;
            var countDict = new Dictionary<int, int>();
            foreach (var item in ShopManager.Instance.GetPurchaseTypeList(PropType.Gun))
            {
                if (!countDict.ContainsKey(item.quality))
                    countDict[item.quality] = 0;
                countDict[item.quality]++;
            }
            if (countDict.ContainsKey(3) && countDict[3] > gunNum)
            {
                gunNum++;
                return 3;
            }
            if (countDict.ContainsKey(2) && countDict[2] > gunNum)
            {
                gunNum++;
                return 2;
            }
            return level;
        }
    }
}
