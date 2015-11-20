using System;
using UnityEngine;

public class GUIIndicator : MonoBehaviour
{
	private UISprite text;

	private float timer;

	private bool flag;

	private void Awake()
	{
		base.transform.localPosition = new Vector3(0f, 0f, 3000f);
		this.text = base.transform.Find("Text").gameObject.GetComponent<UISprite>();
	}

	private void OnEnable()
	{
		this.timer = 0f;
		this.flag = false;
		this.text.enabled = false;
	}

	private void Update()
	{
		this.timer += Time.deltaTime;
		if (this.timer > 0.5f && !this.flag)
		{
			this.flag = true;
			this.text.enabled = true;
		}
	}
}
