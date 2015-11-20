using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PveStart")]
	[Serializable]
	public class MC2S_PveStart : IExtensible
	{
		private int _SceneID;

		private int _Value;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "SceneID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneID
		{
			get
			{
				return this._SceneID;
			}
			set
			{
				this._SceneID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
