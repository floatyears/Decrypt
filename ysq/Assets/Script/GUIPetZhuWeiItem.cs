using System;
using System.Text;
using UnityEngine;

public class GUIPetZhuWeiItem : MonoBehaviour
{
	private GUIPetZhuWeiPopUp mBaseScene;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mLvl;

	private StringBuilder mSb = new StringBuilder(42);

	public PetDataEx mPetDataEx
	{
		get;
		private set;
	}

	public void InitWithBaseScene(GUIPetZhuWeiPopUp baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcon = base.transform.Find("icon").GetComponent<UISprite>();
		this.mIcon.gameObject.SetActive(false);
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mQualityMask.gameObject.SetActive(false);
		this.mLvl = base.transform.Find("lvl").GetComponent<UILabel>();
		this.mLvl.text = string.Empty;
		UIEventListener expr_8E = UIEventListener.Get(base.gameObject);
		expr_8E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8E.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
	}

	public void Refresh(PetDataEx data)
	{
		this.mPetDataEx = data;
		this.Refresh();
		this.RefreshLvlNum();
	}

	private void Refresh()
	{
		if (this.mPetDataEx != null)
		{
			this.mIcon.gameObject.SetActive(true);
			this.mQualityMask.gameObject.SetActive(true);
			this.mIcon.spriteName = this.mPetDataEx.Info.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mPetDataEx.Info.Quality);
		}
		else
		{
			this.mIcon.gameObject.SetActive(false);
			this.mQualityMask.gameObject.SetActive(false);
		}
	}

	public void RefreshLvlNum()
	{
		if (this.mPetDataEx != null)
		{
			int curPageIndex = this.mBaseScene.GetCurPageIndex();
			if (curPageIndex == 0)
			{
				this.mLvl.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("summonLvl")).Append(this.mPetDataEx.Data.Level).ToString();
			}
			else if (curPageIndex == 1)
			{
				this.mLvl.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("jinJie")).Append("+").Append(this.mPetDataEx.Data.Further).ToString();
			}
		}
		else
		{
			this.mLvl.text = string.Empty;
		}
	}

	private void OnItemClick(GameObject go)
	{
		if (this.mPetDataEx != null)
		{
			int curPageIndex = this.mBaseScene.GetCurPageIndex();
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mPetDataEx;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = ((curPageIndex != 1) ? 1 : 2);
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
			GameUIManager.mInstance.uiState.IsShowPetZhuWeiPopUp = true;
			GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
	}
}
