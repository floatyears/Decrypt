using System;

public interface IVoiceRecorder
{
	void Init(string name);

	void Term();

	void StartRecord(string path, int bitDepth, string mrType);

	int StopRecord(bool isCancel);

	bool IsRecording();

	void StartPlay(string path, float volume);

	void StopPlay();

	void SetVoiceVolume(float num);

	bool IsPlaying();

	int GetPowerForRecord();

	float GetPowerForRecordF();
}
