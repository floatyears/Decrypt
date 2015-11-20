using Att;
using System;
using UnityEngine;

public class GUIAwakeRoadSceneNode : MonoBehaviour
{
	private UIButton mBkGroudBtn;

	public GameObject mModelPos;

	private GameObject mModelTmp;

	private GameObject[] mStarScore = new GameObject[3];

	private GameObject mAdventureReady;

	private GameObject mNextLevelEffect;

	private ResourceEntity asyncEntiry;

	public SceneInfo mSceneInfo
	{
		get;
		private set;
	}

	public GameObject NextLevelEffect
	{
		get
		{
			return this.mNextLevelEffect;
		}
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void OnDestroy()
	{
		UIEventListener expr_0B = UIEventListener.Get(base.gameObject);
		expr_0B.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(expr_0B.onClick, new UIEventListener.VoidDelegate(this.OnSceneNodeClicked));
		if (this.mAdventureReady != null)
		{
			UnityEngine.Object.Destroy(this.mAdventureReady);
			this.mAdventureReady = null;
		}
	}

	private void CreateObjects()
	{
		this.mBkGroudBtn = base.transform.GetComponent<UIButton>();
		for (int i = 0; i < 3; i++)
		{
			this.mStarScore[i] = base.transform.FindChild(string.Format("star{0}", i)).gameObject;
		}
		this.mModelPos = base.transform.Find("modelPos").gameObject;
		UIEventListener expr_71 = UIEventListener.Get(base.gameObject);
		expr_71.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_71.onClick, new UIEventListener.VoidDelegate(this.OnSceneNodeClicked));
		this.mNextLevelEffect = base.transform.Find("ui31").gameObject;
		Tools.SetParticleRenderQueue2(this.mNextLevelEffect, 3500);
		this.mNextLevelEffect.SetActive(false);
	}

	private int GetNextUnBattleSceneId()
	{
		foreach (SceneInfo current in Globals.Instance.AttDB.SceneDict.Values)
		{
			if (current.ID / 100000 == 6 && current.ID % 10 != 6 && Globals.Instance.Player.GetSceneScore(current.ID) <= 0)
			{
				return current.ID;
			}
		}
		return 0;
	}

	public void Refresh(SceneInfo info, int sceneScore)
	{
		if (info == null)
		{
			return;
		}
		this.mSceneInfo = info;
		if (sceneScore == 0)
		{
			this.mStarScore[0].SetActive(false);
			this.mStarScore[1].SetActive(false);
			this.mStarScore[2].SetActive(false);
			this.mBkGroudBtn.normalSprite = "Disable";
		}
		else
		{
			this.mBkGroudBtn.normalSprite = "hard";
			switch (sceneScore)
			{
			case 1:
				this.mStarScore[0].SetActive(true);
				this.mStarScore[1].SetActive(false);
				this.mStarScore[2].SetActive(false);
				this.mStarScore[0].transform.localPosition = new Vector3(0f, -35f, 0f);
				break;
			case 2:
				this.mStarScore[0].SetActive(true);
				this.mStarScore[1].SetActive(true);
				this.mStarScore[2].SetActive(false);
				this.mStarScore[0].transform.localPosition = new Vector3(-20f, -32f, 0f);
				this.mStarScore[1].transform.localPosition = new Vector3(20f, -32f, 0f);
				break;
			case 3:
				this.mStarScore[0].SetActive(true);
				this.mStarScore[1].SetActive(true);
				this.mStarScore[2].SetActive(true);
				this.mStarScore[0].transform.localPosition = new Vector3(-34f, -26f, 0f);
				this.mStarScore[1].transform.localPosition = new Vector3(0f, -35f, 0f);
				this.mStarScore[2].transform.localPosition = new Vector3(34f, -26f, 0f);
				break;
			}
		}
		this.CreateModel();
		this.mNextLevelEffect.SetActive(this.mSceneInfo.ID == this.GetNextUnBattleSceneId());
	}

	public void OpenAdventureReadyPanel(bool showMsgTip)
	{
		if (this.mAdventureReady != null)
		{
			return;
		}
		if (this.mSceneInfo.MapID % 100 > 32)
		{
			if (showMsgTip)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("notopened", 0f, 0f);
			}
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)this.mSceneInfo.MinLevel))
		{
			if (showMsgTip)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_2", new object[]
				{
					this.mSceneInfo.MinLevel
				}), 0f, 0f);
			}
			return;
		}
		if (this.mSceneInfo.PreID != 0 && player.GetSceneScore(this.mSceneInfo.PreID) <= 0)
		{
			if (showMsgTip)
			{
				GameUIManager.mInstance.ShowMessageTip("PveR", 2);
			}
			return;
		}
		this.DialogFinish();
	}

	private void OnSceneNodeClicked(GameObject go)
	{
		this.OpenAdventureReadyPanel(true);
	}

	private void DialogFinish()
	{
		GameObject prefab = Res.LoadGUI("GUI/GameUIAdventureReady");
		this.mAdventureReady = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, prefab);
		this.mAdventureReady.AddComponent<GameUIAdventureReady>().Init(this.mSceneInfo);
		this.mAdventureReady.SetActive(true);
		Vector3 localPosition = this.mAdventureReady.transform.localPosition;
		localPosition.z = 3000f;
		this.mAdventureReady.transform.localPosition = localPosition;
	}

	private void ClearModel()
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		if (this.mModelTmp != null)
		{
			UnityEngine.Object.DestroyImmediate(this.mModelTmp);
			this.mModelTmp = null;
		}
	}

	private void CreateModel()
	{
		this.ClearModel();
		if (this.mSceneInfo != null)
		{
			for (int i = 2; i >= 0; i--)
			{
				MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(this.mSceneInfo.Enemy[i]);
				if (info != null)
				{
					this.asyncEntiry = ActorManager.CreateUIMonster(info, 0, false, false, this.mModelPos, info.ScaleInUI * 0.3f, delegate(GameObject go)
					{
						this.asyncEntiry = null;
						this.mModelTmp = go;
						if (this.mModelTmp != null)
						{
							this.mModelTmp.animation.clip = this.mModelTmp.animation.GetClip("std");
							this.mModelTmp.animation.wrapMode = WrapMode.Loop;
							this.mModelTmp.transform.localPosition = new Vector3(0f, 0f, -500f);
							Tools.SetMeshRenderQueue(this.mModelTmp, 3001);
						}
					});
					break;
				}
			}
		}
	}
}
