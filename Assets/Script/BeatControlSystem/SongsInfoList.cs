using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;



namespace DIM{
	namespace BeatsMusicSystem{

		public class SongsInfoList : ScriptableObject {


			public SongsInfo[] SongList;

			[System.Serializable]
			public class SongsInfo {

				public string songName;
				public float bpm;
				public string ownSceneName;
				public SongsPlayMode playMode;
				public SegmentInfo[] segmentList;


				[System.Serializable]
				public class SegmentInfo{
					public string segmentName;
					public float segmentLengh;
					public AudioMixerSnapshot mixerSnapshot;
					public float fadeInTime = 0.0f;
					public float fadeOutTime = 0.0f;

					public MusicClipData[] clipData;
					public List<AudioSource> clipSource;

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
				}

			}
		}

	}
}




