using UnityEngine;
using System.Collections;
using Alta.OJECTPOOLING;
using System.Collections.Generic;

public class ObjectGenerator : MonoBehaviour
{

    /// <summary>
    /// The pools.
    /// </summary>
    private Dictionary<RecycleGameObject, ObjectPool> pools = new Dictionary<RecycleGameObject, ObjectPool>();

    public Vector2[] PostionArray;
    /// <summary>
    /// The prefabs - an array of GameObjects
    /// </summary>
    public GameObject[] gameItems;

    /// <summary>
    /// The delay spawning
    /// </summary>
    public float delay = 2.0f;

    /// <summary>
    /// activate Spawners
    /// </summary>
    public bool active = true;

    /// <summary>
    /// The delay range when spawning an object
    /// </summary>
    public Vector2 delayRange = new Vector2(1, 3);

    public int maxItem = 10;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        // setting StartCoroutine function
        StartCoroutine(GameObjectsGenerator());
    }

    /// <summary>
    /// Randoms the position.
    /// </summary>
    /// <returns>The position.</returns>
    private Vector2 RandomPosition()
    {
        return this.PostionArray[Random.Range(0, PostionArray.Length)];
    }


    public void ResetDelay()
    {
        delay = Random.Range(delayRange.x, delayRange.y);
    }

    /// <summary>
    ///  Function to generate the Game Objects
    /// </summary>
    /// <returns>The objects generator.</returns>
    IEnumerator GameObjectsGenerator()
    {
        yield return new WaitForSeconds(delay);

        if (active)
        {
            Vector2 itemPosition = RandomPosition();

            //Instantiate(gameItems[Random.Range(0, this.gameItems.Length)],
            //            itemPosition, Quaternion.identity);



            // using GameObjectUtil to instantiate a game item
            Instantiate(gameItems[Random.Range(0, this.gameItems.Length)],
                                       itemPosition);

            ResetDelay();

        }

        StartCoroutine(GameObjectsGenerator());
    }


    void OnDestroy()
    {
        pools = new Dictionary<RecycleGameObject, ObjectPool>();
    }

    

    /// <summary>
    /// Gets the object pool.
    /// </summary>
    /// <returns>The object pool.</returns>
    private ObjectPool GetObjectPool(RecycleGameObject objectRef)
    {
        ObjectPool pool = null;
        if (pools.ContainsKey(objectRef))
        {
            pool = pools[objectRef];
        }
        else
        {
            // create an object pool
            var poolContainer = new GameObject(objectRef.gameObject.name + "_ObjectPool");
            pool = poolContainer.AddComponent<ObjectPool>();
            pool.prefab = objectRef;
            pool.transform.SetParent(transform);
            RectTransform p = pool.gameObject.AddComponent<RectTransform>();
            p.anchoredPosition3D = Vector3.zero;
            pool.MaxItem = maxItem;
            pool.transform.localScale = Vector3.one;
            pools.Add(objectRef, pool);
        }

        return pool;
    }


    /// <summary>
    /// Instantiate the specified prefab and position.
    /// </summary>
    /// <param name="prefab">Prefab.</param>
    /// <param name="position">Position.</param>
    public GameObject Instantiate(GameObject prefab, Vector2 anchoredPosition)
    {
        GameObject instance = null;

        // check whether the object contains the recycle script
        var recycleScript = prefab.GetComponent<RecycleGameObject>();
        if (recycleScript != null)
        {

            // getting GameObject from the pool
            var pool = GetObjectPool(recycleScript);
            pool.NextObject(anchoredPosition);
        }
        else
        {

            // create the instance normally
            instance = GameObject.Instantiate(prefab);
            instance.transform.SetParent(transform);
            instance.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            instance.transform.localScale = Vector3.one;

        }

        return instance;
    }

    /// <summary>
    /// Destroy the specified gameObject.
    /// </summary>
    /// <param name="gameObject">Game object.</param>
    public void Destroy(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// Recycles the object.
    /// </summary>
    /// <param name="gameObject">Game object.</param>
    public void RecycleObject(GameObject gameObject)
    {

        // determine the existing of RecycleGameObject
        var recycleGameObject = gameObject.GetComponent<RecycleGameObject>();

        if (recycleGameObject != null)
        {
            recycleGameObject.DisableObject();
        }
        else
        {
            GameObject.Destroy(gameObject);
        }

        // reset Position Only
    }
}
