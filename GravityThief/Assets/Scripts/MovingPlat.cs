using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlat : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int startingPoint;
    [SerializeField] Transform[] points;

    int i;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, points[i].position) < 0.02f){
            i++;
            if(i == points.Length){
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
}
