using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private WinBlockBehaviour _winBlock;

	// Use this for initialization
	void Start () {
        _winBlock = GameObject.Find("WinBlock").GetComponent<WinBlockBehaviour>();
	    _winBlock.SetEnabled(true);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.R))
            StartCoroutine(_winBlock.RestartLevel());
        else if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
	}
}
