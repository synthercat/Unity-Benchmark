using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BenchmarkScene : MonoBehaviour {

	[Tooltip("Write the name of the Scene to Benchmark here")]
	public string sceneToBenchmark;

	[Tooltip("Seconds to wait before initiating measurement (avoids dropouts)")]
	public int secondsBeforeStart = 2;

	[Tooltip("How long will every benchmark last?")]
	public int secondsForBenchmark = 5;

	private float timeToLoad = 0;
	private string[] qNames;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	IEnumerator Start () {
		qNames = QualitySettings.names;
			foreach (string qName in qNames)
			{
				timeToLoad = Time.time;
				AsyncOperation async =	SceneManager.LoadSceneAsync(sceneToBenchmark);
				yield return async;
				timeToLoad = Time.time - timeToLoad;
				print("Time to load : " + timeToLoad);
			}

	}
	
	// Update is called once per frame
	void Update () {
		print("at scene : " + Application.loadedLevel);
	//	print ("Progress : " + operation.progress);
		
	}

	//IEnumerator load()
}
