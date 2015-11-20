using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetFamePlayers")]
	[Serializable]
	public class MS2C_GetFamePlayers : IExtensible
	{
		private readonly List<FamePlayerInfo> _Data = new List<FamePlayerInfo>();

		private int _Praise;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<FamePlayerInfo> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Praise", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Praise
		{
			get
			{
				return this._Praise;
			}
			set
			{
				this._Praise = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
