using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;


namespace DIM {
	namespace MusicControl {
		//****總體時間進行記錄****//
		[System.Serializable]
		public class TotalTimeData{
			public double StartTime;
			public double totalTime;

			public void InitializationSetup(){
				StartTime = AudioSettings.dspTime;
			}
			public double GetTotalTime () {
				totalTime = AudioSettings.dspTime - StartTime;
				return totalTime;
			} 
		}


		//****計算節拍進行到何處的所有資料****//
		[System.Serializable]
		public class BeatsData{

			public float bpm;			//歌曲速度
			public float beatsPerBar = 4;	//幾小節為一拍
			public double beatLength;	//四分音符時間長度
			
			public double totalTime = 0;	//自播放開始經過幾秒
			public double currBeatStartTime;	//當前拍點時間
			public double nextBeatStartTime;	//下一拍時間
			public int currSection = 1;		//進行到第幾小節
			public int currBeat = 1;		//進行到第幾拍
			
			public double beatsLength_wholeNote;		//全音符時間長度
			public double beatsLength_halfNote;			//半音符時間長度
			public double beatsLength_4Note;			//四分音符時間長度
			public double beatsLength_8Note;			//八分音符時間長度
			public double beatsLength_16Note;			//16分衣服時間長度
			public double beatsLength_32Note;			//32分音符時間長度

			//****初始化基本數值，藉由BPM計算後續所需要用的素質****//
			public void InitializationSetup () {
				beatLength = 60.0f / bpm;
				currBeat = 0;
				currSection = 1;
				beatsLength_wholeNote = beatLength * 4;
				beatsLength_halfNote = beatLength * 2;
				beatsLength_4Note = beatLength;
				beatsLength_8Note = beatLength * 0.5;
				beatsLength_16Note = beatLength * 0.25;
				beatsLength_32Note = beatLength * 0.125;
				totalTime = 0;
				currBeatStartTime = 0;
				nextBeatStartTime += beatLength;
			}

			//****計算下一拍點時間****//
			public void GetNextBeatStartTime () {
				nextBeatStartTime = currBeatStartTime + beatLength;
			}

			//****計算當前進行到第幾小節第幾拍****//
			public void SetCurrSectionAndBeat () {
				if(currBeat < beatsPerBar){
					currBeat += 1;
				}else if(currBeat == beatsPerBar){
					currSection += 1;
					currBeat = 1;
				}
			}

		}

		//****計算當前節拍下所應該對應到的輸入拍點、判定範圍等相關資訊****//
		[System.Serializable]
		public class DeterminationData {

			[System.Serializable]
			public class DeterminationModeInfo {
				public string modeName;
				public int availableBeatDegree;	//有效音符底線，如果為八分音符，則精準打擊八分音符後半拍則觸發打擊失敗
				public float rangeLength;	// 判定範圍，幾分音符的長度為單位
				public double perfectTiming;	//本次有效音符最完美時間點
				public int perfectTimingPercentage;		//完美判定範圍百分比
				public int greatTimingPercentage;		//佳判定範圍百分比
				public int goodTimingPercentage;		//尚可判定範圍百分比
				public int failTimingPercentage;		//失敗判定範圍百分比currModeInfo
				public double maximumRange;	//最佳時間和當前時間相減取絕對後最大值將與此數值比較，超出即代表該次打擊失敗，不過還是要計算精準度後判定是否成功
				public double? lastInput_pTime;
				public bool isInputAvailable;
			}

			[System.Serializable]
			public class DetectionDetail {
				public float deviationTime;
				public float deviationTimeFix;
				public int accuratePercentage;
				public double currPerfectTime;
				public double currTime;
				public double currMaximumRange;
				public bool isInputAvailable;
			}

			public EBeatsDeterminationMode currMode;
			public DeterminationModeInfo[] modeInfo = new DeterminationModeInfo[4];
			public DetectionDetail detail;
			public float globalDeviation = 0.0f;


