using Holoville.HOTween;
using System;
using UnityEngine;

public class GUILeftInfo : MonoBehaviour
{
	private UILabel mTitle;

	private UILabel mMsg;

	private UILabel mHp;

	private UILabel mWufang;

	private UILabel mFaFang;

	private UILabel mAttack;

	private UILabel mHpNum;

	private UILabel mWufangNum;

	private UILabel mFaFangNum;

	private UILabel mAttackNum;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	public void CreateObjects()
	{
		this.mTitle = base.transform.Find("title").GetComponent<UILabel>();
		this.mMsg = base.transform.Find("msg").GetComponent<UILabel>();
		this.mHp = base.transform.Find("hp").GetComponent<UILabel>();
		this.mWufang = base.transform.Find("wufang").GetComponent<UILabel>();
		this.mFaFang = base.transform.Find("fafang").GetComponent<UILabel>();
		this.mAttack = base.transform.Find("attack").GetComponent<UILabel>();
		this.mHpNum = this.mHp.transform.Find("num").GetComponent<UILabel>();
		this.mWufangNum = this.mWufang.transform.Find("num").GetComponent<UILabel>();
		this.mFaFangNum = this.mFaFang.transform.Find("num").GetComponent<UILabel>();
		this.mAttackNum = this.mAttack.transform.Find("num").GetComponent<UILabel>();
	}

	public void Refresh(int conLv)
	{
		this.mTitle.text = Singleton<StringManager>.Instance.GetString("XingZuo1");
		this.mMsg.text = Singleton<StringManager>.Instance.GetString("XingZuo2");
		this.mHp.text = Singleton<StringManager>.Instance.GetString("XingZuo3");
		this.mFaFang.text = Singleton<StringManager>.Instance.GetString("XingZuo6");
		this.mWufang.text = Singleton<StringManager>.Instance.GetString("XingZuo5");
		this.mAttack.text = Singleton<StringManager>.Instance.GetString("XingZuo4");
		if (conLv == 0)
		{
			this.mAttackNum.text = Singleton<StringManager>.Instance.GetString("XingZuo26", new object[]
			{
				0
			});
			this.mWufangNum.text = Singleton<StringManager>.Instance.GetString("XingZuo26", new object[]
			{
				0
			});
			this.mHpNum.text = Singleton<StringManager>.Instance.GetString("XingZuo26", new object[]
			{
				0
			});
			this.mFaFangNum.text = Singleton<StringManager>.Instance.GetString("XingZuo26", new object[]
			{
				0
			});
		}
		if (conLv > 0 && ConLevelInfo.GetConInfo(conLv) != null)
		{
			this.RefreshNum(conLv);
		}
	}

	public void RefreshNum(int conLv)
	{
		if (ConLevelInfo.GetConInfo(conLv) != null)
		{
			this.mAttackNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
			{
				ConLevelInfo.GetConInfo(conLv).Attack
			});
			this.mWufangNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
			{
				ConLevelInfo.GetConInfo(conLv).PhysicDefense
			});
			this.mHpNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
			{
				ConLevelInfo.GetConInfo(conLv).MaxHP
			});
			this.mFaFangNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
			{
				ConLevelInfo.GetConInfo(conLv).MagicDefense
			});
		}
	}

	public void TxtChangeAni(int conLv)
	{
		int num = (conLv - 1) % 5 + 1;
		UILabel uILabel = null;
		if (num == 1)
		{
			uILabel = this.mAttackNum;
		}
		if (num == 2)
		{
			uILabel = this.mWufangNum;
		}
		if (num == 3)
		{
			uILabel = this.mFaFangNum;
		}
		if (num == 4)
		{
			uILabel = this.mHpNum;
		}
		if (num == 5)
		{
			return;
		}
		Sequence sequence = new Sequence();
		sequence.Append(HOTween.To(uILabel.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
		sequence.Append(HOTween.To(uILabel.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
		sequence.Play();
	}
}
