using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TopDownPlate;

public class GameTouch : EventTrigger
{
	private GameObject rocker;
	private Vector3 rockerPos;
	private Vector3 rockerScreenPos;
	private float radius = 125.0f;

	void Start()
	{
		rocker = this.transform.GetChild(0).gameObject;  // 获取摇杆


		// 将遥感的中心点的坐标按照本摇杆范围进行重置坐标  将坐标取到  用于回放中心点的坐标
		rockerPos = rocker.transform.position;
		rockerScreenPos = UIManager.Instance.UICamera.WorldToScreenPoint(rockerPos);
	}

	// 开始在屏幕上滑动
	public override void OnDrag(PointerEventData data)
	{
		var mousePos = UIManager.Instance.UICamera.ScreenToWorldPoint(data.position);
		Vector3 dir = new Vector3(mousePos.x, mousePos.y, rockerPos.z) - rockerPos;
		if (Vector3.Distance(rockerScreenPos, Input.mousePosition) <= radius)
		{
			rocker.transform.position = new Vector3(mousePos.x, mousePos.y, rockerPos.z);
		}
		else
		{
			var screenPos = rockerScreenPos + dir.normalized * radius * UIManager.Instance.CanvasScaleFactor;
			rocker.transform.position = UIManager.Instance.UICamera.ScreenToWorldPoint(screenPos);
		}
		dir = dir.normalized;
		InputManager.GetAxisKey("Movement").value = dir.x;
		InputManager.GetAxisKey("Vertical").value = dir.y;
	}

	///这个事件停止在屏幕上滑动
	public override void OnEndDrag(PointerEventData data)
	{
		rocker.transform.position = rockerPos;
	}

	public override void OnBeginDrag(PointerEventData data)
	{
		Debug.Log("OnBeginDrag called.");
	}
	public override void OnDrop(PointerEventData data)
	{
		Debug.Log("OnDrop called.");
	}
	public override void OnCancel(BaseEventData data)
	{
		Debug.Log("OnCancel called.");
	}

	public override void OnDeselect(BaseEventData data)
	{
		Debug.Log("OnDeselect called.");
	}

	public override void OnInitializePotentialDrag(PointerEventData data)
	{
		Debug.Log("OnInitializePotentialDrag called.");
	}

	public override void OnMove(AxisEventData data)
	{
		Debug.Log("OnMove called.");
	}

	public override void OnPointerClick(PointerEventData data)
	{
		Debug.Log("OnPointerClick called.");
	}

	public override void OnPointerDown(PointerEventData data)
	{
		Debug.Log("OnPointerDown called.");
	}

	public override void OnPointerEnter(PointerEventData data)
	{
		Debug.Log("OnPointerEnter called.");
	}

	public override void OnPointerExit(PointerEventData data)
	{
		Debug.Log("OnPointerExit called.");
	}

	public override void OnPointerUp(PointerEventData data)
	{
		Debug.Log("OnPointerUp called.");
		InputManager.GetAxisKey("Movement").value = 0;
		InputManager.GetAxisKey("Vertical").value = 0;
	}

	public override void OnScroll(PointerEventData data)
	{
		Debug.Log("OnScroll called.");
	}

	public override void OnSelect(BaseEventData data)
	{
		Debug.Log("OnSelect called.");
	}

	public override void OnSubmit(BaseEventData data)
	{
		Debug.Log("OnSubmit called.");
	}

	public override void OnUpdateSelected(BaseEventData data)
	{
		Debug.Log("OnUpdateSelected called.");
	}
}