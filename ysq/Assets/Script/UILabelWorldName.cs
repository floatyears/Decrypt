using System;
using UnityEngine;

[AddComponentMenu("NJG MiniMap/NGUI/Interaction/Label World Name"), RequireComponent(typeof(UILabel))]
public class UILabelWorldName : MonoBehaviour
{
	private UILabel label;

	private void Awake()
	{
		this.label = base.GetComponent<UILabel>();
		if (NJGMap.instance != null)
		{
			NJGMap expr_21 = NJGMap.instance;
			expr_21.onWorldNameChanged = (Action<string>)Delegate.Combine(expr_21.onWorldNameChanged, new Action<string>(this.OnNameChanged));
		}
	}

	private void OnNameChanged(string worldName)
	{
		this.label.color = NJGMap.instance.zoneColor;
		this.label.text = worldName;
	}
}
