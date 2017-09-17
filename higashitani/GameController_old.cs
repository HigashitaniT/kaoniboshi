/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    //private float time;
    public GameObject titleText;
    private bool startFlag;

    public bool StartFlag
    {
        get { return startFlag; }
        set { startFlag = value; }
    }
    /*
    public float TimeCount
    {
        get {return time; }
        set { time = value; }
    }
    
    public enum GameStates
    {
        Title,
        Start,
        End
    }
    public GameStates gameStates;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (gameStates)
        {
            case GameStates.Title:
                if (startFlag)
                {
                    titleText.GetComponent<FadeScript>().FadeOut();
                    gameStates = GameStates.Start;
                }
                break;

            case GameStates.Start:
                //Timer();
                break;

            case GameStates.End:
                break;
        }

	}
    /*
    void Timer()
    {
        time -= Time.deltaTime;
        if(time <= 0){
            gameStates = GameStates.End;
        }
    }
}
*/