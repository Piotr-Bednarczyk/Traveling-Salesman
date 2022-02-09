using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameState State;
    public static event Action<GameState> OnGameStageChanged;
    public static List <GameObject> Nodes = new List<GameObject>();
    public static float TimesBetweenLines=0.1f;
    public static float TimesBetweenPaths=0.2f;
    public Text OperationCounter;
    [SerializeField] Text AlogrythmName;
    [SerializeField] LineDrawer line;
    [SerializeField] LineDrawer line2;   
    LineDrawer tech;
    List <LineDrawer> Lines= new List<LineDrawer>();
    private int linecounter=0; 


    [SerializeField] GameObject Podsumowanie;  
    [SerializeField] Text GreedyAlgorythm;
    private int GreedyAlgorythmOperationCounter;
    private float GreedyAlgorythmDistanceCounter;

    [SerializeField] Text BruteAlgorythm;
    private int BruteOperationCounter=0;
    private float BruteDistanceCounter;

    [SerializeField] Text KarpAlgorythm;
    private int KarpOperationCounter=0;
    private float KarpDistanceCounter;

    [SerializeField] Text Optymalna;


    public void ChangeTime(float speed){
        TimesBetweenLines=speed;
        TimesBetweenPaths=1.1f * speed;
    }


    private class Path{
        public List <int> Visited;
        public float value;

        public Path(List <int> Visitedx, float valuex)
        {
            Visited=Visitedx;
            value=valuex;
        }

        public void Print(){
            string log="Path:";
            for(int i=0;i<Visited.Count;i++)
            { 
                    log+=Visited[i];
                    log+=",";
            }
                log+=$": {value}";
                Debug.Log(log);
        }

        public string PrintToString(){
            string log="Path:";
            for(int i=0;i<Visited.Count;i++)
            { 
                    log+=Visited[i];
                    log+=",";
            }
                log+=$": {value}";
                return log;
        }

        public int Last(){
            return Visited[Visited.Count-1];
        }

        public bool Compare(Path second){
            if(this.Visited[Visited.Count-1]!=second.Visited[second.Visited.Count-1]) return false;
            for(int i=0;i<this.Visited.Count;i++)
            {
                bool isnotpresent=true;
                for (int j=0;j<second.Visited.Count;j++)
                {
                    if(this.Visited[i]==second.Visited[j])
                    {
                        isnotpresent=false;
                        break;
                    }
                }
                if(isnotpresent) return false;
            }
            return true;
        }
    }
    Path Optimal;

    IEnumerator Draw(Path Certain)
    {
        for(int i=Lines.Count-1;i>=0;i--)
        {
            if(Lines[i]!=null)
                Lines[i].DestroyGameObject();
        }
        for(int i=0;i<Certain.Visited.Count-1;i++)
        {
            if(linecounter%2==0) tech=Instantiate(line, Nodes[Certain.Visited[i]].transform.position, Quaternion.identity);
            else tech=Instantiate(line2, Nodes[Certain.Visited[i]].transform.position, Quaternion.identity);
            linecounter++;
            tech.StartDrawing(Nodes[Certain.Visited[i+1]]);
            Lines.Add(tech);
            yield return new WaitForSeconds(TimesBetweenLines);
        }       
    }

    IEnumerator DrawLastTables(List <List <Path>> Table){
        Counters.OperationCount=0;
        Counters.DistanceCount=Table[Table.Count-1][0].value;
        BruteDistanceCounter=Counters.DistanceCount;
            for(int j=0;j<Table[Table.Count-1].Count;j++)
            {
                for(int i=Lines.Count-1;i>=0;i--)
                {
                    if(Lines[i]!=null)
                        Lines[i].DestroyGameObject();
                }
                for(int i=0;i<Table[Table.Count-1][j].Visited.Count-1;i++)
                {
                    if(linecounter%2==0) tech=Instantiate(line, Nodes[Table[Table.Count-1][j].Visited[i]].transform.position, Quaternion.identity);
                    else tech=Instantiate(line2, Nodes[Table[Table.Count-1][j].Visited[i]].transform.position, Quaternion.identity);
                    linecounter++;
                    tech.StartDrawing(Nodes[Table[Table.Count-1][j].Visited[i+1]]);
                    Lines.Add(tech);
                    Counters.OperationCount++;
                    BruteOperationCounter++;
                    yield return new WaitForSeconds(TimesBetweenLines);
                }
                if(Counters.DistanceCount>=Table[Table.Count-1][j].value) {
                    Counters.DistanceCount=Table[Table.Count-1][j].value;
                    BruteDistanceCounter=Counters.DistanceCount;
                }
                
                yield return new WaitForSeconds(TimesBetweenPaths);
            }
            StartCoroutine(Draw(Optimal));
    }  

    IEnumerator DrawKarp(List <List <Path>> Table){
        Counters.OperationCount=0;
        Counters.DistanceCount=Table[Table.Count-1][0].value;
        KarpDistanceCounter=Counters.DistanceCount;
        int index=0;
        while(index<Table.Count)
        {
            for(int j=0;j<Table[index].Count;j++)
            {
                for(int i=Lines.Count-1;i>=0;i--)
                {
                    if(Lines[i]!=null)
                        Lines[i].DestroyGameObject();
                }
                for(int i=0;i<Table[index][j].Visited.Count-1;i++)
                {
                    if(linecounter%2==0) tech=Instantiate(line, Nodes[Table[index][j].Visited[i]].transform.position, Quaternion.identity);
                    else tech=Instantiate(line2, Nodes[Table[index][j].Visited[i]].transform.position, Quaternion.identity);
                    linecounter++;
                    tech.StartDrawing(Nodes[Table[index][j].Visited[i+1]]);
                    Lines.Add(tech);
                    yield return new WaitForSeconds(TimesBetweenLines);
                }
                Counters.OperationCount++; 
                KarpOperationCounter++;
                if(index==Table.Count-1 && Counters.DistanceCount>=Table[index][j].value) {
                    Counters.DistanceCount=Table[index][j].value;
                    KarpDistanceCounter=Counters.DistanceCount;
                }
                yield return new WaitForSeconds(TimesBetweenPaths);
            }
            index++;
        }
    }  


    private void Awake() {
        instance=this;
    }

    public void UpdateGameState(GameState newState){

        State=newState;

        switch(newState){
            case GameState.SelectNodes:
                AlogrythmName.text="Wybierz wierzchołki Grafu(Max 8)";
                //do stuff
                break;
            case GameState.DrawGraphs:
                AlogrythmName.text="Graf pełny o podanych wierzchołkach";
                
                for(int i=GridManager.Tiles.Count-1;i>=0;i--)
                {
                    GridManager.Tiles[i].DestroyGameObject();
                }
                GridManager.Tiles=new List<Tile>();
                if(Nodes.Count>=2){
                    for(int i=0;i<Nodes.Count-1;i++)
                        for(int j=i+1;j<Nodes.Count;j++)
                            {
                                tech=Instantiate(line, Nodes[i].transform.position, Quaternion.identity);
                                tech.StartDrawing(Nodes[j]);
                                Lines.Add(tech);
                            }
                }
                break;
            
            case GameState.GreedyAlgorythm:
                 
                AlogrythmName.text="Algorytm Zachłanny";
                for(int i=Lines.Count-1;i>=0;i--)
                {
                    if(Lines[i]!=null)
                        Lines[i].DestroyGameObject();
                }
                float optimaldistance=0f;
                int numberofoperations=0;
                if(Nodes.Count>=2){
                    List <bool> Visited = new List<bool>();
                    for(int i=0;i<Nodes.Count;i++)
                        Visited.Add(false);

                    GameObject CurrentNode = Nodes[0];
                    Visited[0]=true;

                    int CurrentLowestIndex=1;
                    float CurrentLowestDistance=1;
                    while(true)
                    {
                        int j=0;
                        for(;j<Visited.Count;j++)
                        {
                            if(!Visited[j]) 
                            {
                                CurrentLowestIndex=j;
                                CurrentLowestDistance = Vector3.Distance(CurrentNode.transform.position, Nodes[j].transform.position);  
                                break;
                            } 
                            
                        }

                        for(int i=j;i<Nodes.Count;i++)
                        {
                            if(Visited[i]==true) continue;
                            else
                                numberofoperations++;

                            if(Vector3.Distance(CurrentNode.transform.position, Nodes[i].transform.position) < CurrentLowestDistance)
                            {
                                CurrentLowestDistance = Vector3.Distance(CurrentNode.transform.position, Nodes[i].transform.position);
                                CurrentLowestIndex=i;
                                
                            }

                        }
                        if(linecounter%2==0) tech=Instantiate(line, CurrentNode.transform.position, Quaternion.identity);
                        else tech=Instantiate(line2, CurrentNode.transform.position, Quaternion.identity);
                        linecounter++;
                        tech.StartDrawing(Nodes[CurrentLowestIndex]);
                        Lines.Add(tech);
                        Visited[CurrentLowestIndex]=true;
                        CurrentNode=Nodes[CurrentLowestIndex];
                        optimaldistance+=CurrentLowestDistance;
                        bool check=true;
                        for(int i=0;i<Visited.Count;i++)
                            if(!Visited[i]) check=false;
                        if(check)
                            break;
                    }
                    tech=Instantiate(line, CurrentNode.transform.position, Quaternion.identity);
                    tech.StartDrawing(Nodes[0]);
                    Lines.Add(tech);
                    optimaldistance+=Vector3.Distance(CurrentNode.transform.position, Nodes[0].transform.position);

                }
                Counters.DistanceCount=optimaldistance;
                Counters.OperationCount=numberofoperations;
                GreedyAlgorythmDistanceCounter=optimaldistance;
                GreedyAlgorythmOperationCounter=numberofoperations;

                break;

            case GameState.BruteForce:
                AlogrythmName.text="Algorytm Brute Force";
                Debug.Log("Brute Force");
                
                BruteForce();
                //StartCoroutine(Draw(Optimal));
                break;
            case GameState.HeldKarp:
                AlogrythmName.text="Algorytm Helda-Karpa";
                StopAllCoroutines();
                Debug.Log("Karpie");
                HeldKarpAlgorithm();
                //StartCoroutine(Draw(Optimal));
                break;
            case GameState.Finished:
                StopAllCoroutines();
                Podsumowanie.SetActive(true);
                GreedyAlgorythm.text=$"Znaleziona odległość:{GreedyAlgorythmDistanceCounter}\nIlość operacji:{GreedyAlgorythmOperationCounter}\n\nRóżnica między znalezioną a optymalną trasą:\n{GreedyAlgorythmDistanceCounter-Optimal.value}\nWzględnie:{GreedyAlgorythmDistanceCounter/Optimal.value*100}% \n optymalnej trasy.";
                BruteAlgorythm.text=$"Znaleziona odległość:{BruteDistanceCounter}\nIlość operacji:{BruteOperationCounter}\n\nMożliwe było przerwanie działania algorytmu co poskutkuje nieoptymalnym wynikiem.";
                KarpAlgorythm.text=$"Znaleziona odległość:{KarpDistanceCounter}\nIlość operacji:{KarpOperationCounter}\n\nMożliwe było przerwanie działania algorytmu co poskutkuje nieoptymalnym wynikiem.";
                Optymalna.text=$"Optymalna odległość:{Optimal.value}\n\nKliknij aby kontynuować";
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            // default:
            //     throw new ArgumentOutofRangeException(nameof(newState),newState,null);
        }

        OnGameStageChanged?.Invoke(newState);
    }

    //========================================================================================================
    //Brute Force
    //========================================================================================================
    
    private void BruteForce( ){

        List <List <Path>> Table=new List<List <Path>>();         
        for(int i=1;i<Nodes.Count;i++)
        {
            Table.Add(new List<Path>());
            Table[0].Add( new Path(new List<int>(){0,i} ,
            Vector3.Distance(Nodes[0].transform.position,Nodes[i].transform.position)));
        }

        for(int i=0;i<Table.Count;i++)
            for(int j=0;j<Table[i].Count;j++)
            {
                for(int nodenumber=0;nodenumber<Nodes.Count;nodenumber++)
                {
                    bool notpresent=true;
                    for(int k=0;k<Table[i][j].Visited.Count;k++)
                        {   
                            if(nodenumber==Table[i][j].Visited[k])
                            {
                                notpresent=false;
                                break;
                            }
                        }
                    if(notpresent){
                    List <int> tech2=new List<int>(Table[i][j].Visited);
                    tech2.Add(nodenumber); 
                    float tech3= Table[i][j].value + Vector3.Distance( 
                        Nodes[Table[i][j].Last()].transform.position, 
                        Nodes[nodenumber].transform.position);
                    Table[i+1].Add(new Path( tech2 ,  tech3));
                    }
                }
            }

        Table.Add(new List<Path>());
        int lastcolumn=Table.Count-1;          
        for(int i=0;i<Table[lastcolumn-1].Count;i++)
        {

            List <int> tech2=new List<int>(Table[lastcolumn-1][i].Visited);
            tech2.Add(0); 
            float tech3=Table[lastcolumn-1][i].value+Vector3.Distance(
                Nodes[Table[lastcolumn-1][i].Last()].transform.position,
                 Nodes[0].transform.position);
            Table[lastcolumn].Add(new Path( tech2 ,  tech3));

        }

        Optimal=Table[Table.Count-1][0];
        for(int i=1;i<Table[Table.Count-1].Count;i++){
        if(Optimal.value>Table[Table.Count-1][i].value)
            Optimal=Table[Table.Count-1][i];
        }

        //Optimal.Print();
        
        StartCoroutine(DrawLastTables(Table));

        // //==Wypisywanie===
        //     for(int j=0;j<Table[Table.Count-1].Count;j++)
        //     {
        //         Table[Table.Count-1][j].Print();
        //     }
        // //===============

    }
    
    //========================================================================================================
    //Hold my carp
    //========================================================================================================
    private void HeldKarpAlgorithm( ){
        List <List <Path>> Table=new List<List <Path>>();
        for(int i=1;i<Nodes.Count;i++)
        {
            Table.Add(new List<Path>());
            Table[0].Add( new Path(new List<int>(){0,i} , Vector3.Distance(Nodes[0].transform.position,Nodes[i].transform.position)));
        }

        for(int i=0;i<Table.Count-1;i++)
        {
            for(int j=0;j<Table[i].Count;j++)
            {
                for(int nodenumber=0;nodenumber<Nodes.Count;nodenumber++)
                {
                    bool notpresent=true;
                    for(int k=0;k<Table[i][j].Visited.Count;k++)
                        {   
                            if(nodenumber==Table[i][j].Visited[k])
                            {
                                notpresent=false;
                                break;
                            }
                        }
                    if(notpresent){
                    List <int> tech2=new List<int>(Table[i][j].Visited);
                    tech2.Add(nodenumber); 
                    float tech3=Table[i][j].value+Vector3.Distance(Nodes[Table[i][j].Last()].transform.position, Nodes[nodenumber].transform.position);
                    Table[i+1].Add(new Path( tech2 ,  tech3));
                    }
                }
            }
   
            for(int j=0;j<Table[i+1].Count-1;j++)
            {
                for(int k=j+1;k<Table[i+1].Count;k++)
                {
                    if(Table[i+1][j].Compare(Table[i+1][k]))
                    {
                        if(Table[i+1][k].value>=Table[i+1][j].value)
                        {
                            Table[i+1].RemoveAt(k);
                            k--; 
                        }
                        else
                        {
                            Table[i+1].RemoveAt(j);
                            j--;
                            break;
                        }
                    }                    
                }
            }
                       
        }

        Table.Add(new List<Path>());
        int lastcolumn=Table.Count-1;          
        for(int i=0;i<Table[lastcolumn-1].Count;i++)
        {

            List <int> tech2=new List<int>(Table[lastcolumn-1][i].Visited);
            tech2.Add(0); 
            float tech3=Table[lastcolumn-1][i].value+Vector3.Distance(Nodes[Table[lastcolumn-1][i].Last()].transform.position, Nodes[0].transform.position);
            Table[lastcolumn].Add(new Path( tech2 ,  tech3));

        }


        Optimal=Table[Table.Count-1][0];
        for(int i=1;i<Table[Table.Count-1].Count;i++){
        if(Optimal.value>Table[Table.Count-1][i].value)
            Optimal=Table[Table.Count-1][i];
        }

        StartCoroutine(DrawKarp(Table));

    }

    


    private int count=0;
    public void OnClick(int which){
        count+=which;
        if(count==1) UpdateGameState(GameState.DrawGraphs);
        else if(count==2) UpdateGameState(GameState.GreedyAlgorythm);
        else if(count==3) UpdateGameState(GameState.BruteForce);
        else if(count==4) UpdateGameState(GameState.HeldKarp);
        else if(count==5) UpdateGameState(GameState.Finished);
        else
        Debug.Log("DONE");
    }


    public enum GameState{
        SelectNodes,
        GreedyAlgorythm,
        BruteForce,
        HeldKarp,
        DrawGraphs,
        Finished
    }
}
