using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGraph : MonoBehaviour {
    [SerializeField]
    private HitPointController hpCtrl;
    [SerializeField]
    private Text meteorPointText, ufoPointText, marsPointText, popPointText;

    private int meteorPoint, ufoPoint, marsPoint, popPoint;

    [SerializeField]
    private Text meteorSumText, ufoSumText, marsSumText, fullSumText;

	// Use this for initialization
	void Start () {
        StartGraph();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGraph()
    {
        SetPoint();
        Mul();
    }

    void SetPoint()
    {
        meteorPoint = ScoreInfo.Score.MeteorNormal;
        ufoPoint = ScoreInfo.Score.Ufo;
        marsPoint = ScoreInfo.Score.Mars;
        popPoint = (int)hpCtrl.hp / 10;

        meteorPointText.text = meteorPoint.ToString();
        ufoPointText.text = ufoPoint.ToString();
        marsPointText.text = marsPoint.ToString();
        popPointText.text = popPoint.ToString();
    }

    void Mul ()
    {
        meteorSumText.text = (meteorPoint * 10).ToString();
        ufoSumText.text = (ufoPoint * 10).ToString();
        marsSumText.text = (marsPoint * 50).ToString();
        if (GameController.Instance.IsWin)
        {
            fullSumText.text = (((meteorPoint * 10) + (ufoPoint * 10) + (marsPoint * 50 * popPoint))).ToString();
        }
        else
        {
            fullSumText.text = ((meteorPoint * 10) + (ufoPoint * 10) + (marsPoint * 50)).ToString();
        }        
    }
}
