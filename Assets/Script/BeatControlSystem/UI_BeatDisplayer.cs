using UnityEngine;
using System.Collections;
using DIM.UIControl;

public class UI_BeatDisplayer : MonoBehaviour {

	public Music_BeatManager beatsManager;
	public VerticalDisplayer displayer;
	public bool isBeatsComputing;

	public double totalTime;
	public double beatLength;
	public double currBeatStartTime;
	public double AATime;

	public bool isLateBeatsOn;

	void Start () {
		displayer.InitializationSetup();
		beatsManager = GameObject.Find("Music_BeatManager").GetComponent<Music_BeatManager>();
	}
	

	void FixedUpdate () {
		UpdateSynchronize();
		if(isBeatsComputing == true){
			IsPlaying_BeatsComputing();
		}
	}

	public void IsPlaying_BeatsComputing () {
		if(AATime >= beatLength/2 && isLateBeatsOn == false){
			isLateBeatsOn = true;
			displayer.PlayEighthNote(beatsManager.beatsData.currSection, beatsManager.beatsData.currBeat +0.5f , beatsManager.beatsData.bpm);
		}
	}

	public void OnBeats () {
		isLateBeatsOn = false;
		displayer.PlayQuarterNote(beatsManager.beatsData.currSection, beatsManager.beatsData.currBeat, beatsManager.beatsData.bpm);
	}



	public void UpdateSynchronize(){
		totalTime = beatsManager.beatsData.totalTime;
		beatLength = beatsManager.beatsData.beatLength;
		currBeatStartTime = beatsManager.beatsData.currBeatStartTime;
		isBeatsComputing = beatsManager.isBeatsComputing;
		AATime = totalTime - currBeatStartTime;
	}
}
