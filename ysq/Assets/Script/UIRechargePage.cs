using Att;
using System;
using UnityEngine;

public class UIRechargePage : MonoBehaviour
{
	private UIScrollView rechargeScrollView;

	private UITable rechargeTable;

	private void Awake()
	{
		this.rechargeScrollView = base.transform.FindChild("contentsPanel").GetComponent<UIScrollView>();
		this.rechargeTable = this.rechargeScrollView.transform.FindChild("infoContents").gameObject.AddComponent<UITable>();
		this.rechargeTable.columns = 3;
		this.rechargeTable.direction = UITable.Direction.Down;
		this.rechargeTable.sorting = UITable.Sorting.None;
		this.rechargeTable.hideInactive = true;
		this.rechargeTable.keepWithinPanel = true;
		this.rechargeTable.padding = new Vector2(6f, 0f);
		int privilege = Globals.Instance.CliSession.Privilege;
		foreach (PayInfo current in Globals.Instance.AttDB.PayDict.Values)
		{
			if (!current.Test || privilege > 0)
			{
				if (current.Type == 0)
				{
					UIRechargeItem uIRechargeItem = UIRechargeItem.CreateItem(this.rechargeTable.transform, this.rechargeScrollView);
					uIRechargeItem.Init(current);
				}
				else
				{
					UIMonthCard uIMonthCard = UIMonthCard.CreateItem(this.rechargeTable.transform, this.rechargeScrollView);
					uIMonthCard.Init(current);
				}
			}
		}
	}

	public void OnDataUpdate()
	{
		this.rechargeTable.gameObject.BroadcastMessage("Refresh", SendMessageOptions.DontRequireReceiver);
	}
}
