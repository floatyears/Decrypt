using System;
using UnityEngine;

public class GUICreateCharacterScene : GameUISession
{
	private UILabel description;

	private UISprite male;

	private UISprite female;

	private GameObject btnStart;

	private UIInput inputName;

	private bool isMale;

	private GameObject slot;

	private GameObject model;

	private string mCacheName;

	private ResourceEntity asyncEntiry;

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		this.CreateObjects();
		this.OnSceneLoaded();
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		this.CreateModel();
	}

	private void CreateObjects()
	{
		this.description = GameUITools.FindUILabel("WindowBG/Description", base.gameObject);
		this.male = GameUITools.FindUISprite("WindowBG/Male", base.gameObject);
		this.female = GameUITools.FindUISprite("WindowBG/Female", base.gameObject);
		this.btnStart = base.FindGameObject("WindowBG/Start", base.gameObject);
		this.inputName = base.FindGameObject("WindowBG/Name", base.gameObject).GetComponent<UIInput>();
		GameUITools.FindUILabel("WindowBG/Title/Label", base.gameObject).text = Singleton<StringManager>.Instance.GetString("CreateCharacterTitleLabel");
		GameUITools.FindUILabel("Label", this.btnStart).text = Singleton<StringManager>.Instance.GetString("CreateCharacterStartLabel");
		this.isMale = true;
		this.description.text = Singleton<StringManager>.Instance.GetString("CreateCharacterMaleDesc");
		base.RegisterClickEvent("WindowBG/Male", new UIEventListener.VoidDelegate(this.OnMaleClick), base.gameObject);
		base.RegisterClickEvent("WindowBG/Female", new UIEventListener.VoidDelegate(this.OnFemaleClick), base.gameObject);
		base.RegisterClickEvent("WindowBG/Start", new UIEventListener.VoidDelegate(this.OnBtnStartClick), base.gameObject);
		GameObject gameObject = GameUITools.FindGameObject("RollName", this.inputName.gameObject);
		UIEventListener expr_159 = UIEventListener.Get(gameObject.gameObject);
		expr_159.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_159.onClick, new UIEventListener.VoidDelegate(this.OnRollNameClick));
		this.inputName.value = Singleton<StringManager>.Instance.GetRecommendName(true);
		this.mCacheName = this.inputName.value;
		this.slot = base.FindGameObject("Slot", null);
	}

	private void CreateModel()
	{
		this.ClearModel();
		string armorResLoc = (!this.isMale) ? "Player/PlayerG_05b" : "Player/playerb_05b";
		this.asyncEntiry = ActorManager.CreateUIPlayer(armorResLoc, "Player/Weapon_05b", (!this.isMale) ? 1 : 0, 0, true, true, this.slot, 1.2f, delegate(GameObject go)
		{
			this.asyncEntiry = null;
			this.model = go;
			if (this.model != null)
			{
				UIActorController component = this.model.GetComponent<UIActorController>();
				component.PlayUIAction();
				component.PlayVoice(2);
			}
		});
	}

	private void ClearModel()
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		if (this.model != null)
		{
			UnityEngine.Object.DestroyImmediate(this.model);
			this.model = null;
		}
	}

	protected override void OnPreDestroyGUI()
	{
		this.ClearModel();
	}

	private void OnMaleClick(GameObject obj)
	{
		if (this.model == null)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!this.isMale)
		{
			this.isMale = true;
			this.CreateModel();
			this.description.text = Singleton<StringManager>.Instance.GetString("CreateCharacterMaleDesc");
			this.male.spriteName = "selectedMale";
			this.female.spriteName = "female";
			this.RollName();
		}
	}

	private void RollName()
	{
		if (this.mCacheName == this.inputName.value)
		{
			this.inputName.value = Singleton<StringManager>.Instance.GetRecommendName(this.isMale);
			this.mCacheName = this.inputName.value;
		}
	}

	private void OnFemaleClick(GameObject obj)
	{
		if (this.model == null)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.isMale)
		{
			this.isMale = false;
			this.CreateModel();
			this.description.text = Singleton<StringManager>.Instance.GetString("CreateCharacterFemaleDesc");
			this.male.spriteName = "male";
			this.female.spriteName = "selectedFemale";
			this.RollName();
		}
	}

	private void OnBtnStartClick(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (string.IsNullOrEmpty(this.inputName.value))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("CreateCharacterNullNameError"), 0f, 0f);
			return;
		}
		if (Tools.GetLength(this.inputName.value) > 12)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("CreateCharacterMaxNameLengthError"), 0f, 0f);
			return;
		}
		Globals.Instance.CliSession.SendCreatePlayerMsg((!this.isMale) ? 1 : 0, this.inputName.value);
	}

	private void OnRollNameClick(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.inputName.value = Singleton<StringManager>.Instance.GetRecommendName(this.isMale);
		this.mCacheName = this.inputName.value;
	}

	public void OnSceneLoaded()
	{
		Camera.main.transform.position = new Vector3(-0.61f, 1.251f, 2.859f);
		Camera.main.transform.localRotation = Quaternion.Euler(0f, -172.81f, 0f);
	}
}
