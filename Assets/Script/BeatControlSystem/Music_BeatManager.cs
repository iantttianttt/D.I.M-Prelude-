
using UnityEngine;
using System.Collections;
using DIM.MusicControl;

public class Music_BeatManager : MonoBehaviour,IBeatsDataCalculate,IBeatsInputCalculate {

	public TotalTimeData totalTimeData;
	public BeatsData beatsData;
	public DeterminationData determinationData;
	public bool isBeatsComputing;

	public AudioSource beatsTestSound;

	public Music_SegmentManager segmentManager;
	public UI_BeatDisplayer beatsDisplayer;
	public GameManager gameManager;

	void Start () {
		segmentManager = GameObject.Find("Music_SegmentManager").GetComponent<Music_SegmentManager>();
		beatsDisplayer = GameObject.Find("UI_BeatDisplayer").GetComponent<UI_BeatDisplayer>();
		gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
	}

	void Update () {
		determinationData.PerfectTimingCalculate(beatsData);
		if(isBeatsComputing == true){
			IsPlaying_BeatsComputing();
		}


		if(Input.GetKeyDown(KeyCode.Space)){
			GetInput();
		}
		if(Input.GetKeyDown(KeyCode.F)){
			GetInput();
		}
		if(Input.GetKeyDown(KeyCode.J)){
			GetInput();
		}
	}


	#region BeatsData Computing Function
	public void StartPlaying_BeatsComputing () {
		isBeatsComputing = true;
		totalTimeData.InitializationSetup();
		beatsData.InitializationSetup();
		determinationData.InitializationSetup(beatsData.beatLength);
		OnBeatsFunction();
		segmentManager.playback.StartPlaying(beatsData, segmentManager.musicInfo);
	}
	
	public void IsPlaying_BeatsComputing () {
		beatsData.totalTime = totalTimeData.GetTotalTime();
		segmentManager.playback.PlayingFunction(beatsData, segmentManager.musicInfo);
		if(beatsData.totalTime-beatsData.currBeatStartTime >= beatsData.beatLength){
			OnBeatsFunction();
			beatsData.currBeatStartTime = beatsData.nextBeatStartTime;
			beatsData.GetNextBeatStartTime();
		}
	}

	public void OnBeatsFunction () {
//		Debug.Log("On Beat!!");
		beatsData.SetCurrSectionAndBeat();
//		beatsTestSound.Play();
		beatsDisplayer.OnBeats();
	}
	#endregion




	#region Beats Input DeterminationData Computing

	public void PerfectResultsOutput(){
		gameManager.score += 100;
		gameManager.Perfect();
#if DEBUG
		Debug.Log("Perfect!!!");
#endif
	}
	public void GreatResultsOutput(){
		gameManager.score += 80;
		gameManager.Great();
#if DEBUG
		Debug.Log("Great!!!");
#endif
	}
	public void GoodResultsOutput(){
		gameManager.score += 50;
		gameManager.Good();
#if DEBUG
		Debug.Log("Good!!!");
#endif
	}
	public void FailResultsOutput(){
		gameManager.Fail();
#if DEBUG
		Debug.Log("Fail!!!");
#endif
	}

	public void PrintInputResults(DeterminationData.DetectionDetail _detail){
#if DEBUG
		if(_detail.isInputAvailable == true){
			Debug.Log("誤差時間 : " + _detail.deviationTime + " ; " + "誤差修正 : " + _detail.deviationTimeFix + " ; " + "命中百分比 : " + _detail.accuratePercentage + "% ; " + "百分比總時長 : " + _detail.currMaximumRange + " ; " + "精準時間 : " + _detail.currPerfectTime + " ; " + "輸入時間 : " + _detail.currTime);
		}else if(_detail.isInputAvailable == false){
			Debug.Log("該拍點已做過打擊，無法再做打擊精準判定");
		}
#endif
	}

	public void GetInput () {
		determinationData.InputResultCalculate(beatsData,this);
	}

	#endregion
}
