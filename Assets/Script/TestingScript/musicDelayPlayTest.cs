using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class musicDelayPlayTest : MonoBehaviour {



	public AudioSource tastingAudio;
	public AudioSource tastingAudioB;
	public double nextEventTimeA;
	public double nextEventTimeB;
	public int bpm;
	public double time;
	public double st;


	void Start () {
		st = AudioSettings.dspTime;
	}
	

	void Update () {
		time = AudioSettings.dspTime - st;

		if (time + 1.0F > nextEventTimeA) {
			tastingAudio.PlayScheduled(AudioSettings.dspTime + 1.0f);
			Debug.Log("Scheduled source is to start at time : " + nextEventTimeA);
			nextEventTimeA += ((60.0F / bpm)*16);
		}

		if (time + 1.0F > nextEventTimeB) {
			tastingAudioB.PlayScheduled(AudioSettings.dspTime + 1.0f);
			Debug.Log("Scheduled source is to start at time : " + nextEventTimeB);
			nextEventTimeB += ((60.0F / bpm)*16);
		}
	}
}
