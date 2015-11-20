using Att;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using System;
using UnityEngine;

public class GUIPetTrainLvlUpItem : MonoBehaviour
{
	private GUIPetTrainSceneV2 mBaseScene;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mDesc;

	private GameObject mMask;

	private GameObject mEffect0;

	private GameObject mEffect1;

	private Vector3[] mEffect1Path = new Vector3[3];

	private Transform mTmpTransform;

	private ItemInfo mItemInfo;

	private int mIndex;

	private int mHasItemCount;

	public int HasItemCount
	{
		get
		{
			return this.mHasItemCount;
		}
	}

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene, int index)
	{
		this.mBaseScene = baseScene;
		this.mIndex = index;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("item");
		this.mIcon = transform.Find("icon").GetComponent<UISprite>();
		UIEventListener expr_37 = UIEventListener.Get(this.mIcon.gameObject);
		expr_37.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_37.onClick, new UIEventListener.VoidDelegate(this.OnIconClick));
		this.mQualityMask = transform.Find("qualityMask").GetComponent<UISprite>();
		this.mDesc = base.transform.Find("desc").GetComponent<UILabel>();
		this.mMask = base.transform.Find("mask").gameObject;
		this.mEffect0 = base.transform.Find("ui56_2").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect0, 3100);
		this.mEffect1 = base.transform.Find("ui56_1").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect1, 3100);
		this.mTmpTransform = base.transform.Find("tmp");
		this.mEffect1Path[0] = this.mIcon.transform.position;
		this.mEffect1Path[1] = this.mTmpTransform.position;
		this.mEffect1Path[2] = this.mBaseScene.GetCardModelTransform().position;
	}

	public void PlayEffect0()
	{
		NGUITools.SetActive(this.mEffect0, false);
		NGUITools.SetActive(this.mEffect0, true);
	}

	public void HideEffec0()
	{
		NGUITools.SetActive(this.mEffect0, false);
	}

	public void HideEffec1()
	{
		NGUITools.SetActive(this.mEffect1, false);
	}

	public void PlayEffect1()
	{
		this.mEffect1.transform.localPosition = Vector3.zero;
		NGUITools.SetActive(this.mEffect1, false);
		NGUITools.SetActive(this.mEffect1, true);
		HOTween.To(this.mEffect1.transform, 0.3f, new TweenParms().Prop("position", new PlugVector3Path(this.mEffect1Path, PathType.Curved)));
	}

	public void Refresh()
	{
		if (this.mBaseScene.CurPetDataEx != null)
		{
			if (this.mIndex < GameConst.PET_EXP_ITEM_ID.Length)
			{
				this.mItemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.PET_EXP_ITEM_ID[this.mIndex]);
				if (this.mItemInfo != null)
				{
					this.mIcon.spriteName = this.mItemInfo.Icon;
					this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
					this.mHasItemCount = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.PET_EXP_ITEM_ID[this.mIndex]);
					this.mDesc.text = this.mHasItemCount.ToString();
					this.mMask.SetActive(this.mHasItemCount == 0);
				}
			}
		}
		else if (this.mBaseScene.CurLopetDataEx != null && this.mIndex < GameConst.LOPET_EXP_ITEM_ID.Length)
		{
			this.mItemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.LOPET_EXP_ITEM_ID[this.mIndex]);
			if (this.mItemInfo != null)
			{
				this.mIcon.spriteName = this.mItemInfo.Icon;
				this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
				this.mHasItemCount = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.LOPET_EXP_ITEM_ID[this.mIndex]);
				this.mDesc.text = this.mHasItemCount.ToString();
				this.mMask.SetActive(this.mHasItemCount == 0);
			}
		}
	}

	public int GetExpNum()
	{
		return (this.mHasItemCount <= 0 || this.mItemInfo == null) ? 0 : this.mItemInfo.Value1;
	}

	public long GetTotalExpNum()
	{
		int num = this.mHasItemCount * this.GetExpNum();
		return (long)((num < 0) ? 2147483647 : num);
	}

	private void OnIconClick(GameObject go)
	{
		if (this.mItemInfo != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mItemInfo);
		}
	}

	public bool IsItemEmpty()
	{
		return this.mHasItemCount == 0;
	}
}
