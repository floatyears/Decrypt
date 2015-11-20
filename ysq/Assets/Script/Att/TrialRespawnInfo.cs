using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "TrialRespawnInfo")]
	[Serializable]
	public class TrialRespawnInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _InfoID = new List<int>();

		private readonly List<float> _Delay = new List<float>();

		private readonly List<float> _Scale = new List<float>();

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

		[ProtoMember(2, Name = "InfoID", DataFormat = DataFormat.TwosComplement)]
		public List<int> InfoID
		{
			get
			{
				return this._InfoID;
			}
		}

		[ProtoMember(3, Name = "Delay", DataFormat = DataFormat.FixedSize)]
		public List<float> Delay
		{
			get
			{
				return this._Delay;
			}
		}

		[ProtoMember(4, Name = "Scale", DataFormat = DataFormat.FixedSize)]
		public List<float> Scale
		{
			get
			{
				return this._Scale;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
