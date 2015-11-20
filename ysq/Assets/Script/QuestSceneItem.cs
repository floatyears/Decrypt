using Att;
using System;
using UnityEngine;

public class QuestSceneItem : MonoBehaviour
{
	public delegate void QuestItemEvent(QuestInfo info);

	public QuestSceneItem.QuestItemEvent QuestItemClicked;

	private UISprite bg;

	private UILabel title;

	private UILabel target;

	private UILabel progress;

	private GameObject btnGo;

	private UILabel btnGoLb;

	private GameObject btnReceive;

	private Transform Reward;

	private UISprite itemIcon;

	private UISprite petIcon;

	private UISprite specialIcon;

	private UISprite Quality;

	private GameObject[] RewardItem = new GameObject[3];

	public bool isPlaying
	{
		get;
		private set;
	}

	public QuestInfo questInfo
	{
		get;
		private set;
	}

	public void InitQuestItem(QuestInfo qInfo)
	{
		this.bg = base.transform.GetComponent<UISprite>();
		this.title = this.bg.transform.Find("quest").GetComponent<UILabel>();
		this.target = this.bg.transform.Find("questTXT").GetComponent<UILabel>();
		this.progress = base.transform.Find("Label").GetComponent<UILabel>();
		this.btnGo = base.transform.Find("GoBtn").gameObject;
		this.btnGoLb = this.btnGo.transform.Find("Label").GetComponent<UILabel>();
		this.btnReceive = base.transform.Find("ReceiveBtn").gameObject;
		Tools.SetParticleRQWithUIScale(this.btnReceive.transform.FindChild("Sprite/ui67").gameObject, 3100);
		this.Reward = base.transform.Find("Reward");
		Transform transform = base.transform.Find("IconBg");
		this.itemIcon = transform.transform.Find("itemIcon").GetComponent<UISprite>();
		this.petIcon = transform.transform.Find("petIcon").GetComponent<UISprite>();
		this.specialIcon = transform.transform.Find("specialIcon").GetComponent<UISprite>();
		this.Quality = transform.transform.Find("QualityMark").GetComponent<UISprite>();
		UIEventListener expr_189 = UIEventListener.Get(this.bg.gameObject);
		expr_189.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_189.onClick, new UIEventListener.VoidDelegate(this.OnQuestItemClicked));
		UIEventListener expr_1B5 = UIEventListener.Get(this.btnGo);
		expr_1B5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1B5.onClick, new UIEventListener.VoidDelegate(this.OnQuestBtnClicked));
		UIEventListener expr_1E1 = UIEventListener.Get(this.btnReceive);
		expr_1E1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1E1.onClick, new UIEventListener.VoidDelegate(this.OnQuestBtnClicked));
		this.questInfo = qInfo;
		this.isPlaying = false;
		this.itemIcon.gameObject.SetActive(false);
		this.petIcon.gameObject.SetActive(false);
		this.specialIcon.gameObject.SetActive(false);
		switch (this.questInfo.IconType)
		{
		case 0:
		{
			this.itemIcon.gameObject.SetActive(false);
			this.petIcon.gameObject.SetActive(false);
			this.specialIcon.gameObject.SetActive(false);
			SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(qInfo.ID);
			if (info != null)
			{
				MonsterInfo monsterInfo = null;
				for (int i = 2; i >= 0; i--)
				{
					monsterInfo = Globals.Instance.AttDB.MonsterDict.GetInfo(info.Enemy[i]);
					if (monsterInfo != null)
					{
						break;
					}
				}
				if (monsterInfo != null)
				{
					this.petIcon.spriteName = monsterInfo.Icon;
					this.petIcon.gameObject.SetActive(true);
					this.Quality.spriteName = Tools.GetItemQualityIcon(monsterInfo.Quality);
				}
				else
				{
					global::Debug.LogErrorFormat("Can not find EQIcon_Boss Icon {0}", new object[]
					{
						this.questInfo.ID
					});
				}
			}
			break;
		}
		case 1:
		{
			PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(this.questInfo.IconValue);
			if (info2 != null)
			{
				this.petIcon.spriteName = info2.Icon;
				this.petIcon.gameObject.SetActive(true);
				this.Quality.spriteName = Tools.GetItemQualityIcon(info2.Quality);
			}
			else
			{
				global::Debug.LogErrorFormat("Can not find EQIcon_Pet Icon {0} PetID = {1}", new object[]
				{
					this.questInfo.ID,
					this.questInfo.IconValue
				});
			}
			break;
		}
		case 2:
		{
			ItemInfo info3 = Globals.Instance.AttDB.ItemDict.GetInfo(this.questInfo.IconValue);
			if (info3 != null)
			{
				this.itemIcon.spriteName = info3.Icon;
				this.itemIcon.gameObject.SetActive(true);
				this.Quality.spriteName = Tools.GetItemQualityIcon(info3.Quality);
			}
			else
			{
				global::Debug.LogErrorFormat("Can not find EQIcon_Item Icon {0} ItemID = {1}", new object[]
				{
					this.questInfo.ID,
					this.questInfo.IconValue
				});
			}
			break;
		}
		default:
			this.specialIcon.gameObject.SetActive(true);
			this.Quality.spriteName = Tools.GetItemQualityIcon(2);
			break;
		}
		for (int j = 0; j < this.RewardItem.Length; j++)
		{
			if (this.RewardItem[j] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[j]);
				this.RewardItem[j] = null;
			}
		}
		int k = 0;
		int num = 0;
		while (k < this.questInfo.RewardType.Count)
		{
			if (k < this.RewardItem.Length && this.questInfo.RewardType[k] != 0 && this.questInfo.RewardType[k] != 20)
			{
				this.RewardItem[num] = GameUITools.CreateReward(this.questInfo.RewardType[k], this.questInfo.RewardValue1[k], this.questInfo.RewardValue2[k], this.Reward, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (this.RewardItem[num] != null)
				{
					this.RewardItem[num].transform.localPosition = new Vector3((float)(num * 112), 0f, 0f);
					num++;
				}
			}
			k++;
		}
	}

	public void RefreshQuestItem()
	{
		if (GUIQuestScene.IsTrunk(this.questInfo))
		{
			this.title.text = string.Format("[{0}]{1}", Singleton<StringManager>.Instance.GetString("QuestTrunk"), this.questInfo.Name);
		}
		else
		{
			this.title.text = this.questInfo.Name;
		}
		if (Globals.Instance.Player.GetQuestState(this.questInfo.ID) == 1)
		{
			this.target.text = string.Format("{0}\r\n[66ff00]{1}[-]", this.questInfo.Desc, (this.questInfo.Target3.Length == 0) ? this.questInfo.Target : this.questInfo.Target3);
			this.progress.gameObject.SetActive(false);
			this.btnGo.SetActive(false);
			this.btnReceive.SetActive(true);
			this.bg.spriteName = "gold_bg";
		}
		else
		{
			this.target.text = string.Format("{0}\r\n[ffe400]{1}[-]", this.questInfo.Desc, this.questInfo.Target);
			this.progress.gameObject.SetActive(false);
			this.btnGo.SetActive(true);
			this.btnGoLb.text = Singleton<StringManager>.Instance.GetString("QuestTxt3");
			this.btnGoLb.transform.parent.gameObject.SetActive(true);
			this.btnReceive.SetActive(false);
			this.bg.spriteName = "Price_bg";
		}
	}

	public void PlayRemoveAnim()
	{
		TweenPosition component = base.transform.GetComponent<TweenPosition>();
		if (component != null)
		{
			component.from = base.transform.localPosition;
			component.to = base.transform.localPosition;
			TweenPosition expr_40_cp_0 = component;
			expr_40_cp_0.to.x = expr_40_cp_0.to.x + (float)(this.bg.width + 5);
			component.ResetToBeginning();
			component.enabled = true;
			this.isPlaying = true;
		}
	}

	public void OnQuestItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.QuestItemClicked != null)
		{
			this.QuestItemClicked(this.questInfo);
		}
	}

	public void OnQuestBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIQuestInformation.QuestBtnClicked(this.questInfo);
	}
}
