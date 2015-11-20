using System;
using UnityEngine;

public class UIVirtualPad : MonoBehaviour
{
	private GameObject m_gobjDirection;

	private GameObject m_gobjPad;

	private GameObject m_gobgDirectionOrigional;

	private Vector3 m_psDirectionPos;

	private Vector3 m_psPadPos;

	private Vector3 m_psScreenPos;

	private void Awake()
	{
		this.m_psPadPos = default(Vector3);
		this.m_psDirectionPos = default(Vector3);
		this.m_gobjDirection = base.transform.FindChild("paddirection").gameObject;
		this.m_gobjPad = base.transform.FindChild("padbg").gameObject;
		this.m_gobgDirectionOrigional = this.m_gobjPad.transform.FindChild("origional").gameObject;
	}

	public void NeutralDPad()
	{
		if (null == this.m_gobjDirection || null == this.m_gobjPad)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.m_gobjDirection.SetActive(false);
		this.m_gobjPad.SetActive(true);
		this.m_gobgDirectionOrigional.SetActive(true);
		float x = (float)Screen.width * 0.12f;
		float y = (float)Screen.height * 0.12f * (float)Screen.width / (float)Screen.height;
		this.SetDPadInitDirection(new Vector3(x, y, 0f));
	}

	public void SetDPadInitDirection(Vector3 Pos)
	{
		this.m_psScreenPos = Pos;
		this.m_psPadPos.x = (Pos.x / (float)Screen.width * 144f - 72f) * (float)Screen.width / ((float)Screen.height * 1.5f) * 6.6666f;
		this.m_psPadPos.y = (Pos.y / (float)Screen.height * 96f - 48f) * 6.6666f;
		this.m_gobjPad.transform.localPosition = this.m_psPadPos;
	}

	public Vector3 GetDPadScreenPos()
	{
		return this.m_psScreenPos;
	}

	public void SetDPadDirection(Vector3 Pos)
	{
		if (!this.m_gobjDirection.activeInHierarchy)
		{
			this.m_gobjDirection.SetActive(true);
		}
		if (this.m_gobgDirectionOrigional.activeInHierarchy)
		{
			this.m_gobgDirectionOrigional.SetActive(false);
		}
		this.m_psDirectionPos.x = (Pos.x / (float)Screen.width * 144f - 72f) * (float)Screen.width / ((float)Screen.height * 1.5f) * 6.6666f;
		this.m_psDirectionPos.y = (Pos.y / (float)Screen.height * 96f - 48f) * 6.6666f;
		this.m_gobjDirection.transform.localPosition = this.m_psDirectionPos;
	}

	public void DisableDPad()
	{
		if (this.m_gobjDirection == null || this.m_gobjPad == null)
		{
			return;
		}
		base.gameObject.SetActive(false);
		this.m_gobjDirection.SetActive(false);
		this.m_gobjPad.SetActive(false);
		this.m_gobgDirectionOrigional.SetActive(false);
	}

	public void EnableDPad(Vector3 Pos)
	{
		if (this.m_gobjDirection == null || this.m_gobjPad == null)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.m_gobjPad.SetActive(true);
		this.m_gobjDirection.SetActive(true);
		this.m_gobgDirectionOrigional.SetActive(false);
		this.SetDPadDirection(Pos);
		this.SetDPadInitDirection(Pos);
	}
}
