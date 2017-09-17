using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour {

    [SerializeField]
    private float time = 1f;

    public enum ObjectsType
    {
        UIText,
        GameObj,
    }
    public ObjectsType objType;

    Color _color;

    void Start()
    {
        switch (objType)
        {
            case ObjectsType.GameObj:
                _color = gameObject.GetComponent<Image>().color;
                break;

            case ObjectsType.UIText:
                _color = gameObject.GetComponent<Text>().color;
                break;
        }
    }


    public void FadeIn()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", time, "onupdate", "SetValue"));
    }
    public void FadeOut()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "time", time, "onupdate", "SetValue"));
    }

    void SetValue(float alpha)
    {
        _color= new Color(_color.r, _color.g, _color.b, alpha);

        switch (objType)
        {
            case ObjectsType.GameObj:
                gameObject.GetComponent<Image>().color = _color;
                break;

            case ObjectsType.UIText:
                gameObject.GetComponent<Text>().color = _color;
                break;
        }
    }

}
