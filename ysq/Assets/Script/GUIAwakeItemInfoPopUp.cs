using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using System.Text;
using UnityEngine;

public class GUIAwakeItemInfoPopUp : GameUIBasePopup
{
	public enum EOpenType
	{
		EOT_View,
		EOT_Equip,
		EOT_Get
	}

	private UIEventListener.VoidDelegate OnEquipClickEvent;

	private GameObject mInfoLayer;

	private UILabel mName;

	private UILabel mNumValue;

	private UILabel mAtts;

	private UILabel mValues;

	private UILabel mDesc;

	private GameObject mOKBtn;

	private GameObject mEquipBtn;

	private GameObject mGetBtn;

	private GameObject mCreateBtn;

	private GameObject mCreateBtnEffect;

	private AwakeItemDetailLayer mDetailLayer;

	private ItemInfo mItemInfo;

	private GUIAwakeItemInfoPopUp.EOpenType mType;

	public float Time0 = 0.3f;

	public AnimationCurve OpenCurve;

	public AnimationCurve CloseCurve;

	private bool isOpen;

	private StringBuilder tempSb = new StringBuilder();

	public static void Show(ItemInfo info, GUIAwakeItemInfoPopUp.EOpenType type, UIEventListener.VoidDelegate cb)
	{
		if (info == null)
		{
			global::Debug.LogErrorFormat("ItemInfo info is null", new object[0]);
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIAwakeItemInfoPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(info, type, cb);
	}

	public static void TryClose()
	{
		if (GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GUIAwakeItemInfoPopUp)
		{
			GameUIPopupManager.GetInstance().PopState(false, null);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mInfoLayer = GameUITools.FindGameObject("Info", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mInfoLayer);
		this.mName = GameUITools.FindUILabel("Name", this.mInfoLayer);
		this.mNumValue = GameUITools.FindUILabel("Num/Value", this.mInfoLayer);
		this.mAtts = GameUITools.FindUILabel("Atts", this.mInfoLayer);
		this.mValues = GameUITools.FindUILabel("Values", this.mInfoLayer);
		this.mDesc = GameUITools.FindUILabel("Desc", this.mInfoLayer);
		this.mOKBtn = GameUITools.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnOkClick), this.mInfoLayer);
		this.mEquipBtn = GameUITools.RegisterClickEvent("Equip", new UIEventListener.VoidDelegate(this.OnEquipClick), this.mInfoLayer);
		this.mGetBtn = GameUITools.RegisterClickEvent("Get", new UIEventListener.VoidDelegate(this.OnGetClick), this.mInfoLayer);
		this.mCreateBtn = GameUITools.RegisterClickEvent("Create", new UIEventListener.VoidDelegate(this.OnCreateClick), this.mInfoLayer);
		this.mCreateBtnEffect = GameUITools.FindGameObject("Effect", this.mCreateBtn);
		this.mCreateBtnEffect.gameObject.SetActive(false);
		this.mOKBtn.SetActive(false);
		this.mEquipBtn.SetActive(false);
		this.mGetBtn.SetActive(false);
		this.mCreateBtn.SetActive(false);
		this.mDetailLayer = GameUITools.FindGameObject("Detail", base.gameObject).AddComponent<AwakeItemDetailLayer>();
		this.mDetailLayer.Init(this);
		this.mInfoLayer.transform.localPosition = Vector3.zero;
		this.mDetailLayer.transform.localPosition = Vector3.zero;
		this.mDetailLayer.gameObject.SetActive(false);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnOkClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnEquipClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.OnEquipClickEvent != null)
		{
			this.OnEquipClickEvent(go);
		}
	}

	private void OnGetClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mDetailLayer.Refresh(this.mItemInfo, true);
		this.PlayAnim(true, false);
	}

	private void OnCreateClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mDetailLayer.Refresh(this.mItemInfo, false);
		this.PlayAnim(true, false);
	}

	public void PlayAnim(bool open, bool immediate = false)
	{
		if (HOTween.IsTweening(this.mInfoLayer.transform))
		{
			return;
		}
		if (this.OpenCurve != null && this.OpenCurve.keys.Length <= 0)
		{
			this.OpenCurve = null;
		}
		if (this.CloseCurve != null && this.CloseCurve.keys.Length <= 0)
		{
			this.CloseCurve = null;
		}
		if (this.isOpen == open)
		{
			return;
		}
		this.isOpen = open;
		if (immediate)
		{
			if (this.isOpen)
			{
				this.mDetailLayer.gameObject.SetActive(true);
				this.mInfoLayer.transform.localPosition = new Vector3(-190f, 0f, 0f);
				this.mDetailLayer.transform.localPosition = new Vector3(206f, 0f, 0f);
			}
			else
			{
				this.mInfoLayer.transform.localPosition = Vector3.zero;
				this.mDetailLayer.transform.localPosition = Vector3.zero;
				this.OnAnimEnd();
			}
		}
		else if (this.isOpen)
		{
			this.mDetailLayer.gameObject.SetActive(true);
			HOTween.To(this.mInfoLayer.transform, this.Time0, new TweenParms().Prop("localPosition", new Vector3(-190f, 0f, 0f)).Ease(this.OpenCurve));
			HOTween.To(this.mDetailLayer.transform, this.Time0, new TweenParms().Prop("localPosition", new Vector3(206f, 0f, 0f)).Ease(this.OpenCurve));
		}
		else
		{
			HOTween.To(this.mInfoLayer.transform, this.Time0, new TweenParms().Prop("localPosition", new Vector3(0f, 0f, 0f)).Ease(this.CloseCurve));
			HOTween.To(this.mDetailLayer.transform, this.Time0, new TweenParms().Prop("localPosition", new Vector3(0f, 0f, 0f)).Ease(this.CloseCurve).OnComplete(new TweenDelegate.TweenCallback(this.OnAnimEnd)));
		}
	}

	private void OnAnimEnd()
	{
		if (!this.isOpen)
		{
			this.mDetailLayer.gameObject.SetActive(false);
		}
	}

	public override void InitPopUp(ItemInfo info, GUIAwakeItemInfoPopUp.EOpenType type, UIEventListener.VoidDelegate cb)
	{
		if (info == null)
		{
			return;
		}
		this.mItemInfo = info;
		this.OnEquipClickEvent = cb;
		CommonIconItem.Create(this.mInfoLayer, new Vector3(-150f, 160f, 0f), null, false, 0.9f, null).Refresh(this.mItemInfo, false, false, false);
		this.mName.text = info.Name;
		this.mName.color = Tools.GetItemQualityColor(info.Quality);
		this.mAtts.text = this.GetItemAtt(info);
		this.mValues.text = this.GetItemAttValue(info);
		this.mDesc.text = info.Desc;
		this.SwitchType(type, true);
		this.Refresh();
	}

	public void SwitchType(GUIAwakeItemInfoPopUp.EOpenType type, bool isInit = false)
	{
		this.mType = type;
		this.mGetBtn.SetActive(false);
		this.mCreateBtn.SetActive(false);
		this.mEquipBtn.SetActive(false);
		this.mOKBtn.SetActive(false);
		switch (this.mType)
		{
		case GUIAwakeItemInfoPopUp.EOpenType.EOT_View:
			this.mOKBtn.SetActive(true);
			break;
		case GUIAwakeItemInfoPopUp.EOpenType.EOT_Equip:
			this.mEquipBtn.SetActive(true);
			break;
		case GUIAwakeItemInfoPopUp.EOpenType.EOT_Get:
		{
			bool flag = Globals.Instance.AttDB.AwakeRecipeDict.GetInfo(this.mItemInfo.ID) != null;
			bool flag2 = this.mItemInfo.Source != 0;
			if (flag && flag2)
			{
				this.mGetBtn.SetActive(true);
				this.mCreateBtn.SetActive(true);
				this.mGetBtn.transform.localPosition = new Vector3(-80f, -184f, 0f);
				this.mCreateBtn.transform.localPosition = new Vector3(80f, -184f, 0f);
				if (isInit)
				{
					this.PlayAnim(true, true);
					this.mDetailLayer.Refresh(this.mItemInfo, true);
				}
			}
			else if (flag && !flag2)
			{
				this.mCreateBtn.SetActive(true);
				this.mCreateBtn.transform.localPosition = new Vector3(0f, -184f, 0f);
				if (isInit)
				{
					this.PlayAnim(true, true);
					this.mDetailLayer.Refresh(this.mItemInfo, false);
				}
			}
			else if (!flag && flag2)
			{
				this.mGetBtn.SetActive(true);
				this.mGetBtn.transform.localPosition = new Vector3(0f, -184f, 0f);
				if (isInit)
				{
					this.PlayAnim(true, true);
					this.mDetailLayer.Refresh(this.mItemInfo, true);
				}
			}
			else
			{
				this.mOKBtn.SetActive(true);
			}
			break;
		}
		}
	}

	private string GetItemAtt(ItemInfo info)
	{
		this.tempSb.Remove(0, this.tempSb.Length);
		this.tempSb.AppendLine(Tools.GetEAttIDName(1));
		this.tempSb.AppendLine(Tools.GetEAttIDName(2));
		this.tempSb.Append(Tools.GetEAttIDName(20));
		return this.tempSb.ToString();
	}

	private string GetItemAttValue(ItemInfo info)
	{
		this.tempSb.Remove(0, this.tempSb.Length);
		this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
		{
			Tools.GetEAttIDValue(1, info.Value3)
		}));
		this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
		{
			Tools.GetEAttIDValue(2, info.Value1)
		}));
		this.tempSb.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
		{
			Tools.GetEAttIDValue(20, info.Value2)
		}));
		return this.tempSb.ToString();
	}

	public void Refresh()
	{
		this.mNumValue.text = Globals.Instance.Player.ItemSystem.GetItemCount(this.mItemInfo.ID).ToString();
		this.mCreateBtnEffect.SetActive(Globals.Instance.Player.ItemSystem.CanCreateAwakeItem(this.mItemInfo.ID, 1));
	}
}
