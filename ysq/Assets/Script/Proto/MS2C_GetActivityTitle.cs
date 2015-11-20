using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetActivityTitle")]
	[Serializable]
	public class MS2C_GetActivityTitle : IExtensible
	{
		private int _Result;

		private readonly List<int> _Type = new List<int>();

		private readonly List<string> _Title = new List<string>();

		private int _Version;

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

		[ProtoMember(2, Name = "Type", DataFormat = DataFormat.TwosComplement)]
		public List<int> Type
		{
			get
			{
				return this._Type;
			}
		}

		[ProtoMember(3, Name = "Title", DataFormat = DataFormat.Default)]
		public List<string> Title
		{
			get
			{
				return this._Title;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
