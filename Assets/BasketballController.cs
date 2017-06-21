using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class BasketballController : MonoBehaviour 
{
    Vector2 offset;
    Vector2 lastPosition;
    Vector2 currentPosition;
    float velocity = 5;
    float screenBounds = -8;

    private float _gravity = 2;

    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        currentPosition = transform.position;
    }

    void Update()
    {
        lastPosition = currentPosition;
        currentPosition = transform.position;

        if (transform.position.y < screenBounds)
            Destroy(gameObject);
    }

    void OnMouseDown()
    {
        //_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
            new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0; // disable gravity while mouse down
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
    }

    void OnMouseDrag()
    {
        Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 curPosition = (Vector2) Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        gameObject.transform.position = curPosition;

        //_angle = Vector2.Angle(Vector2.up, gameObject.GetComponent<Rigidbody2D>().velocity);

    }

    void OnMouseUp()
    {
        // get direction of movement in last frame
        // move object in that direction at velocity
        Vector2 v = lastPosition - currentPosition;
        //transform.position = lastPosition - currentPosition;


        gameObject.GetComponent<Rigidbody2D>().gravityScale = _gravity; // disable gravity while mouse down
    }
}