using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public GameObject[] mapTiles;
    public GameObject parentObject;

    bool first = true;

    int totalTiles;
    public int targetTiles = 2  ;
    public int currentRowTiles = 0;
    public int targetRowTiles = 8;

    public float sizeOfObject = 0.5f;

    void Start()
    {
        while (totalTiles != targetTiles)
        {
            if (!first)
            {
                if (currentRowTiles > 0)
                {
                    if (currentRowTiles % (targetRowTiles / 4) == 0 && currentRowTiles != 1)
                    {
                        transform.Translate(Vector3.forward * sizeOfObject);
                        transform.Rotate(90, 0, 0);
                    }
                    else
                    {
                        transform.Translate(Vector3.forward * sizeOfObject);
                    }
                }
                else
                {
                    transform.Rotate(-90, 0, 0);

                    transform.position = new Vector3(transform.position.x - sizeOfObject, transform.position.y - sizeOfObject, 0);
                }
                currentRowTiles++;
            }
            else
            {
                transform.position = Vector3.zero;
                first = false;
            }

            var newObject = Instantiate(mapTiles[UnityEngine.Random.Range(0, mapTiles.Count() - 1)], transform.position, Quaternion.identity);
            newObject.transform.parent = parentObject.transform;

            if (currentRowTiles == targetRowTiles)
            {
                targetRowTiles += 8;
                currentRowTiles = 0;
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.position = new Vector3(transform.position.x - sizeOfObject, transform.position.y, 0);
            }

            totalTiles++;

        }
    }

    IEnumerator TileSpawner()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (!first)
        {
            if (currentRowTiles > 0)
            {
                if (currentRowTiles % (targetRowTiles / 4) == 0 && currentRowTiles != 1)
                {
                    transform.Translate(Vector3.forward * sizeOfObject);
                    transform.Rotate(90, 0, 0);
                }
                else
                {
                    transform.Translate(Vector3.forward * sizeOfObject);
                }
            }
            else
            {
                transform.Rotate(-90, 0, 0);

                transform.position = new Vector3(transform.position.x - sizeOfObject, transform.position.y - sizeOfObject, 0);
            }
            currentRowTiles++;
        }
        else
        {
            transform.position = Vector3.zero;
            first = false;
        }

        var newObject = Instantiate(mapTiles[UnityEngine.Random.Range(0, mapTiles.Count() - 1)], transform.position, Quaternion.identity);
        newObject.transform.parent = parentObject.transform;

        if (currentRowTiles == targetRowTiles)
        {
            targetRowTiles += 8;
            currentRowTiles = 0;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.position = new Vector3(transform.position.x - sizeOfObject, transform.position.y, 0);
        }

        totalTiles++;
        StartCoroutine(TileSpawner());
    }
}
