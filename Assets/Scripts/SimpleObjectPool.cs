using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPool : MonoBehaviour {

    //the prefab that this object pool returns instances of
    public GameObject prefab;
    //collection of currently inactive instances of the prefab
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    public GameObject GetObject()
    {
        GameObject spawnedGameObject;

        //if there is an inactive instance of the prefab ready to return, return that
        if (inactiveInstances.Count > 0)
        {
            //remove the instance form the collection of inactive instances
            spawnedGameObject = inactiveInstances.Pop();
        }
        else
        {
            spawnedGameObject = (GameObject) GameObject.Instantiate(prefab);
            
            // add the PooledObject component to the prefab so we know it came from this pool
            PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }

        // put the instance in the root of the scene and enable it
        spawnedGameObject.transform.SetParent(null);
        spawnedGameObject.SetActive(true);

        return spawnedGameObject;
    }

    // return an instance of the prefab to the pool
    public void ReturnObject(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        // if the instance came from this pool, return it to the pool
        if (pooledObject != null && pooledObject.pool == this)
        {
            // make the instance a child of this and disable it
            toReturn.transform.SetParent(transform);
            toReturn.SetActive(false);

            // add the instance to the collection of inactive instances
            inactiveInstances.Push(toReturn);
        }
        else
        {
            Debug.Log(toReturn.name + " was returned to a pool it wasn't spawned from! Destroying it.");
        }
    }

    // a component that simply identifies the pool that a GameObject came from
    public class PooledObject : MonoBehaviour
    {
        public SimpleObjectPool pool;
    }
}
