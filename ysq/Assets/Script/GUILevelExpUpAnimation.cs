using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUILevelExpUpAnimation : MonoBehaviour
{
	public delegate uint ExpCallback(uint level, object obj);

	private UILabel levelLb;

	private UISlider levelSlider;

	private UISprite levelSliderFg;

	private UILabel levelSliderText;

	private GameObject levelImg;

	public bool IsLevelup
	{
		get
		{
			return this.levelImg.activeSelf;
		}
	}

	public void Init()
	{
		this.levelLb = base.transform.GetComponent<UILabel>();
		this.levelSlider = this.levelLb.transform.Find("LevelSlider").GetComponent<UISlider>();
		this.levelSliderFg = this.levelSlider.transform.Find("LevelFg").GetComponent<UISprite>();
		this.levelSliderText = this.levelSlider.transform.Find("LevelText").GetComponent<UILabel>();
		this.levelImg = this.levelLb.transform.Find("Sprite").gameObject;
		this.levelImg.SetActive(false);
		this.levelLb.gameObject.SetActive(false);
	}

	public static uint PlayerExpCallback(uint level, object obj)
	{
		return Globals.Instance.AttDB.LevelDict.GetInfo((int)level).PExp;
	}

	[DebuggerHidden]
	public IEnumerator PlayExpAnim(uint lastLevel, uint lastExp, uint curLevel, uint curExp, float rewardExp, GUILevelExpUpAnimation.ExpCallback getInfoExp, float duration, object obj = null)
	{
        return null;
        //GUILevelExpUpAnimation.<PlayExpAnim>c__Iterator50 <PlayExpAnim>c__Iterator = new GUILevelExpUpAnimation.<PlayExpAnim>c__Iterator50();
        //<PlayExpAnim>c__Iterator.getInfoExp = getInfoExp;
        //<PlayExpAnim>c__Iterator.lastLevel = lastLevel;
        //<PlayExpAnim>c__Iterator.obj = obj;
        //<PlayExpAnim>c__Iterator.curExp = curExp;
        //<PlayExpAnim>c__Iterator.rewardExp = rewardExp;
        //<PlayExpAnim>c__Iterator.duration = duration;
        //<PlayExpAnim>c__Iterator.lastExp = lastExp;
        //<PlayExpAnim>c__Iterator.curLevel = curLevel;
        //<PlayExpAnim>c__Iterator.<$>getInfoExp = getInfoExp;
        //<PlayExpAnim>c__Iterator.<$>lastLevel = lastLevel;
        //<PlayExpAnim>c__Iterator.<$>obj = obj;
        //<PlayExpAnim>c__Iterator.<$>curExp = curExp;
        //<PlayExpAnim>c__Iterator.<$>rewardExp = rewardExp;
        //<PlayExpAnim>c__Iterator.<$>duration = duration;
        //<PlayExpAnim>c__Iterator.<$>lastExp = lastExp;
        //<PlayExpAnim>c__Iterator.<$>curLevel = curLevel;
        //<PlayExpAnim>c__Iterator.<>f__this = this;
        //return <PlayExpAnim>c__Iterator;
	}

	public void RefreshExpItem(uint lastLevel, uint lastExp, uint curLevel, uint curExp, float rewardExp, GUILevelExpUpAnimation.ExpCallback getInfoExp, object obj = null)
	{
		this.levelLb.gameObject.SetActive(true);
		this.levelSlider.gameObject.SetActive(true);
		this.levelLb.text = string.Format("Lv {0}", lastLevel);
		uint num = getInfoExp(curLevel, obj);
		if (num == 0u)
		{
			num = curExp;
		}
		if (curExp >= num)
		{
			this.levelSlider.value = 1f;
			this.levelSliderFg.spriteName = "lvlFg1";
			this.levelSliderText.text = "100%";
		}
		else
		{
			this.levelSlider.value = curExp / num;
			this.levelSliderFg.spriteName = "lvlFg0";
			this.levelSliderText.text = string.Format("{0}%", Mathf.FloorToInt(this.levelSlider.value * 100f));
		}
		if (lastLevel < curLevel)
		{
			this.levelImg.SetActive(true);
		}
	}
}
