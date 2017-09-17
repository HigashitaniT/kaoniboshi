using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HeadModel : MonoBehaviour {

    public HitPointController _hpCtrl;
    public GameObject explosionObj;

    public List<GameObject> particleList = new List<GameObject>();

    //private

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (particleList.Count > 0)
        {
            for (int i = 0; i < particleList.Count; i++)
            {
                if (!particleList[i].GetComponent<ParticleSystem>().isPlaying)
                {
                    DestroyMat(particleList[i]);
                    particleList.RemoveAt(i);
                }
            }
        }
	}
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Meteo" || col.tag == "UFO" || col.tag == "Kasei")
        {
            // 向きの計算
            // 当たった場所から地球への向きベクトルを反転する
            Quaternion lookAt = Quaternion.LookRotation(GameObject.FindGameObjectWithTag("MainCamera").transform.position - transform.position);
            EffectManager.Instance.PlayEffect("explosion", col.transform.position, lookAt);

            _hpCtrl.CollDamage(col.gameObject.tag);

            SoundManeger.Instance.isPlayPlayerHitSe = true;
        }
    }

    void DestroyMat(GameObject particl)
    {
        
        var thisRenderre = particl.GetComponent<Renderer>();
        if (thisRenderre != null && thisRenderre.materials != null)
        {
            foreach (var m in thisRenderre.materials)
            {
                DestroyImmediate(m);
                Destroy(particl);
            }
        }
    }
}
