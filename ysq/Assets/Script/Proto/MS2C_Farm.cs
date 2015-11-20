using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_Farm")]
	[Serializable]
	public class MS2C_Farm : IExtensible
	{
		private int _Result;

		private readonly List<MS2C_PveResult> _Data = new List<MS2C_PveResult>();

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

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<MS2C_PveResult> Data
		{
			get
			{
				return this._Data;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