			public void InitializationSetup(double _beatLength){
				modeInfo[0].modeName = "Half Note Mode";
				modeInfo[0].availableBeatDegree = 2;
				modeInfo[0].rangeLength = 4;
				modeInfo[0].perfectTiming = 0;
				modeInfo[0].perfectTimingPercentage = 60;
				modeInfo[0].greatTimingPercentage = 40;
				modeInfo[0].goodTimingPercentage = 20;
				modeInfo[0].failTimingPercentage = -200;
				modeInfo[0].maximumRange = _beatLength / 2;
				modeInfo[0].lastInput_pTime = null;

				modeInfo[1].modeName = "Quarter Note Mode";
				modeInfo[1].availableBeatDegree = 4;
				modeInfo[1].rangeLength = 4;
				modeInfo[1].perfectTiming = 0;
				modeInfo[1].perfectTimingPercentage = 50;
				modeInfo[1].greatTimingPercentage = 40;
				modeInfo[1].goodTimingPercentage = 30;
				modeInfo[1].failTimingPercentage = -200;
				modeInfo[1].maximumRange = _beatLength / 2;
				modeInfo[1].lastInput_pTime = null;

				modeInfo[2].modeName = "Eighth Note Mode";
				modeInfo[2].availableBeatDegree = 8;
				modeInfo[2].rangeLength = 8;
				modeInfo[2].perfectTiming = 0;
				modeInfo[2].perfectTimingPercentage = 70;
				modeInfo[2].greatTimingPercentage = 50;
				modeInfo[2].goodTimingPercentage = 30;
				modeInfo[2].failTimingPercentage = -200;
				modeInfo[2].maximumRange = _beatLength / 2;
				modeInfo[2].lastInput_pTime = null;

				modeInfo[3].modeName = "Sixteenth Note Mode";
				modeInfo[3].availableBeatDegree = 16;
				modeInfo[3].rangeLength = 16;
				modeInfo[3].perfectTiming = 0;
				modeInfo[3].perfectTimingPercentage = 40;
				modeInfo[3].greatTimingPercentage = 10;
				modeInfo[3].goodTimingPercentage = 0;
				modeInfo[3].failTimingPercentage = -200;
				modeInfo[3].maximumRange = _beatLength / 2;
				modeInfo[3].lastInput_pTime = null;
			}

			public void SwitchDeterminationMode (EBeatsDeterminationMode _mode) {
				switch(_mode){
				case EBeatsDeterminationMode.HalfNote:
					currMode = EBeatsDeterminationMode.HalfNote;
					break;
				case EBeatsDeterminationMode.QuarterNote:
					currMode = EBeatsDeterminationMode.QuarterNote;
					break;
				case EBeatsDeterminationMode.EighthNote:
					currMode = EBeatsDeterminationMode.EighthNote;
					break;
				case EBeatsDeterminationMode.SixteenthNote: 
					currMode = EBeatsDeterminationMode.SixteenthNote;
					break;
				}
			}
		
