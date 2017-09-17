using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour {

	[SerializeField]
	private GameObject earthObj;
    GameObject earthClone;

    private GameObject targetHead;
    private bool IsFind, IsCollback = false, IsFirst = false;

    public int bodyNum;

	// Use this for initialization
    //[SerializeField]
    //List<GameObject> headList = new List<GameObject>();

    public bool Collback{
        get {return IsCollback; }
        set { IsCollback = value; }
    }

	void Start () {
        IsFind = false;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Collback)//見失った通知を受け取ったら親子を外してフラグを下す。
        {
            earthObj.transform.parent = null;
            Collback = false;
            bodyNum--;
        }
    }
    
    public void CreateHead(Transform head)
    {
        earthObj.transform.position = head.position;
        earthObj.transform.parent = head;
        bodyNum++;
    }
}
