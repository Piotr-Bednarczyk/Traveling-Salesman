using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counters : MonoBehaviour
{

    [SerializeField] Text DistanceText;
    [SerializeField] Text OperationText;
    public static float DistanceCount=0f;
    public static int OperationCount=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.State!= GameManager.GameState.SelectNodes)
        {
        DistanceText.text=$"Długość ścieżki:\n{DistanceCount}";
        OperationText.text=$"Liczba operacji:\n{OperationCount}";
        }
    }
}
