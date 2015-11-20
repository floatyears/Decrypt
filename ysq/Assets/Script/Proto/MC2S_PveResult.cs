using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PveResult")]
	[Serializable]
	public class MC2S_PveResult : IExtensible
	{
		private int _ResultKey;

		private int _Score;

		private int _LootMoney;

		private CombatLog _Log;

		private int _SceneID;

		private int _Value;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ResultKey", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResultKey
		{
			get
			{
				return this._ResultKey;
			}
			set
			{
				this._ResultKey = value;
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

		[ProtoMember(3, IsRequired = false, Name = "LootMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LootMoney
		{
			get
			{
				return this._LootMoney;
			}
			set
			{
				this._LootMoney = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Log", DataFormat = DataFormat.Default), DefaultValue(null)]
		public CombatLog Log
		{
			get
			{
				return this._Log;
			}
			set
			{
				this._Log = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "SceneID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
