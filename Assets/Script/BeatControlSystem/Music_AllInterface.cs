

public interface IBeatsDataCalculate{
	void OnBeatsFunction();
	void StartPlaying_BeatsComputing();
	void IsPlaying_BeatsComputing();
}

public interface IBeatsInputCalculate{
	void PerfectResultsOutput();
	void GreatResultsOutput();
	void GoodResultsOutput();
	void FailResultsOutput();
	void PrintInputResults(DIM.MusicControl.DeterminationData.DetectionDetail _detail);
}

public interface IMusicPlayback{

}
