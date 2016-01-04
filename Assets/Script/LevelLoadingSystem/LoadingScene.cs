using UnityEngine;
using System.Collections;

namespace DIM {
	namespace LevelLoadingSystem {
		
		public class LoadingScene : MonoBehaviour {

			IEnumerator Start () {

				yield return new WaitForSeconds(3);

				SceneManager.ins.StartLoadTargetScene();
			}

		}
	}
}


