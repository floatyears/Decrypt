using System;
using System.Runtime.InteropServices;

namespace Util
{
	public class ObscuredFloat
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct FloatIntBytesUnion
		{
			[FieldOffset(0)]
			public float f;

			[FieldOffset(0)]
			public int i;

			[FieldOffset(0)]
			public byte b1;

			[FieldOffset(1)]
			public byte b2;

			[FieldOffset(2)]
			public byte b3;

			[FieldOffset(3)]
			public byte b4;
		}

		private int currentCryptoKey;

		private ObscuredFloat.FloatIntBytesUnion hiddenValue;

		public float Value
		{
			get
			{
				ObscuredFloat.FloatIntBytesUnion floatIntBytesUnion = default(ObscuredFloat.FloatIntBytesUnion);
				floatIntBytesUnion.f = this.hiddenValue.f;
				floatIntBytesUnion.i ^= this.currentCryptoKey;
				return floatIntBytesUnion.f;
			}
			set
			{
				this.hiddenValue.f = value;
				this.hiddenValue.i = (this.hiddenValue.i ^ this.currentCryptoKey);
			}
		}

		public ObscuredFloat(float initValue)
		{
			long ticks = DateTime.Now.Ticks;
			Random random = new Random((int)(ticks & (long)(0xfffffffe)) | (int)(ticks >> 32));
			this.currentCryptoKey = random.Next(268435455);
			this.Value = initValue;
		}
	}
}
