using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        if(GameManager.State==GameManager.GameState.SelectNodes)
        {
            GameManager.Nodes.Remove(gameObject);
            Destroy (gameObject);
        }
    }
}
