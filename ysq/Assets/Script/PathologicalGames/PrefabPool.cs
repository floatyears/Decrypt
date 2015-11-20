using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace PathologicalGames
{
	[Serializable]
	public class PrefabPool
	{
		public string name;

		public Transform prefab;

		internal GameObject prefabGO;

		public int preloadAmount = 1;

		public bool preloadTime;

		public int preloadFrames = 2;

		public float preloadDelay;

		public bool limitInstances;

		public int limitAmount = 100;

		public bool limitFIFO;

		public bool cullDespawned;

		public int cullAbove = 50;

		public int cullDelay = 60;

		public int cullMaxPerPass = 5;

		public bool _logMessages;

		private bool forceLoggingSilent;

		public SpawnPool spawnPool;

		private bool cullingActive;

		internal List<Transform> _spawned = new List<Transform>();

		internal List<Transform> _despawned = new List<Transform>();

		private bool _preloaded;

		public bool logMessages
		{
			get
			{
				if (this.forceLoggingSilent)
				{
					return false;
				}
				if (this.spawnPool.logMessages)
				{
					return this.spawnPool.logMessages;
				}
				return this._logMessages;
			}
		}

		public List<Transform> spawned
		{
			get
			{
				return new List<Transform>(this._spawned);
			}
		}

		public List<Transform> despawned
		{
			get
			{
				return new List<Transform>(this._despawned);
			}
		}

		public int totalCount
		{
			get
			{
				int num = 0;
				num += this._spawned.Count;
				return num + this._despawned.Count;
			}
		}

		internal bool preloaded
		{
			get
			{
				return this._preloaded;
			}
			private set
			{
				this._preloaded = value;
			}
		}

		public PrefabPool(Transform prefab)
		{
			this.prefab = prefab;
			this.prefabGO = prefab.gameObject;
		}

		public PrefabPool()
		{
		}

		internal void inspectorInstanceConstructor()
		{
			this.prefabGO = this.prefab.gameObject;
			this._spawned = new List<Transform>();
			this._despawned = new List<Transform>();
		}

		internal void SelfDestruct()
		{
			this.prefab = null;
			this.prefabGO = null;
			this.spawnPool = null;
			foreach (Transform current in this._despawned)
			{
				if (current != null)
				{
					UnityEngine.Object.DestroyImmediate(current.gameObject);
				}
			}
			foreach (Transform current2 in this._spawned)
			{
				if (current2 != null)
				{
					UnityEngine.Object.DestroyImmediate(current2.gameObject);
				}
			}
			this._spawned.Clear();
			this._despawned.Clear();
		}

		internal bool DespawnInstance(Transform xform)
		{
			return this.DespawnInstance(xform, true);
		}

		internal bool DespawnInstance(Transform xform, bool sendEventMessage)
		{
			if (this.logMessages)
			{
				global::Debug.Log(new object[]
				{
					string.Format("SpawnPool {0} ({1}): Despawning '{2}'", this.spawnPool.poolName, this.prefab.name, xform.name)
				});
			}
			this._spawned.Remove(xform);
			this._despawned.Add(xform);
			if (sendEventMessage)
			{
				xform.gameObject.BroadcastMessage("OnDespawned", this.spawnPool, SendMessageOptions.DontRequireReceiver);
			}
			PoolManagerUtils.SetActive(xform.gameObject, false);
			if (!this.cullingActive && this.cullDespawned && this.totalCount > this.cullAbove)
			{
				this.cullingActive = true;
				this.spawnPool.StartCoroutine(this.CullDespawned());
			}
			return true;
		}

		[DebuggerHidden]
		internal IEnumerator CullDespawned()
		{
            if(logMessages)
            {
                global::Debug.Log(new object[]
				{
					string.Format("SpawnPool {0} ({1}): CULLING TRIGGERED! Waiting {2}sec to begin checking for despawns...", spawnPool.poolName, prefab.name, cullDelay)
				});
            }
            yield return new WaitForSeconds(cullDelay);
            if(totalCount > cullAbove)
            {
                for (int i = 0; i < cullMaxPerPass; i++)
                {
                    if(totalCount <= cullAbove)
                    {
                        break;
                    }
                    Transform inst = null;
                    if (despawned.Count > 0)
                    {
                        inst = despawned[0];
                        despawned.RemoveAt(0);
                        GameObject.Destroy(inst.gameObject);
                        if (logMessages)
                        {
                            global::Debug.Log(new object[]
				            {
                                string.Format("SpawnPool {0} ({1}): CULLING to {2} instances. Now at {3}.", spawnPool.poolName, prefab.name, cullAbove, totalCount)
                            });
                        }
                    }else if(logMessages)
                    {
                        global::Debug.Log(new object[]
				        {
                            string.Format("SpawnPool {0} ({1}): CULLING waiting for despawn. Checking again in {2}sec", spawnPool.poolName, prefab.name, cullDelay)
                        });
                    }
                }
            }
            global::Debug.Log(new object[]
			{
                string.Format("SpawnPool {0} ({1}): CULLING FINISHED! Stopping", spawnPool.poolName, prefab.name)
            });
            cullingActive = false;

            yield return new WaitForSeconds(cullDelay);
		}

		internal Transform SpawnInstance(Vector3 pos, Quaternion rot)
		{
			if (this.limitInstances && this.limitFIFO && this._spawned.Count >= this.limitAmount)
			{
				Transform transform = this._spawned[0];
				if (this.logMessages)
				{
					global::Debug.Log(new object[]
					{
						string.Format("SpawnPool {0} ({1}): LIMIT REACHED! FIFO=True. Calling despawning for {2}...", this.spawnPool.poolName, this.prefab.name, transform)
					});
				}
				this.DespawnInstance(transform);
				this.spawnPool._spawned.Remove(transform);
			}
			Transform transform2;
			if (this._despawned.Count == 0)
			{
				transform2 = this.SpawnNew(pos, rot);
			}
			else
			{
				transform2 = this._despawned[0];
				this._despawned.RemoveAt(0);
				this._spawned.Add(transform2);
				if (transform2 == null)
				{
					string message = "Make sure you didn't delete a despawned instance directly.";
					throw new MissingReferenceException(message);
				}
				if (this.logMessages)
				{
					global::Debug.Log(new object[]
					{
						string.Format("SpawnPool {0} ({1}): respawning '{2}'.", this.spawnPool.poolName, this.prefab.name, transform2.name)
					});
				}
				transform2.position = pos;
				transform2.rotation = rot;
				PoolManagerUtils.SetActive(transform2.gameObject, true);
			}
			return transform2;
		}

		public Transform SpawnNew()
		{
			return this.SpawnNew(Vector3.zero, Quaternion.identity);
		}

		public Transform SpawnNew(Vector3 pos, Quaternion rot)
		{
			if (this.limitInstances && this.totalCount >= this.limitAmount)
			{
				if (this.logMessages)
				{
					global::Debug.Log(new object[]
					{
						string.Format("SpawnPool {0} ({1}): LIMIT REACHED! Not creating new instances! (Returning null)", this.spawnPool.poolName, this.prefab.name)
					});
				}
				return null;
			}
			if (pos == Vector3.zero)
			{
				pos = this.spawnPool.group.position;
			}
			if (rot == Quaternion.identity)
			{
				rot = this.spawnPool.group.rotation;
			}
			Transform transform = (Transform)UnityEngine.Object.Instantiate(this.prefab, pos, rot);
			this.nameInstance(transform);
			if (!this.spawnPool.dontReparent)
			{
				transform.parent = this.spawnPool.group;
			}
			if (this.spawnPool.matchPoolScale)
			{
				transform.localScale = Vector3.one;
			}
			if (this.spawnPool.matchPoolLayer)
			{
				this.SetRecursively(transform, this.spawnPool.gameObject.layer);
			}
			this._spawned.Add(transform);
			if (this.logMessages)
			{
				global::Debug.Log(new object[]
				{
					string.Format("SpawnPool {0} ({1}): Spawned new instance '{2}'.", this.spawnPool.poolName, this.prefab.name, transform.name)
				});
			}
			return transform;
		}

		private void SetRecursively(Transform xform, int layer)
		{
			xform.gameObject.layer = layer;
			foreach (Transform xform2 in xform)
			{
				this.SetRecursively(xform2, layer);
			}
		}

		internal void AddUnpooled(Transform inst, bool despawn)
		{
			this.nameInstance(inst);
			if (despawn)
			{
				PoolManagerUtils.SetActive(inst.gameObject, false);
				this._despawned.Add(inst);
			}
			else
			{
				this._spawned.Add(inst);
			}
		}

		internal void PreloadInstances()
		{
			if (this.preloaded)
			{
				global::Debug.Log(new object[]
				{
					string.Format("SpawnPool {0} ({1}): Already preloaded! You cannot preload twice. If you are running this through code, make sure it isn't also defined in the Inspector.", this.spawnPool.poolName, this.prefab.name)
				});
				return;
			}
			if (this.prefab == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("SpawnPool {0} ({1}): Prefab cannot be null.", this.spawnPool.poolName, this.prefab.name)
				});
				return;
			}
			if (this.limitInstances && this.preloadAmount > this.limitAmount)
			{
				global::Debug.LogWarning(new object[]
				{
					string.Format("SpawnPool {0} ({1}): You turned ON 'Limit Instances' and entered a 'Limit Amount' greater than the 'Preload Amount'! Setting preload amount to limit amount.", this.spawnPool.poolName, this.prefab.name)
				});
				this.preloadAmount = this.limitAmount;
			}
			if (this.cullDespawned && this.preloadAmount > this.cullAbove)
			{
				global::Debug.LogWarning(new object[]
				{
					string.Format("SpawnPool {0} ({1}): You turned ON Culling and entered a 'Cull Above' threshold greater than the 'Preload Amount'! This will cause the culling feature to trigger immediatly, which is wrong conceptually. Only use culling for extreme situations. See the docs.", this.spawnPool.poolName, this.prefab.name)
				});
			}
			if (this.preloadTime)
			{
				if (this.preloadFrames > this.preloadAmount)
				{
					global::Debug.LogWarning(new object[]
					{
						string.Format("SpawnPool {0} ({1}): Preloading over-time is on but the frame duration is greater than the number of instances to preload. The minimum spawned per frame is 1, so the maximum time is the same as the number of instances. Changing the preloadFrames value...", this.spawnPool.poolName, this.prefab.name)
					});
					this.preloadFrames = this.preloadAmount;
				}
				this.spawnPool.StartCoroutine(this.PreloadOverTime());
			}
			else
			{
				this.forceLoggingSilent = true;
				while (this.totalCount < this.preloadAmount)
				{
					Transform xform = this.SpawnNew();
					this.DespawnInstance(xform, false);
				}
				this.forceLoggingSilent = false;
			}
		}

		[DebuggerHidden]
		private IEnumerator PreloadOverTime()
		{
            yield return new WaitForSeconds(preloadDelay);
            int amount = preloadAmount - totalCount;
            if (amount > 0)
            {
                int remainder = amount % preloadFrames;
                int numPerFrame = amount / preloadFrames;
                forceLoggingSilent = true;
                for (int i = 0; i < preloadFrames; i++)
                {
                    int numThisFrame = numPerFrame;
                    if(i == preloadFrames - 1)
                    {
                        numThisFrame += remainder;
                    }

                    Transform inst = null;
                    for (int n = 0; n < numThisFrame; n++)
                    {
                        inst = SpawnNew();
                        if(inst != null)
                        {
                            DespawnInstance(inst, false);
                            yield return n++;
                        }
                    }
                }
                forceLoggingSilent = false;
            }
            //return null;
            //PrefabPool.<PreloadOverTime>c__Iterator7 <PreloadOverTime>c__Iterator = new PrefabPool.<PreloadOverTime>c__Iterator7();
            //<PreloadOverTime>c__Iterator.<>f__this = this;
            //return <PreloadOverTime>c__Iterator;
		}

		public bool Contains(Transform transform)
		{
			if (this.prefabGO == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("SpawnPool {0}: PrefabPool.prefabGO is null", this.spawnPool.poolName)
				});
			}
			bool flag = this.spawned.Contains(transform);
			return flag || this.despawned.Contains(transform);
		}

		private void nameInstance(Transform instance)
		{
			instance.name += (this.totalCount + 1).ToString("#000");
		}
	}
}
