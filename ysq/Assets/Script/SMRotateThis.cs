using System;
using UnityEngine;

[AddComponentMenu("Game/FX/SMRotateThis")]
public class SMRotateThis : MonoBehaviour
{
	public Vector3 rotationVector = new Vector3(90f, 0f, 0f);

	private void Update()
	{
		base.transform.Rotate(this.rotationVector * Time.deltaTime);
	}
}
