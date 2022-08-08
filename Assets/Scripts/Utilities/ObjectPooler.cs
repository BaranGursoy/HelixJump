using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
   #region Singleton

   public static ObjectPooler Instance;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }

      else
      {
         Destroy(gameObject);
      }
   }

   #endregion
   
   public Dictionary<string, Queue<GameObject>> poolDictionary;
   public List<Pool> pools;
   
   private void Start()
   {
      poolDictionary = new Dictionary<string, Queue<GameObject>>();

      foreach (var pool in pools)
      {
         Queue<GameObject> objectPool = new Queue<GameObject>();

         for (int i = 0; i < pool.poolSize; i++)
         {
            var obj = Instantiate(pool.prefab, transform, true);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
         }
         poolDictionary.Add(pool.tag, objectPool);
      }
   }

   public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rotation)
   {
      if (!poolDictionary.ContainsKey(tag))
      {
         return null;
      }
      
      var spawnedObj = poolDictionary[tag].Dequeue();
      
      spawnedObj.SetActive(true);
      spawnedObj.transform.position = pos;
      spawnedObj.transform.rotation = rotation;
      
      poolDictionary[tag].Enqueue(spawnedObj);

      return spawnedObj;
   }
}
