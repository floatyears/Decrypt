using Proto;
using System;
using UnityEngine;

public class WorldBossCombatRankItem : MonoBehaviour
{
	private UILabel mPlayerName;

	private UILabel mRankTxt;

	public void InitWithBaseScene(WorldBossCombatRank baseScene)
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mPlayerName = base.transform.GetComponent<UILabel>();
		this.mRankTxt = base.transform.Find("Label").GetComponent<UILabel>();
		UIDragScrollView uIDragScrollView = base.gameObject.AddComponent<UIDragScrollView>();
		uIDragScrollView.scrollView = base.transform.parent.parent.GetComponent<UIScrollView>();
	}

	public void Refresh(RankData data)
	{
		WorldBossCombatRankItem.RefreshRankItem(this.mPlayerName, this.mRankTxt, data);
	}

	public static void RefreshRankItem(UILabel name, UILabel rankTxt, RankData data)
	{
		if (data.Rank > 0 && data.Rank < 100)
		{
			string text = data.Rank.ToString();
			name.text = string.Format("{0}.  {1}", text.PadLeft(4 * (2 - text.Length), ' '), data.Data.Name);
		}
		else if (data.Rank == 0)
		{
			name.text = string.Format("   -  {0}", data.Data.Name);
		}
		else
		{
			name.text = string.Format("99+ {0}", data.Data.Name);
		}
		if (data.Value >= 1000000L)
		{
			rankTxt.text = data.Value / 10000L + Singleton<StringManager>.Instance.GetString("wan");
		}
		else
		{
			rankTxt.text = data.Value.ToString();
		}
	}
}
