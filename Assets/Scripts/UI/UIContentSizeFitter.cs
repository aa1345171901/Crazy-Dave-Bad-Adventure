using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    internal static class SetPropertyUtility
    {
        public static bool SetColor(ref Color currentValue, Color newValue)
        {
            if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b &&
                currentValue.a == newValue.a)
                return false;

            currentValue = newValue;
            return true;
        }

        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;
            return true;
        }

        public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return false;

            currentValue = newValue;
            return true;
        }
    }

    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    /// <summary>
    /// Resizes a RectTransform to fit the size of its content.
    /// </summary>
    /// <remarks>
    /// The ContentSizeFitter can be used on GameObjects that have one or more ILayoutElement components, such as Text, Image, HorizontalLayoutGroup, VerticalLayoutGroup, and GridLayoutGroup.
    /// </remarks>
    public class UIContentSizeFitter : UIBehaviour, ILayoutSelfController
    {
        /// <summary>
        /// The size fit modes avaliable to use.
        /// </summary>
        public enum FitMode
        {
            /// <summary>
            /// Don't perform any resizing.
            /// </summary>
            Unconstrained,

            /// <summary>
            /// Resize to the minimum size of the content.
            /// </summary>
            MinSize,

            MaxSize,

            /// <summary>
            /// Resize to the preferred size of the content.
            /// </summary>
            PreferredSize
        }

        [SerializeField] protected FitMode m_HorizontalFit = FitMode.Unconstrained;
        [SerializeField] protected int m_MaxHorizontal = 900;
        [SerializeField] protected int m_MaxVertical = 100;

        /// <summary>
        /// The fit mode to use to determine the width.
        /// </summary>
        public FitMode horizontalFit
        {
            get { return m_HorizontalFit; }
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_HorizontalFit, value)) SetDirty();
            }
        }

        [SerializeField] protected FitMode m_VerticalFit = FitMode.Unconstrained;

        /// <summary>
        /// The fit mode to use to determine the height.
        /// </summary>
        public FitMode verticalFit
        {
            get { return m_VerticalFit; }
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_VerticalFit, value)) SetDirty();
            }
        }

        [System.NonSerialized] private RectTransform m_Rect;

        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        // field is never assigned warning
#pragma warning disable 649
        private DrivenRectTransformTracker m_Tracker;
#pragma warning restore 649

        protected UIContentSizeFitter()
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }

        private void HandleSelfFittingAlongAxis(int axis)
        {
            FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
            if (fitting == FitMode.Unconstrained)
            {
                // Keep a reference to the tracked transform, but don't control its properties:
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
                return;
            }

            m_Tracker.Add(this, rectTransform,
                (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));

            // Set size to min or preferred size
            if (fitting == FitMode.MinSize)
            {
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis,
                    LayoutUtility.GetMinSize(m_Rect, axis));
            }
            else if (fitting == FitMode.MaxSize)
            {
                //增加maxSize判断返回
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, GetMaxSize(m_Rect, axis));
            }
            else
            {
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis,
                    LayoutUtility.GetPreferredSize(m_Rect, axis));
            }
        }

        /// <summary>
        /// 增加maxSize获取
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        private float GetMaxSize(RectTransform rect, int axis)
        {
            var size = axis == 0 ? LayoutUtility.GetPreferredWidth(rect) : LayoutUtility.GetPreferredHeight(rect);

            if (axis == 0)
            {
                if (size > m_MaxHorizontal)
                {
                    return m_MaxHorizontal;
                }
            }
            else
            {
                if (size > m_MaxVertical)
                {
                    return m_MaxVertical;
                }
            }

            return size;
        }


        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }

        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }

#endif
    }
}
