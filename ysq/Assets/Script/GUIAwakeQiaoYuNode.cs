using Att;
using System;
using UnityEngine;

public class GUIAwakeQiaoYuNode : MonoBehaviour
{
	private GameObject mAdventureReady;

	private UISprite mBgSp;

	private GameObject mEffect35;

	private BoxCollider mBoxCollider;

	public SceneInfo mSceneInfo
	{
		get;
		private set;
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

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBgSp = base.transform.GetComponent<UISprite>();
		UIEventListener expr_1C = UIEventListener.Get(base.gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnSceneNodeClicked));
		this.mBoxCollider = base.transform.GetComponent<BoxCollider>();
		this.mEffect35 = base.transform.Find("ui35").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect35, 4000);
		NGUITools.SetActive(this.mEffect35, false);
	}

	public void Refresh(SceneInfo info)
	{
		if (info == null)
		{
			return;
		}
		this.mSceneInfo = info;
		if (Globals.Instance.Player.GetSceneTimes(this.mSceneInfo.ID) > 0)
		{
			this.mBoxCollider.enabled = false;
			this.mBgSp.spriteName = "awakeBtnGrey";
			NGUITools.SetActive(this.mEffect35, false);
		}
		else
		{
			this.mBoxCollider.enabled = true;
			this.mBgSp.spriteName = "awakeBtn";
			if (Globals.Instance.Player.GetSceneScore(this.mSceneInfo.ID - 1) > 0)
			{
				NGUITools.SetActive(this.mEffect35, false);
				NGUITools.SetActive(this.mEffect35, true);
			}
			else
			{
				NGUITools.SetActive(this.mEffect35, false);
			}
		}
	}

	public void OpenAdventureReadyPanel(bool showMsgTip)
	{
		if (this.mAdventureReady != null)
		{
			return;
		}
		if (this.mSceneInfo.MapID % 100 > 18)
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
}
