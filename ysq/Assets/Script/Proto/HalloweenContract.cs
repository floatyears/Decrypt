using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "HalloweenContract")]
	[Serializable]
	public class HalloweenContract : IExtensible
	{
		private int _ID;

		private readonly List<int> _MyNums = new List<int>();

		private int _FirstLuckNum;

		private int _SecondLuckNum;

		private int _ThirdLuckNum;

		private string _FirstLuckPlayer = string.Empty;

		private string _SecondLuckPlayer = string.Empty;

		private string _ThirdLuckPlayer = string.Empty;

		private readonly List<int> _LuckyNums = new List<int>();

		private readonly List<int> _UnluckyNums = new List<int>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, Name = "MyNums", DataFormat = DataFormat.TwosComplement)]
		public List<int> MyNums
		{
			get
			{
				return this._MyNums;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "FirstLuckNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FirstLuckNum
		{
			get
			{
				return this._FirstLuckNum;
			}
			set
			{
				this._FirstLuckNum = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "SecondLuckNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SecondLuckNum
		{
			get
			{
				return this._SecondLuckNum;
			}
			set
			{
				this._SecondLuckNum = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ThirdLuckNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ThirdLuckNum
		{
			get
			{
				return this._ThirdLuckNum;
			}
			set
			{
				this._ThirdLuckNum = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "FirstLuckPlayer", DataFormat = DataFormat.Default), DefaultValue("")]
		public string FirstLuckPlayer
		{
			get
			{
				return this._FirstLuckPlayer;
			}
			set
			{
				this._FirstLuckPlayer = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "SecondLuckPlayer", DataFormat = DataFormat.Default), DefaultValue("")]
		public string SecondLuckPlayer
		{
			get
			{
				return this._SecondLuckPlayer;
			}
			set
			{
				this._SecondLuckPlayer = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "ThirdLuckPlayer", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ThirdLuckPlayer
		{
			get
			{
				return this._ThirdLuckPlayer;
			}
			set
			{
				this._ThirdLuckPlayer = value;
			}
		}

		[ProtoMember(9, Name = "LuckyNums", DataFormat = DataFormat.TwosComplement)]
		public List<int> LuckyNums
		{
			get
			{
				return this._LuckyNums;
			}
		}

		[ProtoMember(10, Name = "UnluckyNums", DataFormat = DataFormat.TwosComplement)]
		public List<int> UnluckyNums
		{
			get
			{
				return this._UnluckyNums;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
