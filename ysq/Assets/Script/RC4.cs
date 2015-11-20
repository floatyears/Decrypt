using System;

public class RC4
{
	private readonly byte[] state;

	private byte x;

	private byte y;

	public RC4(byte[] key)
	{
		this.state = new byte[256];
		this.x = (this.y = 0);
		this.KeySetup(key);
	}

	public int Process(byte[] buffer, int start, int count)
	{
		return this.InternalTransformBlock(buffer, start, count, buffer, start);
	}

	private void KeySetup(byte[] key)
	{
		byte b = 0;
		byte b2 = 0;
		for (int i = 0; i < 256; i++)
		{
			this.state[i] = (byte)i;
		}
		this.x = 0;
		this.y = 0;
		for (int j = 0; j < 256; j++)
		{
			b2 = (byte)(key[(int)b] + this.state[j] + b2);
			byte b3 = this.state[j];
			this.state[j] = this.state[(int)b2];
			this.state[(int)b2] = b3;
			b = (byte)((int)(b + 1) % key.Length);
		}
	}

	private int InternalTransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		for (int i = 0; i < inputCount; i++)
		{
			this.x += 1;
			this.y = (byte)(this.state[(int)this.x] + this.y);
			byte b = this.state[(int)this.x];
			this.state[(int)this.x] = this.state[(int)this.y];
			this.state[(int)this.y] = b;
			byte b2 = (byte)(this.state[(int)this.x] + this.state[(int)this.y]);
			outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ this.state[(int)b2]);
		}
		return inputCount;
	}
}
