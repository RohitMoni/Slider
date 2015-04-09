using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinScreenBehaviour : MonoBehaviour
{
    private bool _doneFading;
    private Image _finalScreenText;

    private void Start()
    {
        _doneFading = true;
        _finalScreenText = GameObject.Find("Image").GetComponent<Image>();
        StartCoroutine(FadeScreenIn(2f));
    }

    // Update is called once per frame
	void Update ()
	{
	    if (!_doneFading)
	        return;

        if (Input.anyKey)
            GoToMainMenu();
	}

    public void GoToMainMenu()
    {
        if (!_doneFading)
            return;

        //Debug.Log("Go to next level");
        StartCoroutine(MainMenuRoutine());
    }

    IEnumerator MainMenuRoutine()
    {
        StartCoroutine(FadeScreenOut(1f));
        while (!_doneFading)
        {
            yield return null;
        }

        Application.LoadLevel("MainMenu");
    }

    IEnumerator FadeScreenIn(float time)
    {
        _doneFading = false;
        var deltaTime = 0f;
        while (deltaTime < time)
        {
            deltaTime += Time.deltaTime;
            _finalScreenText.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, (deltaTime / time));
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
            _finalScreenText.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (deltaTime / time));
            yield return null;
        }

        _doneFading = true;
        yield return null;
    }
}
