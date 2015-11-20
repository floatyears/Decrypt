using System;
using UnityEngine;

public class GUILopetTitleInfo : MonoBehaviour
{
	private UILabel mName;

	private UILabel mLevelValue;

	private GUIStars mStars;

	private bool initFlag;

	private void CreateObjects()
	{
		this.mName = GameUITools.FindUILabel("name", base.gameObject);
		this.mLevelValue = GameUITools.FindUILabel("lvlTxt/num", base.gameObject);
		this.mStars = GameUITools.FindGameObject("stars", base.gameObject).AddComponent<GUIStars>();
		this.mStars.Init(5);
		this.mStars.Hide();
		this.initFlag = true;
		Vector3 localPosition = this.mLevelValue.transform.parent.localPosition;
		localPosition.x = -50f;
		this.mLevelValue.transform.parent.localPosition = localPosition;
	}

	public void Refresh(LopetDataEx data)
	{
		if (!this.initFlag)
		{
			this.CreateObjects();
		}
		if (data.Data.Awake > 0u)
		{
			this.mName.text = Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				data.Info.Name,
				data.Data.Awake
			});
		}
		else
		{
			this.mName.text = data.Info.Name;
		}
		this.mName.color = Tools.GetItemQualityColor(data.Info.Quality);
		this.mLevelValue.text = data.Data.Level.ToString();
	}
}
