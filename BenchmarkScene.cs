using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class BenchmarkScene : MonoBehaviour
{

	// ----- VARRIABLES -----

	[Tooltip ("Write the name of the Scene to Benchmark here")]
	public string sceneToBenchmark;

	[Tooltip ("Additional text-comments for the output file")]
	public string textToWrite;
	// Text to be written on the textfile

	[Tooltip ("Seconds to wait before initiating measurement (avoids dropouts)")]
	public int secondsBeforeStart = 3;

	[Tooltip ("How long will every benchmark last?")]
	public int secondsForBenchmark = 5;

	private float timeToLoad = 0; // Helper for counting the loading times
	private float fps = 0f;
	private float sumfps;
	private float minfps = 1000f;
	private float maxfps = 0f;
	private int fCount = 0;
	private float timeToEnd;
	private string pather; 	// Path for the file
	private string fileName; // File name for the output info

	// ----- END OF VARRIABLES -----

	void Awake () // Calls for file preperation and stays in next scene
	{
		PrepareFile ();
		DontDestroyOnLoad (transform.gameObject);
	}

	IEnumerator Start () // Loops throughout the Quality settings
	{
		string[] qNames = QualitySettings.names;
		for (int i = 0; i < qNames.Length; i++) // Loop this for all quality presets
		{
			QualitySettings.SetQualityLevel (i, true);
			timeToLoad = Time.time;
			AsyncOperation async =	SceneManager.LoadSceneAsync (sceneToBenchmark);
			yield return async;
			timeToLoad = Time.time - timeToLoad;
			textToWrite = "\r\n" + qNames[i].ToUpper() + "\r\n" + "Time to load = " + timeToLoad.ToString () + "\r\n";
			File.AppendAllText (fileName, textToWrite);
			sumfps = 0;
			minfps = 1000f;
			maxfps = 0f;
			fCount = 0;
			if (secondsBeforeStart < 1)
				secondsBeforeStart = 1;
			yield return new WaitForSeconds (secondsBeforeStart);
			timeToEnd = Time.unscaledTime + secondsForBenchmark;
			while (timeToEnd > Time.unscaledTime) 
			{ // Count FPS
				yield return new WaitForEndOfFrame ();
				float deltaTime = Time.unscaledDeltaTime;
				float interp = deltaTime / (0.5f + deltaTime);
				float currentFPS = 1.0f / deltaTime;
				if (fps == 0f)
					fps = currentFPS;
				fps = Mathf.Lerp (fps, currentFPS, interp);
				sumfps += fps;
				minfps = Mathf.Min (minfps, fps);
				maxfps = Mathf.Max (maxfps, fps);
				fCount++;
			}
			textToWrite = "Min FPS : " + minfps + " Max FPS : " + maxfps + "\r\n" +
			              "Total F : " + fCount + "\r\n" +
		                  "Average : " + (sumfps / fCount).ToString () + "\r\n";
			File.AppendAllText (fileName, textToWrite);
			StartCoroutine (WritePNG (pather.ToString () + qNames [i].ToString () + ".PNG"));
		}
		Application.Quit ();
	}


	IEnumerator WritePNG (string imgFileName)
	{
		// Delete previusly created file
		if (File.Exists (imgFileName)) 
			File.Delete (imgFileName);
		

		// We should only read the screen buffer after rendering is complete
		yield return new WaitForEndOfFrame ();

		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);

		// Read screen contents into the texture
		tex.ReadPixels (new Rect (0, 0, width, height), 0, 0);
		tex.Apply ();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG ();
		Destroy (tex);

		File.WriteAllBytes (imgFileName, bytes);
	}

	void PrepareFile ()
	{
		if (textToWrite != null)
			textToWrite = textToWrite + "\r\n";    // Add a new line under the comments if there are any
		textToWrite = textToWrite + "Scene    : " + sceneToBenchmark + "\r\n";
		textToWrite = textToWrite + "Platform : " + Application.platform.ToString () + "\r\n";
		
		switch (Application.platform) 
		{
			case RuntimePlatform.Android: 
			pather = "/sdcard/UnityBench/";
			break;

			case RuntimePlatform.WindowsEditor:
			pather = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments).ToString () + "/UnityBench/";
			break;

			case RuntimePlatform.WindowsPlayer:
			pather = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments).ToString () + "/UnityBench/";
			break;

			case RuntimePlatform.LinuxEditor:
			pather = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments).ToString () + "/UnityBench/";
			break;

			case RuntimePlatform.LinuxPlayer:
			pather = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments).ToString () + "/UnityBench/";
			break;
		}

		if (pather == null)
			Application.Quit ();
		
		Directory.CreateDirectory (pather);

		fileName = pather + sceneToBenchmark + ".txt";
		if (File.Exists (fileName))
			File.Delete (fileName);

		File.AppendAllText (fileName, textToWrite);
	}
}
