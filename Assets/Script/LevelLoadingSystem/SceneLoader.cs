using UnityEngine;
using System.Collections;

namespace DIM {
	namespace LevelLoadingSystem {
		
		public class SceneLoader : MonoBehaviour {

			public enum Scenes{
				Intro = 0,
				Menu,
				ShowcaseLevel,
				UnlimitedLevel,
				Level_01,
				Level_02,
				Level_03,
				Level_04,
				Level_05
			}

			public static SceneLoader ins;

			public Scenes curSceneName;

			void Awake(){
				if(ins == null){

					ins = this;
					GameObject.DontDestroyOnLoad(gameObject);
					SceneManager.ins.OwnSecne = (int)this.curSceneName;

				}else if(ins != this){

					Destroy(gameObject);
				}
			}

			public void LoadLevel(Scenes target){

				SceneManager.ins.LoadScene((int)target);
			}
		}
	}
}

