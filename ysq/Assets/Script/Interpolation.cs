using System;
using UnityEngine;

public class Interpolation
{
	public static float Linear(float v0, float v1, float factor)
	{
		return v0 * (1f - factor) + v1 * factor;
	}

	public static Vector2 Linear(Vector2 v0, Vector2 v1, float factor)
	{
		return v0 * (1f - factor) + v1 * factor;
	}

	public static Vector3 Linear(Vector3 v0, Vector3 v1, float factor)
	{
		return v0 * (1f - factor) + v1 * factor;
	}

	public static Quaternion Linear(Quaternion q0, Quaternion q1, float factor)
	{
		return Quaternion.Slerp(q0, q1, factor);
	}

	public static Color Linear(Color c0, Color c1, float factor)
	{
		return c0 * (1f - factor) + c1 * factor;
	}

	public static float Cosine(float v1, float v2, float factor)
	{
		factor = (1f - Mathf.Cos(factor * 3.14159274f)) * 0.5f;
		return v1 * (1f - factor) + v2 * factor;
	}

	public static Vector3 Cosine(Vector3 v1, Vector3 v2, float factor)
	{
		factor = (1f - Mathf.Cos(factor * 3.14159274f)) * 0.5f;
		return v1 * (1f - factor) + v2 * factor;
	}

	public static Quaternion Cosine(Quaternion q1, Quaternion q2, float factor)
	{
		factor = (1f - Mathf.Cos(factor * 3.14159274f)) * 0.5f;
		return Quaternion.Slerp(q1, q2, factor);
	}

	public static float Hermite(float start, float end, float factor)
	{
		factor = factor * factor * (3f - 2f * factor);
		return Mathf.Lerp(start, end, factor);
	}

	public static Vector3 Hermite(Vector3 v0, Vector3 v1, float factor)
	{
		factor = factor * factor * (3f - 2f * factor);
		return v0 * (1f - factor) + v1 * factor;
	}

	public static Color Hermite(Color v0, Color v1, float factor)
	{
		factor = factor * factor * (3f - 2f * factor);
		return v0 * (1f - factor) + v1 * factor;
	}

	public static Quaternion Hermite(Quaternion q1, Quaternion q2, float factor)
	{
		factor = factor * factor * (3f - 2f * factor);
		return Quaternion.Slerp(q1, q2, factor);
	}

	public static float GetHermiteTangent(float distance0, float distance1, float duration0, float duration1)
	{
		return (distance0 / duration0 + distance1 / duration1) * 0.5f;
	}

	public static Vector3 GetHermiteTangent(Vector3 distance0, Vector3 distance1, float duration0, float duration1)
	{
		return distance0 * (0.5f / duration0) + distance1 * (0.5f / duration1);
	}

	public static float Hermite(float pos0, float pos1, float tan0, float tan1, float factor, float duration)
	{
		float num = factor * factor;
		float num2 = num * factor;
		return pos0 * (2f * num2 - 3f * num + 1f) + pos1 * (3f * num - 2f * num2) + (tan0 * (num2 - 2f * num + factor) + tan1 * (num2 - num)) * duration;
	}

	public static Vector3 Hermite(Vector3 pos0, Vector3 pos1, Vector3 tan0, Vector3 tan1, float factor, float duration)
	{
		float num = factor * factor;
		float num2 = num * factor;
		return pos0 * (2f * num2 - 3f * num + 1f) + pos1 * (3f * num - 2f * num2) + (tan0 * (num2 - 2f * num + factor) + tan1 * (num2 - num)) * duration;
	}

	public static float Hermite(float tan0, float tan1, float factor, float duration)
	{
		float num = factor * factor;
		float num2 = num * factor;
		return 3f * num - 2f * num2 + (tan0 * (num2 - 2f * num + factor) + tan1 * (num2 - num)) * duration;
	}

	private static Quaternion SlerpNoInvert(Quaternion from, Quaternion to, float factor)
	{
		float num = Quaternion.Dot(from, to);
		if (Mathf.Abs(num) > 0.999999f)
		{
			return Quaternion.Lerp(from, to, factor);
		}
		float num2 = Mathf.Acos(Mathf.Clamp(num, -1f, 1f));
		float num3 = 1f / Mathf.Sin(num2);
		float num4 = Mathf.Sin((1f - factor) * num2) * num3;
		float num5 = Mathf.Sin(factor * num2) * num3;
		return new Quaternion(num4 * from.x + num5 * to.x, num4 * from.y + num5 * to.y, num4 * from.z + num5 * to.z, num4 * from.w + num5 * to.w);
	}

	private static Quaternion Log(Quaternion q)
	{
		float num = Mathf.Acos(Mathf.Clamp(q.w, -1f, 1f));
		float num2 = Mathf.Sin(num);
		if (Mathf.Abs(num2) < 1E-06f)
		{
			return new Quaternion(0f, 0f, 0f, 0f);
		}
		num /= num2;
		return new Quaternion(q.x * num, q.y * num, q.z * num, 0f);
	}

	private static Quaternion Exp(Quaternion q)
	{
		float num = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);
		float num2 = Mathf.Sin(num);
		float w = Mathf.Cos(num);
		if (Mathf.Abs(num) < 1E-06f)
		{
			return new Quaternion(0f, 0f, 0f, w);
		}
		num2 /= num;
		return new Quaternion(q.x * num2, q.y * num2, q.z * num2, w);
	}

	public static Quaternion GetSquadControlRotation(Quaternion past, Quaternion current, Quaternion future)
	{
		Quaternion lhs = new Quaternion(-current.x, -current.y, -current.z, current.w);
		Quaternion quaternion = Interpolation.Log(lhs * past);
		Quaternion quaternion2 = Interpolation.Log(lhs * future);
		Quaternion q = new Quaternion(-0.25f * (quaternion.x + quaternion2.x), -0.25f * (quaternion.y + quaternion2.y), -0.25f * (quaternion.z + quaternion2.z), -0.25f * (quaternion.w + quaternion2.w));
		return current * Interpolation.Exp(q);
	}

	public static Quaternion Squad(Quaternion from, Quaternion to, Quaternion ctrlFrom, Quaternion ctrlTo, float factor)
	{
		return Interpolation.SlerpNoInvert(Interpolation.SlerpNoInvert(from, to, factor), Interpolation.SlerpNoInvert(ctrlFrom, ctrlTo, factor), factor * 2f * (1f - factor));
	}
}
