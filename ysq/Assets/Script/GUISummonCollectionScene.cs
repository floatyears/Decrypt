using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public sealed class GUISummonCollectionScene : GameUISession
{
	public SummonCollectionLayer mSummonCollectionLayer;

	public static void TryOpen()
	{
		if (!Tools.CanPlay(GameConst.GetInt32(28), true))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WBTip1", new object[]
			{
				GameConst.GetInt32(28)
			}), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUISummonCollectionScene>(null, false, true);
	}

	private void InitPetAndSetInfos()
	{
		foreach (PetInfo current in Globals.Instance.AttDB.PetDict.Values)
		{
			if (current.ID != 90000 && current.ID != 90001 && !current.ShowCollection && current.ID != Globals.Instance.Player.TeamSystem.GetPet(0).Info.ID)
			{
				switch (current.ElementType)
				{
				case 1:
					this.mSummonCollectionLayer.mFirePetInfos.Add(current);
					break;
				case 2:
					this.mSummonCollectionLayer.mWoodPetInfos.Add(current);
					break;
				case 3:
					this.mSummonCollectionLayer.mWaterPetInfos.Add(current);
					break;
				case 4:
					this.mSummonCollectionLayer.mLightPetInfos.Add(current);
					break;
				case 5:
					this.mSummonCollectionLayer.mDarkPetInfos.Add(current);
					break;
				}
			}
		}
		this.mSummonCollectionLayer.SortInfos();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle/WindowBg");
		this.mSummonCollectionLayer = transform.Find("collectionLayer").gameObject.AddComponent<SummonCollectionLayer>();
		this.mSummonCollectionLayer.InitWithBaseScene(this);
		this.InitPetAndSetInfos();
		base.StartCoroutine(this.DoInitCollectionItems());
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		GameUIManager.mInstance.GetTopGoods().Show("summonCollection");
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		PetSubSystem expr_38 = Globals.Instance.Player.PetSystem;
		expr_38.AddPetEvent = (PetSubSystem.AddPetCallback)Delegate.Combine(expr_38.AddPetEvent, new PetSubSystem.AddPetCallback(this.OnAddPetEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		PetSubSystem expr_1E = Globals.Instance.Player.PetSystem;
		expr_1E.AddPetEvent = (PetSubSystem.AddPetCallback)Delegate.Remove(expr_1E.AddPetEvent, new PetSubSystem.AddPetCallback(this.OnAddPetEvent));
	}

	private void OnAddPetEvent(PetDataEx data)
	{
		this.mSummonCollectionLayer.Refresh(data);
	}

	[DebuggerHidden]
	private IEnumerator DoInitCollectionItems()
	{
        return null;
        //GUISummonCollectionScene.<DoInitCollectionItems>c__Iterator95 <DoInitCollectionItems>c__Iterator = new GUISummonCollectionScene.<DoInitCollectionItems>c__Iterator95();
        //<DoInitCollectionItems>c__Iterator.<>f__this = this;
        //return <DoInitCollectionItems>c__Iterator;
	}
}
