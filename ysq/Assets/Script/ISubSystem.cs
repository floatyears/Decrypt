using System;

public interface ISubSystem
{
	void Init();

	void Update(float elapse);

	void Destroy();
}
