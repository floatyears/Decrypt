using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_EnterGame")]
	[Serializable]
	public class MC2S_EnterGame : IExtensible
	{
		private string _ID;

		private string _Platform = string.Empty;

		private string _Channel = string.Empty;

		private string _key = string.Empty;

		private bool _Reconnect;

		private string _IP = string.Empty;

		private string _MAC = string.Empty;

		private string _DeviceID = string.Empty;

		private string _ClientVersion = string.Empty;

		private string _OSName = string.Empty;

		private string _OSVersion = string.Empty;

		private int _DeviceHeight;

		private int _DeviceWidth;

		private string _ISP = string.Empty;

		private string _Network = string.Empty;

		private string _DeviceModel = string.Empty;

		private string _UDID = string.Empty;

		private string _GuestID = string.Empty;

		private string _AppChannel = string.Empty;

		private string _PayChannel = string.Empty;

		private string _SDKVersion = string.Empty;

		private int _Publisher;

		private uint _DLLCrc;

		private uint _ResCrc;

		private string _Urs = string.Empty;

		private bool _Break;

		private int _ServerID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.Default)]
		public string ID
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

		[ProtoMember(2, IsRequired = false, Name = "Platform", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Platform
		{
			get
			{
				return this._Platform;
			}
			set
			{
				this._Platform = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Channel", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Channel
		{
			get
			{
				return this._Channel;
			}
			set
			{
				this._Channel = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "key", DataFormat = DataFormat.Default), DefaultValue("")]
		public string key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Reconnect", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Reconnect
		{
			get
			{
				return this._Reconnect;
			}
			set
			{
				this._Reconnect = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "IP", DataFormat = DataFormat.Default), DefaultValue("")]
		public string IP
		{
			get
			{
				return this._IP;
			}
			set
			{
				this._IP = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "MAC", DataFormat = DataFormat.Default), DefaultValue("")]
		public string MAC
		{
			get
			{
				return this._MAC;
			}
			set
			{
				this._MAC = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "DeviceID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string DeviceID
		{
			get
			{
				return this._DeviceID;
			}
			set
			{
				this._DeviceID = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "ClientVersion", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ClientVersion
		{
			get
			{
				return this._ClientVersion;
			}
			set
			{
				this._ClientVersion = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "OSName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string OSName
		{
			get
			{
				return this._OSName;
			}
			set
			{
				this._OSName = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "OSVersion", DataFormat = DataFormat.Default), DefaultValue("")]
		public string OSVersion
		{
			get
			{
				return this._OSVersion;
			}
			set
			{
				this._OSVersion = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "DeviceHeight", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DeviceHeight
		{
			get
			{
				return this._DeviceHeight;
			}
			set
			{
				this._DeviceHeight = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "DeviceWidth", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DeviceWidth
		{
			get
			{
				return this._DeviceWidth;
			}
			set
			{
				this._DeviceWidth = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "ISP", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ISP
		{
			get
			{
				return this._ISP;
			}
			set
			{
				this._ISP = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "Network", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Network
		{
			get
			{
				return this._Network;
			}
			set
			{
				this._Network = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "DeviceModel", DataFormat = DataFormat.Default), DefaultValue("")]
		public string DeviceModel
		{
			get
			{
				return this._DeviceModel;
			}
			set
			{
				this._DeviceModel = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "UDID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string UDID
		{
			get
			{
				return this._UDID;
			}
			set
			{
				this._UDID = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "GuestID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string GuestID
		{
			get
			{
				return this._GuestID;
			}
			set
			{
				this._GuestID = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "AppChannel", DataFormat = DataFormat.Default), DefaultValue("")]
		public string AppChannel
		{
			get
			{
				return this._AppChannel;
			}
			set
			{
				this._AppChannel = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "PayChannel", DataFormat = DataFormat.Default), DefaultValue("")]
		public string PayChannel
		{
			get
			{
				return this._PayChannel;
			}
			set
			{
				this._PayChannel = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "SDKVersion", DataFormat = DataFormat.Default), DefaultValue("")]
		public string SDKVersion
		{
			get
			{
				return this._SDKVersion;
			}
			set
			{
				this._SDKVersion = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "Publisher", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Publisher
		{
			get
			{
				return this._Publisher;
			}
			set
			{
				this._Publisher = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "DLLCrc", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint DLLCrc
		{
			get
			{
				return this._DLLCrc;
			}
			set
			{
				this._DLLCrc = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "ResCrc", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ResCrc
		{
			get
			{
				return this._ResCrc;
			}
			set
			{
				this._ResCrc = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "Urs", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Urs
		{
			get
			{
				return this._Urs;
			}
			set
			{
				this._Urs = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "Break", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Break
		{
			get
			{
				return this._Break;
			}
			set
			{
				this._Break = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "ServerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ServerID
		{
			get
			{
				return this._ServerID;
			}
			set
			{
				this._ServerID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
