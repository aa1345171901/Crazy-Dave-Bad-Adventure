using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UITargetPosi : UIBehaviour, ILayoutController
{
    public Transform target;
    public Vector2 offsetPosi;

    private Vector2 lastStartPosi;
    private Vector2 lastEndPosi;

    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
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

    void Update()
    {
        UpdatePosi();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdatePosi();
    }

    /// <summary>
    /// 跟踪对象
    /// </summary>
    public void Target(Transform target)
    {
        this.target = target;
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
        if (target == null)
        {
            return;
        }
        if (Mathf.Approximately(transform.position.x, lastStartPosi.x) && Mathf.Approximately(transform.position.y, lastStartPosi.y) &&
            Mathf.Approximately(target.position.x, lastEndPosi.x) && Mathf.Approximately(target.position.y, lastEndPosi.y))
        {
            return;
        }
        lastStartPosi = transform.position;
        lastEndPosi = target.position;

        var vec = target.transform.localToWorldMatrix.MultiplyPoint(new Vector3(offsetPosi.x, offsetPosi.y, 0));
        vec.z = transform.position.z;
        transform.position = vec;
    }
}