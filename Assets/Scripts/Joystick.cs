using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public Vector2 startPoint, endPoint, initialPosition;
    public bool toutch = false;
    public Player player;
    public float xmin, xmax, ymin, ymax;
    private void Start()
    {
        Input.multiTouchEnabled = true;
        initialPosition = transform.position;
    }
    //private void OnMouseDown()
    //{
    //    startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    print("tg");
    //}
    //private void OnMouseDrag()
    //{
    //    toutch = true;
    //    endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //}
    //private void OnMouseUp()
    //{
    //    toutch = false;
    //    transform.position = initialPosition;
    //}
    void Update()
    {
        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if(myTouches[i].deltaPosition.x <xmax && myTouches[i].deltaPosition.x > xmin && myTouches[i].deltaPosition.y < ymax && myTouches[i].deltaPosition.y > ymin)
            {
                endPoint = myTouches[i].deltaPosition;
                Vector3 offset = endPoint - startPoint;
                Vector3 direction = Vector3.ClampMagnitude(offset, 1f);
                player.MovePlayer(direction, endPoint);
                transform.position = new Vector3(startPoint.x + direction.x, startPoint.y + direction.y);
            }
        }
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                //toutch = true;
                endPoint = touch.deltaPosition;
                Vector3 offset = endPoint - startPoint;
                Vector3 direction = Vector3.ClampMagnitude(offset, 1f);
                player.MovePlayer(direction, endPoint);
                transform.position = new Vector3(startPoint.x + direction.x, startPoint.y + direction.y);
            }
        }
        // Do a thing with that touch

        if (toutch)
        {
            //Vector3 offset = endPoint - startPoint;
            //Vector3 direction = Vector3.ClampMagnitude(offset, 1f);
            //player.MovePlayer(direction,endPoint);
            //transform.position = new Vector3(startPoint.x + direction.x, startPoint.y + direction.y);
        }
    }
}
