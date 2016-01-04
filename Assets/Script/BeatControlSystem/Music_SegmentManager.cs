using UnityEngine;
using System.Collections;
using DIM.MusicControl;

public class Music_SegmentManager : MonoBehaviour {
	
	public MainMusicInfo musicInfo;
	public MusicPlayback playback;


	void Start () {
		musicInfo.InitializationSetup(this.transform);
		playback.InitializationSetup(musicInfo);
	}

	void Update () {

	}
}
