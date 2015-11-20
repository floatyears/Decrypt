using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUILvlUpSelPetSceneV2 : GameUISession
{
	private UILabel mCanGetExpName;

	private UILabel mCanGetExpNum;

	private UILabel mNeedExpNum;

	public GUILvlUpSelectItemTable mGUILvlUpSelectItemTable;

	private PetDataEx[] mSelPetDatas = new PetDataEx[5];

	private PetDataEx mCurPetDataEx;

	private int mLvlUpNeedExpNum;

	public PetDataEx CurPetDataEx
	{
		get
		{
			return this.mCurPetDataEx;
		}
		set
		{
			this.mCurPetDataEx = value;
			if (this.mCurPetDataEx != null)
			{
				this.LvlUpNeedExpNum = (int)(this.mCurPetDataEx.GetMaxExp() - this.mCurPetDataEx.Data.Exp);
			}
			else
			{
				this.LvlUpNeedExpNum = 0;
			}
		}
	}

	public int LvlUpNeedExpNum
	{
		get
		{
			return this.mLvlUpNeedExpNum;
		}
		set
		{
			this.mLvlUpNeedExpNum = value;
		}
	}

	public void SetSelectPetDatas(PetDataEx[] pdExs)
	{
		for (int i = 0; i < 5; i++)
		{
			this.mSelPetDatas[i] = pdExs[i];
		}
		this.InitPetItems();
		this.Refresh();
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private bool IsSelectPetData(PetDataEx pdEx)
	{
		for (int i = 0; i < 5; i++)
		{
			if (this.mSelPetDatas[i] != null && this.mSelPetDatas[i].Data.ID == pdEx.Data.ID)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsSelectPetsEnough()
	{
		return this.mGUILvlUpSelectItemTable.IsSelectPetsEnough();
	}

	public bool IsFromRecycle()
	{
		return this.mCurPetDataEx == null;
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle/WindowBg");
		this.mCanGetExpName = transform.Find("txt0").GetComponent<UILabel>();
		this.mCanGetExpNum = transform.Find("txt0/num").GetComponent<UILabel>();
		this.mNeedExpNum = transform.Find("txt1/num").GetComponent<UILabel>();
		this.mGUILvlUpSelectItemTable = transform.Find("bagPanel/bagContents").gameObject.AddComponent<GUILvlUpSelectItemTable>();
		this.mGUILvlUpSelectItemTable.maxPerLine = 2;
		this.mGUILvlUpSelectItemTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mGUILvlUpSelectItemTable.cellWidth = 450f;
		this.mGUILvlUpSelectItemTable.cellHeight = 135f;
		this.mGUILvlUpSelectItemTable.gapWidth = 2f;
		this.mGUILvlUpSelectItemTable.gapHeight = 2f;
		this.mGUILvlUpSelectItemTable.InitWithBaseScene(this);
		GameObject gameObject = transform.Find("sureBtn").gameObject;
		UIEventListener expr_E9 = UIEventListener.Get(gameObject);
		expr_E9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E9.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("equipImprove34");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	public void OnBackClick(GameObject go)
	{
		if (this.IsFromRecycle())
		{
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_PetBreak);
		}
		else
		{
			List<PetDataEx> list = new List<PetDataEx>();
			for (int i = 0; i < 5; i++)
			{
				list.Add(this.mSelPetDatas[i]);
			}
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mCurPetDataEx;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 1;
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 1;
			Type type = GameUIManager.mInstance.GobackSession();
			if (type == typeof(GUIPetTrainSceneV2))
			{
				GUIPetTrainSceneV2 session = GameUIManager.mInstance.GetSession<GUIPetTrainSceneV2>();
				if (session != null)
				{
					session.SetTuiShiItems(list);
				}
			}
		}
	}

	public void OnSureBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (this.IsFromRecycle())
		{
			MC2S_PetBreakUp mC2S_PetBreakUp = new MC2S_PetBreakUp();
			foreach (PetDataEx current in this.mGUILvlUpSelectItemTable.GetSelectPets())
			{
				mC2S_PetBreakUp.PetID.Add(current.GetID());
			}
			GameUIManager.mInstance.uiState.PetBreakUpData = mC2S_PetBreakUp;
			mC2S_PetBreakUp = null;
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_PetBreak);
		}
		else
		{
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mCurPetDataEx;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 1;
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 1;
			Type type = GameUIManager.mInstance.GobackSession();
			if (type == typeof(GUIPetTrainSceneV2))
			{
				GUIPetTrainSceneV2 session = GameUIManager.mInstance.GetSession<GUIPetTrainSceneV2>();
				if (session != null)
				{
					session.SetTuiShiItems(this.mGUILvlUpSelectItemTable.GetSelectPets());
				}
			}
		}
	}

	private void InitPetItems()
	{
		this.mGUILvlUpSelectItemTable.ClearData();
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (!current.IsBattling() && !current.IsPetAssisting() && (this.mCurPetDataEx == null || current.Data.ID != this.mCurPetDataEx.Data.ID))
			{
				this.mGUILvlUpSelectItemTable.AddData(new GUILvlUpSelectItemData(current, this.IsSelectPetData(current)));
			}
		}
	}

	public void Refresh()
	{
		if (!this.IsFromRecycle())
		{
			this.mCanGetExpNum.text = this.mGUILvlUpSelectItemTable.GetCanGetExpNum().ToString();
			this.mNeedExpNum.text = this.LvlUpNeedExpNum.ToString();
		}
		else
		{
			this.mCanGetExpName.text = Singleton<StringManager>.Instance.GetString("recycle18");
			this.mCanGetExpNum.text = this.mGUILvlUpSelectItemTable.GetSelectPets().Count.ToString();
			this.mNeedExpNum.transform.parent.gameObject.SetActive(false);
		}
	}
}
