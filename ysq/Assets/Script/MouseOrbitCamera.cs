using System;
using UnityEngine;

public class MouseOrbitCamera : MonoBehaviour
{
	public float ZoomSpeed = 10f;

	public float MovingSpeed = 0.5f;

	public float RotateSpeed = 1f;

	public float distance = 30f;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButton(2))
		{
			float num = Input.GetAxis("Mouse X") * this.MovingSpeed;
			float num2 = Input.GetAxis("Mouse Y") * this.MovingSpeed;
			Quaternion rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			base.transform.position = rotation * new Vector3(-num, 0f, -num2) + base.transform.position;
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			float num3 = -Input.GetAxis("Mouse ScrollWheel") * this.distance;
			base.transform.Translate(0f, 0f, -num3);
			this.distance += num3;
		}
		if (Input.GetMouseButton(1))
		{
			float yAngle = Input.GetAxis("Mouse X") * this.RotateSpeed;
			float xAngle = -Input.GetAxis("Mouse Y") * this.RotateSpeed;
			Vector3 b = base.transform.rotation * new Vector3(0f, 0f, this.distance) + base.transform.position;
			base.transform.Rotate(0f, yAngle, 0f, Space.World);
			base.transform.Rotate(xAngle, 0f, 0f);
			base.transform.position = base.transform.rotation * new Vector3(0f, 0f, -this.distance) + b;
		}
	}
}
