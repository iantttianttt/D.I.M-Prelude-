using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using DIM.LevelLoadingSystem;
using DIM.BeatsMusicSystem;

public class CreateHolder : MonoBehaviour{

	[MenuItem("Custom Editor/Create Scene List")]
	static void CreateSceneList(){

		//資料 Asset 路徑
		string holderAssetPath = "Assets/Settings/";

		if(!Directory.Exists(holderAssetPath)) Directory.CreateDirectory(holderAssetPath);

		//建立實體
		SceneNameList holder = ScriptableObject.CreateInstance<SceneNameList> ();

		//使用 holder 建立名為 dataHolder.asset 的資源
		AssetDatabase.CreateAsset(holder, holderAssetPath + "SceneList.asset");
	}


	[MenuItem("Custom Editor/Create Songs List")]
	static void CreateSongsList(){

		//資料 Asset 路徑
		string holderAssetPath = "Assets/Settings/";

		if(!Directory.Exists(holderAssetPath)) Directory.CreateDirectory(holderAssetPath);

		//建立實體
		SongsInfoList holder = ScriptableObject.CreateInstance<SongsInfoList> ();

		//使用 holder 建立名為 dataHolder.asset 的資源
		AssetDatabase.CreateAsset(holder, holderAssetPath + "SongsList.asset");
	}
}
