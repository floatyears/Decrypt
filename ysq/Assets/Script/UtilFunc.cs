using System;
using UnityEngine;

public static class UtilFunc
{
	private static uint x;

	private static uint y;

	private static uint z;

	private static uint w;

	public static float SpringDamp(float current, float target, float smoothTime, float springConst, float springLen)
	{
		int num = (int)(smoothTime * 1000f);
		if (num > 250)
		{
			num = 250;
		}
		float num2 = 0.04f * springConst;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		for (int i = 0; i < num; i++)
		{
			if (current > target)
			{
				float num3 = current - target;
				if (springLen > num3)
				{
					break;
				}
				float num4 = num2 * (springLen - num3);
				current += num3 * num4;
			}
			else
			{
				float num5 = target - current;
				if (springLen > num5)
				{
					break;
				}
				float num6 = num2 * (springLen - num5);
				current -= num5 * num6;
			}
		}
		return current;
	}

	public static Vector3 SpringDamp(Vector3 current, Vector3 target, float smoothTime, float springConst, float springLen)
	{
		int num = (int)(smoothTime * 1000f);
		if (num > 250)
		{
			num = 250;
		}
		float num2 = 0.04f * springConst;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = current - target;
			float magnitude = vector.magnitude;
			if (springLen > magnitude)
			{
				break;
			}
			float d = num2 * (springLen - magnitude);
			current += vector.normalized * d;
		}
		return current;
	}

	public static void SetSeed(uint seed)
	{
		UtilFunc.x = seed;
		UtilFunc.y = UtilFunc.x * 1812433253u + 1u;
		UtilFunc.z = UtilFunc.y * 1812433253u + 1u;
		UtilFunc.w = UtilFunc.z * 1812433253u + 1u;
	}

	public static uint GetSeed()
	{
		return UtilFunc.x;
	}

	public static uint GetRandom()
	{
		uint num = UtilFunc.x ^ UtilFunc.x << 11;
		UtilFunc.x = UtilFunc.y;
		UtilFunc.y = UtilFunc.z;
		UtilFunc.z = UtilFunc.w;
		return UtilFunc.w = (UtilFunc.w ^ UtilFunc.w >> 19 ^ (num ^ num >> 8));
	}

	public static int RangeRandom(int min, int max)
	{
		if (min < max)
		{
			int num = max - min;
			int num2 = (int)((ulong)UtilFunc.GetRandom() % (ulong)((long)num));
			return num2 + min;
		}
		return min;
	}

	public static float GetFloatRandom()
	{
		uint random = UtilFunc.GetRandom();
		return (random & 8388607u) * 1.192093E-07f;
	}

	public static float RangeRandom(float min, float max)
	{
		float floatRandom = UtilFunc.GetFloatRandom();
		return min * floatRandom + (1f - floatRandom) * max;
	}
}
