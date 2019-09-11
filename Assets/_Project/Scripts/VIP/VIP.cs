using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VIP : MonoBehaviour
{
    public GameObject target;
    public float maxRadius;
    public float minRadius;
    public float speed;

    public Vector3 currentDest;

    enum State
    {
        None,
        Enroute,
        Ponder
    }
    private State state;
    Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        state = State.None;
        StartCoroutine("Ponder");
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Enroute)
        {
            if (Vector3.Distance(transform.position, currentDest) < 0.3)
            {
                state = State.Ponder;
                body.velocity = Vector3.zero;
                StartCoroutine("Ponder");
            }
            else
            {
                
                Vector3 direction = (currentDest - transform.position).normalized;
                body.velocity = direction * speed * Time.deltaTime;
            }
        }
        if (Vector3.Distance(currentDest, target.transform.position) > maxRadius)
        {
            currentDest = genRandomDest();
            state = State.Enroute;
        }
    }

    IEnumerator Ponder()
    {
        float randWait = Random.Range(1f, 2f);
        print(randWait);
        yield return new WaitForSeconds(randWait);
        if (state != State.Enroute)
        {
            currentDest = genRandomDest();
            state = State.Enroute;
        }
    }

    Vector3 genRandomDest()
    {
        Vector3 targetPos = target.transform.position;
        float randRad = Random.Range(minRadius, maxRadius);
        float randAngle = Random.Range(0, 2 * Mathf.PI);
        // Convert to polar to cartesian coordinate
        float x = randRad * Mathf.Cos(randAngle);
        float y = randRad * Mathf.Sin(randAngle);
        Vector3 dest = new Vector3(x, y, 0);
        return targetPos + dest;
    }

}
