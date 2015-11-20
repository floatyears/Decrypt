using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("Game/Camera/Camera Shake")]
public class CameraShake : MonoBehaviour
{
	internal class ShakeState
	{
		internal readonly Vector3 startPosition;

		internal readonly Quaternion startRotation;

		internal readonly Vector2 guiStartPosition;

		internal Vector3 shakePosition;

		internal Quaternion shakeRotation;

		internal Vector2 guiShakePosition;

		internal ShakeState(Vector3 position, Quaternion rotation, Vector2 guiPosition)
		{
			this.startPosition = position;
			this.startRotation = rotation;
			this.guiStartPosition = guiPosition;
			this.shakePosition = position;
			this.shakeRotation = rotation;
			this.guiShakePosition = guiPosition;
		}
	}

	private const bool checkForMinimumValues = true;

	private const float minShakeValue = 0.001f;

	private const float minRotationValue = 0.001f;

	public List<Camera> cameras = new List<Camera>();

	public int numberOfShakes = 2;

	public Vector3 shakeAmount = Vector3.one;

	public Vector3 rotationAmount = Vector3.one;

	public float distance = 0.1f;

	public float speed = 50f;

	public float decay = 0.2f;

	public float guiShakeModifier = 1f;

	public bool multiplyByTimeScale = true;

	private Rect shakeRect;

	private bool shaking;

	private bool cancelling;

	private Dictionary<Camera, List<CameraShake.ShakeState>> states = new Dictionary<Camera, List<CameraShake.ShakeState>>();

	private Dictionary<Camera, int> shakeCount = new Dictionary<Camera, int>();

	public static CameraShake _instance;

	public Action cameraShakeStarted;

	public Action allCameraShakesCompleted;

	private List<Vector3> offsetCache = new List<Vector3>(10);

	private List<Quaternion> rotationCache = new List<Quaternion>(10);

	public static CameraShake Instance
	{
		get
		{
			if (CameraShake._instance == null && Camera.main != null)
			{
				CameraShake._instance = Tools.GetSafeComponent<CameraShake>(Camera.main.gameObject);
			}
			return CameraShake._instance;
		}
		set
		{
			CameraShake._instance = value;
		}
	}

	public static bool isShaking
	{
		get
		{
			return CameraShake.Instance.IsShaking();
		}
	}

	public static bool isCancelling
	{
		get
		{
			return CameraShake.Instance.IsCancelling();
		}
	}

	private void OnEnable()
	{
		if (this.cameras.Count < 1 && base.camera)
		{
			this.cameras.Add(base.camera);
		}
		if (this.cameras.Count < 1 && Camera.main)
		{
			this.cameras.Add(Camera.main);
		}
		if (this.cameras.Count < 1)
		{
			global::Debug.LogError(new object[]
			{
				"Camera Shake: No cameras assigned in the inspector!"
			});
		}
		CameraShake.Instance = this;
	}

	private void OnDestroy()
	{
		CameraShake.Instance = null;
	}

	public static void Shake()
	{
		CameraShake.Instance.DoShake();
	}

	public static void Shake(int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float guiShakeModifier, bool multiplyByTimeScale)
	{
		CameraShake.Instance.DoShake(numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, guiShakeModifier, multiplyByTimeScale);
	}

	public static void Shake(Action callback)
	{
		CameraShake.Instance.DoShake(callback);
	}

	public static void Shake(int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float guiShakeModifier, bool multiplyByTimeScale, Action callback)
	{
		CameraShake.Instance.DoShake(numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, guiShakeModifier, multiplyByTimeScale, callback);
	}

	public static void CancelShake()
	{
		CameraShake.Instance.DoCancelShake();
	}

	public static void CancelShake(float time)
	{
		CameraShake.Instance.DoCancelShake(time);
	}

	public static void BeginShakeGUI()
	{
		CameraShake.Instance.DoBeginShakeGUI();
	}

	public static void EndShakeGUI()
	{
		CameraShake.Instance.DoEndShakeGUI();
	}

	public static void BeginShakeGUILayout()
	{
		CameraShake.Instance.DoBeginShakeGUILayout();
	}

	public static void EndShakeGUILayout()
	{
		CameraShake.Instance.DoEndShakeGUILayout();
	}

	public bool IsShaking()
	{
		return this.shaking;
	}

	public bool IsCancelling()
	{
		return this.cancelling;
	}

	public void DoShake()
	{
		Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
		foreach (Camera current in this.cameras)
		{
			base.StartCoroutine(this.DoShake_Internal(current, insideUnitSphere, this.numberOfShakes, this.shakeAmount, this.rotationAmount, this.distance, this.speed, this.decay, this.guiShakeModifier, this.multiplyByTimeScale, null));
		}
	}

	public void DoShake(int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float guiShakeModifier, bool multiplyByTimeScale)
	{
		Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
		foreach (Camera current in this.cameras)
		{
			base.StartCoroutine(this.DoShake_Internal(current, insideUnitSphere, numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, guiShakeModifier, multiplyByTimeScale, null));
		}
	}

