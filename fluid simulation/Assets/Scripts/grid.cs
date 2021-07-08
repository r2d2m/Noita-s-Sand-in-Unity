using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class grid : MonoBehaviour
{
    [SerializeField] float height;
    [SerializeField] float width;
    [SerializeField] bool showGrid; //TODO I'm currently just using it to validate
    [Space(5,order=1)]
    [Range(0, 1)] [SerializeField] float gridX;
    [Range(0, 1)] [SerializeField] float gridY;
    
    float originX;
    float originY;
    float squareX;
    float squareY;
    float sizeX;
    float sizeY;
    float posX;
    float posY;
    UnityEngine.Object crate;
    Camera cam;

    GameObject c;

    public List<(float,float)> filledList = new List<(float, float)>();

    public bool started;

    void Begin() //I've put this inside onValidate but Screen size is returned wrong
    {
        cam = FindObjectOfType<Camera>();
        Vector3 origin = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0));
        originX = origin.x;
        originY = origin.y;
        crate = Resources.Load("Prefabs/Crate");
        Vector3 coord = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));
        height = coord.y*2;
        width = coord.x*2;
        squareX = (float) Math.Round((double)(gridX * width), 1); // I'm rounding it because operations on small floats causes problems
        squareY = (float) Math.Round((double)(gridY * height),1);
    }

    private void Update()
    {
        if (c==null)
        {
            Begin();
            c = (GameObject)Instantiate(crate, new Vector3(posX, posY, 0), Quaternion.identity);
            SpriteRenderer sprite = c.GetComponent<SpriteRenderer>();
            c.transform.localScale = new Vector3(squareX, squareY);
            sizeX = sprite.bounds.size.x / 2;
            sizeY = sprite.bounds.size.y / 2;
        }
        else if  (c != null)
        {
            Vector2 mouseInWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            posX = originX + sizeX + ((int)((mouseInWorld.x - originX) / squareX)) * squareX;
            posY = originY - sizeY - ((int)((-mouseInWorld.y + originY) / squareY)) * squareY;
            c.transform.position = new Vector3(posX, posY,0);
            if (Input.GetMouseButton(0) && !filledList.Contains((posX,posY)))
            {
                particle pcl = c.GetComponent<particle>();
                pcl.reference = filledList.Count;
                pcl.launched = true;
                pcl.positionX = posX;
                pcl.positionY = posY;
                pcl.squareX = squareX;
                pcl.squareY = squareY;
                c = null;
                filledList.Add((posX, posY));
            }
        }
    }
}
