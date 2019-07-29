using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGamePlay : MonoBehaviour {

	//public GameObject startInfo;
	//public GameObject downlodInfo;
	public Slider progressBar;
	WWW www;
	public Sprite grid;
	public GameObject player;
	public GameObject node;


	// Use this for initialization
	void Start () 
	{
		progressBar.enabled = true;
		StartCoroutine(DownloadAssetFromServer());
	}

	void Update()
	{
		if (www != null && www.progress != 0 && progressBar.value != 1) 
		{
			progressBar.value = www.progress;
		}

	}
		

	public void OnClickStart()
	{
		//startInfo.SetActive (false);

		//downlodInfo.SetActive (true);
	}

//	public void OnClickXorO()
//	{
//		SceneManager.LoadScene (1);
//	}

	public IEnumerator DownloadAssetFromServer()
	{
		
		string serverUrl = "https://www.dropbox.com/sh/j8le9t8ulymxf9h/AADgDSP9vu4l2AMw9V_H7FHJa?dl=1";

		//string serverUrl = "https://www.dropbox.com/s/wzw1oxzey5c0fdt/image?dl=1";

		string filePath= Path.Combine(Application.persistentDataPath, "grid.unity3d");

		if (File.Exists(filePath))
		{
			yield return new WaitForSeconds(0f);
			LoadAssetBundle();

		}
		else
		{
			//startInfo.SetActive (false);	
			progressBar.gameObject.SetActive(true);

			www = new WWW(serverUrl);

			yield return www;

			yield return new WaitUntil(() => www.isDone);

			File.WriteAllBytes(filePath, www.bytes);


			yield return new WaitForSeconds(1f);
			LoadAssetBundle();
		}
	}

	public void LoadAssetBundle()
	{
		if (!File.Exists(Path.Combine(Application.persistentDataPath, "grid.unity3d")))
			return;
		var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, "grid.unity3d"));

		if (myLoadedAssetBundle == null)
		{
			Debug.Log("Failed to load AssetBundle!");
			return;
		}
			

		player = myLoadedAssetBundle.LoadAsset<GameObject>("Player")as GameObject;
		node = myLoadedAssetBundle.LoadAsset<GameObject>("Node")as GameObject;
		//grid = myLoadedAssetBundle.LoadAsset<Sprite>("image");
//		progressBar.gameObject.SetActive(false);
//		//startInfo.SetActive (true);
//		myLoadedAssetBundle.Unload(false);
	}

}
