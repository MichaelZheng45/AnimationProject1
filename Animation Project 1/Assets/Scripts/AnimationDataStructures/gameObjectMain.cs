using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameObjectMain : MonoBehaviour
{
    public AnimationData animData;
    public List<GameObject> ObjectHierarchy;

    public void newList()
    {
        ObjectHierarchy = new List<GameObject>();
    }
    public void addObject(GameObject newObject)
    {
        ObjectHierarchy.Add(newObject);
    }

    public GameObject getObject(int index)
    {
        return ObjectHierarchy[index];
    }

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
