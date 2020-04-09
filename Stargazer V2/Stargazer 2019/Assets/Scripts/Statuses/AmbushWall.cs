using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushWall : MonoBehaviour
{

    public Transform targetRef;
    public GameObject effect;
    public float revealRange = 20;

    private Vector2 position;

    private List<Node> pathfindNodes = new List<Node>();

    void Start()
    {
        position = new Vector2(transform.position.x, transform.position.z);
    }

    public void AddPathFindNode(Node node)
    {
        pathfindNodes.Add(node);
    }

    void Reveal()
    {
        if (effect != null)
        {
            GameObject explode = Instantiate(effect, transform.position, transform.rotation) as GameObject;
            explode.transform.localScale = Vector3.one * transform.localScale.magnitude / 3;
            ParticleSystem parts = explode.GetComponent<ParticleSystem>();
            float totalDuration = parts.main.duration + parts.main.startLifetime.constant;
            Destroy(explode, totalDuration);
        }

        foreach (Node node in pathfindNodes)
        {
            node.walkable = true;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetRef != null)
        {
            Vector2 targetPos = new Vector2(targetRef.position.x, targetRef.position.z);
            if (Vector2.Distance(targetPos, position) <= revealRange)
            {
                Reveal();
            }
        }
    }
}
