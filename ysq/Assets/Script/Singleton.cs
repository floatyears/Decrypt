using System;

public class Singleton<T> where T : new()
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (Singleton<T>._instance == null)
			{
				Singleton<T>._instance = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			}
			return Singleton<T>._instance;
		}
	}

	protected Singleton()
	{
		Debug.Assert(Singleton<T>._instance == null, "Singleton can only has one instance.");
	}
}