	public void DoShake(Action callback)
	{
		Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
		foreach (Camera current in this.cameras)
		{
			base.StartCoroutine(this.DoShake_Internal(current, insideUnitSphere, this.numberOfShakes, this.shakeAmount, this.rotationAmount, this.distance, this.speed, this.decay, this.guiShakeModifier, this.multiplyByTimeScale, callback));
		}
	}

	public void DoShake(int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float guiShakeModifier, bool multiplyByTimeScale, Action callback)
	{
		Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
		foreach (Camera current in this.cameras)
		{
			base.StartCoroutine(this.DoShake_Internal(current, insideUnitSphere, numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, guiShakeModifier, multiplyByTimeScale, callback));
		}
	}

	public void DoCancelShake()
	{
		if (this.shaking && !this.cancelling)
		{
			this.shaking = false;
			base.StopAllCoroutines();
			foreach (Camera current in this.cameras)
			{
				if (this.shakeCount.ContainsKey(current))
				{
					this.shakeCount[current] = 0;
				}
				this.ResetState(current.transform, current);
			}
		}
	}

	public void DoCancelShake(float time)
	{
		if (this.shaking && !this.cancelling)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.DoResetState(this.cameras, this.shakeCount, time));
		}
	}

	public void DoBeginShakeGUI()
	{
		this.CheckShakeRect();
		GUI.BeginGroup(this.shakeRect);
	}

	public void DoEndShakeGUI()
	{
		GUI.EndGroup();
	}

	public void DoBeginShakeGUILayout()
	{
		this.CheckShakeRect();
		GUILayout.BeginArea(this.shakeRect);
	}

	public void DoEndShakeGUILayout()
	{
		GUILayout.EndArea();
	}

	private void OnDrawGizmosSelected()
	{
		foreach (Camera current in this.cameras)
		{
			if (current)
			{
				if (this.IsShaking())
				{
					Vector3 vector = current.worldToCameraMatrix.GetColumn(3);
					vector.z *= -1f;
					vector = current.transform.position + current.transform.TransformPoint(vector);
					Quaternion q = CameraShake.QuaternionFromMatrix(current.worldToCameraMatrix.inverse * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, -1f)));
					Matrix4x4 matrix = Matrix4x4.TRS(vector, q, current.transform.lossyScale);
					Gizmos.matrix = matrix;
				}
				else
				{
					Matrix4x4 matrix2 = Matrix4x4.TRS(current.transform.position, current.transform.rotation, current.transform.lossyScale);
					Gizmos.matrix = matrix2;
				}
				Gizmos.DrawWireCube(Vector3.zero, this.shakeAmount);
				Gizmos.color = Color.cyan;
				if (current.isOrthoGraphic)
				{
					Vector3 center = new Vector3(0f, 0f, (current.nearClipPlane + current.farClipPlane) * 0.5f);
					Vector3 size = new Vector3(current.orthographicSize / current.aspect, current.orthographicSize * 2f, current.farClipPlane - current.nearClipPlane);
					Gizmos.DrawWireCube(center, size);
				}
				else
				{
					Gizmos.DrawFrustum(Vector3.zero, current.fieldOfView, current.farClipPlane, current.nearClipPlane, 0.7f / current.aspect);
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator DoShake_Internal(Camera cam, Vector3 seed, int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float guiShakeModifier, bool multiplyByTimeScale, Action callback)
	{
        return null;
        //CameraShake.<DoShake_Internal>c__IteratorC <DoShake_Internal>c__IteratorC = new CameraShake.<DoShake_Internal>c__IteratorC();
        //<DoShake_Internal>c__IteratorC.seed = seed;
        //<DoShake_Internal>c__IteratorC.cam = cam;
        //<DoShake_Internal>c__IteratorC.numberOfShakes = numberOfShakes;
        //<DoShake_Internal>c__IteratorC.distance = distance;
        //<DoShake_Internal>c__IteratorC.multiplyByTimeScale = multiplyByTimeScale;
        //<DoShake_Internal>c__IteratorC.guiShakeModifier = guiShakeModifier;
        //<DoShake_Internal>c__IteratorC.rotationAmount = rotationAmount;
        //<DoShake_Internal>c__IteratorC.shakeAmount = shakeAmount;
        //<DoShake_Internal>c__IteratorC.speed = speed;
        //<DoShake_Internal>c__IteratorC.decay = decay;
        //<DoShake_Internal>c__IteratorC.callback = callback;
        //<DoShake_Internal>c__IteratorC.<$>seed = seed;
        //<DoShake_Internal>c__IteratorC.<$>cam = cam;
        //<DoShake_Internal>c__IteratorC.<$>numberOfShakes = numberOfShakes;
        //<DoShake_Internal>c__IteratorC.<$>distance = distance;
        //<DoShake_Internal>c__IteratorC.<$>multiplyByTimeScale = multiplyByTimeScale;
        //<DoShake_Internal>c__IteratorC.<$>guiShakeModifier = guiShakeModifier;
        //<DoShake_Internal>c__IteratorC.<$>rotationAmount = rotationAmount;
        //<DoShake_Internal>c__IteratorC.<$>shakeAmount = shakeAmount;
        //<DoShake_Internal>c__IteratorC.<$>speed = speed;
        //<DoShake_Internal>c__IteratorC.<$>decay = decay;
        //<DoShake_Internal>c__IteratorC.<$>callback = callback;
        //<DoShake_Internal>c__IteratorC.<>f__this = this;
        //return <DoShake_Internal>c__IteratorC;
	}

	private Vector3 GetGeometricAvg(List<CameraShake.ShakeState> states, bool position)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = (float)states.Count;
		foreach (CameraShake.ShakeState current in states)
		{
			if (position)
			{
				num -= current.shakePosition.x;
				num2 -= current.shakePosition.y;
				num3 -= current.shakePosition.z;
			}
			else
			{
				num += current.guiShakePosition.x;
				num2 += current.guiShakePosition.y;
			}
		}
		return new Vector3(num / num4, num2 / num4, num3 / num4);
	}

	private Quaternion GetAvgRotation(List<CameraShake.ShakeState> states)
	{
		Quaternion shakeRotation = new Quaternion(0f, 0f, 0f, 0f);
		foreach (CameraShake.ShakeState current in states)
		{
			if (Quaternion.Dot(current.shakeRotation, shakeRotation) > 0f)
			{
				shakeRotation.x += current.shakeRotation.x;
				shakeRotation.y += current.shakeRotation.y;
				shakeRotation.z += current.shakeRotation.z;
				shakeRotation.w += current.shakeRotation.w;
			}
			else
			{
				shakeRotation.x += -current.shakeRotation.x;
				shakeRotation.y += -current.shakeRotation.y;
				shakeRotation.z += -current.shakeRotation.z;
				shakeRotation.w += -current.shakeRotation.w;
			}
		}
		float num = Mathf.Sqrt(shakeRotation.x * shakeRotation.x + shakeRotation.y * shakeRotation.y + shakeRotation.z * shakeRotation.z + shakeRotation.w * shakeRotation.w);
		if (num > 0.0001f)
		{
			shakeRotation.x /= num;
			shakeRotation.y /= num;
			shakeRotation.z /= num;
			shakeRotation.w /= num;
		}
		else
		{
			shakeRotation = states[0].shakeRotation;
		}
		return shakeRotation;
	}

	private void CheckShakeRect()
	{
		if ((float)Screen.width != this.shakeRect.width || (float)Screen.height != this.shakeRect.height)
		{
			this.shakeRect.width = (float)Screen.width;
			this.shakeRect.height = (float)Screen.height;
		}
	}

	private float GetPixelWidth(Transform cachedTransform, Camera cachedCamera)
	{
		Vector3 position = cachedTransform.position;
		Vector3 a = cachedCamera.WorldToScreenPoint(position - cachedTransform.forward * 0.01f);
		Vector3 position2 = Vector3.zero;
		if (a.x > 0f)
		{
			position2 = a - Vector3.right;
		}
		else
		{
			position2 = a + Vector3.right;
		}
		if (a.y > 0f)
		{
			position2 = a - Vector3.up;
		}
		else
		{
			position2 = a + Vector3.up;
		}
		position2 = cachedCamera.ScreenToWorldPoint(position2);
		return 1f / (cachedTransform.InverseTransformPoint(position) - cachedTransform.InverseTransformPoint(position2)).magnitude;
	}

	private void ResetState(Transform cachedTransform, Camera cam)
	{
		cam.ResetWorldToCameraMatrix();
		this.shakeRect.x = 0f;
		this.shakeRect.y = 0f;
		this.states[cam].Clear();
	}

	[DebuggerHidden]
	private IEnumerator DoResetState(List<Camera> cameras, Dictionary<Camera, int> shakeCount, float time)
	{
        return null;
        //CameraShake.<DoResetState>c__IteratorD <DoResetState>c__IteratorD = new CameraShake.<DoResetState>c__IteratorD();
        //<DoResetState>c__IteratorD.cameras = cameras;
        //<DoResetState>c__IteratorD.shakeCount = shakeCount;
        //<DoResetState>c__IteratorD.time = time;
        //<DoResetState>c__IteratorD.<$>cameras = cameras;
        //<DoResetState>c__IteratorD.<$>shakeCount = shakeCount;
        //<DoResetState>c__IteratorD.<$>time = time;
        //<DoResetState>c__IteratorD.<>f__this = this;
        //return <DoResetState>c__IteratorD;
	}

	private static Quaternion QuaternionFromMatrix(Matrix4x4 m)
	{
		return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
	}

	private static void NormalizeQuaternion(ref Quaternion q)
	{
		float num = 0f;
		for (int i = 0; i < 4; i++)
		{
			num += q[i] * q[i];
		}
		float num2 = 1f / Mathf.Sqrt(num);
		for (int j = 0; j < 4; j++)
		{
			int index;
			int expr_43 = index = j;
			float num3 = q[index];
			q[expr_43] = num3 * num2;
		}
	}
}
