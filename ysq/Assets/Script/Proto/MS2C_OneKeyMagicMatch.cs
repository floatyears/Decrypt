using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_OneKeyMagicMatch")]
	[Serializable]
	public class MS2C_OneKeyMagicMatch : IExtensible
	{
		private int _Result;

		private readonly List<int> _SelfMagicType = new List<int>();

		private readonly List<int> _TargetMagicType = new List<int>();

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

		[ProtoMember(2, Name = "SelfMagicType", DataFormat = DataFormat.TwosComplement)]
		public List<int> SelfMagicType
		{
			get
			{
				return this._SelfMagicType;
			}
		}

		[ProtoMember(3, Name = "TargetMagicType", DataFormat = DataFormat.TwosComplement)]
		public List<int> TargetMagicType
		{
			get
			{
				return this._TargetMagicType;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
