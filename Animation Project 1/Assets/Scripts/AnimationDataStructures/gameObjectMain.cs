using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameObjectMain : MonoBehaviour
{
    public List<GameObject> ObjectHierarchy;
	public List<int> parentIndex;
	public int objCount = 0;
    public void newList()
    {
		objCount = 0;
		parentIndex = new List<int>();
        ObjectHierarchy = new List<GameObject>();
    }
    public void addObject(GameObject newObject)
    {
        ObjectHierarchy.Add(newObject);
    }
	
	public int addObjectWithIndex(GameObject currentObject, int pIndex)
	{
		objCount++;
		ObjectHierarchy.Add(currentObject);
		parentIndex.Add(pIndex);
		return objCount - 1;
	}
 
   public GameObject getObject(int index)
    {
        return ObjectHierarchy[index];
    }


}
