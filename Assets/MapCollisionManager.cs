using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MapCollisionManager : MonoBehaviour
{
    public GameObject targetPrefab;

    public List<Vector3> hasBeen = new List<Vector3>();

    public List<Vector3> tar = new List<Vector3>();

    bool foundTarget = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (other.name == gameObject.transform.parent.name)
            {
                gameObject.GetComponent<MapCollisionManager>().hasBeen.Add(gameObject.transform.position);
                gameObject.transform.parent.GetComponent<Entity>().path.AddRange( gameObject.GetComponent<MapCollisionManager>().hasBeen);
                gameObject.transform.parent.GetComponent<Entity>().currentState = Entity.EntityState.Travelling;
                foundTarget = true;

                foreach (Transform g in gameObject.transform.parent.transform)
                {
                    Destroy(g.gameObject);
                }
            }
            else
            {
                print($"Found non-target entity at {gameObject.transform.position}");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7 && !foundTarget)
        {
            var currentPos = gameObject.transform.position;

            List<Vector3> nearbyPaths = new List<Vector3>();
            gameObject.GetComponent<MapCollisionManager>().hasBeen.Add(currentPos);

            foreach (GameObject g in GameObject.Find("Pathing").GetComponent<Pathing>().Paths)
            {
                if (Vector3.Distance(g.transform.position, currentPos) <= 1.3f && Vector3.Distance(g.transform.position, currentPos) != 0)
                {
                    nearbyPaths.Add(g.transform.position);
                }
            }

            foreach (Vector3 v in nearbyPaths)
            {
                bool _hasBeen = false;

                foreach (Vector3 e in hasBeen)
                {
                    if (e == v)
                    {
                        _hasBeen = true;
                        break;
                    }
                }

                if (!_hasBeen)
                {
                    tar.Add(v);
                }
            }

            bool first = true;
            foreach (Vector3 v in tar)
            {
                if (first)
                {
                    StartCoroutine(moveThisObject(v));
                    first = false;
                }
                else
                {
                    StartCoroutine(MakeNewObject(v, hasBeen));
                }
            }

            tar.Clear();
        }
    }

    public IEnumerator moveThisObject(Vector3 targetPos)
    {
        yield return null;

        gameObject.transform.position = targetPos;
    }

    public IEnumerator MakeNewObject(Vector3 targetPos, List<Vector3> existingHasBeen)
    {
        yield return null;

        var makeNewObject = Instantiate(targetPrefab, targetPos, Quaternion.identity);

        makeNewObject.transform.SetParent( gameObject.transform.parent);
        makeNewObject.GetComponent<MapCollisionManager>().hasBeen = new List<Vector3>();
        makeNewObject.GetComponent<MapCollisionManager>().hasBeen.AddRange(existingHasBeen);
        makeNewObject.transform.localScale = gameObject.transform.localScale;
    }
}