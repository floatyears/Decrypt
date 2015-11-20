using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class GUIAlchemy : GameUISession
{
	private UILabel diamond;

	private UILabel gold;

	private UILabel times;

	private GameObject noCount;

	private GameObject batchAck;

	private UILabel batchTimes;

	private UILabel batchCost;

	private UILabel batchValue;

	private int batCount;

	private GameObject record;

	private GUID2MRecordTable mGUID2MRecordTable;

	private GameObject mGold;

	private UILabel mGoldTxt;

	private UILabel mMultipleLabel;

	private GameObject mFanBeiTag;

	private D2MCriticalLayer mD2MCriticalLayer;

	private StringBuilder mStringBuilder = new StringBuilder();

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		Globals.Instance.CliSession.Register(221, new ClientSession.MsgHandler(this.OnMsgDiamond2Money));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		Globals.Instance.CliSession.Unregister(221, new ClientSession.MsgHandler(this.OnMsgDiamond2Money));
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.FindGameObject("Info/Diamond", null);
		this.diamond = gameObject.GetComponent<UILabel>();
		gameObject = base.FindGameObject("Info/Gold", null);
		this.gold = gameObject.GetComponent<UILabel>();
		Transform transform = base.transform.Find("Info");
		this.mFanBeiTag = transform.Find("Tag").gameObject;
		this.mMultipleLabel = this.mFanBeiTag.transform.Find("Label").GetComponent<UILabel>();
		gameObject = base.FindGameObject("times", null);
		this.times = gameObject.GetComponent<UILabel>();
		base.SetLabelLocalText("winBG/Title", "alchemyTitle", null);
		base.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), null);
		base.RegisterClickEvent("winBG/closeBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), null);
		gameObject = base.RegisterClickEvent("AlchemyBtn", new UIEventListener.VoidDelegate(this.OnAlchemyClick), null);
		base.SetLabelLocalText("Label", "d2m", gameObject);
		gameObject = base.RegisterClickEvent("BatchAlchemyBtn", new UIEventListener.VoidDelegate(this.OnBatchAlchemyClick), null);
		base.SetLabelLocalText("Label", "d2mBat", gameObject);
		this.noCount = base.FindGameObject("NoCount", null);
		gameObject = base.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnNoCountCancelClick), this.noCount);
		base.SetLabelLocalText("Label", "d2mCancel", gameObject);
		gameObject = base.RegisterClickEvent("Vip", new UIEventListener.VoidDelegate(this.OnVipClick), this.noCount);
		base.SetLabelLocalText("Label", "d2mVIP", gameObject);
		this.batchAck = base.FindGameObject("BatchAck", null);
		gameObject = base.FindGameObject("Label0", this.batchAck);
		this.batchTimes = gameObject.GetComponent<UILabel>();
		gameObject = base.FindGameObject("Label1", this.batchAck);
		base.SetLabelLocalText(gameObject, "d2mBatCost");
		gameObject = base.FindGameObject("Label1/Diamond/Label", this.batchAck);
		this.batchCost = gameObject.GetComponent<UILabel>();
		gameObject = base.FindGameObject("Label2", this.batchAck);
		base.SetLabelLocalText(gameObject, "d2mBatValue");
		gameObject = base.FindGameObject("Label2/Gold/Label", this.batchAck);
		this.batchValue = gameObject.GetComponent<UILabel>();
		gameObject = base.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnBatCancelClick), this.batchAck);
		base.SetLabelLocalText("Label", "d2mCancel", gameObject);
		gameObject = base.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnBatOKClick), this.batchAck);
		base.SetLabelLocalText("Label", "d2mOK", gameObject);
		this.record = base.FindGameObject("Record", null);
		this.record.SetActive(false);
		this.mGUID2MRecordTable = this.record.transform.Find("contentsPanel/recordContents").gameObject.AddComponent<GUID2MRecordTable>();
		this.mGUID2MRecordTable.maxPerLine = 1;
		this.mGUID2MRecordTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mGUID2MRecordTable.cellWidth = 540f;
		this.mGUID2MRecordTable.cellHeight = 45f;
		this.mGUID2MRecordTable.scrollBar = this.record.transform.Find("contentsScrollBar").GetComponent<UIScrollBar>();
		this.mGUID2MRecordTable.bgScrollView = this.record.GetComponent<UIDragScrollView>();
		this.mGold = base.transform.Find("goldGo").gameObject;
		this.mGoldTxt = this.mGold.transform.Find("gold/Label").GetComponent<UILabel>();
		this.mGold.SetActive(false);
		this.mD2MCriticalLayer = base.transform.Find("ResultEffect").gameObject.AddComponent<D2MCriticalLayer>();
		this.mD2MCriticalLayer.InitWithBaseLayer();
		this.mD2MCriticalLayer.gameObject.SetActive(false);
		this.RefreshMultiple();
		this.Refresh();
		GameUITools.PlayOpenWindowAnim(base.gameObject.transform, null, true);
	}

	private void RefreshMultiple()
	{
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(6);
		if (valueMod != null)
		{
			this.mFanBeiTag.SetActive(true);
			this.mMultipleLabel.text = Singleton<StringManager>.Instance.GetString("ShopCommon5", new object[]
			{
				valueMod.Value1 / 100
			});
		}
		else
		{
			this.mFanBeiTag.SetActive(false);
		}
	}

	private void CloseAnimation()
	{
		GameUITools.PlayCloseWindowAnim(base.gameObject.transform, new TweenDelegate.TweenCallback(this.OnCloseAnimEnd), true);
	}

	private void OnCloseAnimEnd()
	{
		this.mD2MCriticalLayer.DestroyEffect();
		base.Close();
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.CloseAnimation();
	}

	private void OnAlchemyClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.D2MCount >= this.GetMaxD2MCount())
		{
			this.noCount.SetActive(true);
			return;
		}
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.GetD2MCost(), 0))
		{
			return;
		}
		this.batCount = 0;
		MC2S_Diamond2Money mC2S_Diamond2Money = new MC2S_Diamond2Money();
		mC2S_Diamond2Money.Count = 1;
		Globals.Instance.CliSession.Send(220, mC2S_Diamond2Money);
	}

	private void OnBatchAlchemyClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.batCount = this.GetMaxD2MCount() - Globals.Instance.Player.Data.D2MCount;
		if (this.batCount <= 0)
		{
			this.noCount.SetActive(true);
			return;
		}
		if (this.batCount > 5)
		{
			this.batCount = 5;
		}
		int num = Globals.Instance.Player.Data.D2MCount + 1;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.batCount; i++)
		{
			D2MInfo info = Globals.Instance.AttDB.D2MDict.GetInfo(num + i);
			if (info == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("D2MDict.GetInfo error, ID = {0}", num + i)
				});
			}
			else
			{
				num2 += info.D2MCost;
				num3 += info.D2MValue;
			}
		}
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, num2, 0))
		{
			return;
		}
		this.batchAck.SetActive(true);
		this.batchTimes.text = Singleton<StringManager>.Instance.GetString("d2mBatTimes", new object[]
		{
			this.batCount
		});
		this.batchCost.text = num2.ToString();
		this.batchValue.text = num3.ToString();
	}

	private void OnNoCountCancelClick(GameObject go)
	{
		this.noCount.SetActive(false);
	}

	private void OnVipClick(GameObject go)
	{
		this.noCount.SetActive(false);
		GameUIVip.OpenVIP(0);
		this.CloseAnimation();
	}

	private void OnBatCancelClick(GameObject go)
	{
		this.batchAck.SetActive(false);
	}

	private void OnBatOKClick(GameObject go)
	{
		this.batchAck.SetActive(false);
		MC2S_Diamond2Money mC2S_Diamond2Money = new MC2S_Diamond2Money();
		mC2S_Diamond2Money.Count = this.batCount;
		Globals.Instance.CliSession.Send(220, mC2S_Diamond2Money);
	}

	public void Refresh()
	{
		int num = Globals.Instance.Player.Data.D2MCount + 1;
		if (num > 300)
		{
			num = 300;
		}
		D2MInfo info = Globals.Instance.AttDB.D2MDict.GetInfo(num);
		if (info == null)
		{
			info = Globals.Instance.AttDB.D2MDict.GetInfo(num - 1);
		}
		if (info != null)
		{
			this.diamond.text = info.D2MCost.ToString();
			Sequence sequence = new Sequence();
			sequence.Append(HOTween.To(this.diamond.gameObject.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
			sequence.Append(HOTween.To(this.diamond.gameObject.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one)));
			sequence.Play();
			this.gold.text = info.D2MValue.ToString();
			Sequence sequence2 = new Sequence();
			sequence2.Append(HOTween.To(this.gold.gameObject.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
			sequence2.Append(HOTween.To(this.gold.gameObject.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one)));
			sequence2.Play();
			int maxD2MCount = this.GetMaxD2MCount();
			this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
			this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("d2mTimes")).Append(" ");
			int num2 = maxD2MCount - Globals.Instance.Player.Data.D2MCount;
			if (num2 <= 0)
			{
				this.mStringBuilder.Append("[ff0000]").Append(0).Append("[-]");
			}
			else
			{
				this.mStringBuilder.Append(num2);
			}
			this.mStringBuilder.Append("/").Append(maxD2MCount);
			this.times.text = this.mStringBuilder.ToString();
		}
	}

	public int GetMaxD2MCount()
	{
		VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
		if (vipLevelInfo == null)
		{
			return 0;
		}
		return vipLevelInfo.D2MCount;
	}

	public int GetD2MCost()
	{
		int num = Globals.Instance.Player.Data.D2MCount + 1;
		if (num > 300)
		{
			num = 300;
		}
		D2MInfo info = Globals.Instance.AttDB.D2MDict.GetInfo(num);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("D2MDict.GetInfo error, ID = {0}", num)
			});
			return 0;
		}
		return info.D2MCost;
	}

	public int GetD2MValue()
	{
		int num = Globals.Instance.Player.Data.D2MCount + 1;
		if (num > 300)
		{
			num = 300;
		}
		D2MInfo info = Globals.Instance.AttDB.D2MDict.GetInfo(num);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("D2MDict.GetInfo error, ID = {0}", num)
			});
			return 0;
		}
		return info.D2MValue;
	}

	public void OnMsgDiamond2Money(MemoryStream stream)
	{
		MS2C_Diamond2Money mS2C_Diamond2Money = Serializer.NonGeneric.Deserialize(typeof(MS2C_Diamond2Money), stream) as MS2C_Diamond2Money;
		if (mS2C_Diamond2Money.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_Diamond2Money.Result);
			return;
		}
		if (!this.record.activeInHierarchy)
		{
			this.record.SetActive(true);
		}
		base.StartCoroutine(this.ShowResultEffect(mS2C_Diamond2Money.Data));
	}

	[DebuggerHidden]
	private IEnumerator ShowResultEffect(List<D2MData> data)
	{
        return null;
        //GUIAlchemy.<ShowResultEffect>c__Iterator65 <ShowResultEffect>c__Iterator = new GUIAlchemy.<ShowResultEffect>c__Iterator65();
        //<ShowResultEffect>c__Iterator.data = data;
        //<ShowResultEffect>c__Iterator.<$>data = data;
        //<ShowResultEffect>c__Iterator.<>f__this = this;
        //return <ShowResultEffect>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator UpdateScrollBar()
	{
        return null;
        //GUIAlchemy.<UpdateScrollBar>c__Iterator66 <UpdateScrollBar>c__Iterator = new GUIAlchemy.<UpdateScrollBar>c__Iterator66();
        //<UpdateScrollBar>c__Iterator.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator;
	}

	public void AddOneRecord(D2MData data)
	{
		this.mGUID2MRecordTable.AddData(new GUID2MRecordData(data));
		if (data.Crit > 1)
		{
			this.mD2MCriticalLayer.Refresh(data.Crit);
			this.mD2MCriticalLayer.PlayD2MCriticalAnim();
		}
		this.mGold.SetActive(true);
		this.mGoldTxt.text = data.Money.ToString();
		Sequence sequence = new Sequence();
		sequence.Append(HOTween.To(this.mGold.transform, 0f, new TweenParms().Prop("localPosition", Vector3.zero)));
		sequence.Insert(0f, HOTween.To(this.mGold.transform, 0.01f, new TweenParms().Prop("localScale", new Vector3(3f, 3f, 3f))));
		sequence.Append(HOTween.To(this.mGold.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInBack)));
		sequence.AppendInterval(0.5f);
		sequence.Append(HOTween.To(this.mGold.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(0f, 120f, 0f)).Ease(EaseType.EaseOutQuad)));
		sequence.Append(HOTween.To(this.mGold.transform, 0f, new TweenParms().Prop("localScale", Vector3.zero)));
		sequence.Play();
		this.mGUID2MRecordTable.Reposition(true);
		if (this.mGUID2MRecordTable.mDatas.Count >= 3)
		{
			base.StartCoroutine(this.UpdateScrollBar());
		}
	}
}
