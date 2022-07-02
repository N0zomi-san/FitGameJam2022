using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainP : MonoBehaviour
{
    // Start is called before the first frame update
     public float mousespeed = 20f;
    Vector2 lastClickedPos;
    public Vector3 StartPos;
    public Vector3 EndPos;

    public GameObject Stamp;
    public GameObject Paper;
    public GameObject Certified;
    GameObject b;   
    GameObject c;
    GameObject d;
    bool De;
    bool Gold;
    int Points;

    Ray ray;
    RaycastHit hit;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        {
            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float step  = mousespeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         if(Physics.Raycast(ray, out hit))
         {
             print (hit.collider.name);
         }
    }

    public void OnMouseOver ()
    {
        float step  = mousespeed * Time.deltaTime;
         if(Input.GetMouseButtonDown(0))
            {
                if (b) Destroy(b);
                if (De == false)
                {
                b = Instantiate(Stamp);
                b.transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);
                RemovePaper();
                GivePaper();
                }
            }
         if(Input.GetMouseButtonDown(1))
            {
                //GivePaper();
                //b.AddComponent<SpriteRenderer>().color = Color.red;
            
            }
    }

    void GivePaper()
    {
        if (c == false)
        {
            float step  = mousespeed * Time.deltaTime;
            c = Instantiate(Paper);
            c.transform.position = StartPos;
            c.transform.position = EndPos;
            if (20 <= Random.Range(0f, 100.0f) )
            {
                d = Instantiate(Certified);
                d.transform.position = EndPos;
                d.transform.parent = c.transform;
                Gold = true;
            } else Gold = false;
        }
    }

    void RemovePaper()
    {
        if (c)
        {
            Destroy(c);
            IncreasePoint();
        }
    }

    void IncreasePoint()
    {
        if (Gold)
        {
            Points += 1;
            Debug.Log(Points); 
        }
    }
}

