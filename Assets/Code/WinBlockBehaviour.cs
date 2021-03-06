﻿using System;
using System.Linq;
using UnityEngine;
using System.Collections;

public class WinBlockBehaviour : MonoBehaviour
{
    private bool _enabled;
    private GameObject _fadeScreen;
    private bool _doneFading;
    private int _counter;

	// Use this for initialization
	void Start ()
	{
	    _fadeScreen = GameObject.Find("FadeScreen");
	    StartCoroutine(FadeScreenOut(1f));
	    _counter = GameObject.FindGameObjectsWithTag("Pickup").Count();
        CheckEnabled();
	}

    public bool GetIsFading()
    {
        return !_doneFading;
    }

    public void PickupTriggered()
    {
        _counter--;
        CheckEnabled();
    }

    private void CheckEnabled()
    {
        SetEnabled(_counter <= 0);
    }

    public void CheckWinCondition(Vector3 playerPosition)
    {
        if (!_enabled || _counter > 0)
            return;

        if (Mathf.Abs(playerPosition.x - transform.position.x) < 0.05f && Mathf.Abs(playerPosition.z - transform.position.z) < 0.05f)
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(FadeToWin());
        }
    }

    public void RestartLevel()
    {
        StartCoroutine(FadeToRestart());
    }

    public void GoToMainMenu()
    {
        StartCoroutine(FadeToMainMenu());
    }

    public void SetEnabled(bool isEnabled)
    {
        _enabled = isEnabled;
        transform.GetChild(0).gameObject.SetActive(isEnabled);
    }

    IEnumerator FadeToRestart()
    {
        if (!_doneFading)
            yield break;

        StartCoroutine(FadeScreenIn(0.5f));
        while (!_doneFading)
        {
            yield return null;
        }

        // Restart level here
        Application.LoadLevel(Application.loadedLevelName);
    }

    IEnumerator FadeToMainMenu()
    {
        if (!_doneFading)
            yield break;

        StartCoroutine(FadeScreenIn(0.5f));
        while (!_doneFading)
        {
            yield return null;
        }

        // Restart level here
        Application.LoadLevel("MainMenu");
    }

    IEnumerator FadeToWin()
    {
        StartCoroutine(FadeScreenIn(2f));
        while (!_doneFading)
        {
            yield return null;
        }

        // Restart level here
        var currentLevelName = Application.loadedLevelName;
        var levelNumberString = currentLevelName.Substring(5);
        var levelNumber = int.Parse(levelNumberString);

        if (Application.levelCount-2 > levelNumber)
            Application.LoadLevel("Level" + (levelNumber + 1));
        else
            Application.LoadLevel("WinScreen");
    }

    IEnumerator FadeScreenIn(float time)
    {
        _doneFading = false;
        var deltaTime = 0f;
        while (deltaTime < time)
        {
            deltaTime += Time.deltaTime;
            if (_fadeScreen)
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
            if (_fadeScreen)
                _fadeScreen.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (deltaTime / time));
            yield return null;
        }

        _doneFading = true;
        yield return null;
    }
}
