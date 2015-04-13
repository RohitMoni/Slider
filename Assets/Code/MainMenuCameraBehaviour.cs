using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuCameraBehaviour : MonoBehaviour
{
    private bool _doneFading= true;
    private float _mousePositionX = 0;
    private float _mouseMovementX =0;
    private GameObject _fadePanel;

    void Start()
    {
        _fadePanel = GameObject.Find("Fade Panel");
        StartCoroutine(FadeScreenOut(1f));
    }

    // Update is called once per frame
	void Update ()
	{
	    transform.Rotate(Vector3.up, 0.1f);

        _mouseMovementX = _mousePositionX - Input.mousePosition.x;
        _mousePositionX = Input.mousePosition.x;

        transform.Rotate(Vector3.up, _mouseMovementX * 0.1f);

        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
	    if (Input.anyKeyDown)
	        StartLevel(1);
	}

    public void StartLevel(int number)
    {
        if (!_doneFading)
            return;

        StartCoroutine(FadeToGoToLevel(number));
    }

    IEnumerator FadeToGoToLevel(int number)
    {
        if (!_doneFading)
            yield break;

        StartCoroutine(FadeScreenIn(1f));
        while (!_doneFading)
        {
            yield return null;
        }

        Application.LoadLevel("Level" + number);
    }

    IEnumerator FadeScreenIn(float time)
    {
        _doneFading = false;
        var deltaTime = 0f;
        while (deltaTime < time)
        {
            deltaTime += Time.deltaTime;
            _fadePanel.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, (deltaTime / time));
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
            _fadePanel.GetComponent<Image>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (deltaTime / time));
            yield return null;
        }

        _doneFading = true;
        yield return null;
    }
}
