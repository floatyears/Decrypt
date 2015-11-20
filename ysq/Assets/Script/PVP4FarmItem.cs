using Att;
using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class PVP4FarmItem : MonoBehaviour
{
	private Transform itemReward;

	public bool isPlaying
	{
		get;
		private set;
	}

	public void Init(int timeIndex, MS2C_PvpArenaResult data)
	{
		GameUITools.FindUILabel("times/Label", base.gameObject).text = Singleton<StringManager>.Instance.GetString("Pillage11", new object[]
		{
			timeIndex + 1
		});
		GameUITools.FindUILabel("exp/Label", base.gameObject).text = string.Format("{0:#,###0}", data.Exp);
		GameUITools.FindUILabel("money/Label", base.gameObject).text = string.Format("{0:#,###0}", data.Money);
		GameUITools.FindUILabel("honor/Label", base.gameObject).text = string.Format("{0:#,###0}", data.Honor);
		this.itemReward = base.transform.FindChild("item");
		if (data.ExtraItemID != 0 && data.ExtraItemCount != 0)
		{
			this.RefreshItemReward(data.ExtraItemID, data.ExtraItemCount);
		}
		else if (data.ExtraDiamond != 0)
		{
			GameObject gameObject = GameUITools.CreateReward(2, data.ExtraDiamond, data.ExtraDiamond, this.itemReward, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
			if (gameObject == null)
			{
				global::Debug.LogErrorFormat("GameUITools.CreateReward Diamond Error", new object[0]);
				this.itemReward.gameObject.SetActive(false);
			}
			else
			{
				this.itemReward.gameObject.SetActive(true);
				Vector3 localPosition = this.itemReward.localPosition;
				localPosition.y = 0f;
				this.itemReward.localPosition = localPosition;
			}
			gameObject.AddComponent<UIDragScrollView>();
			Transform transform = this.itemReward.FindChild("itemName");
			transform.gameObject.SetActive(false);
		}
		else if (data.ExtraMoney != 0)
		{
			GameObject gameObject2 = GameUITools.CreateReward(1, data.ExtraMoney, data.ExtraMoney, this.itemReward, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
			if (gameObject2 == null)
			{
				global::Debug.LogErrorFormat("GameUITools.CreateReward Money Error", new object[0]);
				this.itemReward.gameObject.SetActive(false);
			}
			else
			{
				this.itemReward.gameObject.SetActive(true);
				Vector3 localPosition2 = this.itemReward.localPosition;
				localPosition2.y = 0f;
				this.itemReward.localPosition = localPosition2;
			}
			gameObject2.AddComponent<UIDragScrollView>();
			Transform transform2 = this.itemReward.FindChild("itemName");
			transform2.gameObject.SetActive(false);
		}
	}

	private void RefreshItemReward(int itemID, int itenCount)
	{
		this.itemReward.gameObject.SetActive(false);
		if (itemID != 0)
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(itemID);
			if (info == null)
			{
				global::Debug.LogErrorFormat("Can not find item Info {0}", new object[]
				{
					itemID
				});
			}
			else
			{
				GameObject gameObject = GameUITools.CreateReward(3, itemID, itenCount, this.itemReward, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
				if (gameObject == null)
				{
					global::Debug.LogErrorFormat("GameUITools.CreateReward Error {0}", new object[]
					{
						itemID
					});
				}
				gameObject.AddComponent<UIDragScrollView>();
				UILabel component = this.itemReward.FindChild("itemName").GetComponent<UILabel>();
				component.text = info.Name;
				component.color = Tools.GetItemQualityColor(info.Quality);
				component.gameObject.SetActive(true);
				this.itemReward.gameObject.SetActive(true);
			}
		}
	}

	public void ShowAnim()
	{
		this.isPlaying = true;
		base.StartCoroutine(this.PlayAnim());
	}

	[DebuggerHidden]
	private IEnumerator PlayAnim()
	{
        return null;
        //PVP4FarmItem.<PlayAnim>c__Iterator80 <PlayAnim>c__Iterator = new PVP4FarmItem.<PlayAnim>c__Iterator80();
        //<PlayAnim>c__Iterator.<>f__this = this;
        //return <PlayAnim>c__Iterator;
	}
}
