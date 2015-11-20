using System;
using UnityEngine;

public class TrinketRefineExpItem : MonoBehaviour
{
	private ItemDataEx mData;

	private int mNeedCount;

	private UISprite mIcon;

	private UISprite mQualityMark;

	private UILabel mValue;

	public GameObject ui56_2;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcon = GameUITools.RegisterClickEvent("Icon", new UIEventListener.VoidDelegate(this.OnIconClick), base.gameObject).GetComponent<UISprite>();
		this.mQualityMark = GameUITools.FindUISprite("QualityMark", base.gameObject);
		this.mValue = GameUITools.FindUILabel("Value", base.gameObject);
		this.ui56_2 = GameUITools.FindGameObject("ui56_2", base.gameObject);
		Tools.SetParticleRQWithUIScale(this.ui56_2, 4500);
		NGUITools.SetActive(this.ui56_2, false);
	}

	private void OnIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (this.mData.Info.Type == 1)
		{
			GameUIManager.mInstance.ShowItemInfo(this.mData.Info);
		}
		else if (this.mData.Info.ID == GameConst.GetInt32(102))
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mData.Info);
		}
		else
		{
			global::Debug.LogErrorFormat("TrinketUpgrade RefineLayer wrong type ID : {0}", new object[]
			{
				this.mData.Info.ID
			});
		}
	}

	private int GetCurCount()
	{
		if (this.mData.Info.Type == 1)
		{
			return Globals.Instance.Player.ItemSystem.GetTrinketRefineTrinketCount(this.mData);
		}
		return this.mData.GetCount();
	}

	public void Refresh(ItemDataEx data, int needCount)
	{
		this.mData = data;
		this.mNeedCount = needCount;
		if (data == null || needCount < 1)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.gameObject.SetActive(true);
			this.mIcon.spriteName = this.mData.Info.Icon;
			this.mQualityMark.spriteName = Tools.GetItemQualityIcon(this.mData.Info.Quality);
			this.mValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				this.GetCurCount(),
				this.mNeedCount
			});
			if (this.GetCurCount() < needCount)
			{
				this.mValue.color = Color.red;
			}
			else
			{
				this.mValue.color = Color.white;
			}
		}
	}

	public void PlayAnim()
	{
		NGUITools.SetActive(this.ui56_2, false);
		NGUITools.SetActive(this.ui56_2, true);
	}
}
