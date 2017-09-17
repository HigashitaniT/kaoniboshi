using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;

    public GameObject firstBody;
    
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    public BodySourceManager _BodyManager;
    public HeadController _HeadCtrl;
    public GameController _GameCtrl;

    private bool IsTrak = false;
    private bool isUpLeftHand = false;
    private bool isUpRightHand = false;

    private Vector3 rightHand;
    private Vector3 leftHand;
    private Vector3 head;
    private Vector3 rightElbow;
    private Vector3 leftElbow;

    private ulong first_id;

    public int pointLimit;
    private int bodyCount;
    private int chargePoint;

    //private static GameObject[] bodyList = new GameObject[10];

    bool isCreate = false;

    public int _ChargePoint
    {
        get { return chargePoint; }
        set { chargePoint = value; }
    }

    public bool IsTraked
    {
        get { return IsTrak; }
        set { IsTrak = value; }
    }

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        /*
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        */
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
    
    void Update () 
    {
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                IsTraked = false;
                firstBody = null;
                if (first_id == trackingId)//消えるIDが初めに取得したIDと同じかどうか
                {
                    _HeadCtrl.Collback = true;//見失った通知
                    first_id = 0;
                    isCreate = false;
                }
                Destroy(_Bodies[trackingId]);
                bodyCount--;
                
				//_HeadCtrl.ListArrangement ();
                _Bodies.Remove(trackingId);
            }
        }

        foreach(var body in data)
        {
            if (body == null)
            {
                IsTraked = false;
                continue;
            }
            
            if(body.IsTracked)
            {
                IsTraked = true;
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    bodyCount++;
                }

                RefreshBodyObject(body, _Bodies[body.TrackingId], body.TrackingId);
                StartCheck();
                EnergyCharge();
            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        
        GameObject body = new GameObject("Body:" + id);
        if (firstBody == null)
        {
            firstBody = body;
        }
        //bodyList[bodyCount] = body;
        
        
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            jointObj.GetComponent<BoxCollider>().enabled = false;
            //jointObj.GetComponent<MeshRenderer>().enabled = false;
            
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);
            
            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
            
            if(jt == Kinect.JointType.Head && first_id == id)
            {
                _HeadCtrl.CreateHead(jointObj.transform);
            }
            if (GameController.Instance.gameStates != GameController.GameStates.Title)
            {
                jointObj.SetActive(false);
            }
            else if (Kinect.JointType.HipLeft <= jt && Kinect.JointType.FootRight >= jt)
            {
                jointObj.SetActive(false);
            }

            if(Kinect.JointType.Head == jt)
            {
                jointObj.SetActive(true);
            }

            if(isCreate == true && first_id != id)
            {
                body.SetActive(false);
            }
            else// isCreate == false 一回目通る
            {
                first_id = id;
                isCreate = true;
            }
        }
        return body;
    }
    
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject, ulong id)
    {
        if(isCreate == false)
        {
            first_id = id;
            isCreate = true;
            bodyObject.SetActive(true);
        }

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;
            if (firstBody == null)
            {
                firstBody = bodyObject;
            }
            
            if(_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            if (jt == Kinect.JointType.Head && first_id == id)
            {
                _HeadCtrl.CreateHead(jointObj);
            }

            if (jt == Kinect.JointType.HandLeft && first_id == id)
            {
                leftHand = jointObj.localPosition;
            }
            else if (jt == Kinect.JointType.HandRight && first_id == id)
            {
                rightHand = jointObj.localPosition;
            }
            else if (jt == Kinect.JointType.Head && first_id == id)
            {
                head = jointObj.localPosition;
            }
            else if (jt == Kinect.JointType.ElbowLeft && first_id == id)
            {
                leftElbow = jointObj.localPosition;
            }
            else if (jt == Kinect.JointType.ElbowRight && first_id == id)
            {
                rightElbow = jointObj.localPosition;
            }
            if (GameController.Instance.gameStates != GameController.GameStates.Title)
            {
                jointObj.GetComponent<MeshRenderer>().enabled = false;
                jointObj.GetComponent<LineRenderer>().enabled = false;
            }
            else
            {
                LineRenderer lr = jointObj.GetComponent<LineRenderer>();
                if (targetJoint.HasValue)
                {
                    lr.SetPosition(0, jointObj.localPosition);
                    lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                    lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
                }
                else
                {
                    lr.enabled = false;
                }
            }
        }
    }
    
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.yellow;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    private void StartCheck()
    {
        float LhandPosY = leftHand.y;
        float RhandPosY = rightHand.y;
        float headPosY = head.y;

        if(headPosY <= LhandPosY && headPosY <= RhandPosY)
        {
            _GameCtrl.IsStart = true;
        }
    }

    private void EnergyCharge()
    {
        if (chargePoint >= pointLimit) return;
        if (leftHand.y >= leftElbow.y && isUpLeftHand == false)
        {
            isUpLeftHand = true;
        }
        if (leftHand.y <= leftElbow.y && isUpLeftHand == true)
        {
            isUpLeftHand = false;
            chargePoint++;
        }
        if (rightHand.y >= rightElbow.y && isUpRightHand == false)
        {
            isUpRightHand = true;
        }
        if (rightHand.y <= rightElbow.y && isUpRightHand == true)
        {
            isUpRightHand = false;
            chargePoint++;
        }
    }

    public Vector3 LeftHandPosition()
    {
        return leftHand;
    }
    public Vector3 RightHandPosition()
    {
        return rightHand;
    }
    public Vector3 HeadPosition()
    {
        return head;
    }
    public Vector3 LeftElbowPosition()
    {
        return leftElbow;
    }
    public Vector3 RightElbowPosition()
    {
        return rightElbow;
    }
    public GameObject FirstBody()
    {
        return firstBody;
    }
}
