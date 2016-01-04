using UnityEngine;
using System.Collections;

namespace DIM {
	namespace LevelLoadingSystem {
		public class SceneManager : MonoBehaviour {
			private enum Status{

				None,
				Prepare,
				Start,
				Loading,
				Complete
			}

			public static SceneManager ins;

			public SceneNames sceneNames;

			private Status status;
			private AsyncOperation loadOperation;
			private int own;

			public float Progress{
				get{
					if(this.loadOperation == null){
						return 0;
					}else{
						return this.loadOperation.progress;
					}
				}
			}

			public int OwnSecne{
				set{
					this.own  = Mathf.Clamp(value , 0 , this.sceneNames.scenes.Length - 1);
				}
			}

			void Awake(){

				if(ins == null){

					ins = this;
					GameObject.DontDestroyOnLoad(gameObject);

					if(!string.IsNullOrEmpty(this.sceneNames.initializationScene)){
						Application.LoadLevel(this.sceneNames.initializationScene);
					}

				}else if(ins != this){

					Destroy(gameObject);
				}
			}

			public void LoadScene(int idx){

				if(this.status != Status.None && this.status != Status.Complete) return;

				if(idx >= this.sceneNames.scenes.Length) return;

				StartCoroutine(this.AsyncLoadScene(this.sceneNames.scenes[idx]));
			}

			public void LoadOwnScene(){

				this.LoadScene(this.own);
			}

			public void StartLoadTargetScene(){

				this.status = Status.Start;
			}

			private IEnumerator AsyncLoadScene(DIM.LevelLoadingSystem.SceneNames.SceneNameHolder sceneName){

				yield return StartCoroutine(this.LoadLoadingScene(sceneName));

				yield return StartCoroutine(this.LoadTargetScene(sceneName));
			}

			private IEnumerator LoadLoadingScene(DIM.LevelLoadingSystem.SceneNames.SceneNameHolder sceneName){

				if(string.IsNullOrEmpty(sceneName.loadingSceneName)) yield break;

				this.status = Status.Prepare;

				this.loadOperation = sceneName.isAdditiveLoading ? Application.LoadLevelAdditiveAsync(sceneName.loadingSceneName) : Application.LoadLevelAsync(sceneName.loadingSceneName);

				yield return this.loadOperation;

				while(this.status == Status.Prepare){

					yield return null;
				}
			}

			private IEnumerator LoadTargetScene(DIM.LevelLoadingSystem.SceneNames.SceneNameHolder sceneName){

				this.status = Status.Loading;

				if(string.IsNullOrEmpty(sceneName.sceneName)){

					this.status = Status.None;
					yield break;
				}

				this.loadOperation = Application.LoadLevelAsync(sceneName.sceneName);
				yield return this.loadOperation;

				this.status = Status.Complete;
			}
		}

	}
}