			public void PerfectTimingCalculate (BeatsData _bData) {
				switch(currMode){
				case EBeatsDeterminationMode.HalfNote:
					if(_bData.currBeat == 1 || _bData.currBeat == 3){
						if(_bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_4Note){
							modeInfo[0].perfectTiming = _bData.currBeatStartTime;
						}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_4Note){
							modeInfo[0].perfectTiming = _bData.nextBeatStartTime + _bData.beatsLength_4Note;
						}
					}else if(_bData.currBeat == 2 || _bData.currBeat == 4){
						if(_bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_4Note){
							modeInfo[0].perfectTiming = _bData.nextBeatStartTime;
						}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_4Note){
							modeInfo[0].perfectTiming = _bData.nextBeatStartTime;
						}
					}
					break;
				case EBeatsDeterminationMode.QuarterNote:
					if(_bData.totalTime <= _bData.currBeatStartTime + (_bData.beatsLength_8Note)){
						modeInfo[1].perfectTiming = _bData.currBeatStartTime;
					}else if(_bData.totalTime > _bData.currBeatStartTime + (_bData.beatsLength_8Note)){
						modeInfo[1].perfectTiming = _bData.nextBeatStartTime;
					}
					break;
				case EBeatsDeterminationMode.EighthNote:
					if(_bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_16Note){
						modeInfo[2].perfectTiming = _bData.currBeatStartTime;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_16Note && _bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_8Note){
						modeInfo[2].perfectTiming = _bData.currBeatStartTime + _bData.beatsLength_8Note;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_8Note && _bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_16Note){
						modeInfo[2].perfectTiming = _bData.currBeatStartTime + _bData.beatsLength_8Note;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_16Note && _bData.totalTime < _bData.nextBeatStartTime){
						modeInfo[2].perfectTiming = _bData.nextBeatStartTime;
					}
					break;
				case EBeatsDeterminationMode.SixteenthNote:
					if(_bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_32Note){
						modeInfo[3].perfectTiming = _bData.currBeatStartTime;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_32Note && _bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_16Note){
						modeInfo[3].perfectTiming = _bData.currBeatStartTime + _bData.beatsLength_16Note;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_16Note && _bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_16Note + _bData.beatsLength_32Note){
						modeInfo[3].perfectTiming = _bData.currBeatStartTime + _bData.beatsLength_16Note;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_16Note + _bData.beatsLength_32Note && _bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_8Note){
						modeInfo[3].perfectTiming = _bData.currBeatStartTime + _bData.beatsLength_8Note;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_8Note && _bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_32Note){
						modeInfo[3].perfectTiming = _bData.currBeatStartTime + _bData.beatsLength_8Note;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_32Note && _bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_16Note){
						modeInfo[3].perfectTiming = _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_16Note;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_16Note && _bData.totalTime <= _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_16Note + _bData.beatsLength_32Note){
						modeInfo[3].perfectTiming = _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_16Note;
					}else if(_bData.totalTime > _bData.currBeatStartTime + _bData.beatsLength_8Note + _bData.beatsLength_16Note + _bData.beatsLength_32Note && _bData.totalTime < _bData.nextBeatStartTime){
						modeInfo[3].perfectTiming = _bData.nextBeatStartTime;
					}
					break;
				}
			}

			int GetAccuratePercentage (float _tTime,float _pTime,float _maximumRange){
				float fix_tTime = _tTime - globalDeviation;
				float deviationTime = Mathf.Abs(fix_tTime - _pTime);

				int accuratePercentage = (int)(100 - (deviationTime / (_maximumRange / 100)));
				DetalRecord(_tTime - _pTime, fix_tTime - _pTime, accuratePercentage, _pTime, _tTime, _maximumRange);
				return accuratePercentage;
			}

			void InputAvailableCalaulate (BeatsData _bData) {
				switch(currMode){
				case EBeatsDeterminationMode.HalfNote:
					if(modeInfo[0].lastInput_pTime == null || modeInfo[0].perfectTiming > modeInfo[0].lastInput_pTime + _bData.beatsLength_4Note){
						modeInfo[0].lastInput_pTime = modeInfo[0].perfectTiming;
						modeInfo[0].isInputAvailable = true;
						detail.isInputAvailable = true;
					}else{
						modeInfo[0].isInputAvailable = false;
						detail.isInputAvailable = false;
					}
					break;
				case EBeatsDeterminationMode.QuarterNote:
					if(modeInfo[1].lastInput_pTime == null || modeInfo[1].perfectTiming > modeInfo[1].lastInput_pTime + _bData.beatsLength_8Note){
						modeInfo[1].lastInput_pTime = modeInfo[1].perfectTiming;
						modeInfo[1].isInputAvailable = true;
						detail.isInputAvailable = true;
					}else{
						modeInfo[1].isInputAvailable = false;
						detail.isInputAvailable = false;
					}
					break;
				case EBeatsDeterminationMode.EighthNote:
					if(modeInfo[2].lastInput_pTime == null || modeInfo[2].perfectTiming > modeInfo[2].lastInput_pTime + _bData.beatsLength_16Note){
						modeInfo[2].lastInput_pTime = modeInfo[2].perfectTiming;
						modeInfo[2].isInputAvailable = true;
						detail.isInputAvailable = true;
					}else{
						modeInfo[2].isInputAvailable = false;
						detail.isInputAvailable = false;
					}
					break;
				case EBeatsDeterminationMode.SixteenthNote:
					if(modeInfo[3].lastInput_pTime == null || modeInfo[3].perfectTiming > modeInfo[3].lastInput_pTime + _bData.beatsLength_32Note){
						modeInfo[3].lastInput_pTime = modeInfo[3].perfectTiming;
						modeInfo[3].isInputAvailable = true;
						detail.isInputAvailable = true;
					}else{
						modeInfo[3].isInputAvailable = false;
						detail.isInputAvailable = false;
					}
					break;
				}
			}



