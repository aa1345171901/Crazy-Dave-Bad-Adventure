using UnityEngine;

namespace TopDownPlate
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask This, LayerMask target)
        {
            int value = 1 << target;
            if ((This & value) == value)
                return true;
            return false;
        }
    }
}
