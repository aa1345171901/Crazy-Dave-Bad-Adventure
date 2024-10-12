
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 跟踪rect大小位置
/// </summary>
[ExecuteInEditMode]
public class UITargetSizeDelta : UIBehaviour, ILayoutController
{
    public RectTransform rect;
    public Vector2Int offsetSize;
    public Vector2Int minSize;
    public Vector2Int maxSize;
    public bool isTargetX = true;
    public bool isTargetY = true;

    private RectTransform re;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        UpdatePosi();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdatePosi();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosi();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdatePosi();
    }

    public void SetLayoutHorizontal()
    {
        UpdatePosi();
    }

    public void SetLayoutVertical()
    {
        UpdatePosi();
    }

    public void UpdatePosi()
    {
        if (rect == null)
        {
            return;
        }
        if (re == null)
        {
            re = GetComponent<RectTransform>();
        }
        Vector2Int size = new Vector2Int((int)rect.sizeDelta.x, (int)rect.sizeDelta.y) + offsetSize;
        size.x = isTargetX ? size.x : (int)re.sizeDelta.x;
        size.y = isTargetY ? size.y : (int)re.sizeDelta.y;

        if (size.x != (int)re.sizeDelta.x || size.y != (int)re.sizeDelta.y || !Application.isPlaying)
        {
            if (minSize.x != 0)
            {
                size.x = Mathf.Clamp(size.x, minSize.x, int.MaxValue);
            }
            if (minSize.y != 0)
            {
                size.y = Mathf.Clamp(size.y, minSize.y, int.MaxValue);
            }
            if (maxSize.x != 0)
            {
                size.x = Mathf.Clamp(size.x, 0, maxSize.x);
            }
            if (maxSize.y != 0)
            {
                size.y = Mathf.Clamp(size.y, 0, maxSize.y);
            }

            re.sizeDelta = size;
        }
    }
}