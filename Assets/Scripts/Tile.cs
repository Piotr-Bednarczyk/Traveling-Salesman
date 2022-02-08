using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject Node;
    public LayerMask Nodes;

    private bool WithNode=false;
 
    public void Init(bool isOffset) {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }
 
    void OnMouseEnter() {
        _highlight.SetActive(true);
    }
 
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void Update() {
        if(Physics2D.OverlapCircle(transform.position, 0.2f,Nodes))
            WithNode=true;
        else
            WithNode=false;
    }
    
    void OnMouseDown(){

        if(!WithNode)
        {
        GameObject techNode = Instantiate(Node, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        GameManager.Nodes.Add(techNode);
        WithNode=true;
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

}