using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class PillageRecordItem : UICustomGridItem
{
	private PillageRecordItemData RecordData;

	private UILabel Desc;

	private UILabel TimeTip;

	private UIButton pk;

	public void Init()
	{
		this.CreateObjets();
	}

	public override void Refresh(object _data)
	{
		if (this.RecordData == _data)
		{
			return;
		}
		this.RecordData = (PillageRecordItemData)_data;
		this.ShowRecordData(this.RecordData.RecordData);
	}

	private void ShowRecordData(PillageRecord _data)
	{
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(_data.ItemID);
		if (info == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.TimeTip.text = PillageRecordItem.FormatTimeStr(Globals.Instance.Player.GetTimeStamp() - _data.TimeStamp);
		string @string = Singleton<StringManager>.Instance.GetString("Pillage2");
		if (!string.IsNullOrEmpty(@string))
		{
			this.Desc.text = string.Format(@string, Tools.GetItemQualityColorHex(info.Quality), info.Name, _data.Name);
		}
		base.gameObject.SetActive(true);
	}

	public static string FormatTimeStr(int timecount)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (timecount > 86400)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt3", new object[]
			{
				timecount / 86400
			}));
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt10"));
			return stringBuilder.ToString();
		}
		if (timecount > 3600)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt2", new object[]
			{
				timecount / 3600
			}));
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt10"));
			return stringBuilder.ToString();
		}
		if (timecount > 60)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt1", new object[]
			{
				timecount / 60
			}));
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt10"));
			return stringBuilder.ToString();
		}
		stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt0", new object[]
		{
			timecount
		}));
		stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt10"));
		return stringBuilder.ToString();
	}

	private void CreateObjets()
	{
		this.Desc = base.transform.FindChild("desc").GetComponent<UILabel>();
		this.TimeTip = base.transform.FindChild("time").GetComponent<UILabel>();
		this.TimeTip.color = Color.white;
		this.pk = base.transform.FindChild("pk").GetComponent<UIButton>();
		UIEventListener expr_71 = UIEventListener.Get(this.pk.gameObject);
		expr_71.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_71.onClick, new UIEventListener.VoidDelegate(this.OnPkTraget));
	}

	private void OnPkTraget(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.RecordData == null)
		{
			return;
		}
		GUIPillageScene.RequestPvpPillageStart(this.RecordData.RecordData.PlayerID, this.RecordData.RecordData.ItemID, false);
	}
}
