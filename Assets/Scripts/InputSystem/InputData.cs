using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [Serializable]
    public class Key
    {
        public string name;
        public KeyCode keyCode;
        public KeyCodeType keyType = KeyCodeType.Once;
        public bool enable = true;
        [ReadOnly]
        public bool IsDown = false;
        [HideInInspector]
        public Action Down;
    }

    public enum KeyCodeType
    {
        Once,
        Continuity
    }

    [Serializable]
    public class ValueKey
    {
        public string name;
        [Tooltip("当前按住时间")]
        public float currValue = 0;
        [Tooltip("最短按住时间")]
        public float minTime = 0.1f;
        [Tooltip("最长按住时间")]
        public float maxTime = 0.3f;
        public KeyCode keyCode;
        [ReadOnly]
        public bool enable = true;
        [HideInInspector]
        public Action started;
        [HideInInspector]
        public Action performed;
        [HideInInspector]
        public Action canceled;
    }

    [Serializable]
    public class AxisKey
    {
        public string name;
        public float value = 0;
        public float addSpeed = 5;
        public Vector2 range = new Vector2(-1, 1);
        public KeyCode min, max;
        [ReadOnly]
        public bool enable = true;
        public void SetKey(KeyCode a, KeyCode b)
        {
            min = a;
            max = b;
        }
    }

    [CreateAssetMenu]
    public class InputData : ScriptableObject
    {
        public List<Key> keys = new List<Key>() { new Key() };
        public List<ValueKey> valueKeys = new List<ValueKey>() { new ValueKey() };
        public List<AxisKey> axisKeys = new List<AxisKey>() { new AxisKey() };

        public void SetKey(string name, KeyCode key)
        {
            Key key1 = GetKey(name);
            if (key1 != null)
            {
                key1.keyCode = key;
            }
        }

        public void SetAxisKey(string name, KeyCode a, KeyCode b)
        {
            AxisKey axisKey = GetAxisKey(name);
            if (axisKey != null)
            {
                axisKey.SetKey(a, b);
            }
        }

        public void SetValueKey(string name, KeyCode key)
        {
            ValueKey valueKey = GetValueKey(name);
            if (valueKey != null)
            {
                valueKey.keyCode = key;
            }
        }

        public ValueKey GetValueKey(string name)
        {
            int len = valueKeys.Count;
            for (int i = 0; i < len; i++)
            {
                if (valueKeys[i].name == name)
                {
                    return valueKeys[i];
                }
            }
            Debug.LogError("ValueKey:" + name + "不存在");
            return null;
        }

        public AxisKey GetAxisKey(string name)
        {
            int len = axisKeys.Count;
            for (int i = 0; i < len; i++)
            {
                if (axisKeys[i].name == name)
                {

                    return axisKeys[i];
                }
            }
            Debug.LogError("AxisKey:" + name + "不存在");
            return null;
        }

        public Key GetKey(string name)
        {
            int len = keys.Count;
            for (int i = 0; i < len; i++)
            {
                if (keys[i].name == name)
                {
                    return keys[i];
                }
            }
            Debug.LogError("Key:" + name + "不存在");
            return null;
        }

        public float Axis(string name)
        {
            AxisKey axisKey = GetAxisKey(name);
            if (axisKey != null)
            {
                return axisKey.value;
            }
            return 0;
        }

        public bool GetKeyDown(string name)
        {
            Key key = GetKey(name);
            if (key != null)
            {
                return key.IsDown;
            }
            return false;
        }

        public ValueKey GetValue(string name)
        {
            ValueKey valueKey = GetValueKey(name);
            if (valueKey != null)
            {
                return valueKey;
            }
            return null;
        }

        public void SetKeyEnable(string name, bool enable)
        {
            Key key = GetKey(name);
            if (key != null)
            {
                key.enable = enable;
                key.IsDown = false;
            }
        }

        public void SetValueKeyEnable(string name, bool enable)
        {
            ValueKey valueKey = GetValueKey(name);
            if (valueKey != null)
            {
                valueKey.enable = enable;
                valueKey.currValue = 0;
            }
        }

        public void SetAxisKeyEnable(string name, bool enable)
        {
            AxisKey axisKey = GetAxisKey(name);
            if (axisKey != null)
            {
                axisKey.enable = enable;
                axisKey.value = 0;
            }
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void AcceptInput()
        {
            UpdateKeys();
            UpdateValueKey();
            UpdateAllAxisKey();
        }

        private void UpdateKeys()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].enable)
                {
                    keys[i].IsDown = false;
                    switch (keys[i].keyType)
                    {
                        case KeyCodeType.Once:
                            if (Input.GetKeyDown(keys[i].keyCode))
                            {
                                keys[i].IsDown = true;
                                keys[i].Down?.Invoke();
                            }
                            break;
                        case KeyCodeType.Continuity:
                            if (Input.GetKeyDown(keys[i].keyCode))
                            {
                                keys[i].Down?.Invoke();
                            }
                            if (Input.GetKey(keys[i].keyCode))
                            {
                                keys[i].IsDown = true;
                            }
                            break;
                    }
                }
            }
        }

        private void UpdateValueKey()
        {
            int len = valueKeys.Count;
            for (int i = 0; i < len; i++)
            {
                if (valueKeys[i].enable)
                {
                    if (Input.GetKeyDown(valueKeys[i].keyCode))
                    {
                        valueKeys[i].started?.Invoke();
                    }
                    if (Input.GetKey(valueKeys[i].keyCode) && valueKeys[i].currValue < valueKeys[i].maxTime)
                    {
                        valueKeys[i].currValue = Mathf.Clamp(valueKeys[i].currValue + Time.deltaTime, 0, valueKeys[i].maxTime);
                    }
                    else
                    {
                        valueKeys[i].performed?.Invoke();
                    }
                    if (Input.GetKeyUp(valueKeys[i].keyCode))
                    {
                        valueKeys[i].canceled?.Invoke();
                        valueKeys[i].currValue = 0;
                    }
                }
            }
        }

        private void UpdateAllAxisKey()
        {
            int len = axisKeys.Count;
            for (int i = 0; i < len; i++)
            {
                UpdateAxisKey(axisKeys[i]);
            }
        }

        private void UpdateAxisKey(AxisKey axisKey)
        {
            if (!axisKey.enable)
                return;
            if (Input.GetKey(axisKey.min) || Input.GetKey(axisKey.max))
            {
                if (Input.GetKey(axisKey.min))
                    axisKey.value = Mathf.Clamp(axisKey.value - axisKey.addSpeed * Time.deltaTime, axisKey.range.x, -0.1f);
                else if (Input.GetKey(axisKey.max))
                    axisKey.value = Mathf.Clamp(axisKey.value + axisKey.addSpeed * Time.deltaTime, 0.1f, axisKey.range.y);
            }
            else
            {
                axisKey.value = Mathf.Lerp(axisKey.value, 0, Time.deltaTime * axisKey.addSpeed * 4);
                if (Mathf.Abs(axisKey.value) < 0.1f)
                {
                    if (Input.GetKey(axisKey.min))
                        axisKey.value = -0.1f;
                    else if (Input.GetKey(axisKey.max))
                        axisKey.value = 0.1f;
                    else
                        axisKey.value = 0f;
                }
            }
        }
    }
}