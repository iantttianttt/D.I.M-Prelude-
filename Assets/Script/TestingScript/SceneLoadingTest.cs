using UnityEngine;
using System.Collections;
using DIM.LevelLoadingSystem;


public class SceneLoadingTest : MonoBehaviour {

	void OnGUI(){
		if(GUILayout.Button("Load Intro scene")) SceneLoader.ins.LoadLevel(SceneLoader.Scenes.Intro);
		if(GUILayout.Button("Load Menu scene")) SceneLoader.ins.LoadLevel(SceneLoader.Scenes.Menu);
		if(GUILayout.Button("Load ShowcaseLevel scene")) SceneLoader.ins.LoadLevel(SceneLoader.Scenes.ShowcaseLevel);
		if(GUILayout.Button("Load UnlimitedLevel scene")) SceneLoader.ins.LoadLevel(SceneLoader.Scenes.UnlimitedLevel);

		GUILayout.Box(GameSceneManager.ins.Progress.ToString());
	}
}