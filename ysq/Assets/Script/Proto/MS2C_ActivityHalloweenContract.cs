using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityHalloweenContract")]
	[Serializable]
	public class MS2C_ActivityHalloweenContract : IExtensible
	{
		private int _Result;

		private readonly List<HalloweenContract> _Contracts = new List<HalloweenContract>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, Name = "Contracts", DataFormat = DataFormat.Default)]
		public List<HalloweenContract> Contracts
		{
			get
			{
				return this._Contracts;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
