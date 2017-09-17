using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour {

    private float direction = 0f;

    //for raycast sweep
    public float distance = 1000f;
    public int theAngle = 180;
    public int segments = 180;
    public float speed = 100;

    private List<GameObject> targets = new List<GameObject>();
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    private GameObject head;

    [SerializeField]
    private bool IsRenderer = true;

    private float time;
	public float destroyTime;
	// Use this for initialization
	void Start () {
        transform.position = GameObject.Find("tikyuu02").gameObject.transform.position;
        head = GameObject.Find("tikyuu02").gameObject;
        int randDir = Random.Range(0, 2);

        //if (randDir == 0)
        //    direction = 1000f;
        //else
        //    direction = -1000f;
        //transform.LookAt(new Vector3(direction, transform.position.y, 0), transform.up);
	}
	
	// Update is called once per frame
	void Update () {
		if(!IsRenderer)
        {

            Destroy(this.transform.parent.gameObject);
        }
        
        if (target == null)
        {
            RaycastSweep();
        }

        if (target != null)
        {
            //Debug.Log("target");
            //Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, head.transform.position.z);
            transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, target.transform.position, speed * Time.deltaTime);
            transform.parent.transform.LookAt(target.transform,transform.parent.transform.up);
            
        }

        time += Time.deltaTime;
		if (time >= destroyTime)
        {
            IsRenderer = false;
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Meteo" || col.tag == "UFO" || col.tag == "Kasei")
        {
            SoundManeger.Instance.isPlayMissileHitSe = true;
            EffectManager.Instance.PlayEffect("explosion", this.transform.position, Quaternion.identity);
            //Debug.Log("当たった：" + col.tag);
            Destroy(this.transform.parent.gameObject);
            
        }
    }

    public void RaycastSweep()
    {
        //Vector3 startPos = transform.position;
        //Vector3 targetPos = new Vector3(direction, 0, 0);
        //int startAngle = (int)(-theAngle * 0.5);
        //int finishAngle = (int)(theAngle * 0.5);

        //int inc = (int)(theAngle / segments);

        //RaycastHit hit;
        //for (int i = startAngle; i <= finishAngle; i += inc)
        //{
        //    targetPos = (Quaternion.Euler(0, 0, i) * transform.forward).normalized * distance;
        //    //Debug.Log (i);
        //    if (Physics.Linecast(startPos, targetPos, out hit))
        //    {
        //        Debug.Log("Linecast");
        //        if (hit.collider.tag == "Meteo")
        //        {
        //            Debug.Log ("Meteo");
        //            targets.Add(hit.collider.gameObject);
        //        }

        //        else if(hit.collider.tag == "UFO")
        //        {
        //            targets.Add(hit.collider.gameObject);
        //        }

        //    }
        //    //Debug.DrawLine (startPos, targetPos, Color.green);
        //}
        GameObject closestMeteorite = null;
        if (GameController.Instance.enemyList.Count <= 0)
        {
            this.transform.parent.transform.position += this.transform.parent.transform.forward/5;
        }
        else
        {
            closestMeteorite = GameController.Instance.enemyList[0].gameObject;
        }

        for (int j = 0; j < GameController.Instance.enemyList.Count; j++)
        {
            if (GameController.Instance.enemyList[j] != null)
            {
                if (Vector3.Distance(closestMeteorite.transform.position, head.transform.position) > Vector3.Distance(GameController.Instance.enemyList[j].transform.position, head.transform.position))
                {
                        closestMeteorite = GameController.Instance.enemyList[j];
                }
            }
        }
        target = closestMeteorite;
    }
}
