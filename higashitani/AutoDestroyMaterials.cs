using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyMaterials : MonoBehaviour {

    [System.NonSerialized]
    public bool IsRoot = true;

	// Use this for initialization
	void Start () {
        if(IsRoot)
        foreach (var p in this.GetComponentsInChildren<ParticleSystem>())
        {
            p.gameObject.AddComponent<AutoDestroyMaterials>().IsRoot = false;
        }
        foreach (var r in this.GetComponentsInChildren<Renderer>())
        {
            r.gameObject.AddComponent<AutoDestroyMaterials>().IsRoot = false;
        }
    }
	
	// Update is called once per frame
	void OnDestroy () {
        var thisRenderer = this.GetComponent<Renderer>();
        if (thisRenderer != null && thisRenderer.materials != null)
        {
            foreach (var m in thisRenderer.materials)
            {
                Debug.Log("Matelial Destroy!");
                DestroyImmediate(m);
            }
        }
	}
}
