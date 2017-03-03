using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System.IO;

public class BenchmarkScene : MonoBehaviour {

	// ----- VARRIABLES -----
	[Tooltip("Write the name of the Scene to Benchmark here")]
	public string sceneToBenchmark;

	[Tooltip("Additional text-comments for the output file")]
	public string textToWrite;   // Text to be written on the textfile

	[Tooltip("Seconds to wait before initiating measurement (avoids dropouts)")]
	public int secondsBeforeStart = 2;

	[Tooltip("How long will every benchmark last?")]
	public int secondsForBenchmark = 5;

	private float timeToLoad = 0; // Helper for counting the loading times
	private string pather;		  // Path for the file
	private string fileName;	  // File name for the output info
	// ----- END OF VARRIABLES -----

	void Awake() // Calls for file preperation and stays in next scene
	{
		PrepareFile ();
		DontDestroyOnLoad(transform.gameObject);
	}

	IEnumerator Start () // Loops throughout the Quality settings
	{
		string[] qNames = QualitySettings.names;
		for (int i = 0; i < qNames.Length; i++)
			{
				QualitySettings.SetQualityLevel(i, true);
				timeToLoad = Time.time;
				AsyncOperation async =	SceneManager.LoadSceneAsync(sceneToBenchmark);
				yield return async;
				timeToLoad = Time.time - timeToLoad;
				textToWrite = "Time to load " + qNames [i].ToString () + " = " + timeToLoad.ToString() + "\r\n";
				File.AppendAllText(fileName, textToWrite);
				yield return new WaitForSeconds (secondsBeforeStart);
				StartCoroutine(WritePNG(pather.ToString()+qNames[i].ToString()+".PNG"));
			}
	}

	IEnumerator WritePNG(string imgFileName) 
	{
		// Delete previusly created file
		if (File.Exists (imgFileName)) {
			File.Delete (imgFileName);
		}

		// We should only read the screen buffer after rendering is complete
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy(tex);

		File.WriteAllBytes(imgFileName, bytes);
	}

	void PrepareFile()
	{
		if (textToWrite != null)
		textToWrite = textToWrite + "\r\n";    // Add a new line under the comments if there are any
		textToWrite = textToWrite + "Scene    : " + sceneToBenchmark + "\r\n";
		textToWrite = textToWrite + "Platform : " + Application.platform.ToString() + "\r\n";
		
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
		Directory.CreateDirectory(pather);

		fileName =  pather + sceneToBenchmark +".txt";
		if (File.Exists (fileName))
			File.Delete (fileName);

		File.AppendAllText(fileName, textToWrite);
	}
}
