using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Utils/TriggerEnter2D")]
    [RequireComponent(typeof(Collider2D))]
    public class Trigger2D : MonoBehaviour
    {
        /// <summary>
        /// 触发的类型
        /// </summary>
        public enum TriggerType { Tag, Layer, TagOrLayer, TagAndLayer }
        [Header("TriggerType")]
        [Tooltip("选择tag，layer当目标进入范围时触发")]
        public TriggerType triggerType;
        [Tooltip("tag列表")]
        public List<string> tags;
        public LayerMask layerMasks;

        public bool IsTrigger { get; private set; }

        public GameObject Target;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (triggerType)
            {
                case TriggerType.Tag:
                    if (ListTags(collision.tag))
                    {
                        Trigger(collision);
                    }
                    break;
                case TriggerType.Layer:
                    if (layerMasks.Contains(collision.gameObject.layer))
                    {
                        Trigger(collision);
                    }
                    break;
                case TriggerType.TagOrLayer:
                    if (layerMasks.Contains(collision.gameObject.layer) || ListTags(collision.tag))
                    {
                        Trigger(collision);
                    }
                    break;
                case TriggerType.TagAndLayer:
                    if (layerMasks.Contains(collision.gameObject.layer) && ListTags(collision.tag))
                    {
                        Trigger(collision);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Trigger(Collider2D collision)
        {
            IsTrigger = true;
            Target = collision.gameObject;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            IsTrigger = false;
            Target = null;
        }

        private bool ListTags(string tag)
        {
            bool result = false;
            foreach (var item in tags)
            {
                if (item.Equals(tag))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}