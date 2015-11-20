using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/FX/SMPrefabGenerator")]
public class SMPrefabGenerator : MonoBehaviour
{
	public GameObject[] createThis;

	public float MaxLifeTime = 2f;

	private int rndNr;

	public int thisManyTimes = 3;

	public float overThisTime = 1f;

	public float xWidth;

	public float yWidth;

	public float zWidth;

	public float xRotMax;

	public float yRotMax = 180f;

	public float zRotMax;

	public bool allUseSameRotation;

	private bool allRotationDecided;

	public bool detachToWorld = true;

	private float x_cur;

	private float y_cur;

	private float z_cur;

	private float xRotCur;

	private float yRotCur;

	private float zRotCur;

	private float timeCounter;

	private int effectCounter;

	private float trigger;

	private List<GameObject> createdGOs = new List<GameObject>();

	private void Start()
	{
		if (this.thisManyTimes < 1)
		{
			this.thisManyTimes = 1;
		}
		this.trigger = this.overThisTime / (float)this.thisManyTimes;
	}

	private void OnSpawned()
	{
		this.allRotationDecided = false;
		this.x_cur = 0f;
		this.y_cur = 0f;
		this.z_cur = 0f;
		this.xRotCur = 0f;
		this.yRotCur = 0f;
		this.zRotCur = 0f;
		this.timeCounter = 0f;
		this.effectCounter = 0;
		if (this.thisManyTimes < 1)
		{
			this.thisManyTimes = 1;
		}
		this.trigger = this.overThisTime / (float)this.thisManyTimes;
	}

	private void Update()
	{
		if (this.createThis.Length == 0)
		{
			return;
		}
		this.timeCounter += Time.deltaTime;
		if (this.timeCounter > this.trigger && this.effectCounter <= this.thisManyTimes)
		{
			this.rndNr = Mathf.FloorToInt(UnityEngine.Random.value * (float)this.createThis.Length);
			if (this.createThis[this.rndNr] == null)
			{
				return;
			}
			this.x_cur = base.transform.position.x + UnityEngine.Random.value * this.xWidth - this.xWidth * 0.5f;
			this.y_cur = base.transform.position.y + UnityEngine.Random.value * this.yWidth - this.yWidth * 0.5f;
			this.z_cur = base.transform.position.z + UnityEngine.Random.value * this.zWidth - this.zWidth * 0.5f;
			if (!this.allUseSameRotation || !this.allRotationDecided)
			{
				this.xRotCur = base.transform.rotation.x + UnityEngine.Random.value * this.xRotMax * 2f - this.xRotMax;
				this.yRotCur = base.transform.rotation.y + UnityEngine.Random.value * this.yRotMax * 2f - this.yRotMax;
				this.zRotCur = base.transform.rotation.z + UnityEngine.Random.value * this.zRotMax * 2f - this.zRotMax;
				this.allRotationDecided = true;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(this.createThis[this.rndNr], new Vector3(this.x_cur, this.y_cur, this.z_cur), base.transform.rotation) as GameObject;
			if (gameObject != null)
			{
				gameObject.transform.Rotate(this.xRotCur, this.yRotCur, this.zRotCur);
				if (!this.detachToWorld)
				{
					gameObject.transform.parent = base.transform;
					this.createdGOs.Add(gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(gameObject, this.MaxLifeTime);
				}
			}
			this.timeCounter -= this.trigger;
			this.effectCounter++;
		}
	}

	private void OnDespawned()
	{
		for (int i = 0; i < this.createdGOs.Count; i++)
		{
			if (this.createdGOs[i] != null)
			{
				UnityEngine.Object.Destroy(this.createdGOs[i]);
			}
		}
	}
}
