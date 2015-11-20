using System;
using UnityEngine;

public class MainSkillButtonBase : MonoBehaviour
{
	protected int m_skillIndex = -1;

	public int SkillIndex
	{
		get
		{
			return this.m_skillIndex;
		}
		private set
		{
			this.m_skillIndex = value;
		}
	}
}
