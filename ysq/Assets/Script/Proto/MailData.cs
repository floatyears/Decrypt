using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MailData")]
	[Serializable]
	public class MailData : IExtensible
	{
		private uint _MailID;

		private string _Title = string.Empty;

		private string _Sender = string.Empty;

		private int _TimeStamp;

		private readonly List<int> _AffixType = new List<int>();

		private readonly List<int> _AffixValue1 = new List<int>();

		private readonly List<int> _AffixValue2 = new List<int>();

		private int _ContentType;

		private string _ContentText = string.Empty;

		private readonly List<int> _ContentValue1 = new List<int>();

		private readonly List<int> _ContentValue2 = new List<int>();

		private bool _Read;

		private int _From;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MailID", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MailID
		{
			get
			{
				return this._MailID;
			}
			set
			{
				this._MailID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Title", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				this._Title = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Sender", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Sender
		{
			get
			{
				return this._Sender;
			}
			set
			{
				this._Sender = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}

		[ProtoMember(5, Name = "AffixType", DataFormat = DataFormat.TwosComplement)]
		public List<int> AffixType
		{
			get
			{
				return this._AffixType;
			}
		}

		[ProtoMember(6, Name = "AffixValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> AffixValue1
		{
			get
			{
				return this._AffixValue1;
			}
		}

		[ProtoMember(7, Name = "AffixValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> AffixValue2
		{
			get
			{
				return this._AffixValue2;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "ContentType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ContentType
		{
			get
			{
				return this._ContentType;
			}
			set
			{
				this._ContentType = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "ContentText", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ContentText
		{
			get
			{
				return this._ContentText;
			}
			set
			{
				this._ContentText = value;
			}
		}

		[ProtoMember(10, Name = "ContentValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> ContentValue1
		{
			get
			{
				return this._ContentValue1;
			}
		}

		[ProtoMember(11, Name = "ContentValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> ContentValue2
		{
			get
			{
				return this._ContentValue2;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "Read", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Read
		{
			get
			{
				return this._Read;
			}
			set
			{
				this._Read = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "From", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int From
		{
			get
			{
				return this._From;
			}
			set
			{
				this._From = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
