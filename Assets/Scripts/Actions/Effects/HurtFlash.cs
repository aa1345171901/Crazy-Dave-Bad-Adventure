using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtFlash : MonoBehaviour
{

	const int DefaultFlashCount = 3;

	public int flashCount = DefaultFlashCount;
	public Color flashColor = Color.white;
	public Color enchantedColor = new Color(1, 0, 1);
	[Range(1f / 120f, 1f / 15f)]
	public float interval = 1f / 60f;
	public string fillPhaseProperty = "_FillPhase";
	public string fillColorProperty = "_FillColor";

	public Material[] materials;

	private bool isEnchanted;

	public void Flash()
	{	if (isEnchanted)
			return;
		if (materials.Length == 0)
			materials = GetComponent<MeshRenderer>().materials;

		StartCoroutine(FlashRoutine());
	}

	public void BeEnchanted()
    {
		if(materials.Length == 0)
			materials = GetComponent<MeshRenderer>().materials;
		StopCoroutine(FlashRoutine());
		isEnchanted = true;
		int fillPhase = Shader.PropertyToID(fillPhaseProperty);
		int fillColor = Shader.PropertyToID(fillColorProperty);
		for (int j = 0; j < materials.Length; j++)
		{
			materials[j].SetColor(fillColor, enchantedColor);
			materials[j].SetFloat(fillPhase, 0.5f);
		}
	}

	public void BeResume()
	{
		isEnchanted = false;
		int fillPhase = Shader.PropertyToID(fillPhaseProperty);
		for (int i = 0; i < flashCount; i++)
		{
			for (int j = 0; j < materials.Length; j++)
			{
				materials[j].SetFloat(fillPhase, 0f);
			}
		}
	}

	IEnumerator FlashRoutine()
	{
		if (flashCount < 0) flashCount = DefaultFlashCount;
		int fillPhase = Shader.PropertyToID(fillPhaseProperty);
		int fillColor = Shader.PropertyToID(fillColorProperty);

		var wait = new WaitForSeconds(interval);

		for (int i = 0; i < flashCount; i++)
		{
            for (int j = 0; j < materials.Length; j++)
            {
				materials[j].SetColor(fillColor, flashColor);
				materials[j].SetFloat(fillPhase, 1f);
			}
			yield return wait;

			for (int j = 0; j < materials.Length; j++)
			{
				materials[j].SetFloat(fillPhase, 0f);
			}
			yield return wait;
		}

		yield return null;
	}
}
