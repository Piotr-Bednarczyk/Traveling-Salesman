using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    Camera cam;
    LineRenderer lr;


    // Start is called before the first frame update
    void Start()
    {
        lr= GetComponent<LineRenderer>();
        //cam=Camera.main;
        //startPos=gameObject.transform.position;
        //endPos=gameObject.transform.position;
        lr.enabled=true;
    }

    // Update is called once per frame
    void Update()
    {
        
        lr.SetPosition(0, startPos);
        lr.SetPosition(1,endPos);
        
    }

    public void StartDrawing(GameObject secondObject){
        startPos=gameObject.transform.position;
        startPos.z=0;
        endPos = secondObject.transform.position;
        endPos.z = 0;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