			public void InputResultCalculate (BeatsData _bData,IBeatsInputCalculate _callSourceScript) {
//				PerfectTimingCalculate(_bData);
				InputAvailableCalaulate(_bData);

				int accuratePercentage;
				switch(currMode){
				case EBeatsDeterminationMode.HalfNote:
					accuratePercentage = GetAccuratePercentage ((float)_bData.totalTime, (float)modeInfo[0].perfectTiming, (float)modeInfo[0].maximumRange);
					InputResultsOutput(accuratePercentage,_callSourceScript);
					break;
				case EBeatsDeterminationMode.QuarterNote:
					accuratePercentage = GetAccuratePercentage ((float)_bData.totalTime, (float)modeInfo[1].perfectTiming, (float)modeInfo[1].maximumRange);
					InputResultsOutput(accuratePercentage,_callSourceScript);
					break;
				case EBeatsDeterminationMode.EighthNote:
					accuratePercentage = GetAccuratePercentage ((float)_bData.totalTime, (float)modeInfo[2].perfectTiming, (float)modeInfo[2].maximumRange);
					InputResultsOutput(accuratePercentage,_callSourceScript);
					break;
				case EBeatsDeterminationMode.SixteenthNote:
					accuratePercentage = GetAccuratePercentage ((float)_bData.totalTime, (float)modeInfo[3].perfectTiming, (float)modeInfo[3].maximumRange);
					InputResultsOutput(accuratePercentage,_callSourceScript);
					break;
				}
			}

			
			void InputResultsOutput (int _accuratePercentage,IBeatsInputCalculate _callSourceScript){
				
				_callSourceScript.PrintInputResults(detail);

				if(detail.isInputAvailable == true){
					switch(currMode){
					case EBeatsDeterminationMode.HalfNote:
						if(_accuratePercentage >= modeInfo[0].perfectTimingPercentage){
							_callSourceScript.PerfectResultsOutput();
						}else if(_accuratePercentage < modeInfo[0].perfectTimingPercentage && _accuratePercentage >= modeInfo[0].greatTimingPercentage){
							_callSourceScript.GreatResultsOutput();
						}else if(_accuratePercentage < modeInfo[0].greatTimingPercentage && _accuratePercentage >= modeInfo[0].goodTimingPercentage){
							_callSourceScript.GoodResultsOutput();
						}else if(_accuratePercentage < modeInfo[0].goodTimingPercentage && _accuratePercentage >= modeInfo[0].failTimingPercentage){
							_callSourceScript.FailResultsOutput();
						}
						break;
					case EBeatsDeterminationMode.QuarterNote:
						if(_accuratePercentage >= modeInfo[1].perfectTimingPercentage){
							_callSourceScript.PerfectResultsOutput();
						}else if(_accuratePercentage < modeInfo[1].perfectTimingPercentage && _accuratePercentage >= modeInfo[1].greatTimingPercentage){
							_callSourceScript.GreatResultsOutput();
						}else if(_accuratePercentage < modeInfo[1].greatTimingPercentage && _accuratePercentage >= modeInfo[1].goodTimingPercentage){
							_callSourceScript.GoodResultsOutput();
						}else if(_accuratePercentage < modeInfo[1].goodTimingPercentage && _accuratePercentage >= modeInfo[1].failTimingPercentage){
							_callSourceScript.FailResultsOutput();
						}
						break;
					case EBeatsDeterminationMode.EighthNote:
						if(_accuratePercentage >= modeInfo[2].perfectTimingPercentage){
							_callSourceScript.PerfectResultsOutput();
						}else if(_accuratePercentage < modeInfo[2].perfectTimingPercentage && _accuratePercentage >= modeInfo[2].greatTimingPercentage){
							_callSourceScript.GreatResultsOutput();
						}else if(_accuratePercentage < modeInfo[2].greatTimingPercentage && _accuratePercentage >= modeInfo[2].goodTimingPercentage){
							_callSourceScript.GoodResultsOutput();
						}else if(_accuratePercentage < modeInfo[2].goodTimingPercentage && _accuratePercentage >= modeInfo[2].failTimingPercentage){
							_callSourceScript.FailResultsOutput();
						}
						break;
					case EBeatsDeterminationMode.SixteenthNote:
						if(_accuratePercentage >= modeInfo[3].perfectTimingPercentage){
							_callSourceScript.PerfectResultsOutput();
						}else if(_accuratePercentage < modeInfo[3].perfectTimingPercentage && _accuratePercentage >= modeInfo[3].greatTimingPercentage){
							_callSourceScript.GreatResultsOutput();
						}else if(_accuratePercentage < modeInfo[3].greatTimingPercentage && _accuratePercentage >= modeInfo[3].goodTimingPercentage){
							_callSourceScript.GoodResultsOutput();
						}else if(_accuratePercentage < modeInfo[3].goodTimingPercentage && _accuratePercentage >= modeInfo[3].failTimingPercentage){
							_callSourceScript.FailResultsOutput();
						}
						break;
					}
				}else if(detail.isInputAvailable == false){
					_callSourceScript.FailResultsOutput();
				}

			}


