using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour {

    public static ResultController Instance { get; private set; }
    [SerializeField]
    private List<string> gender = new List<string>(), ages = new List<string>(), saveLavel = new List<string>(), singleResult = new List<string>();

    public HitPointController hpCtrl;

    [SerializeField]
    private GameObject result_Ui;

    private string genderString, agesString, saveLavelString, singleResultString;

    [SerializeField]
    private Text titled, zinkou;

    private int woman,man;
    public List<int> ageList = new List<int>();

    [SerializeField]
    private Vector2 highScoerRange;

    [SerializeField]
    private Vector2 midScoerRange;

    [SerializeField]
    private Vector2 lowScoerRange;

    void Awake()
    {
        if (Instance != null)
        {
            enabled = false;
            DestroyImmediate(this);
            return;
        }
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        Param();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Param()
    {
        int hp = (int)hpCtrl.hp / 10;
        zinkou.text = hp.ToString() + "億人";

        result_Ui.SetActive(true); 

        if (hp <= 0)
        {
            singleResultString = singleResult[Random.Range(1, 5)];
            titled.text = singleResultString;
            return;
        }

        woman = Random.Range(0, hp);
        man = hp - woman;

        if (woman > man)
        {
            genderString = gender[1];
        }
        else
        {
            genderString = gender[0];
        }

        int agehp = hp;
        int max = 0;
        for (int i = 0; i < ages.Count; i++)
        {
            ageList.Add(Random.Range(0, agehp));
            agehp = agehp - ageList[i];
            if(i > 0 && ageList[i] > ageList[i-1])
            {
                max = i;
            }
        }
        agesString = ages[max];


        if(hp == hpCtrl.MaxHp / 10)
        {
            singleResultString = singleResult[0];
            titled.text = singleResultString;
            return;
        }
        else if (hp > ((hpCtrl.MaxHp / 10) / 3) * 2) //48以上
        {
            saveLavelString = saveLavel[Random.Range((int)highScoerRange.x, (int)highScoerRange.y + 1)];
            titled.text = agesString + genderString + saveLavelString;
            return;
        }
        else if (hp > (hpCtrl.MaxHp / 10) / 3) //24以上
        {
            saveLavelString = saveLavel[Random.Range((int)midScoerRange.x, (int)midScoerRange.y + 1)];
            titled.text = agesString + genderString + saveLavelString;
            return;
        }
        else //1～23
        {
            saveLavelString = saveLavel[Random.Range((int)lowScoerRange.x, (int)lowScoerRange.y + 1)];
            titled.text = agesString + genderString + saveLavelString;
            return;
        }

    }
}
