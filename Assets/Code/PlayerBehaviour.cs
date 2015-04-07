using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class PlayerBehaviour : MonoBehaviour
{
    /* Public References */
    public GameObject WallBlock;

    /* References */

    /* Properties */
    private List<int> _movementQueue; // 1 = Up, 2 = Down, 3 = Left, 4 = Right
    private bool _isMoving;

    /* Constants */
    private const float SlideSpeed = 0.3f;


	// Use this for initialization
	void Start ()
	{
        _movementQueue = new List<int>(10);
	    _isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            _movementQueue.Add(1);
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            _movementQueue.Add(2);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _movementQueue.Add(3);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
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

        CreateBlockAt(startPosition);
        _isMoving = false;
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

        yield return null;
    }
}
