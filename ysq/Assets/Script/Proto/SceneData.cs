using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "SceneData")]
	[Serializable]
	public class SceneData : IExtensible
	{
		private int _SceneID;

		private int _Score;

		private int _Times;

		private int _CoolDown;

		private int _ResetCount;

		private int _SceneCount;

		private bool _QuestReward;

		private int _SceneCount2;

		private int _SceneCount3;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "SceneID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneID
		{
			get
			{
				return this._SceneID;
			}
			set
			{
				this._SceneID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Times", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Times
		{
			get
			{
				return this._Times;
			}
			set
			{
				this._Times = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "CoolDown", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CoolDown
		{
			get
			{
				return this._CoolDown;
			}
			set
			{
				this._CoolDown = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ResetCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResetCount
		{
			get
			{
				return this._ResetCount;
			}
			set
			{
				this._ResetCount = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "SceneCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneCount
		{
			get
			{
				return this._SceneCount;
			}
			set
			{
				this._SceneCount = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "QuestReward", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool QuestReward
		{
			get
			{
				return this._QuestReward;
			}
			set
			{
				this._QuestReward = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "SceneCount2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneCount2
		{
			get
			{
				return this._SceneCount2;
			}
			set
			{
				this._SceneCount2 = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "SceneCount3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneCount3
		{
			get
			{
				return this._SceneCount3;
			}
			set
			{
				this._SceneCount3 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
