using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour
{

    grid GRID;
    public float positionX;
    public float positionY;
    public int reference; //Would be easier on C
    public float squareX;
    public float squareY;
    public bool launched;

    [SerializeField] float inactiveTime = 0f;
    [SerializeField] float timeofMove = 1f;
    [SerializeField] bool isStatic;
    [SerializeField] Color color = Color.white;

    [SerializeField] bool isFluid;

    bool active;
    bool isRunning;
    bool checkOnce;
    void Start()
    {
        GRID = FindObjectOfType<grid>();
        StartCoroutine(ACTIVATE(inactiveTime));
        GetComponent<SpriteRenderer>().color = color;
    }

    void OnValidate()
    {
        GetComponent<SpriteRenderer>().color = color;
    }
    void Update()
    {
        if (launched && active && !isStatic && GRID.started)
        {
            GRID.filledList[reference] = (positionX, positionY);
            if (!isRunning)
            {
                isRunning = true;
                StartCoroutine(MOVE(new Vector3(0,-squareY,0),timeofMove));
            }
        }
    }
    IEnumerator ACTIVATE(float time)
    {
        yield return new WaitForSeconds(time);
        active = true;
    }
    IEnumerator MOVE(Vector3 dir,float time)
    {
        while (isRunning)
        {
           yield return new WaitForSeconds(time);
           Transform t = this.transform;
            if (us.listContains(GRID.filledList, (t.position.x + dir.x, t.position.y + dir.y), 0.001f))
            {
                if (!us.listContains(GRID.filledList, ((t.position.x + squareX), t.position.y - squareY), 0.001f))
                {
                    t.position += new Vector3(squareX, -squareY, 0);
                    positionX += squareX;
                    positionY -= squareY;
                    GRID.filledList[reference] = (t.position.x, t.position.y); //i'm using posistioX and Y it does not make any sens
                    continue;
                }
                else if (!us.listContains(GRID.filledList, ((t.position.x - squareX), t.position.y - squareY), 0.001f))
                {
                    t.position -= new Vector3(squareX, squareY, 0);
                    positionX -= squareX;
                    positionY -= squareY;
                    GRID.filledList[reference] = (t.position.x, t.position.y);
                    continue;
                }
                else if (isFluid && !checkOnce)
                {
                    if (!us.listContains(GRID.filledList, (t.position.x + squareX, t.position.y), 0.001f))
                    {
                        checkOnce = true;
                        t.position += Vector3.right * squareX;
                        positionX += squareX;
                        continue;
                    }
                    else if (!us.listContains(GRID.filledList, (t.position.x - squareX, t.position.y), 0.001f))
                    {
                        checkOnce = true;
                        t.position += Vector3.left * squareX;
                        positionX -= squareX;
                        continue;
                    }
                    else break;
                }
                else break;
            }
           t.position+= dir;
           positionX += dir[0];
           positionY += dir[1];
           GRID.filledList[reference] = (t.position.x, t.position.y);
        }
        isRunning = false;
        yield return null;
    }

}
