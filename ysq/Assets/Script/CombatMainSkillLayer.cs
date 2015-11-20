using System;
using UnityEngine;

public class CombatMainSkillLayer : MonoBehaviour
{
	public CombatMainSkillButton SkillBtn1
	{
		get;
		private set;
	}

	public CombatMainSkillButton SkillBtn2
	{
		get;
		private set;
	}

	public CombatMainSkillButton SkillBtn3
	{
		get;
		private set;
	}

	public CombatMainCWSkillButton SkillBtn4
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.SkillBtn1 = base.transform.FindChild("skill1").gameObject.AddComponent<CombatMainSkillButton>();
		this.SkillBtn2 = base.transform.FindChild("skill2").gameObject.AddComponent<CombatMainSkillButton>();
		this.SkillBtn3 = base.transform.FindChild("skill3").gameObject.AddComponent<CombatMainSkillButton>();
		this.SkillBtn4 = base.transform.FindChild("skill4").gameObject.AddComponent<CombatMainCWSkillButton>();
		this.SkillBtn4.gameObject.SetActive(false);
	}

	public void SetState(int nState)
	{
		this.SkillBtn1.SetState(nState);
		this.SkillBtn2.SetState(nState);
		this.SkillBtn3.SetState(nState);
		this.SkillBtn4.SetState(nState);
		LopetDataEx curLopet = Globals.Instance.Player.LopetSystem.GetCurLopet(true);
		this.SkillBtn4.gameObject.SetActive(curLopet != null);
	}
}
