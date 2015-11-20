using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class HUDTextManager : MonoBehaviour
{
	private List<ShowData> textLists = new List<ShowData>();

	public int maxCountPreFrame = 2;

	public void RequestShow(ActorController actor, EShowType showType, int value = 0, string text = null, int type = 0)
	{
		ShowData showData = new ShowData();
		showData.actor = actor;
		showData.showType = showType;
		showData.type = type;
		showData.value = value;
		showData.text = text;
		this.textLists.Add(showData);
	}

	private void FixedUpdate()
	{
		int num = 0;
		for (int i = 0; i < this.textLists.Count; i++)
		{
			this.Show(this.textLists[i]);
			num++;
			if (num >= this.maxCountPreFrame)
			{
				break;
			}
		}
		if (num > 0)
		{
			this.textLists.RemoveRange(0, num);
		}
	}

	private void Show(ShowData data)
	{
		if (data == null || data.actor == null)
		{
			return;
		}
		switch (data.showType)
		{
		case EShowType.EST_Text:
			data.actor.UIText.AddText(data.text, data.type);
			break;
		case EShowType.EST_Damage:
			data.actor.UIText.AddDamage(data.value, data.type);
			break;
		case EShowType.EST_SkillDamage:
			data.actor.UIText.AddSkillDamage(data.value);
			break;
		case EShowType.EST_CriticalDamage:
			data.actor.UIText.AddCriticalDamage(data.value);
			break;
		case EShowType.EST_Heal:
			data.actor.UIText.AddHeal(data.value);
			break;
		}
	}

	public void Clear()
	{
		this.textLists.Clear();
	}
}
