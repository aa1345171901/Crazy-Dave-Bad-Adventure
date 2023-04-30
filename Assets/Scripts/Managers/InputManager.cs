using System;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Managers/InputManager")]
    public class InputManager : MonoBehaviour
    {
        public string path = "InputData";
        static InputData inputData;

        private void Awake()
        {
            inputData = Resources.Load<InputData>(path);
            if (inputData == null)
            {
                throw new Exception("inputData:无法加载");
            }
        }

        private void Update()
        {
#if !UNITY_ANDROID
            inputData.AcceptInput();
#endif
        }

        public static float GetAxis(string name)
        {
            return inputData.Axis(name);
        }

        public static void SetAxisKeyEnable(string name, bool enable = true)
        {
            inputData.SetAxisKeyEnable(name, enable);
        }

        public static bool GetKeyDown(string name)
        {
            return inputData.GetKeyDown(name);
        }

        public static Key GetKey(string name)
        {
            return inputData.GetKey(name);
        }

        public static ValueKey GetValueKey(string name)
        {
            return inputData.GetValue(name);
        }

        public static AxisKey GetAxisKey(string name)
        {
            return inputData.GetAxisKey(name);
        }

        #region 改键
        public static void SetKey(string name, KeyCode key)
        {
            inputData.SetKey(name, key);
        }

        public static void SetValueKey(string name, KeyCode key)
        {
            inputData.SetValueKey(name, key);
        }

        public static void SetAxisKey(string name, KeyCode key, bool isMin)
        {
            if (isMin)
                inputData.SetAxisKey(name, key, KeyCode.None);
            else
                inputData.SetAxisKey(name, KeyCode.None, key);
        }
        #endregion
    }
}
