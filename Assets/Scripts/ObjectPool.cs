using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public List<GameObject> objectsToPool;
    public int amountToPool;

    private void Awake() {
        SharedInstance = this;
    }

    private void Start() {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i<amountToPool; i++) {
            tmp = Instantiate(objectsToPool[0]);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
        for(int i = 0; i<amountToPool; i++) {
            tmp = Instantiate(objectsToPool[1]);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject(int elementNum){
        int num=0;
        int AmountToPool = amountToPool;
        if(elementNum == 1) {
            num += amountToPool;
            AmountToPool += amountToPool;
        }
        for(int i=num; i<AmountToPool; i++){
            if(!pooledObjects[i].activeInHierarchy){
                return pooledObjects[i];
            }
        }
        return null;
    }
}
