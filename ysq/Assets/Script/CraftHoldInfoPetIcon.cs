using Att;
using System;
using UnityEngine;

public class CraftHoldInfoPetIcon : MonoBehaviour
{
	private UISprite mIcon;

	private UISprite mFrame;

	private UISlider mRankHp;

	public void Init()
	{
		this.mIcon = base.transform.GetComponent<UISprite>();
		this.mFrame = base.transform.Find("Frame").GetComponent<UISprite>();
		this.mRankHp = base.transform.Find("hp").GetComponent<UISlider>();
	}

	public void Refresh(int id, float health)
	{
		if (id == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(id);
		if (info == null)
		{
			global::Debug.LogErrorFormat("Get Pet Info Error , ID : {0} ", new object[]
			{
				id
			});
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		this.mIcon.spriteName = info.Icon;
		this.mFrame.spriteName = Tools.GetItemQualityIcon(info.Quality);
		this.mRankHp.value = Mathf.Clamp01(health / 10000f);
	}
}
