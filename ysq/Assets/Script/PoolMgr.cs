using Att;
using PathologicalGames;
using PigeonCoopToolkit.Effects.Trails;
using System;
using UnityEngine;

public class PoolMgr
{
	public static SpawnPool spawnPool;

	public static SpawnPool spawnPoolByName;

	public static void CreatePrefabPool(Transform prefab, int preloadAmount = 2, int cullAbove = 5)
	{
		if (PoolMgr.spawnPool == null)
		{
			global::Debug.Log(new object[]
			{
				"spawnPool is not created!"
			});
			return;
		}
		PrefabPool prefabPool = PoolMgr.spawnPool.GetPrefabPool(prefab);
		if (prefabPool != null)
		{
			return;
		}
		prefabPool = new PrefabPool(prefab);
		prefabPool.preloadAmount = preloadAmount;
		prefabPool.cullDespawned = true;
		prefabPool.cullAbove = cullAbove;
		prefabPool.cullDelay = 3;
		prefabPool._logMessages = false;
		PoolMgr.spawnPool.CreatePrefabPool(prefabPool);
	}

	public static void CreatePrefabPool(string prefabName, int preloadAmount = 1, int cullAbove = 5)
	{
		if (PoolMgr.spawnPoolByName == null)
		{
			global::Debug.Log(new object[]
			{
				"spawnPoolByName is not created!"
			});
			return;
		}
		if (PoolMgr.spawnPoolByName.prefabs.ContainsKey(prefabName))
		{
			return;
		}
		GameObject gameObject = Res.Load<GameObject>(prefabName, false);
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Res.Load error, name = {0}", prefabName)
			});
			return;
		}
		PrefabPool prefabPool = new PrefabPool(gameObject.transform);
		prefabPool.name = prefabName;
		prefabPool.preloadAmount = preloadAmount;
		prefabPool.cullDespawned = true;
		prefabPool.cullAbove = cullAbove;
		prefabPool.cullDelay = 3;
		prefabPool._logMessages = false;
		PoolMgr.spawnPoolByName.CreatePrefabPool(prefabPool);
	}

	public static void CreatePetPrefabPool(PetInfo pInfo)
	{
		if (pInfo == null)
		{
			return;
		}
		PoolMgr.CreateSkillPrefabPool(pInfo.PlayerSkillID);
		for (int i = 0; i < pInfo.SkillID.Count; i++)
		{
			PoolMgr.CreateSkillPrefabPool(pInfo.SkillID[i]);
		}
		if (EffectSoundManager.IsEffectSoundOptionOn())
		{
			Globals.Instance.EffectSoundMgr.CacheSoundResourceSync(pInfo.HitSound, 360f);
			Globals.Instance.EffectSoundMgr.CacheSoundResourceSync(pInfo.DeadSound, 360f);
		}
	}

	public static void CreateMonsterPrefabPool(MonsterInfo mInfo)
	{
		if (mInfo == null)
		{
			return;
		}
		for (int i = 0; i < mInfo.SkillID.Count; i++)
		{
			PoolMgr.CreateSkillPrefabPool(mInfo.SkillID[i]);
		}
		if (EffectSoundManager.IsEffectSoundOptionOn())
		{
			Globals.Instance.EffectSoundMgr.CacheSoundResourceSync(mInfo.HitSound, 360f);
			Globals.Instance.EffectSoundMgr.CacheSoundResourceSync(mInfo.DeadSound, 360f);
		}
	}

	public static void CreateSkillPrefabPool(int skillID)
	{
		if (skillID == 0)
		{
			return;
		}
		SkillInfo info = Globals.Instance.AttDB.SkillDict.GetInfo(skillID);
		if (info == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(info.CastAction))
		{
			PoolMgr.CreatePrefabPool(info.CastAction, 1, 5);
			Transform transform = PoolMgr.spawnPoolByName.Spawn(info.CastAction);
			PoolMgr.CreateActionPrefabPool(transform);
			PoolMgr.spawnPoolByName.Despawn(transform);
		}
		for (int i = 0; i < info.HitAction.Count; i++)
		{
			if (!string.IsNullOrEmpty(info.HitAction[i]))
			{
				PoolMgr.CreatePrefabPool(info.HitAction[i], 1, 5);
				Transform transform2 = PoolMgr.spawnPoolByName.Spawn(info.HitAction[i]);
				PoolMgr.CreateActionPrefabPool(transform2);
				PoolMgr.spawnPoolByName.Despawn(transform2);
			}
		}
		if (info.ComboSkillID != 0)
		{
			PoolMgr.CreateSkillPrefabPool(info.ComboSkillID);
		}
		for (int j = 0; j < info.EffectType.Count; j++)
		{
			if (info.EffectType[j] == 1)
			{
				PoolMgr.CreateSkillPrefabPool(info.Value3[j]);
			}
			else if (info.EffectType[j] == 2)
			{
				PoolMgr.CreateBuffPrefabPool(info.Value3[j]);
			}
			else if (info.EffectType[j] == 7)
			{
				AreaEffectInfo info2 = Globals.Instance.AttDB.AreaEffectDict.GetInfo(info.Value3[j]);
				if (info2 != null)
				{
					if (!string.IsNullOrEmpty(info2.ResLoc))
					{
						PoolMgr.CreatePrefabPool(info2.ResLoc, 1, 5);
					}
					PoolMgr.CreateSkillPrefabPool(info2.TriggerSkillID);
				}
			}
			else if (info.EffectType[j] == 8)
			{
				AreaEffectInfo info3 = Globals.Instance.AttDB.AreaEffectDict.GetInfo(info.Value3[j]);
				if (info3 != null)
				{
					if (!string.IsNullOrEmpty(info3.ResLoc))
					{
						PoolMgr.CreatePrefabPool(info3.ResLoc, 1, 5);
					}
					PoolMgr.CreateSkillPrefabPool(info3.TriggerSkillID);
				}
				PoolMgr.CreateBuffPrefabPool(info.Value4[j]);
			}
		}
	}

	public static void CreateBuffPrefabPool(int buffID)
	{
		if (buffID == 0)
		{
			return;
		}
		BuffInfo info = Globals.Instance.AttDB.BuffDict.GetInfo(buffID);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("BuffDict.GetInfo error, ID = {0}", buffID)
			});
			return;
		}
		if (!string.IsNullOrEmpty(info.AddAction))
		{
			PoolMgr.CreatePrefabPool(info.AddAction, 1, 5);
			Transform transform = PoolMgr.spawnPoolByName.Spawn(info.AddAction);
			PoolMgr.CreateActionPrefabPool(transform);
			PoolMgr.spawnPoolByName.Despawn(transform);
		}
		if (!string.IsNullOrEmpty(info.RemoveAction))
		{
			PoolMgr.CreatePrefabPool(info.RemoveAction, 1, 5);
			Transform transform2 = PoolMgr.spawnPoolByName.Spawn(info.RemoveAction);
			PoolMgr.CreateActionPrefabPool(transform2);
			PoolMgr.spawnPoolByName.Despawn(transform2);
		}
		for (int i = 0; i < info.EffectType.Count; i++)
		{
			if (info.EffectType[i] == 28)
			{
				if (info.Value3[i] == 0)
				{
					PoolMgr.CreateSkillPrefabPool(info.Value4[i]);
				}
				else
				{
					PoolMgr.CreateBuffPrefabPool(info.Value4[i]);
				}
			}
		}
	}

	public static void CreateActionPrefabPool(Transform action)
	{
		if (action == null)
		{
			return;
		}
		InstantiateBase[] components = action.GetComponents<InstantiateBase>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].prefab != null)
			{
				PoolMgr.CreatePrefabPool(components[i].prefab.transform, 2, 5);
			}
		}
		MissileAction[] components2 = action.GetComponents<MissileAction>();
		for (int j = 0; j < components2.Length; j++)
		{
			if (components2[j].MissilePrefab != null)
			{
				PoolMgr.CreatePrefabPool(components2[j].MissilePrefab.transform, 2, 5);
			}
			if (components2[j].explodePrefab != null)
			{
				PoolMgr.CreatePrefabPool(components2[j].explodePrefab.transform, 2, 5);
			}
		}
		if (EffectSoundManager.IsEffectSoundOptionOn())
		{
			PlaySoundAction[] components3 = action.GetComponents<PlaySoundAction>();
			for (int k = 0; k < components3.Length; k++)
			{
				Globals.Instance.EffectSoundMgr.CacheSoundResourceSync(components3[k].soundName, 360f);
			}
		}
	}

	public static Transform SpawnParticleSystem(Transform prefab, Vector3 pos, Quaternion rot, float playbackSpeed = 1f)
	{
		Transform transform = PoolMgr.spawnPool.Spawn(prefab, pos, rot);
		ParticleSystem[] componentsInChildren = transform.gameObject.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].playbackSpeed = playbackSpeed;
			componentsInChildren[i].Simulate(0f, false, true);
			componentsInChildren[i].Play();
		}
		Animator[] componentsInChildren2 = transform.gameObject.GetComponentsInChildren<Animator>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].speed = playbackSpeed;
		}
		TrailRenderer_Base[] componentsInChildren3 = transform.gameObject.GetComponentsInChildren<TrailRenderer_Base>();
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			if (!(componentsInChildren3[k] == null))
			{
				if (componentsInChildren3[k].Emit)
				{
					componentsInChildren3[k].OnStartEmit();
				}
			}
		}
		return transform;
	}

	public static Transform Spawn(string prefabName)
	{
		return PoolMgr.Spawn(prefabName, Vector3.zero, Quaternion.identity);
	}

	public static Transform Spawn(string prefabName, Vector3 pos, Quaternion rot)
	{
		if (PoolMgr.spawnPoolByName == null)
		{
			global::Debug.LogError(new object[]
			{
				"spawnPoolByName are not created!"
			});
			return null;
		}
		if (string.IsNullOrEmpty(prefabName))
		{
			return null;
		}
		if (!PoolMgr.spawnPoolByName.prefabs.ContainsKey(prefabName))
		{
			global::Debug.LogError(new object[]
			{
				string.Format("prefabPool [{0}] is not created!", prefabName)
			});
			return null;
		}
		return PoolMgr.spawnPoolByName.Spawn(prefabName, pos, rot);
	}

	public static void Despawn(Transform inst)
	{
		PoolMgr.spawnPoolByName.Despawn(inst);
	}

	public static void Despawn(Transform inst, float seconds)
	{
		PoolMgr.spawnPoolByName.Despawn(inst, seconds);
	}
}
