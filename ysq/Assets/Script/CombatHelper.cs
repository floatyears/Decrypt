using System;
using UnityEngine;

public class CombatHelper
{
	public static GameObject PlayParticleToWorld(string perfabName, float duration, Vector3 pos, Quaternion rot)
	{
		GameObject gameObject = Res.Load<GameObject>(perfabName, false);
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Can not load the particle: name = " + perfabName
			});
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, pos, rot) as GameObject;
		UnityEngine.Object.DestroyObject(gameObject2, duration);
		return gameObject2;
	}

	public static float Distance2D(Vector3 a, Vector3 b)
	{
		return Mathf.Sqrt(CombatHelper.DistanceSquared2D(a, b));
	}

	public static float DistanceSquared2D(Vector3 a, Vector3 b)
	{
		return (a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z);
	}

	public static Vector3 GetSlotPos(Vector3 basePosition, Transform transform, int slot, bool forceFollow = false, bool resurrect = false)
	{
		Vector3 result = Vector3.zero;
        switch (slot)
        {
            case 7:
                if (forceFollow)
                {
                    result = basePosition + transform.right * 0.8f + transform.forward * 0.3f;
                }
                else if (!resurrect)
                {
                    result = basePosition + transform.right * 1f + transform.forward * 0.5f;
                }
                else
                {
                    result = basePosition + transform.right * 0.5f + transform.forward * 0.3f;
                }
                break;
            case 8:
                if (forceFollow)
                {
                    result = basePosition - transform.right * 0.8f + transform.forward * 0.3f;
                }
                else if (!resurrect)
                {
                    result = basePosition - transform.right * 1f + transform.forward * 0.5f;
                }
                else
                {
                    result = basePosition - transform.right * 0.5f + transform.forward * 0.3f;
                }
                break;
            case 9:
                result = basePosition + transform.right * 0.6f;
                break;
            case 131:
                if (forceFollow)
                {
                    result = basePosition - transform.right * 0.8f + transform.forward * 0.8f;
                }
                else if (!resurrect)
                {
                    result = basePosition - transform.right * 1.2f + transform.forward * 1.5f;
                }
                else
                {
                    result = basePosition - transform.right * 0.5f + transform.forward * 0.5f;
                }
                return result;
            case 132:
            case 111:
                if (forceFollow)
                {
                    result = basePosition + transform.forward * 1.2f;
                }
                else if (!resurrect)
                {
                    result = basePosition + transform.forward * 2f;
                }
                else
                {
                    result = basePosition + transform.forward * 0.5f;
                }
                break;
            case 133:
                if (forceFollow)
                {
                    result = basePosition + transform.right * 0.8f + transform.forward * 0.8f;
                }
                else if (!resurrect)
                {
                    result = basePosition + transform.right * 1.2f + transform.forward * 1.5f;
                }
                else
                {
                    result = basePosition + transform.right * 0.5f + transform.forward * 0.5f;
                }
                return result;
            case 231:
                if (forceFollow)
                {
                    result = basePosition - transform.right * 0.8f - transform.forward * 0.5f;
                }
                else if (!resurrect)
                {
                    result = basePosition - transform.right * 1f - transform.forward * 1.2f;
                }
                else
                {
                    result = basePosition - transform.right * 0.5f - transform.forward * 0.5f;
                }
                return result;
            case 232:
            case 211:
                if (forceFollow)
                {
                    result = basePosition - transform.forward * 1f;
                }
                else if (!resurrect)
                {
                    result = basePosition - transform.forward * 1.2f;
                }
                else
                {
                    result = basePosition - transform.forward * 0.5f;
                }
                return result;
            case 233:
                if (forceFollow)
                {
                    result = basePosition + transform.right * 0.8f - transform.forward * 0.5f;
                }
                else if (!resurrect)
                {
                    result = basePosition + transform.right * 1f - transform.forward * 1.2f;
                }
                else
                {
                    result = basePosition + transform.right * 0.5f - transform.forward * 0.5f;
                }
                return result;
            case 121:
                if (forceFollow)
                {
                    result = basePosition - transform.right * 0.5f + transform.forward * 0.8f;
                }
                else if (!resurrect)
                {
                    result = basePosition - transform.right * 0.8f + transform.forward * 1.5f;
                }
                else
                {
                    result = basePosition - transform.right * 0.5f + transform.forward * 0.5f;
                }
                return result;
            case 122:
                if (forceFollow)
                {
                    result = basePosition + transform.right * 0.5f + transform.forward * 0.8f;
                }
                else if (!resurrect)
                {
                    result = basePosition + transform.right * 0.8f + transform.forward * 1.5f;
                }
                else
                {
                    result = basePosition + transform.right * 0.5f + transform.forward * 0.5f;
                }
                return result;
            case 221:
                if (forceFollow)
                {
                    result = basePosition - transform.right * 0.5f - transform.forward * 0.5f;
                }
                else if (!resurrect)
                {
                    result = basePosition - transform.right * 0.7f - transform.forward * 1.2f;
                }
                else
                {
                    result = basePosition - transform.right * 0.5f - transform.forward * 0.5f;
                }
                return result;
            case 222:
                if (forceFollow)
                {
                    result = basePosition + transform.right * 0.5f - transform.forward * 0.5f;
                }
                else if (!resurrect)
                {
                    result = basePosition + transform.right * 0.7f - transform.forward * 1.2f;
                }
                else
                {
                    result = basePosition + transform.right * 0.5f - transform.forward * 0.5f;
                }
                return result;
        }
		return result;
	}
}
