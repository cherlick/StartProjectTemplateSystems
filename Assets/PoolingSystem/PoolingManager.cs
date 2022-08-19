using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectNameTemplate.Tools;

namespace ProjectNameTemplate.PoolingSystem
{
    public class PoolingManager : Singleton<PoolingManager>
    {

        [SerializeField] private List<PoolData> poolsToCreateInGame;
        [SerializeField] private List<PoolData> poolsToCreateInEditor;
        private Dictionary<string, Queue<GameObject>> poolOfObjects = new Dictionary<string, Queue<GameObject>>();

        

        private void Awake() 
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                AddToQueueFromEditor(this.transform.GetChild(i).gameObject);
            }
            poolsToCreateInGame.ForEach(pool => CreatePool(pool));
            
        }

#if UNITY_EDITOR
        public void CreatePoolsFromEditor()
        {
            DeletePoolsFromEditor();
            poolsToCreateInEditor.ForEach(pool => CreatePool(pool));
        }

        public void DeletePoolsFromEditor()
        {
            for (int i = this.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(this.transform.GetChild(0).gameObject);
            }
        }
#endif

        public GameObject GetObjectFromPool (string poolKey)
        {
            GameObject objectRequested = null;
            PoolData poolFromWhereObjectWasCreated = GetPoolFromList(poolKey);

            if (poolFromWhereObjectWasCreated != null)
            {
                if (poolOfObjects.TryGetValue(poolKey, out Queue<GameObject> poolList))
                {
                    Debug.Log($"count {poolList.Count} poolKey {poolKey}");
                    if (poolList.Count == 0)
                    {
                        objectRequested = poolFromWhereObjectWasCreated.allowtoIncrement 
                            ? CreateObject(poolFromWhereObjectWasCreated) 
                            : objectRequested;

                        if (objectRequested != null)
                        {
                            poolList.Enqueue(objectRequested);
                        }
                        
                    }
                    else
                    {
                        objectRequested = poolList.Dequeue();
                        if (!poolFromWhereObjectWasCreated.disableAutoEnqueue)
                        {
                            Debug.Log($"object {objectRequested.name} add to queue {poolList.Count}");
                            poolList.Enqueue(objectRequested);
                            Debug.Log($"poollist queue with key {poolKey} {poolOfObjects[poolKey].Count}");
                        }
                    }
                }
            }

            return objectRequested; 
        }

        public void ReturnObject (GameObject objectToEnqueue, string poolKey) 
        {
            if (poolOfObjects.TryGetValue (poolKey, out Queue<GameObject> poolList))
            {
                objectToEnqueue.SetActive(false);
                objectToEnqueue.transform.SetParent(transform);
                poolList.Enqueue (objectToEnqueue);
            }
        }

        private void CreatePool(PoolData newPool)
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            
            for (int i = 0; i < newPool.startSize; i++)
            {
                newQueue.Enqueue(CreateObject(newPool));
            }

            string keyName = GetPoolKey(newPool);
            if (!poolOfObjects.ContainsKey(keyName))
            {
                poolOfObjects.Add(keyName, newQueue);
            }
        }

        private void AddToQueueFromEditor(GameObject objectToAdd)
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();

            if (poolOfObjects.ContainsKey(objectToAdd.name))
            {
                poolOfObjects[objectToAdd.name].Enqueue(objectToAdd);
            }
            else
            {
                newQueue.Enqueue(objectToAdd);
                poolOfObjects.Add(objectToAdd.name, newQueue);
            }
        }

        private GameObject CreateObject (PoolData pool) 
        {
            GameObject obj = Instantiate(pool.prefab);

            obj.name = GetPoolKey(pool);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);

            return obj;
        }

        private PoolData GetPoolFromList(string poolKey)
        {
            PoolData poolRequested = null;

            poolRequested = CheckInPoolList(poolKey, poolsToCreateInGame);

            return poolRequested == null ? CheckInPoolList(poolKey, poolsToCreateInEditor) : poolRequested;
        }

        private PoolData CheckInPoolList(string poolKey, List<PoolData> listToCicle)
        {
            foreach (var pool in listToCicle)
            {
                if (pool.useNewKey)
                {
                    if (pool.newKey == poolKey)
                    {
                        return pool;
                    }
                }
                else if (pool.prefab.name == poolKey)
                {
                    return pool;
                }
            }
            return null;
        }

        private string GetPoolKey(PoolData pool) => pool.useNewKey ? pool.newKey : pool.prefab.name;
    }

}

