using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrate : MonoBehaviour
{
    public Transform _srcLeft;
    public Transform _srcRight;
    public Transform _srcHMD;
    private GameObject _srcMid;

    public Transform _destLeft;
    public Transform _destRight;
    private GameObject _destMid;
    private GameObject _destHMD;

    public GameObject _prefabMid;
    public GameObject _prefabHMD;

    public Rect _buttonRect;

    void Start(){
        _srcMid = Instantiate(_prefabMid, new Vector3(0, 0, 0), Quaternion.identity);
       _srcMid.name = "SrcMid";
        _destMid = Instantiate(_prefabMid, new Vector3(0, 0, 0), Quaternion.identity);
       _destMid.name = "DestMid";

       _destHMD = Instantiate(_prefabHMD, new Vector3(10,10, 10), Quaternion.identity);
    }

    void OnGUI()
    {
        if (GUI.Button(_buttonRect, "CalibrateFromVRControllers"))
        {
            //DoCalibrate();
        }
    }


    public void DoCalibrate(Transform src, Transform dest)
    {       
        // direction from controller mid to base mid point
        Vector3 translate = dest.position - src.position;       

        // Compute the difference in orientation from previous directions
       src.SetPositionAndRotation(dest.position,dest.rotation) ;
    }


    public void Update(){
        Vector3 srcRightToLeft = _srcLeft.position - _srcRight.position;
        Vector3 srcMidPos = _srcRight.position + srcRightToLeft*0.5f; 
        _srcMid.transform.position = srcMidPos;
        
        Vector3 hmdToMid = srcMidPos - _srcHMD.position;

        Vector3 destRightToLeft = _destLeft.position - _destRight.position;   
        Vector3 destMidPos = _destRight.position + destRightToLeft*0.5f;
        _destMid.transform.position = destMidPos;
        
        Quaternion rotSrc2Dest = Quaternion.FromToRotation(srcRightToLeft, destRightToLeft);        
        _destMid.transform.rotation = rotSrc2Dest;
        
        _destHMD.transform.position = destMidPos - rotSrc2Dest * hmdToMid;
        Quaternion rotHmd2Src = Quaternion.FromToRotation(_srcHMD.forward, -srcRightToLeft);
        rotHmd2Src = Quaternion.Inverse(rotHmd2Src);
        _destHMD.transform.rotation = rotSrc2Dest*rotHmd2Src;
    }
}
