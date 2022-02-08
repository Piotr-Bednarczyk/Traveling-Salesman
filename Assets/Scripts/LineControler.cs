using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineControler : MonoBehaviour
{

    [SerializeField] private LineRenderer lr;
    private Transform[] points;
    // Start is called before the first frame update
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    public void SetupLine(GameObject sp,GameObject ep)
    {
        Transform[] localpoints={sp.transform,ep.transform};
        lr.positionCount = localpoints.Length;
        this.points=localpoints;
    }

    void Update()
    {
        if(points!=null)
        {
            for(int i=0;i<points.Length;i++){
                lr.SetPosition(i, points[i].position);
            }
        }
    }
}
