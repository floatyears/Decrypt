using Att;
using Proto;
using System;
using UnityEngine;

public class PillageTargetItem : MonoBehaviour
{
	private const int NUM_ICON = 4;

	private UILabel Level;

	private UILabel Name;

	private UILabel Rate;

	private UISprite[] Icon = new UISprite[4];

	private UISprite[] Quality = new UISprite[4];

	private UIButton farm;

	[NonSerialized]
	public UIButton pk;

	private RankData data;

	public bool Farm
	{
		get
		{
			return this.farm.gameObject.activeSelf;
		}
		set
		{
			this.farm.gameObject.SetActive(value);
		}
	}

	public void ShowRankData(RankData _data)
	{
		if (this.data == null)
		{
			this.CreateObjets();
		}
		this.data = _data;
		this.Level.text = string.Format("Lv {0}", this.data.Data.Level);
		this.Name.text = this.data.Data.Name;
		this.Name.color = Tools.GetItemQualityColor(LocalPlayer.GetQuality(this.data.Data.ConstellationLevel));
		this.Rate.text = Singleton<StringManager>.Instance.GetString(string.Format("PillageR{0}", this.data.Value));
		this.Rate.color = Tools.GetItemQualityColor((int)this.data.Value);
		FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(this.data.Data.FashionID);
		if (info != null)
		{
			this.Icon[0].spriteName = info.Icon;
		}
		this.Quality[0].spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.data.Data.ConstellationLevel));
		this.Icon[0].gameObject.SetActive(true);
		int i = 1;
		int num = 0;
		while (i < 4)
		{
			if (num >= this.data.Data.PetInfoID.Count)
			{
				this.Icon[i].gameObject.SetActive(false);
				num++;
				i++;
			}
			else if (this.data.Data.PetInfoID[num] == 0)
			{
				num++;
			}
			else
			{
				PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(this.data.Data.PetInfoID[num]);
				if (info2 == null)
				{
					global::Debug.LogErrorFormat("can not find anra target pet info {0}", new object[]
					{
						this.data.Data.PetInfoID[num]
					});
					num++;
				}
				else
				{
					this.Icon[i].spriteName = info2.Icon;
					this.Quality[i].spriteName = Tools.GetItemQualityIcon(info2.Quality);
					this.Icon[i].gameObject.SetActive(true);
					num++;
					i++;
				}
			}
		}
	}

	private void CreateObjets()
	{
		this.Level = base.transform.FindChild("Level").GetComponent<UILabel>();
		this.Name = base.transform.FindChild("Name").GetComponent<UILabel>();
		this.Rate = base.transform.FindChild("Quality").GetComponent<UILabel>();
		Transform transform = base.transform.FindChild("Team");
		for (int i = 0; i < 4; i++)
		{
			this.Icon[i] = transform.FindChild(string.Format("Icon{0}", i)).GetComponent<UISprite>();
			this.Quality[i] = this.Icon[i].transform.FindChild("Quality").GetComponent<UISprite>();
			this.Icon[i].gameObject.SetActive(false);
		}
		this.farm = transform.FindChild("pk5").GetComponent<UIButton>();
		this.pk = transform.FindChild("pk").GetComponent<UIButton>();
		UIEventListener expr_10A = UIEventListener.Get(this.farm.gameObject);
		expr_10A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_10A.onClick, new UIEventListener.VoidDelegate(this.OnFarmTraget));
		UIEventListener expr_13B = UIEventListener.Get(this.pk.gameObject);
		expr_13B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_13B.onClick, new UIEventListener.VoidDelegate(this.OnPkTraget));
	}

	public void OnPkTraget(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.data == null)
		{
			return;
		}
		if (GameUIManager.mInstance.uiState.PillageItem == null)
		{
			return;
		}
		if (this.Farm && Tools.CanPlay(GameConst.GetInt32(9), true))
		{
			GUIPillageScene.RequestPvpPillageFarm(this.data.Data.GUID, 1, false);
		}
		else if ((float)Globals.Instance.Player.TeamSystem.GetCombatValue() / (float)this.data.Data.CombatValue > 1.5f)
		{
			GUIPassCombatPopUp.Show(new GUIPassCombatPopUp.VoidCallback(this.SendPassFarmOnceMsg), new GUIPassCombatPopUp.VoidCallback(this.SendPKMsg));
		}
		else
		{
			this.SendPKMsg();
		}
	}

	private void SendPKMsg()
	{
		GUIPillageScene.RequestPvpPillageStart(this.data.Data.GUID, GameUIManager.mInstance.uiState.PillageItem.ID, this.Farm);
	}

	private void SendPassFarmOnceMsg()
	{
		GUIPillageScene.RequestPvpPillageFarm(this.data.Data.GUID, 1, true);
	}

	private void OnFarmTraget(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.data == null)
		{
			return;
		}
		GUIPillageScene.RequestPvpPillageFarm(this.data.Data.GUID, 5, false);
	}
}
