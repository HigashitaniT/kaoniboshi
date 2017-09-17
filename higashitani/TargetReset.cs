using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReset : MonoBehaviour {

    #region Singlton
    public static TargetReset Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
        {
            enabled = false;
            DestroyImmediate(this);
            return;
        }
        Instance = this;
        ScoreInfo.Read();
    }
    #endregion

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GetComponentInChildren<MissileScript>().Target = null;
            }
        }
    }

}
