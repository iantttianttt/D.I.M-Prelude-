using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int score;
	float timer;
	bool isStart;
	Music_BeatManager beatsManager;
	public Text text;

	Animator perfect;
	Animator great;
	Animator good;
	Animator fail;


	void Start () {
		beatsManager = GameObject.Find("Music_BeatManager").GetComponent<Music_BeatManager>();
		perfect = GameObject.Find("Perfect").GetComponent<Animator>();
		great = GameObject.Find("Great").GetComponent<Animator>();
		good = GameObject.Find("Good").GetComponent<Animator>();
		fail = GameObject.Find("Fail").GetComponent<Animator>();
	}
	

	void Update () {
		if(isStart == false){
			timer += Time.deltaTime;
		}

		if(timer>5f){
			isStart = true;
			beatsManager.StartPlaying_BeatsComputing();
			timer = 0;
		}

		text.text = score.ToString();
	}

	public void Perfect() {
		perfect.Play(0);
	}
	public void Great() {
		great.Play(0);
	}
	public void Good() {
		good.Play(0);
	}
	public void Fail() {
		fail.Play(0);
	}
}
