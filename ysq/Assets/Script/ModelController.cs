using System;
using UnityEngine;

[AddComponentMenu("Game/Character/ModelController")]
public sealed class ModelController : MonoBehaviour
{
	public GameObject normalModel;

	public GameObject specialModel;

	private int state;

	private void Start()
	{
		this.UpdateModel();
	}

	public void SetModelState(int value)
	{
		this.state = value;
		this.UpdateModel();
	}

	public Animation GetAnimation()
	{
		if (this.state == 0)
		{
			if (this.normalModel != null)
			{
				return this.normalModel.animation;
			}
		}
		else if (this.specialModel != null)
		{
			return this.specialModel.animation;
		}
		return base.gameObject.animation;
	}

	private void UpdateModel()
	{
		if (this.state == 0)
		{
			if (this.normalModel != null && !this.normalModel.activeInHierarchy)
			{
				this.normalModel.SetActive(true);
			}
			if (this.specialModel != null && this.specialModel.activeInHierarchy)
			{
				this.specialModel.SetActive(false);
			}
		}
		else
		{
			if (this.normalModel != null && this.normalModel.activeInHierarchy)
			{
				this.normalModel.SetActive(false);
			}
			if (this.specialModel != null && !this.specialModel.activeInHierarchy)
			{
				this.specialModel.SetActive(true);
			}
		}
	}
}
