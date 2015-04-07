using UnityEngine;
using System.Collections;

public class WinBlockBehaviour : MonoBehaviour
{
    private bool _enabled;
    private GameObject _fadeScreen;
    private bool _doneFading;

	// Use this for initialization
	void Start ()
	{
	    _fadeScreen = GameObject.Find("FadeScreen");
	    StartCoroutine(FadeScreenOut(1f));
	}

    public void CheckWinCondition(Vector3 playerPosition)
    {
        if (!_enabled)
            return;

        if (playerPosition.x - transform.position.x < 0.2f && playerPosition.z - transform.position.z < 0.2f)
        {
            Debug.Log("Won Level!");
            StartCoroutine(FadeToWin());
        }
    }

    public void RestartLevel()
    {
        StartCoroutine(FadeToRestart());
    }

    public void SetEnabled(bool isEnabled)
    {
        _enabled = isEnabled;
        transform.GetChild(0).gameObject.SetActive(isEnabled);
    }

    public IEnumerator FadeToRestart()
    {
        StartCoroutine(FadeScreenIn(0.5f));
        while (!_doneFading)
        {
            yield return null;
        }

        // Restart level here
        Application.LoadLevel(Application.loadedLevelName);
    }

    IEnumerator FadeToWin()
    {
        StartCoroutine(FadeScreenIn(4f));
        while (!_doneFading)
        {
            yield return null;
        }

        // Restart level here
        var currentLevelName = Application.loadedLevelName;
        var levelNumber = int.Parse(currentLevelName.Substring(currentLevelName.Length-1, 1));

        Application.LoadLevel("Level" + (levelNumber+1));
    }

    IEnumerator FadeScreenIn(float time)
    {
        _doneFading = false;
        var deltaTime = 0f;
        while (deltaTime < time)
        {
            deltaTime += Time.deltaTime;
            _fadeScreen.GetComponent<Renderer>().material.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, (deltaTime / time));
            yield return null;
        }

        _doneFading = true;
        yield return null;
    }

    IEnumerator FadeScreenOut(float time)
    {
        _doneFading = false;
        var deltaTime = 0f;
        while (deltaTime < time)
        {
            deltaTime += Time.deltaTime;
            _fadeScreen.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (deltaTime / time));
            yield return null;
        }

        _doneFading = true;
        yield return null;
    }
}
