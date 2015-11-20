using Att;
using System;
using UnityEngine;

public class GUIAttributeValue : MonoBehaviour
{
	private UILabel mHP;

	private UILabel mAttack;

	private UILabel mPhysicD;

	private UILabel mMagicD;

	private bool initFlag;

	private void CreateObjects()
	{
		this.mHP = GameUITools.FindUILabel("HP/Value", base.gameObject);
		this.mAttack = GameUITools.FindUILabel("Attack/Value", base.gameObject);
		this.mPhysicD = GameUITools.FindUILabel("PhysicD/Value", base.gameObject);
		this.mMagicD = GameUITools.FindUILabel("MagicD/Value", base.gameObject);
		this.initFlag = true;
	}

	public void Refresh(LopetDataEx data)
	{
		if (!this.initFlag)
		{
			this.CreateObjects();
		}
		if (data == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		data.GetAttribute(ref num, ref num2, ref num3, ref num4);
		this.mHP.text = num.ToString();
		this.mAttack.text = num2.ToString();
		this.mPhysicD.text = num3.ToString();
		this.mMagicD.text = num4.ToString();
	}

	public void Refresh(PetDataEx data, bool isAtt = true)
	{
		if (!this.initFlag)
		{
			this.CreateObjects();
		}
		if (data == null)
		{
			return;
		}
		if (isAtt)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			data.GetAttribute(ref num, ref num2, ref num3, ref num4);
			this.mHP.text = num.ToString();
			this.mAttack.text = num2.ToString();
			this.mPhysicD.text = num3.ToString();
			this.mMagicD.text = num4.ToString();
		}
		else
		{
			CultivateInfo info = Globals.Instance.AttDB.CultivateDict.GetInfo((int)((data.Data.Level + 9u) / 10u));
			if (info != null && data.Info.Quality >= 0)
			{
				if (data.Info.Quality < info.MaxCulMaxHP.Count)
				{
					this.mHP.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
					{
						data.Data.MaxHP,
						info.MaxCulMaxHP[data.Info.Quality]
					});
				}
				if (data.Info.Quality < info.MaxCulAttack.Count)
				{
					this.mAttack.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
					{
						data.Data.Attack,
						info.MaxCulAttack[data.Info.Quality]
					});
				}
				if (data.Info.Quality < info.MaxCulPhysicDefense.Count)
				{
					this.mPhysicD.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
					{
						data.Data.PhysicDefense,
						info.MaxCulPhysicDefense[data.Info.Quality]
					});
				}
				if (data.Info.Quality < info.MaxCulMagicDefense.Count)
				{
					this.mMagicD.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
					{
						data.Data.MagicDefense,
						info.MaxCulMagicDefense[data.Info.Quality]
					});
				}
			}
		}
	}
}
