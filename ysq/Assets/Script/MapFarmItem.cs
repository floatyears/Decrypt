using Att;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MapFarmItem : MonoBehaviour
{
	private List<GameObject> mMapRewardItems = new List<GameObject>();

	public bool isPlaying
	{
		get;
		private set;
	}

	public void InitMapFarmItem(GameUIMapFarm baseScene, int timeIndex, MS2C_PveResult pveData, SceneInfo sceneInfo)
	{
		base.transform.FindChild("Sprite/Label").GetComponent<UILabel>().text = string.Format(Singleton<StringManager>.Instance.GetString("FormFinishText"), Singleton<StringManager>.Instance.GetString("FormFinishTimes").Substring(timeIndex, 1));
		base.transform.FindChild("money/Label").GetComponent<UILabel>().text = string.Format("{0:#,###0}", pveData.LootMoney);
		base.transform.FindChild("exp/Label").GetComponent<UILabel>().text = pveData.LootExp.ToString();
		Transform transform = base.transform.FindChild("item");
		if (sceneInfo.Difficulty == 9)
		{
			transform.FindChild("noItemTips").gameObject.SetActive(false);
			GameObject gameObject = GameUITools.CreateReward(15, sceneInfo.RewardEmblem, 0, transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			if (gameObject == null)
			{
				return;
			}
			gameObject.transform.localScale = Vector3.zero;
			this.mMapRewardItems.Add(gameObject);
		}
		else if (pveData.Items.Count == 0)
		{
			transform.FindChild("noItemTips").gameObject.SetActive(true);
		}
		if (pveData.Items.Count != 0)
		{
			transform.FindChild("noItemTips").gameObject.SetActive(false);
			int num = 0;
			while (num < pveData.Items.Count && num < 4)
			{
				OpenLootData openLootData = pveData.Items[num];
				if (openLootData != null)
				{
					GameObject gameObject2 = GameUITools.CreateReward(3, openLootData.InfoID, (int)openLootData.Count, transform, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
					if (!(gameObject2 == null))
					{
						gameObject2.transform.localScale = Vector3.zero;
						this.mMapRewardItems.Add(gameObject2);
					}
				}
				num++;
			}
		}
	}

	public void ShowMapFarmItemAnim(int timeIndex)
	{
		this.isPlaying = true;
		base.StartCoroutine(this.PlayMapFarmItemAnim(timeIndex));
	}

	[DebuggerHidden]
	public IEnumerator PlayMapFarmItemAnim(int timeIndex)
	{
        return null;
        //MapFarmItem.<PlayMapFarmItemAnim>c__IteratorA2 <PlayMapFarmItemAnim>c__IteratorA = new MapFarmItem.<PlayMapFarmItemAnim>c__IteratorA2();
        //<PlayMapFarmItemAnim>c__IteratorA.<>f__this = this;
        //return <PlayMapFarmItemAnim>c__IteratorA;
	}
}
