using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class PlayerBehaviour : MonoBehaviour
{
    /* Public References */
    public GameObject WallBlock;

    /* References */
    private WinBlockBehaviour _winBlock;
    private Transform _wallsAnchor;

    /* Properties */
    private List<int> _movementQueue; // 1 = Up, 2 = Down, 3 = Left, 4 = Right
    private bool _isMoving;

    /* Constants */
    private const float SlideSpeed = 0.2f;

	// Use this for initialization
	void Start ()
	{
        _movementQueue = new List<int>(10);
	    _isMoving = false;
	    _winBlock = GameObject.Find("WinBlock").GetComponent<WinBlockBehaviour>();
	    _wallsAnchor = GameObject.Find("Walls").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (_winBlock.GetIsFading())
	        return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _movementQueue.Add(1);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _movementQueue.Add(2);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _movementQueue.Add(3);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _movementQueue.Add(4);
        }

	    if (_movementQueue.Count != 0 && !_isMoving)
	    {
	        var movement = new Vector3();
	        switch (_movementQueue[0])
	        {
	            case 1:
	                movement.z += transform.localScale.x;
	                break;
                case 2:
	                movement.z -= transform.localScale.x;
	                break;
                case 3:
	                movement.x -= transform.localScale.x;
	                break;
                case 4:
	                movement.x += transform.localScale.x;
	                break;
	        }

	        var canMove = !Physics.Raycast(transform.position, movement, transform.localScale.x);

	        if (canMove)
	        {
                GetComponent<AudioSource>().Play();
	            StartCoroutine(Slide(transform.position, transform.position + movement, SlideSpeed));
	            _isMoving = true;
	        }

	        _movementQueue.RemoveAt(0);
	    }
	}

    float SmoothTimeValue(float time)
    {
        return Mathf.SmoothStep(0.0f, 1.0f, time);
    }

    void CreateBlockAt(Vector3 position)
    {
        var block = Instantiate(WallBlock, position, Quaternion.identity) as GameObject;
        block.transform.SetParent(_wallsAnchor);
        StartCoroutine(Grow(block, 0.0f, 0.3f, 0.5f));
    }

    IEnumerator Slide(Vector3 startPosition, Vector3 finalPosition, float time)
    {
        var deltaTime = 0f;
        while (deltaTime < time)
        {
            deltaTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, finalPosition, SmoothTimeValue(deltaTime / time));
            yield return null;
        }

        transform.position = finalPosition;
        CreateBlockAt(startPosition);
        _isMoving = false;
        _winBlock.CheckWinCondition(transform.position);
        yield return null;
    }

    IEnumerator Grow(GameObject sceneObject, float startScale, float endScale, float time)
    {
        sceneObject.transform.localScale = new Vector3(startScale, startScale, startScale);
        var deltaTime = 0f;
        while (deltaTime < time)
        {
            deltaTime += Time.deltaTime;
            sceneObject.transform.localScale = Vector3.Lerp(new Vector3(startScale, startScale, startScale), new Vector3(endScale, endScale, endScale), SmoothTimeValue(deltaTime / time));
            yield return null;
        }
        
        sceneObject.transform.localScale = new Vector3(endScale, endScale, endScale);
        yield return null;
    }
}
