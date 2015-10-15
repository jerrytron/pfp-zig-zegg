using UnityEngine;
using System.Collections;

public class Toolbox : MonoBehaviour {
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!

	public GameManager gameManager = new GameManager();

	public static float deltaTime;
	public float previousTime = 0.0f;

	public static Toolbox instance;

	// (optional) allow runtime registration of global objects
	/*public static T RegisterComponent<T> () where T: Component {
		return Instance.GetOrAddComponent<T>();
	}*/

	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
			// Your initialization code here.
			Debug.Log("Awake");
			gameManager.Initialize();
		} else {
			DestroyImmediate(gameObject);
		}
	}

	void Start() {
		Debug.Log("Start");
		gameManager.StateStart();
	}

	void OnEnable() {
		Debug.Log("OnEnable");
		gameManager.StateOnEnable();
	}

	void OnDisable() {
		Debug.Log("OnDisable");
		gameManager.StateOnDisable();
	}
	
	void Update() {
		gameManager.StateUpdate();
		deltaTime = Time.realtimeSinceStartup - previousTime;
		previousTime = Time.time;
	}

	void OnGUI() {
		gameManager.StateOnGUI();
	}

	void OnLevelWasLoaded() {
		gameManager.StateOnLevelWasLoaded();
	}

	void OnApplicationPause(bool aPaused) {
		gameManager.StateOnApplicationPause(aPaused);
	}

	void OnApplicationFocus() {
		gameManager.StateOnApplicationFocus();
	}

}