using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingScript : MonoBehaviour {

	// Use this for initialization
    public GameObject earth;
    public float pullForce;

    private Vector3 earthFirstPosition;

	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(transform.position.x, transform.position.y, earth.transform.position.z);

        Vector3 forceDirection = earth.transform.position - transform.position;
        GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.deltaTime);
	}
}
