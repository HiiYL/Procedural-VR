using UnityEngine;
using System.Collections;

public class ObjectPooling : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize;

    private GameObject[] pool;

    void Start()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }

    public GameObject RetrieveInstance()
    {
        foreach (GameObject go in pool)
        {
            if (!go.activeSelf)
            {
                go.SetActive(true);
                return go;
            }
        }

        return null;
    }

    public void DevolveInstance(GameObject go)
    {
        if (go.GetComponent<TrailRenderer>())
        {
            go.GetComponent<TrailRenderer>().Clear();
        }
        go.SetActive(false);
    }
}
