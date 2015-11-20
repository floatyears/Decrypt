using Att;
using Proto;
using System;
using UnityEngine;

public class GuildCraftMapTipForTaken : MonoBehaviour
{
	private UILabel mTowerName;

	private UILabel mTakenGuild;

	private UISprite mMoneySp;

	private UISprite mMoneyQuality;

	private UILabel mMoneyDesc;

	private UILabel mDiamondDesc;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTowerName = base.transform.Find("towerName").GetComponent<UILabel>();
		this.mTakenGuild = base.transform.Find("txt0/name").GetComponent<UILabel>();
		this.mMoneyDesc = base.transform.Find("txt1/money").GetComponent<UILabel>();
		this.mMoneySp = base.transform.Find("txt1/icon").GetComponent<UISprite>();
		this.mMoneyQuality = base.transform.Find("txt1/qualityMask").GetComponent<UISprite>();
		this.mDiamondDesc = base.transform.Find("txt2/diamond").GetComponent<UILabel>();
	}

	public void ShowTip(GameObject parentGo, GuildWarClientCity gwCC)
	{
		if (gwCC == null)
		{
			return;
		}
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo != null)
		{
			GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(gwCC.City.CityId);
			if (info == null)
			{
				return;
			}
			base.transform.parent = parentGo.transform;
			base.transform.localPosition = new Vector3(-130f, -120f, 0f);
			base.transform.localScale = Vector3.one;
			this.mTowerName.text = info.CastleName;
			if (gwCC.City.CityId == 1)
			{
				this.mTowerName.color = Tools.GetItemQualityColor(3);
			}
			else if (gwCC.City.CityId == 2)
			{
				this.mTowerName.color = Tools.GetItemQualityColor(2);
			}
			else
			{
				this.mTowerName.color = Tools.GetItemQualityColor(1);
			}
			this.mTakenGuild.text = ((!string.IsNullOrEmpty(gwCC.GuildName)) ? gwCC.GuildName : Singleton<StringManager>.Instance.GetString("guildCraft5"));
			for (int i = 0; i < info.RewardType.Count; i++)
			{
				if (info.RewardType[i] == 3)
				{
					ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(info.RewardValue1[i]);
					if (info2 != null)
					{
						this.mMoneySp.spriteName = info2.Icon;
						this.mMoneyQuality.spriteName = Tools.GetItemQualityIcon(info2.Quality);
						this.mMoneyDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft4", new object[]
						{
							info.RewardValue2[i]
						});
					}
				}
				else if (info.RewardType[i] == 15)
				{
					this.mDiamondDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft4", new object[]
					{
						info.RewardValue1[i]
					});
				}
			}
			base.gameObject.SetActive(true);
		}
	}

	public void HideTip()
	{
		base.gameObject.SetActive(false);
	}
}
