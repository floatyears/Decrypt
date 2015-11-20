using ProtoBuf;
using System;
using System.ComponentModel;

namespace MG
{
	[ProtoContract(Name = "ConfigData")]
	[Serializable]
	public class ConfigData : IExtensible
	{
		private string _Account = string.Empty;

		private string _Password = string.Empty;

		private string _ServerName = string.Empty;

		private int _ServerID;

		private bool _Music = true;

		private bool _Sound = true;

		private int _GraphQuality;

		private bool _NoScreenLock = true;

		private int _StartMovieVersion;

		private bool _Voice = true;

		private bool _ShowHPBar = true;

		private bool _TwelveEnergy = true;

		private bool _EighteenEnergy = true;

		private bool _CCSdk;

		private bool _EnergyFull;

		private bool _WebViewDontShow;

		private long _WebViewDontShowTimeStamp;

		private bool _LastGMLogin;

		private bool _TwentyOneEnergy = true;

		private bool _KoreaBuyWndDontShow;

		private long _KoreaBuyWndDontShowTimeStamp;

		private bool _UserAgreement;

		private bool _PetShopRefresh;

		private bool _JueXingShopRefresh;

		private bool _ShieldPartyInvite;

		private bool _ShieldPartyInteraction;

		private int _ChatPositionX = -552;

		private int _ChatPositionY = -158;

		private bool _WorldBossNotify = true;

		private bool _AutoPlayChatVoice;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Account", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Account
		{
			get
			{
				return this._Account;
			}
			set
			{
				this._Account = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Password", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ServerName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ServerName
		{
			get
			{
				return this._ServerName;
			}
			set
			{
				this._ServerName = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ServerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "Music", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool Music
		{
			get
			{
				return this._Music;
			}
			set
			{
				this._Music = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Sound", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool Sound
		{
			get
			{
				return this._Sound;
			}
			set
			{
				this._Sound = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "GraphQuality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GraphQuality
		{
			get
			{
				return this._GraphQuality;
			}
			set
			{
				this._GraphQuality = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "NoScreenLock", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool NoScreenLock
		{
			get
			{
				return this._NoScreenLock;
			}
			set
			{
				this._NoScreenLock = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "StartMovieVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StartMovieVersion
		{
			get
			{
				return this._StartMovieVersion;
			}
			set
			{
				this._StartMovieVersion = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Voice", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool Voice
		{
			get
			{
				return this._Voice;
			}
			set
			{
				this._Voice = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "ShowHPBar", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool ShowHPBar
		{
			get
			{
				return this._ShowHPBar;
			}
			set
			{
				this._ShowHPBar = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "TwelveEnergy", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool TwelveEnergy
		{
			get
			{
				return this._TwelveEnergy;
			}
			set
			{
				this._TwelveEnergy = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "EighteenEnergy", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool EighteenEnergy
		{
			get
			{
				return this._EighteenEnergy;
			}
			set
			{
				this._EighteenEnergy = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "CCSdk", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool CCSdk
		{
			get
			{
				return this._CCSdk;
			}
			set
			{
				this._CCSdk = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "EnergyFull", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool EnergyFull
		{
			get
			{
				return this._EnergyFull;
			}
			set
			{
				this._EnergyFull = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "WebViewDontShow", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool WebViewDontShow
		{
			get
			{
				return this._WebViewDontShow;
			}
			set
			{
				this._WebViewDontShow = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "WebViewDontShowTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long WebViewDontShowTimeStamp
		{
			get
			{
				return this._WebViewDontShowTimeStamp;
			}
			set
			{
				this._WebViewDontShowTimeStamp = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "LastGMLogin", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool LastGMLogin
		{
			get
			{
				return this._LastGMLogin;
			}
			set
			{
				this._LastGMLogin = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "TwentyOneEnergy", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool TwentyOneEnergy
		{
			get
			{
				return this._TwentyOneEnergy;
			}
			set
			{
				this._TwentyOneEnergy = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "KoreaBuyWndDontShow", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool KoreaBuyWndDontShow
		{
			get
			{
				return this._KoreaBuyWndDontShow;
			}
			set
			{
				this._KoreaBuyWndDontShow = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "KoreaBuyWndDontShowTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long KoreaBuyWndDontShowTimeStamp
		{
			get
			{
				return this._KoreaBuyWndDontShowTimeStamp;
			}
			set
			{
				this._KoreaBuyWndDontShowTimeStamp = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "UserAgreement", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool UserAgreement
		{
			get
			{
				return this._UserAgreement;
			}
			set
			{
				this._UserAgreement = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "PetShopRefresh", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool PetShopRefresh
		{
			get
			{
				return this._PetShopRefresh;
			}
			set
			{
				this._PetShopRefresh = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "JueXingShopRefresh", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool JueXingShopRefresh
		{
			get
			{
				return this._JueXingShopRefresh;
			}
			set
			{
				this._JueXingShopRefresh = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "ShieldPartyInvite", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool ShieldPartyInvite
		{
			get
			{
				return this._ShieldPartyInvite;
			}
			set
			{
				this._ShieldPartyInvite = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "ShieldPartyInteraction", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool ShieldPartyInteraction
		{
			get
			{
				return this._ShieldPartyInteraction;
			}
			set
			{
				this._ShieldPartyInteraction = value;
			}
		}

		[ProtoMember(31, IsRequired = false, Name = "ChatPositionX", DataFormat = DataFormat.TwosComplement), DefaultValue(-552)]
		public int ChatPositionX
		{
			get
			{
				return this._ChatPositionX;
			}
			set
			{
				this._ChatPositionX = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "ChatPositionY", DataFormat = DataFormat.TwosComplement), DefaultValue(-158)]
		public int ChatPositionY
		{
			get
			{
				return this._ChatPositionY;
			}
			set
			{
				this._ChatPositionY = value;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "WorldBossNotify", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool WorldBossNotify
		{
			get
			{
				return this._WorldBossNotify;
			}
			set
			{
				this._WorldBossNotify = value;
			}
		}

		[ProtoMember(34, IsRequired = false, Name = "AutoPlayChatVoice", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool AutoPlayChatVoice
		{
			get
			{
				return this._AutoPlayChatVoice;
			}
			set
			{
				this._AutoPlayChatVoice = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
