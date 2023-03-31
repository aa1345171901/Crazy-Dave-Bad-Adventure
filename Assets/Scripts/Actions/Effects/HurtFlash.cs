using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtFlash : MonoBehaviour
{

	const int DefaultFlashCount = 3;

	public int flashCount = DefaultFlashCount;
	public Color flashColor = Color.white;
	[Range(1f / 120f, 1f / 15f)]
	public float interval = 1f / 60f;
	public string fillPhaseProperty = "_FillPhase";
	public string fillColorProperty = "_FillColor";

	public Material[] materials;

	public void Flash()
	{
			materials = GetComponent<MeshRenderer>().materials;

		StartCoroutine(FlashRoutine());
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
