using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "QuestInfo")]
	[Serializable]
	public class QuestInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private int _IconType;

		private int _IconValue;

		private string _Icon = string.Empty;

		private int _ShowLevel;

		private string _Desc = string.Empty;

		private string _Desc2 = string.Empty;

		private string _Target = string.Empty;

		private string _Target2 = string.Empty;

		private string _Target3 = string.Empty;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

		private int _MapUIDialogID;

		private int _SceneStartDialogID;

		private int _SceneBossDialogID;

		private int _SceneWinDialogID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(2, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "IconType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int IconType
		{
			get
			{
				return this._IconType;
			}
			set
			{
				this._IconType = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "IconValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int IconValue
		{
			get
			{
				return this._IconValue;
			}
			set
			{
				this._IconValue = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Icon
		{
			get
			{
				return this._Icon;
			}
			set
			{
				this._Icon = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ShowLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShowLevel
		{
			get
			{
				return this._ShowLevel;
			}
			set
			{
				this._ShowLevel = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Desc
		{
			get
			{
				return this._Desc;
			}
			set
			{
				this._Desc = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Desc2", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Desc2
		{
			get
			{
				return this._Desc2;
			}
			set
			{
				this._Desc2 = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Target", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Target
		{
			get
			{
				return this._Target;
			}
			set
			{
				this._Target = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Target2", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Target2
		{
			get
			{
				return this._Target2;
			}
			set
			{
				this._Target2 = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Target3", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Target3
		{
			get
			{
				return this._Target3;
			}
			set
			{
				this._Target3 = value;
			}
		}

		[ProtoMember(12, Name = "RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardType
		{
			get
			{
				return this._RewardType;
			}
		}

		[ProtoMember(13, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
		}

		[ProtoMember(14, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "MapUIDialogID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MapUIDialogID
		{
			get
			{
				return this._MapUIDialogID;
			}
			set
			{
				this._MapUIDialogID = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "SceneStartDialogID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneStartDialogID
		{
			get
			{
				return this._SceneStartDialogID;
			}
			set
			{
				this._SceneStartDialogID = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "SceneBossDialogID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneBossDialogID
		{
			get
			{
				return this._SceneBossDialogID;
			}
			set
			{
				this._SceneBossDialogID = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "SceneWinDialogID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneWinDialogID
		{
			get
			{
				return this._SceneWinDialogID;
			}
			set
			{
				this._SceneWinDialogID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
