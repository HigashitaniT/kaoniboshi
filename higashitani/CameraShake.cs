using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public float width, time;
    public GameObject head;

    Vector3 headPos;
    Vector3 myPos;

    //public GameObject parent;

	// Use this for initialization
	void Start () {
		//Shake ();
        //headPos = head.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //iTween.MoveUpdate(this.gameObject,iTween.Hash("z",(head.transform.position.z-17f),"time",2f));
        this.gameObject.transform.position = new Vector3(this.transform.position.x,
                                                         this.transform.position.y,
                                                         head.transform.position.z - 10);
	}
	public void Shake()
	{
		//iTween.ShakePosition(this.GetComponent<Camera>(),iTween.Hash("y",0.6f,"time",0.5f));
		iTween.ShakePosition(this.gameObject,iTween.Hash("y",width,"time",time));
	}
}