			public void DetalRecord (float _deviationTime, float _deviationTimeFix, int _accuratePercentage, double _currPerfectTime, double _currTime, double _currMaximumRange){
				detail.deviationTime = _deviationTime;
				detail.deviationTimeFix = _deviationTimeFix;
				detail.accuratePercentage = _accuratePercentage;
				detail.currPerfectTime = _currPerfectTime;
				detail.currTime = _currTime;
				detail.currMaximumRange = _currMaximumRange;
			}

		}

		//****紀錄樂曲段落相關資訊****//
		[System.Serializable]
		public class MainMusicInfo{
			[System.Serializable]
			public class MusicSegmentData{
				[System.Serializable]
				public class MusicClipData{
					public string clipName;
					public AudioClip clips;
					public AudioMixerGroup mixerGroup;
					[Range(0.0f, 1.0f)] public float volume = 1.0f;
					[Range(-1.0f, 1.0f)] public float pan = 0.0f;
					[Range(0.0f, 1.0f)] public float reverbZoneMix = 0.0f;
					public float startTime = 0.0f;
				}
				public string segmentName;
				public float segmentLengh;
				public int repetitions;
				public AudioMixerSnapshot mixerSnapshot;
				public float fadeInTime = 0.0f;
				public float fadeOutTime = 0.0f;
				public MusicClipData[] clipData;
				public List<AudioSource> clipSource;
			}

			public List<List<AudioSource>> segmentSource;
			public MusicSegmentData[] segmentData;


			public void InitializationSetup (Transform _ThisGameObject) {
				GameObject mainMusicSource = new GameObject("mainMusicSource");
				mainMusicSource.transform.SetParent(_ThisGameObject);
				segmentSource = new List<List<AudioSource>>();
				int degmentCount = 0;
				
				foreach(var _Segment in segmentData){
					GameObject segmentMusicSource = new GameObject(_Segment.segmentName);
					segmentMusicSource.transform.SetParent(mainMusicSource.transform);
					segmentData[degmentCount].clipSource = new List<AudioSource>();
					segmentSource.Add(segmentData[degmentCount].clipSource);
					int clipCount = 0;
					
					foreach(var _Clips in _Segment.clipData){
						GameObject clipsMusicSource = new GameObject(_Clips.clipName);
						clipsMusicSource.transform.SetParent(segmentMusicSource.transform);
						AudioSource s =  clipsMusicSource.AddComponent<AudioSource>();
						s.clip = _Clips.clips;
						s.outputAudioMixerGroup = _Clips.mixerGroup;
						s.volume = _Clips.volume;
						s.panStereo = _Clips.pan;
						s.reverbZoneMix = _Clips.reverbZoneMix;
						s.spatialBlend = 0.0f;
						s.dopplerLevel = 0.0f;
						s.playOnAwake = false;
						segmentData[degmentCount].clipSource.Add(s);
						clipCount ++;
					}
					degmentCount ++;
				}
			}
		}

		[System.Serializable]
		public class MusicPlayback{
			public double totalTime;
			public double beatLength;	//四分音符時間長度
			public double currBeatStartTime;	//當前拍點時間
			public double nextBeatStartTime;	//下一拍時間
			
			public double currSegmentStartTime;
			public double nextSegmentStartTime;
			public DIM.MusicControl.MainMusicInfo.MusicSegmentData currSegmentData;
			public DIM.MusicControl.MainMusicInfo.MusicSegmentData nextSegmentData;
			public int currSegmentNumber;
			public bool isLoadNextSegment;
			
			private float beatsPerBar = 4;	//幾小節為一拍
			private int currSection = 1;		//進行到第幾小節
			private int currBeat = 1;		//進行到第幾拍
			

			public void InitializationSetup(MainMusicInfo _MusicInfo){
				currSegmentData = _MusicInfo.segmentData[0];
				nextSegmentData = _MusicInfo.segmentData[0];
				currSegmentStartTime = 0;
				isLoadNextSegment = false;
			}

			public void StartPlaying (BeatsData _BeatsData, MainMusicInfo _MusicInfo) {
				VariableUpdate(_BeatsData);
				foreach(AudioSource a in currSegmentData.clipSource){
					a.Play();
				}
				SetNextSegment(_MusicInfo);
				SetNextSegmentStartTime(0, beatLength, currSegmentData.segmentLengh);
			}
			
			public void PlayingFunction (BeatsData _BeatsData, MainMusicInfo _MusicInfo) {
				VariableUpdate(_BeatsData);
//				if(totalTime > nextSegmentStartTime - 2f){
//					if(isLoadNextSegment == false){
//						PlayNextSegment();
//						isLoadNextSegment = true;
//					}
					if(totalTime >= nextSegmentStartTime){
						currSegmentStartTime = nextSegmentStartTime;
						currSegmentData = nextSegmentData;
						PlayNextSegment();
						SetNextSegment(_MusicInfo);
						SetNextSegmentStartTime(currBeatStartTime, beatLength, currSegmentData.segmentLengh);
						isLoadNextSegment = false;
					}
//				}
			}
			

			public void VariableUpdate (BeatsData _BeatsData) {
				totalTime = _BeatsData.totalTime;
				beatLength = _BeatsData.beatLength;
				beatsPerBar = _BeatsData.beatsPerBar;
				currBeatStartTime = _BeatsData.currBeatStartTime;
				nextBeatStartTime = _BeatsData.nextBeatStartTime;
				currSection = _BeatsData.currSection;
				currBeat = _BeatsData.currBeat;
			}
			
			public void PlayNextSegment () {
				foreach(AudioSource _clipData in nextSegmentData.clipSource){
//					_clipData.PlayScheduled(nextSegmentStartTime);
					_clipData.Play();
				}
			}
			
			public void SetNextSegment(MainMusicInfo _MusicInfo) {
				if(currSegmentNumber+1 < _MusicInfo.segmentData.Length){
					nextSegmentData = _MusicInfo.segmentData[currSegmentNumber+1];
					currSegmentNumber ++;
				}else if(currSegmentNumber+1 == _MusicInfo.segmentData.Length){
					nextSegmentData = _MusicInfo.segmentData[currSegmentNumber];
				}
			}
			
			public void SetNextSegmentStartTime (double _currStartTime, double _beatLengh, float _segmentLengh) {
				nextSegmentStartTime = _currStartTime + (_segmentLengh * _beatLengh);
			}
			
			
			
			//				public void RepetitionCalculate () {
			//
			//				}
			
			
			
			public void MusicOver () {
				Debug.Log("Over");
			}
			
			
			
			
			
			
			
			
			
			
			
		}






















	}

}
