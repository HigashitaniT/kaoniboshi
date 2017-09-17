using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextModel : MonoBehaviour {

    private TextMesh texts;

    private GameObject faceObj;

    private string[] coment =
        {
        "キャー",
        "oh---",
        "助けてー",
        "おぎょぇぇ～～！！",
        "ひでぶっ！",
        "我が人生に一片の悔いなし！",
        "待って、うわーー…",
        "オワタ＼(^o^)／",
        "許してください…",
        "そんなことより仕事ください",
        "え、今日は休んでいいのか！",
        "いいぞ、明日も休みだぞ",
        "オイオイオイ死んだわコレ",
        "ほう、隕石ですか…",
        "あぁ＾～",
        "んにゃああああ＾～",
        "俺は不滅だぁああああああ",
        "地球こわれる",
        "うわぁあああああああ",
        "ヒエェエエエエエ",
        "【悲報】ノストラダムス、今日だった",
    };

    void Start () {

        texts = GetComponent<TextMesh>();
        Rigidbody rb = this.GetComponent<Rigidbody>();
        faceObj = GameObject.FindGameObjectWithTag("Face");

        int i = Random.Range(0, coment.Length);
        texts.text = coment[i];
        rb.AddForce(Random.Range(-500,-800), 0, 0);
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x,transform.position.y, faceObj.transform.position.z);

        if (this.transform.position.x <= -50f)
        {
            int i = Random.Range(0, coment.Length);
            texts.text = coment[i];
            transform.position = new Vector3(17f, Random.Range(1,6), 0);
        }

        if(GameController.Instance.gameStates != GameController.GameStates.Title)
        {
            Destroy(this.gameObject);
        }
    }
}
