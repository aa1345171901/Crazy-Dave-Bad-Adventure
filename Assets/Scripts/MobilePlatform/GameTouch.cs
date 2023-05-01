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
		rocker = this.transform.GetChild(0).gameObject;  // ��ȡҡ��


		// ��ң�е����ĵ�����갴�ձ�ҡ�˷�Χ������������  ������ȡ��  ���ڻط����ĵ������
		rockerPos = rocker.transform.position;
		rockerScreenPos = UIManager.Instance.UICamera.WorldToScreenPoint(rockerPos);
	}

	// ��ʼ����Ļ�ϻ���
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

	///����¼�ֹͣ����Ļ�ϻ���
	public override void OnEndDrag(PointerEventData data)
	{
		rocker.transform.position = rockerPos;
	}

	public override void OnBeginDrag(PointerEventData data)
	{

	}
	public override void OnDrop(PointerEventData data)
	{

	}
	public override void OnCancel(BaseEventData data)
	{

	}

	public override void OnDeselect(BaseEventData data)
	{

	}

	public override void OnInitializePotentialDrag(PointerEventData data)
	{

	}

	public override void OnMove(AxisEventData data)
	{

	}

	public override void OnPointerClick(PointerEventData data)
	{

	}

	public override void OnPointerDown(PointerEventData data)
	{

	}

	public override void OnPointerEnter(PointerEventData data)
	{

	}

	public override void OnPointerExit(PointerEventData data)
	{

	}

	public override void OnPointerUp(PointerEventData data)
	{
		InputManager.GetAxisKey("Movement").value = 0;
		InputManager.GetAxisKey("Vertical").value = 0;
	}

	public override void OnScroll(PointerEventData data)
	{

	}

	public override void OnSelect(BaseEventData data)
	{

	}

	public override void OnSubmit(BaseEventData data)
	{

	}

	public override void OnUpdateSelected(BaseEventData data)
	{

	}
}