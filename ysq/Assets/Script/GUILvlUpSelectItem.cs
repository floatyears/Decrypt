using System;
using System.Text;
using UnityEngine;

public class GUILvlUpSelectItem : UICustomGridItem
{
	private GUILvlUpSelPetSceneV2 mBaseScene;

	public GUILvlUpSelectItemData mPetData;

	private UISprite mPetIcon;

	private UISprite mQualityMask;

	private UILabel mLvlNum;

	private UILabel mZiZhiNum;

	private UILabel mName;

	private UILabel mPetType;

	private UILabel mLvlNum2;

	private UILabel mExpNum;

	private GameObject mAwakeStarsGo;

	private GameObject mAwakeTxtGo;

	private UILabel mAwakeLvl;

	private UISprite[] mAwakeStars = new UISprite[5];

	public GUILvlUpSelectToggleBtn mGUISelectToggleBtn;

	private StringBuilder mSb = new StringBuilder(42);

	public bool IsToggleChecked
	{
		get
		{
			return this.mGUISelectToggleBtn.IsChecked;
		}
		set
		{
			this.mGUISelectToggleBtn.IsChecked = value;
		}
	}

	public void InitWithBaseScene(GUILvlUpSelPetSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("iconBg");
		this.mPetIcon = transform.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = transform.Find("qualityMask").GetComponent<UISprite>();
		this.mLvlNum = transform.Find("Lv").GetComponent<UILabel>();
		this.mZiZhiNum = base.transform.Find("ziZhiNum").GetComponent<UILabel>();
		this.mName = base.transform.Find("name").GetComponent<UILabel>();
		Transform transform2 = base.transform.Find("infoBg");
		this.mPetType = transform2.Find("typeName").GetComponent<UILabel>();
		this.mLvlNum2 = transform2.Find("num").GetComponent<UILabel>();
		this.mAwakeStarsGo = transform2.Find("stars").gameObject;
		for (int i = 0; i < 5; i++)
		{
			this.mAwakeStars[i] = this.mAwakeStarsGo.transform.Find(string.Format("star{0}", i)).GetComponent<UISprite>();
		}
		this.mAwakeStarsGo.SetActive(false);
		this.mAwakeTxtGo = transform2.Find("jueXingTxt").gameObject;
		this.mAwakeLvl = this.mAwakeTxtGo.transform.Find("jueXingNum").GetComponent<UILabel>();
		this.mAwakeTxtGo.SetActive(false);
		this.mExpNum = base.transform.Find("txt0/num").GetComponent<UILabel>();
		this.mGUISelectToggleBtn = base.transform.Find("togBtnBg").gameObject.AddComponent<GUILvlUpSelectToggleBtn>();
		this.mGUISelectToggleBtn.InitToggleBtn(false);
		GUILvlUpSelectToggleBtn expr_1B6 = this.mGUISelectToggleBtn;
		expr_1B6.ToggleChangedEvent = (GUILvlUpSelectToggleBtn.ToggleChangedCallback)Delegate.Combine(expr_1B6.ToggleChangedEvent, new GUILvlUpSelectToggleBtn.ToggleChangedCallback(this.OnToggleBtnChanged));
		GUILvlUpSelectToggleBtn expr_1DD = this.mGUISelectToggleBtn;
		expr_1DD.ToggleClickEvent = (GUILvlUpSelectToggleBtn.ToggleClickCallback)Delegate.Combine(expr_1DD.ToggleClickEvent, new GUILvlUpSelectToggleBtn.ToggleClickCallback(this.OnToggleBtnClick));
	}

	public override void Refresh(object data)
	{
		if (this.mPetData == data)
		{
			return;
		}
		this.mPetData = (GUILvlUpSelectItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mPetData != null && this.mPetData.mPetDataEx != null)
		{
			this.mPetIcon.spriteName = this.mPetData.mPetDataEx.Info.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mPetData.mPetDataEx.Info.Quality);
			this.mLvlNum.text = string.Format("Lv{0}", this.mPetData.mPetDataEx.Data.Level);
			this.mLvlNum2.text = this.mPetData.mPetDataEx.Data.Level.ToString();
			this.mZiZhiNum.text = this.mPetData.mPetDataEx.Info.SubQuality.ToString();
			this.mPetType.text = Tools.GetPetTypeStrDesc(this.mPetData.mPetDataEx.Info.Type);
			this.mSb.Remove(0, this.mSb.Length).Append(Tools.GetPetName(this.mPetData.mPetDataEx.Info));
			if (this.mPetData.mPetDataEx.Data.Further > 0u)
			{
				this.mSb.Append("+").Append(this.mPetData.mPetDataEx.Data.Further);
			}
			this.mName.text = this.mSb.ToString();
			this.mName.color = Tools.GetItemQualityColor(this.mPetData.mPetDataEx.Info.Quality);
			if (!this.mBaseScene.IsFromRecycle())
			{
				this.mExpNum.text = this.mPetData.CanGetExpNum().ToString();
			}
			else
			{
				this.mExpNum.transform.parent.gameObject.SetActive(false);
			}
			if ((ulong)this.mPetData.mPetDataEx.Data.Level >= (ulong)((long)GameConst.GetInt32(26)))
			{
				uint num = 0u;
				uint petStarAndLvl = Tools.GetPetStarAndLvl(this.mPetData.mPetDataEx.Data.Awake, out num);
				this.mAwakeStarsGo.SetActive(true);
				this.mAwakeTxtGo.SetActive(true);
				this.mAwakeLvl.text = Singleton<StringManager>.Instance.GetString("PetAwake2", new object[]
				{
					petStarAndLvl,
					num
				});
				for (int i = 0; i < 5; i++)
				{
					this.mAwakeStars[i].spriteName = (((long)i >= (long)((ulong)petStarAndLvl)) ? "starBg" : "star");
				}
			}
			else
			{
				this.mAwakeStarsGo.SetActive(false);
				this.mAwakeTxtGo.SetActive(false);
			}
			this.IsToggleChecked = this.mPetData.mIsSelected;
		}
	}

	public void OnToggleBtnClick()
	{
		if (!this.IsToggleChecked)
		{
			if (this.mBaseScene.IsSelectPetsEnough())
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt1", 0f, 0f);
				return;
			}
			if (!this.mBaseScene.IsFromRecycle() && this.mPetData.mPetDataEx.Info.Quality > 1)
			{
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("petTrainTxt2"), MessageBox.Type.OKCancel, null);
				GameMessageBox expr_78 = gameMessageBox;
				expr_78.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_78.OkClick, new MessageBox.MessageDelegate(this.OnSureClick));
				GameMessageBox expr_9A = gameMessageBox;
				expr_9A.CancelClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_9A.CancelClick, new MessageBox.MessageDelegate(this.OnCancelClick));
				return;
			}
			this.IsToggleChecked = !this.IsToggleChecked;
		}
		else
		{
			this.IsToggleChecked = !this.IsToggleChecked;
		}
	}

	private void OnSureClick(object obj)
	{
		this.IsToggleChecked = true;
	}

	private void OnCancelClick(object obj)
	{
		this.IsToggleChecked = false;
	}

	private void OnToggleBtnChanged(bool isChecked)
	{
		this.mPetData.mIsSelected = isChecked;
		this.mBaseScene.Refresh();
	}
}
