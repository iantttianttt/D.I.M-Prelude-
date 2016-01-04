using UnityEngine;
using System.Collections;


namespace DIM {
	namespace LevelLoadingSystem {
		
		public class SceneNames : MonoBehaviour {

			public string initScene;
			public SceneNameHolder[] scenes;

			[System.Serializable]
			public class SceneNameHolder{
				public string own;
				public string loading;
				public bool isAdditiveLoading;
			}
		}

	}
}

