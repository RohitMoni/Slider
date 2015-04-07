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
	    SetEnabled(false);
	}

    public void CheckWinCondition(Vector3 playerPosition)
    {
        if (!_enabled)
            return;

        if (playerPosition.x == transform.position.x && playerPosition.z == transform.position.z)
        {
            Debug.Log("Won Level!");
            StartCoroutine(FadeScreenIn(5f));
        }
    }

    public IEnumerator RestartLevel()
    {
        StartCoroutine(FadeScreenIn(0.5f));
        while (!_doneFading)
        {
            yield return null;
        }

        // Restart level here
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void SetEnabled(bool isEnabled)
    {
        _enabled = isEnabled;
        transform.GetChild(0).gameObject.SetActive(isEnabled);
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
