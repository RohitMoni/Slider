using UnityEngine;
using System.Collections;

public class PickupBehaviour : MonoBehaviour {

	// Update is called once per frame
	void Update ()
	{
	    transform.Rotate(Vector3.up, 1);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Player")
            return;

        Destroy(gameObject);
        GameObject.Find("WinBlock").GetComponent<WinBlockBehaviour>().PickupTriggered();
    }
}
