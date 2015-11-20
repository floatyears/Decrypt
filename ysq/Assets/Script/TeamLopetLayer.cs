using System;
using UnityEngine;

public class TeamLopetLayer : MonoBehaviour
{
	private GUITeamManageSceneV2 mBaseScene;

	private LopetDataEx mCurLopet;

	private GameObject mModelPos;

	private LopetInfoSkillLayer mLopetSkills;

	private bool initFlag;

	private UIActorController mUIActorController;

	private ResourceEntity asyncEntiry;

	public GameObject mModelTmp
	{
		get;
		set;
	}

	public void Init(GUITeamManageSceneV2 basescene)
	{
		if (this.initFlag)
		{
			return;
		}
		this.initFlag = true;
		this.mBaseScene = basescene;
		this.mCurLopet = Globals.Instance.Player.LopetSystem.GetCurLopet(this.mBaseScene.IsLocalPlayer);
		GameObject gameObject = GameUITools.RegisterClickEvent("changeLopetBtn", new UIEventListener.VoidDelegate(this.OnChangeBtnClick), base.gameObject);
		gameObject.SetActive(this.mBaseScene.IsLocalPlayer);
		if (this.mCurLopet == null)
		{
			GameUITools.SetLabelLocalText("Label", "Lopet0", gameObject);
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.x = -40f;
			gameObject.transform.localPosition = localPosition;
		}
		else
		{
			GameUITools.SetLabelLocalText("Label", "Lopet1", gameObject);
		}
		GameUITools.FindUISprite("New", gameObject).enabled = Globals.Instance.Player.LopetSystem.HasLopet2Change();
		gameObject = GameUITools.FindGameObject("topInfo", base.gameObject);
		if (this.mCurLopet == null)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.AddComponent<GUILopetTitleInfo>().Refresh(this.mCurLopet);
			UITable component = GameUITools.FindGameObject("rightInfo/panel/content", base.gameObject).GetComponent<UITable>();
			GameUITools.FindGameObject("a", component.gameObject).AddComponent<GUIAttributeValue>().Refresh(this.mCurLopet);
			this.mLopetSkills = GameUITools.FindGameObject("b", component.gameObject).AddComponent<LopetInfoSkillLayer>();
			this.mLopetSkills.Init();
			this.mLopetSkills.Refresh(this.mCurLopet);
		}
		this.CreateModel();
		gameObject = GameUITools.RegisterClickEvent("yangCBtn", new UIEventListener.VoidDelegate(this.OnYangChengClick), base.gameObject);
		GameUITools.FindGameObject("Effect", gameObject).SetActive(Tools.CanCurLopetAwake() || Tools.CanCurLopetLevelup());
		gameObject.SetActive(this.mBaseScene.IsLocalPlayer && Globals.Instance.Player.LopetSystem.GetCurLopet(this.mBaseScene.IsLocalPlayer) != null);
	}

	private void CreateModel()
	{
		if (this.mCurLopet != null)
		{
			this.mModelPos = GameUITools.FindGameObject("modelPos", base.gameObject);
			this.ClearModel();
			this.asyncEntiry = ActorManager.CreateUILopet(this.mBaseScene.IsLocalPlayer, 0, true, true, this.mModelPos, 1f, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				this.mModelTmp = go;
				if (this.mModelTmp != null)
				{
					Tools.SetMeshRenderQueue(this.mModelTmp, 3900);
					this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
					UIActorController expr_46 = this.mUIActorController;
					expr_46.ClickEvent = (UIActorController.VoidCallBack)Delegate.Combine(expr_46.ClickEvent, new UIActorController.VoidCallBack(this.OnGameModelClick));
				}
			});
		}
	}

	private void OnChangeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.LopetSystem.Values.Count <= 0)
		{
			GUIHowGetPetItemPopUp.ShowNullLopet();
			return;
		}
		GUISelectLopetBagScene.Show();
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
			this.mUIActorController = null;
		}
	}

	private void OnYangChengClick(GameObject go)
	{
		this.OnGameModelClick();
	}

	private void OnGameModelClick()
	{
		if (this.mCurLopet != null)
		{
			GameUIManager.mInstance.ShowLopetInfo(this.mCurLopet);
		}
	}
}
