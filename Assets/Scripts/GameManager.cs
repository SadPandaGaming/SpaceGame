using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //non networked stuff in this component

    public static GameManager access;
    public static GameObject networkManager;
    public Transform backgroundRoot;

    public static GameObject clientGameObject;
    public static PlayerController clientController;

    public GameObject WorldCanvasObject;

    Text countdown;

    IEnumerator Start () {
        Application.targetFrameRate = 60;
		yield return 0; //0 will wait 1 frame.
        networkManager = GameObject.Find("_NetworkManager");

        //if we loaded a scene at startup that isn't lobby -- switch to lobby scene.
        if (networkManager == null) { SceneManager.LoadScene("Lobby", LoadSceneMode.Single); yield break; }
        access = this;
        CreateBackground();
	}

    public void CreateBackground() {

        int x = 0;
        int y = 0;

        int gridX = 5;
        int gridY = 5;

        float bgSizeX = 1920 / 100;
        float bgSizeY = 1200 / 100;

        float xStart = -((gridX - 1) * bgSizeX) * 0.5f;
        float yStart = -((gridY - 1) * bgSizeY) * 0.5f;

        //create background from resource folder
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Space_Background") as GameObject, 
            new Vector3(xStart + x * bgSizeX, yStart + y * bgSizeY, 0),
            Quaternion.identity,
            backgroundRoot);

        x = 1; //we just placed the first one.

        //duplicate background
        while (y < gridY) {
            Instantiate(
                go, 
                new Vector3(xStart + x * bgSizeX, yStart + y * bgSizeY, 0),
                Quaternion.identity,
                backgroundRoot);
            x++;
            if (x >= gridX) { y++; x = 0; }
        }
    }

}
