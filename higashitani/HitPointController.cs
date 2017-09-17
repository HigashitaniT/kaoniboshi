using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPointController : MonoBehaviour {

    public ShakeTest _CameraShake;
    public GameController _GameCtrl;
    public GameObject earthObj;

    [SerializeField]
    private GameObject damageText;

    [SerializeField]
    private GameObject text, earthImage;

    [SerializeField]
    private int maxPos, minPos,speed,startPos,endPos,damagePoint;

    [SerializeField]
    private float maxHp = 720;
    public float hp;

    private int onceEnd = 0;
    public Text zinnkou;

    Image earthSprite;
    public float MaxHp
    {
        get { return maxHp; }
        private set { maxHp = value; }
    }
    public int Speed
    {
        get {return speed; }
        set {speed = value; }
    }
    public int EndPos
    {
        get { return endPos; }
        set { endPos = value; }
    }

    bool flug = false;

    // Use this for initialization
    void Start () {
        earthSprite = earthImage.GetComponent<Image>();
        hp = maxHp;
        //TextMove();
        //CollDamage();
	}
	
	// Update is called once per frame
	void Update () {
        int zinkouti = (int)hp / 10;
        GameController.Instance.HitPoint = zinkouti;
        if (zinkouti >= 0)
        {
            zinnkou.text = zinkouti.ToString();
        }
        if (zinkouti <= 0)
        {
            hp = 0;
            SoundManeger.Instance.isPlayPlayerOrMarsDeathSe = true;
            if (onceEnd < 1)
            {
                onceEnd++;
                _GameCtrl.gameStates = GameController.GameStates.End;
            }
            GameController.Instance.earthBakuhatu.transform.position = earthObj.gameObject.transform.position;
            GameController.Instance.earthBakuhatu.SetActive(true);
            earthObj.SetActive(false);

        }
    }
    

    public void CollDamage(string name)
    {
        int damage = 0;
        switch(name)
        {
            case "UFO":
                damage = 3;
                break;
            case "Meteo":
                damage = 1;
                break;
            case "Kasei":
                damage = 10;//(int)((maxHp/10) / 4);
                break;
        }
        GameObject instanceText = Instantiate(damageText, damageText.transform.position, Quaternion.identity);
        //instanceText.transform.parent = GameObject.Find("HitDamagePanel").transform;
        instanceText.transform.SetParent(GameObject.Find("HitDamagePanel").transform, false);
        instanceText.GetComponent<Text>().text = ((-damage).ToString() + "億");
        StartCoroutine("Damage",damage);
    }
    

    IEnumerator Damage(int damage)
    {
        _CameraShake.ShakeT();

        int i = 0;
        while (damage > i)
        {
            hp -= 10;
            earthSprite.fillAmount = hp / maxHp;
            i++;
            yield return null;
        }
    }

    void TextMove()
    {
        int randomPos = Random.Range(minPos, maxPos);
        GameObject cloneText = Instantiate(text, new Vector3(startPos, randomPos, text.transform.position.z), Quaternion.identity);
    }
}
