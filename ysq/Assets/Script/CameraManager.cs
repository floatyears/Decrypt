using Att;
using NtUniSdk.Unity3d;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
	private Camera mainCamera;

	private float camInitFov;

	private float camTargetDist;

	private float camTargetOffset = 1f;

	private float camTargetOffset2 = 2f;

	private Vector3 camInitPos;

	private Vector3 camTargetDir;

	private Vector3 camCurBasePos;

	private Vector3 camCurTargetDir;

	private Vector3 camCurForward;

	private float camCurTargetDist;

	private bool camPause;

	private bool camLock;

	private Vector3 camLookPos;

	private float saveCamTargetDist;

	private Vector3 saveCamTargetDir;

	[HideInInspector]
	public bool screenShockTest;

	private float screenShockTotalTime = 0.4f;

	private float screenShockFrequency = 300f;

	private float screenShockAmplitude = 1f;

	private Vector3 screenShockInitCameraEulerAngles;

	private float screenShockInitTime;

	private bool screenShockPlay;

	[HideInInspector]
	public bool fovShockTest;

	private float fovShockTotalTime = 0.2f;

	private float fovShockFrequency = 80f;

	private float fovShockAmplitude = 2f;

	private float fovShockInitTime;

	private bool fovShockPlay;

	[HideInInspector]
	public bool posShockTest;

	private float posShockTotalTime = 0.2f;

	private float posShockFrequency = 60f;

	private float posShockAmplitude = 0.05f;

	private float posShockInitTime;

	private bool posShockPlay;

	public bool resultCamTest;

	private float resultCamLookYOfset;

	private float resultCamTotalTime;

	private float resultCamInitTime;

	private bool resultCamPlay;

	public bool targetCamTest;

	public GameObject Target;

	private float targetCamTotalTime;

	private float targetCamInitTime;

	private bool targetCamPlay;

	private float worldBossHeight;

	private float worldBossRidous;

	private bool worldBossAttackTest;

	public bool worldBossAttackPlay;

	private float posConst = 0.1f;

	private float posConstLen = 0.01f;

	private float distConst = 0.05f;

	private ESceneType scenetype;

	public bool dynamicCam;

	private int camFocusIdx;

	public float PVPCameraMoveSpeed = 120f;

	private ActorController selectActor;

	private Vector3 tempPos = Vector3.zero;

	public ActorController SelectActor
	{
		get
		{
			return this.selectActor;
		}
		set
		{
			this.selectActor = value;
		}
	}

	public void Pause()
	{
		this.camPause = true;
	}

	public void Resume()
	{
		this.camPause = false;
	}

	public Vector3 GetCamCurBasePos()
	{
		return this.camCurBasePos;
	}

	public void SetCamLookYOfset(float ofset)
	{
		this.resultCamLookYOfset = ofset;
	}

	public void InitForGameScene(Vector3 initPos, bool autoPlay)
	{
		this.Clear();
		this.camInitPos = initPos;
		this.mainCamera = Camera.main;
		Tools.GetSafeComponent<TransparentDiffuse>(this.mainCamera.gameObject);
		this.camInitFov = 30f;
		SceneInfo sceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		this.scenetype = (ESceneType)sceneInfo.Type;
		switch (this.scenetype)
		{
		case ESceneType.EScene_World:
		case ESceneType.EScene_Pillage:
			if (autoPlay)
			{
				this.SetAutoCamValue();
			}
			else
			{
				this.SetSingleCamValue();
			}
			break;
		case ESceneType.EScene_Trial:
		case ESceneType.EScene_MemoryGear:
			this.SetTrailTowerCamValue();
			break;
		case ESceneType.EScene_Arena:
			this.SetPvpCamValue();
			break;
		case ESceneType.EScene_WorldBoss:
			this.SetWorldBossCamValue();
			break;
		case ESceneType.EScene_GuildBoss:
			this.SetWorldBossCamValue();
			break;
		case ESceneType.EScene_KingReward:
			if (sceneInfo.SubType == 1)
			{
				this.SetWorldBossCamValue();
			}
			else if (autoPlay)
			{
				this.SetAutoCamValue();
			}
			else
			{
				this.SetSingleCamValue();
			}
			break;
		case ESceneType.EScene_OrePillage:
			this.SetOrePillageCamValue();
			break;
		case ESceneType.EScene_GuildPvp:
			this.SetGuildPvpCamValue();
			break;
		}
		this.camCurTargetDir = this.camTargetDir;
		this.camCurTargetDist = this.camTargetDist;
		this.camCurBasePos = this.camInitPos;
		this.RefreshMainCamera(this.camInitPos, true);
	}

	public void Clear()
	{
		this.camPause = false;
		this.camLock = false;
		this.screenShockTest = false;
		this.screenShockPlay = false;
		this.fovShockTest = false;
		this.fovShockPlay = false;
		this.posShockTest = false;
		this.posShockPlay = false;
		this.resultCamTest = false;
		this.resultCamLookYOfset = 0.5f;
		this.resultCamTotalTime = 2f;
		this.resultCamPlay = false;
		this.targetCamTest = false;
		this.Target = null;
		this.targetCamTotalTime = 1f;
		this.targetCamPlay = false;
		this.worldBossHeight = 4f;
		this.worldBossRidous = 5.5f;
		this.worldBossAttackPlay = false;
		this.dynamicCam = false;
	}

	public void SetAutoCamValue()
	{
		this.SetCamDist(10f, true);
		this.SetCamAngle(36f, true);
	}

	public void SetSingleCamValue()
	{
		this.SetCamDist(12.5f, true);
		this.SetCamAngle(41f, true);
	}

	public void SetTrailTowerCamValue()
	{
		this.SetCamDist(14.5f, true);
		this.SetCamAngle(35f, true);
	}

	public void SetWorldBossCamValue()
	{
		this.scenetype = ESceneType.EScene_WorldBoss;
		this.targetCamTotalTime = 2f;
		this.SetCamDist(12f + (this.worldBossHeight - 3.6f) * 4f, true);
		this.SetCamAngle(30f + (4f - this.worldBossHeight) * 2f, true);
	}

	public void SetGuildPvpCamValue()
	{
		this.dynamicCam = false;
		this.camInitPos = new Vector3(1.3f, -1.2f, 1f);
		this.targetCamInitTime = -1f;
		this.SetCamDist(18f, true);
		this.SetCamAngle(35f, true);
	}

	public void SetPvpCamValue()
	{
		this.dynamicCam = false;
		this.camInitPos = new Vector3(0f, 0.9f, 0f);
		this.targetCamInitTime = -1f;
		this.SetCamDist(18f, true);
		this.SetCamAngle(35f, true);
	}

	private void SetOrePillageCamValue()
	{
		this.dynamicCam = false;
		this.camInitPos = new Vector3(8.1f, -2.8f, 2.7f);
		this.targetCamInitTime = -1f;
		this.SetCamDist(18f, true);
		this.SetCamAngle(35f, true);
	}

	private void SetResultCamValue()
	{
		this.SetCamDist(6f, true);
		this.SetCamAngle(25f, true);
		this.SetCamLookYOfset(0.5f);
	}

	private void SetResultBossCamValue()
	{
		this.SetCamDist(8f + this.worldBossRidous, true);
		this.SetCamAngleXY(35f, 40f, true);
	}

	public void SetCamDist(float dist, bool force)
	{
		if (force || !this.camLock)
		{
			this.camLock = false;
			this.camTargetDist = dist;
		}
	}

	public void SetCamAngle(float angle, bool force)
	{
		if (force || !this.camLock)
		{
			this.camLock = false;
			this.camTargetDir.x = 0f;
			this.camTargetDir.z = 1f;
			this.camTargetDir.y = Mathf.Tan(0.0174532924f * angle);
			this.camTargetDir.Normalize();
		}
	}

	public void SetCamAngleX(float angle, bool force)
	{
		if (force || !this.camLock)
		{
			this.camLock = false;
			this.camTargetDir.x = Mathf.Tan(0.0174532924f * angle);
			this.camTargetDir.z = 1f;
			this.camTargetDir.y = 1f;
			this.camTargetDir.Normalize();
		}
	}

	public void SetCamAngleXY(float angleX, float angleY, bool force)
	{
		if (force || !this.camLock)
		{
			this.camLock = false;
			this.camTargetDir.x = Mathf.Tan(0.0174532924f * angleX);
			this.camTargetDir.z = 1f;
			this.camTargetDir.y = Mathf.Tan(0.0174532924f * angleY);
			this.camTargetDir.Normalize();
		}
	}

	public void SaveCurCamValue()
	{
		this.saveCamTargetDist = this.camTargetDist;
		this.saveCamTargetDir = this.camTargetDir;
	}

	public void ReleaseSaveCamValue()
	{
		this.camTargetDir = this.saveCamTargetDir;
		this.SetCamDist(this.saveCamTargetDist, true);
	}

	public void NormalUpdate(GameObject obj, ActorController actorCtrler, bool force = false)
	{
		Vector3 position = obj.transform.position;
		bool flag = false;
		if (!flag)
		{
			flag = this.WorldBossCamEffect(ref position);
		}
		if (!flag)
		{
			flag = this.PvpCamEffect(ref position);
		}
		if (!flag)
		{
			flag = this.SingleCamEffect(ref position);
		}
		if (!flag)
		{
			flag = this.CombatCamEffect(ref position, obj, actorCtrler);
		}
		this.RefreshMainCamera(position, force);
	}

	private void RefreshMainCamera(Vector3 basePos, bool force)
	{
		if (!this.camPause || force)
		{
			this.ResultCamEffect();
			this.DampCamParameter(basePos);
			this.camLookPos.x = this.camCurBasePos.x;
			this.camLookPos.y = this.camCurBasePos.y + this.resultCamLookYOfset;
			this.camLookPos.z = this.camCurBasePos.z;
			this.ScreenShockEffect();
			this.mainCamera.transform.position = this.camLookPos + this.camCurTargetDir * this.camCurTargetDist;
			this.mainCamera.transform.LookAt(this.camLookPos, Vector3.up);
			this.FovShockEffect();
			this.PosShockEffect();
		}
	}

	private void DampCamParameter(Vector3 basePos)
	{
		if (this.camCurTargetDir != this.camTargetDir)
		{
			this.camCurTargetDir = UtilFunc.SpringDamp(this.camCurTargetDir, this.camTargetDir, Time.smoothDeltaTime, 0.1f, 0.01f);
		}
		if (!Mathf.Approximately(this.camCurTargetDist, this.camTargetDist))
		{
			this.camCurTargetDist = UtilFunc.SpringDamp(this.camCurTargetDist, this.camTargetDist, Time.smoothDeltaTime, this.distConst, 0.01f);
		}
		if (this.camCurBasePos != basePos)
		{
			this.camCurBasePos = UtilFunc.SpringDamp(this.camCurBasePos, basePos, Time.smoothDeltaTime, this.posConst, this.posConstLen);
		}
	}

	private bool SingleCamEffect(ref Vector3 basePos)
	{
		if (this.targetCamTest && !this.targetCamPlay && this.Target != null)
		{
			this.targetCamTest = false;
			this.targetCamPlay = true;
			this.targetCamInitTime = Time.time;
			this.SaveCurCamValue();
			this.SetCamDist(17f, true);
		}
		if (this.targetCamPlay)
		{
			if (null == this.Target || Time.time - this.targetCamInitTime >= this.targetCamTotalTime)
			{
				this.ReleaseSaveCamValue();
				this.targetCamPlay = false;
			}
			else
			{
				basePos += (this.Target.gameObject.transform.position - basePos) * 0.7f;
			}
			return true;
		}
		return false;
	}

	private bool WorldBossCamEffect(ref Vector3 basePos)
	{
		if (this.scenetype != ESceneType.EScene_WorldBoss)
		{
			return false;
		}
		if (null == this.Target)
		{
			return false;
		}
		if (this.resultCamTest && !this.resultCamPlay)
		{
			this.resultCamTest = false;
			this.resultCamPlay = true;
			this.resultCamInitTime = Time.time;
			this.SaveCurCamValue();
			this.worldBossHeight = 2f;
			this.SetResultBossCamValue();
		}
		if (this.resultCamPlay)
		{
			if (Time.time - this.resultCamInitTime >= this.resultCamTotalTime)
			{
				this.ReleaseSaveCamValue();
				this.worldBossHeight = 4f;
				this.resultCamPlay = false;
			}
			basePos = this.Target.gameObject.transform.position;
			return true;
		}
		if (this.worldBossAttackPlay && !this.worldBossAttackTest)
		{
			this.worldBossAttackTest = true;
			this.SaveCurCamValue();
			this.worldBossHeight = 5.5f;
			this.SetWorldBossCamValue();
		}
		if (!this.worldBossAttackPlay && this.worldBossAttackTest)
		{
			this.worldBossHeight = 4f;
			this.ReleaseSaveCamValue();
			this.worldBossAttackTest = false;
		}
		if (this.worldBossAttackPlay)
		{
			Vector3 position = this.Target.gameObject.transform.position;
			basePos = position + (basePos - position) * 0.5f;
			return true;
		}
		return false;
	}

	private ActorController UpdateCurActor()
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		ActorController actorCtrler;
		if ((actorCtrler = this.SelectActor) != null && !actorCtrler.IsDead && actorCtrler.gameObject.activeInHierarchy)
		{
			return actorCtrler;
		}
		if ((this.scenetype == ESceneType.EScene_Arena || this.scenetype == ESceneType.EScene_OrePillage || this.scenetype == ESceneType.EScene_GuildPvp) && Globals.Instance.ActorMgr.PlayerCtrler != null && (actorCtrler = Globals.Instance.ActorMgr.PlayerCtrler.ActorCtrler) != null && !actorCtrler.IsDead)
		{
			return actorCtrler;
		}
		if (this.targetCamInitTime == -1f)
		{
			this.targetCamInitTime = Time.time;
			int num = this.camFocusIdx;
			int max = 5;
			this.camFocusIdx = UtilFunc.RangeRandom(0, max);
			while (this.camFocusIdx == num || actors[this.camFocusIdx] == null)
			{
				this.camFocusIdx = UtilFunc.RangeRandom(0, max);
			}
		}
		return actors[this.camFocusIdx];
	}

	private bool PvpCamEffect(ref Vector3 basePos)
	{
		if (this.scenetype != ESceneType.EScene_Arena && this.scenetype != ESceneType.EScene_OrePillage && this.scenetype != ESceneType.EScene_GuildPvp)
		{
			return false;
		}
		if (Globals.Instance.ActorMgr.CurScene == null)
		{
			return false;
		}
		if (Globals.Instance.ActorMgr.CurScene.CurStatus == 1)
		{
			basePos = this.camInitPos;
			return true;
		}
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		if (Globals.Instance.ActorMgr.CurScene.CurStatus == 2)
		{
			if (this.dynamicCam)
			{
				basePos = this.camCurBasePos;
			}
			else
			{
				ActorController actorController = this.UpdateCurActor();
				if (actorController != null && !actorController.IsDead && actorController.gameObject.activeInHierarchy)
				{
					basePos = actorController.transform.position;
				}
			}
		}
		else
		{
			this.SetCamDist(20f, false);
			Vector3 a = Vector3.zero;
			int count = actors.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				ActorController actorController2 = actors[i];
				if (actorController2 != null && !actorController2.IsDead)
				{
					a += actorController2.transform.position;
					num++;
				}
			}
			if (num > 0)
			{
				basePos = a / (float)num;
			}
		}
		return true;
	}

	private bool CombatCamEffect(ref Vector3 basePos, GameObject obj, ActorController actorCtrler)
	{
		if (this.resultCamPlay)
		{
			return false;
		}
		if (actorCtrler != null)
		{
			if (actorCtrler.AiCtrler.Target != null && actorCtrler.GetDistance2D(actorCtrler.AiCtrler.Target.gameObject.transform.position) < actorCtrler.AiCtrler.AttackDistance)
			{
				if (actorCtrler.NavAgent.velocity.sqrMagnitude <= 0f)
				{
					basePos += (actorCtrler.AiCtrler.Target.gameObject.transform.position - basePos) * 0.5f;
				}
				else
				{
					basePos += obj.transform.forward * this.camTargetOffset;
				}
			}
			else if (actorCtrler.AiCtrler.EnableAI)
			{
				Vector3 vector = obj.transform.forward * this.camTargetOffset2;
				if (this.camCurForward != vector)
				{
					this.camCurForward = UtilFunc.SpringDamp(this.camCurForward, vector, Time.smoothDeltaTime, 0.05f, 0.01f);
				}
				basePos += this.camCurForward;
			}
			else
			{
				basePos += obj.transform.forward * this.camTargetOffset;
			}
		}
		else
		{
			basePos += obj.transform.forward * this.camTargetOffset;
		}
		return true;
	}

	private bool ResultCamEffect()
	{
		if (this.resultCamTest && !this.resultCamPlay)
		{
			this.resultCamTest = false;
			this.resultCamPlay = true;
			this.SaveCurCamValue();
			this.SetResultCamValue();
			return true;
		}
		return false;
	}

	private bool ScreenShockEffect()
	{
		if (this.screenShockTest)
		{
			this.screenShockTest = false;
			this.screenShockPlay = true;
			this.screenShockInitTime = Time.time;
			this.screenShockInitCameraEulerAngles = this.mainCamera.transform.eulerAngles;
		}
		if (this.screenShockPlay)
		{
			Vector3 eulerAngles = this.mainCamera.transform.eulerAngles;
			eulerAngles.z = Mathf.Sin((this.screenShockInitTime - Time.time) * this.screenShockFrequency) * this.screenShockAmplitude;
			this.mainCamera.transform.eulerAngles = eulerAngles;
			if (Time.time - this.screenShockInitTime >= this.screenShockTotalTime)
			{
				this.mainCamera.transform.eulerAngles = this.screenShockInitCameraEulerAngles;
				this.screenShockPlay = false;
			}
			return true;
		}
		return false;
	}

	private bool FovShockEffect()
	{
		if (this.fovShockTest)
		{
			this.fovShockTest = false;
			this.fovShockPlay = true;
			this.fovShockInitTime = Time.time;
		}
		if (this.fovShockPlay)
		{
			this.mainCamera.fieldOfView = this.camInitFov + Mathf.Sin((this.fovShockInitTime - Time.time) * this.fovShockFrequency) * this.fovShockAmplitude;
			if (Time.time - this.fovShockInitTime >= this.fovShockTotalTime)
			{
				this.mainCamera.fieldOfView = this.camInitFov;
				this.fovShockPlay = false;
			}
			return true;
		}
		this.mainCamera.fieldOfView = this.camInitFov;
		return false;
	}

	private bool PosShockEffect()
	{
		if (this.posShockTest)
		{
			this.posShockTest = false;
			this.posShockPlay = true;
			this.posShockInitTime = Time.time;
		}
		if (this.posShockPlay)
		{
			this.camLookPos.z = this.camLookPos.z + Mathf.Sin((this.posShockInitTime - Time.time) * this.posShockFrequency) * this.posShockAmplitude;
			this.mainCamera.transform.position = this.camLookPos + this.camCurTargetDir * this.camCurTargetDist;
			this.mainCamera.transform.LookAt(this.camLookPos, this.mainCamera.transform.up);
			if (Time.time - this.posShockInitTime >= this.posShockTotalTime)
			{
				this.posShockPlay = false;
			}
			return true;
		}
		return false;
	}

	private void LateUpdate()
	{
		if ((this.scenetype == ESceneType.EScene_Arena || this.scenetype == ESceneType.EScene_OrePillage || this.scenetype == ESceneType.EScene_GuildPvp) && Globals.Instance.ActorMgr.CurScene != null && Globals.Instance.ActorMgr.CurScene.CurStatus == 2)
		{
			this.InputProcess();
		}
	}

	private void InputProcess()
	{
		if (!this.ProcessTouch())
		{
			this.ProcessGamePad();
		}
	}

	private void ProcessSimulater()
	{
		if (Input.GetMouseButton(0))
		{
			this.TouchMove(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.TouchUp();
		}
	}

	private bool ProcessTouch()
	{
		if (Input.touchCount <= 0)
		{
			return false;
		}
		Touch touch = Input.GetTouch(0);
		switch (touch.phase)
		{
		case TouchPhase.Moved:
		case TouchPhase.Stationary:
			this.TouchMove(touch.position);
			break;
		case TouchPhase.Ended:
			this.TouchUp();
			break;
		}
		return true;
	}

	private void ProcessGamePad()
	{
		if (!GamePadMgr.isConnnected)
		{
			return;
		}
		if (GamePadMgr.GetKeyButton())
		{
			Vector3 vector = new Vector3(GamePadMgr.moveX, GamePadMgr.moveY, 0f);
			Vector3 a = (vector.magnitude <= 1f) ? vector : vector.normalized;
			vector = this.tempPos - a * 60f;
			this.TouchMove(vector);
		}
		if (GamePadMgr.GetKeyButtonUp())
		{
			this.TouchUp();
		}
	}

	private void TouchMove(Vector3 inputPosition)
	{
		if (!Globals.Instance.CameraMgr.dynamicCam)
		{
			Globals.Instance.CameraMgr.dynamicCam = true;
		}
		if (this.tempPos == Vector3.zero)
		{
			this.tempPos = inputPosition;
			return;
		}
		this.camCurBasePos.x = this.camCurBasePos.x + (inputPosition.x - this.tempPos.x) / this.PVPCameraMoveSpeed;
		this.camCurBasePos.z = this.camCurBasePos.z + (inputPosition.y - this.tempPos.y) * 1.43f / this.PVPCameraMoveSpeed;
		if (this.camCurBasePos.x > 10f)
		{
			this.camCurBasePos.x = 10f;
		}
		if (this.camCurBasePos.x < -10f)
		{
			this.camCurBasePos.x = -10f;
		}
		if (this.camCurBasePos.z > 10f)
		{
			this.camCurBasePos.z = 10f;
		}
		if (this.camCurBasePos.z < -10f)
		{
			this.camCurBasePos.z = -10f;
		}
		this.tempPos = inputPosition;
	}

	private void TouchUp()
	{
		this.tempPos = Vector3.zero;
	}
}
