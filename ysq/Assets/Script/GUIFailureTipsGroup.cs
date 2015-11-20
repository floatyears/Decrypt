using System;
using UnityEngine;

public class GUIFailureTipsGroup : MonoBehaviour
{
	private const int mMaxMarkNum = 2;

	private int mCurMarkNum;

	private bool CanPetMarkShow()
	{
		if (this.mCurMarkNum >= 2)
		{
			return false;
		}
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return false;
		}
		for (int i = 1; i < 4; i++)
		{
			PetDataEx pet = teamSystem.GetPet(i);
			if (pet != null && Tools.CanPetLvlUp(pet))
			{
				this.mCurMarkNum++;
				return true;
			}
		}
		return false;
	}

	private bool CanEquipsMarkShow()
	{
		if (this.mCurMarkNum >= 2)
		{
			return false;
		}
		if (Globals.Instance.Player.TeamSystem == null)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(i);
			if (socket != null)
			{
				for (int j = 0; j < 4; j++)
				{
					ItemDataEx equip = socket.GetEquip(j);
					if (equip != null)
					{
						if (equip.CanEnhance())
						{
							this.mCurMarkNum++;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private bool CanShengQiMarkShow()
	{
		if (this.mCurMarkNum >= 2)
		{
			return false;
		}
		if (Globals.Instance.Player.TeamSystem == null)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(i);
			if (socket != null)
			{
				for (int j = 4; j < 6; j++)
				{
					ItemDataEx equip = socket.GetEquip(j);
					if (equip != null)
					{
						if (equip.CanEnhance())
						{
							this.mCurMarkNum++;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private bool CanXingZuoMarkShow()
	{
		if (this.mCurMarkNum >= 2)
		{
			return false;
		}
		if (RedFlagTools.CanShowXingZuoMark())
		{
			this.mCurMarkNum++;
			return true;
		}
		return false;
	}

	private void Awake()
	{
		string @string = Singleton<StringManager>.Instance.GetString("recommendText2");
		GameObject gameObject = GameUITools.RegisterClickEvent("choujiang", new UIEventListener.VoidDelegate(this.OnChoujiangClick), base.gameObject);
		UILabel component = gameObject.transform.Find("Label").GetComponent<UILabel>();
		component.overflowMethod = UILabel.Overflow.ShrinkContent;
		component.width = 140;
		gameObject = GameUITools.RegisterClickEvent("pets", new UIEventListener.VoidDelegate(this.OnPetsClick), base.gameObject);
		UILabel component2 = gameObject.transform.Find("Label").GetComponent<UILabel>();
		component2.overflowMethod = UILabel.Overflow.ShrinkContent;
		component2.width = 140;
		GameObject gameObject2 = gameObject.transform.Find("mark").gameObject;
		gameObject2.transform.Find("txt").GetComponent<UILabel>().text = @string;
		gameObject2.SetActive(this.CanPetMarkShow());
		gameObject = GameUITools.RegisterClickEvent("equips", new UIEventListener.VoidDelegate(this.OnEquipsClick), base.gameObject);
		UILabel component3 = gameObject.transform.Find("Label").GetComponent<UILabel>();
		component3.overflowMethod = UILabel.Overflow.ShrinkContent;
		component3.width = 140;
		GameObject gameObject3 = gameObject.transform.Find("mark").gameObject;
		gameObject3.transform.Find("txt").GetComponent<UILabel>().text = @string;
		gameObject3.SetActive(this.CanEquipsMarkShow());
		gameObject = GameUITools.RegisterClickEvent("shengQis", new UIEventListener.VoidDelegate(this.OnShengqiClick), base.gameObject);
		UILabel component4 = gameObject.transform.Find("Label").GetComponent<UILabel>();
		component4.overflowMethod = UILabel.Overflow.ShrinkContent;
		component4.width = 140;
		GameObject gameObject4 = gameObject.transform.Find("mark").gameObject;
		gameObject4.transform.Find("txt").GetComponent<UILabel>().text = @string;
		gameObject4.SetActive(this.CanShengQiMarkShow());
		gameObject = GameUITools.RegisterClickEvent("xingzuo", new UIEventListener.VoidDelegate(this.OnXingzuoClick), base.gameObject);
		UILabel component5 = gameObject.transform.Find("Label").GetComponent<UILabel>();
		component5.overflowMethod = UILabel.Overflow.ShrinkContent;
		component5.width = 140;
		GameObject gameObject5 = gameObject.transform.Find("mark").gameObject;
		gameObject5.transform.Find("txt").GetComponent<UILabel>().text = @string;
		gameObject5.SetActive(this.CanXingZuoMarkShow());
		Transform transform = base.transform.Find("Tips");
		if (transform != null)
		{
			UILabel component6 = transform.GetComponent<UILabel>();
			component6.overflowMethod = UILabel.Overflow.ShrinkContent;
			component6.width = 500;
		}
	}

	private void OnChoujiangClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResultSceneInfo = null;
		GameUIManager.mInstance.ChangeSession<GUIRollSceneV2>(null, true, true);
	}

	private void OnPetsClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResultSceneInfo = null;
		GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, true, true);
	}

	private void OnEquipsClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResultSceneInfo = null;
		GameUIManager.mInstance.ChangeSession<GUIEquipBagScene>(null, true, true);
	}

	private void OnShengqiClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResultSceneInfo = null;
		GameUIManager.mInstance.ChangeSession<GUITrinketBagScene>(null, true, true);
	}

	private void OnXingzuoClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResultSceneInfo = null;
		if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(7)))
		{
			GameUIManager.mInstance.ChangeSession<GUIConstellationScene>(null, true, true);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("XingZuo27", new object[]
			{
				GameConst.GetInt32(7)
			}), 0f, 0f);
		}
	}
}
