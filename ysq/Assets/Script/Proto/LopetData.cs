using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "LopetData")]
	[Serializable]
	public class LopetData : IExtensible
	{
		private ulong _ID;

		private int _InfoID;

		private uint _Exp;

		private uint _Level;

		private uint _Awake;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ID
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

		[ProtoMember(2, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Exp
		{
			get
			{
				return this._Exp;
			}
			set
			{
				this._Exp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Awake", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Awake
		{
			get
			{
				return this._Awake;
			}
			set
			{
				this._Awake = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
