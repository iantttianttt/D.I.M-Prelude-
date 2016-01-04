using UnityEngine;
using System.Collections;

namespace DIM {
	namespace UIControl {
		
		[System.Serializable]
		public class VerticalDisplayer{

			[System.Serializable]
			public class BeatsData{
				[System.Serializable]
				public class QuarterNote{
					public int sections;
					public float beats;
					public Animator beatAnim;
					public bool isUsing;
				}
				[System.Serializable]
				public class EighthNote{
					public int sections;
					public float beats;
					public Animator beatAnim;
					public bool isUsing;
				}

				public QuarterNote QuarterData;
				public EighthNote EighthData;
			}

			public GameObject[] beatsObj_4note = new GameObject[6];
			public GameObject[] beatsObj_8note = new GameObject[6];
			public BeatsData[] beatsData = new BeatsData[6];



			public void InitializationSetup(){
				for(int i = 0; i<beatsData.Length; i++){
					beatsData[i].QuarterData.beatAnim = beatsObj_4note[i].GetComponent<Animator>();
					beatsData[i].EighthData.beatAnim = beatsObj_8note[i].GetComponent<Animator>();
				}
			}

			public float GetAnimationSpeed(float _bpm){
				float speed = _bpm/120;
				return speed;
			}


			public void PlayQuarterNote (int _sections, float _beats, float _bpm) {
				foreach(var a in beatsData){
					if(a.QuarterData.isUsing == true){
						continue;
					}else{
						a.QuarterData.beatAnim.SetTrigger("StartTheBeats");
						a.QuarterData.beatAnim.speed = GetAnimationSpeed(_bpm);
						a.QuarterData.isUsing = true;
						a.QuarterData.sections = _sections;
						a.QuarterData.beats = _beats;
						return;
					}
				}
			}

			public void PlayEighthNote (int _sections, float _beats, float _bpm) {
				foreach(var a in beatsData){
					if(a.EighthData.isUsing == true){
						continue;
					}else{
						a.EighthData.beatAnim.SetTrigger("StartTheBeats");
						a.EighthData.beatAnim.speed = GetAnimationSpeed(_bpm);
						a.EighthData.isUsing = true;
						a.EighthData.sections = _sections;
						a.EighthData.beats = _beats;
						return;
					}
				}
			}
		}
	}
}
