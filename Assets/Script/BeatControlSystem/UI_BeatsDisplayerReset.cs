using UnityEngine;
using System.Collections;
using DIM.UIControl;

public class UI_BeatsDisplayerReset : MonoBehaviour {

	public enum BeatType{
		QuarterNote,
		EighthNote,
	}

	public BeatType beatType;
	public int arrayNumber;
	UI_BeatDisplayer displayer;

	void Start () {
		displayer = GameObject.Find("UI_BeatDisplayer").GetComponent<UI_BeatDisplayer>();
	}
	

	void Update () {
	
	}

	public void Reset() {
		if(beatType == BeatType.QuarterNote){
			displayer.displayer.beatsData[arrayNumber].QuarterData.isUsing = false;
		}else if(beatType == BeatType.EighthNote){
			displayer.displayer.beatsData[arrayNumber].EighthData.isUsing = false;
		}

	}
}
