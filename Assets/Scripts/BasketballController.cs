using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class BasketballController : MonoBehaviour 
{
    Vector2 offset;
    Vector2 lastPosition;
    Vector2 currentPosition;
    Vector2 startPosition = new Vector2(0, -2);

    SpriteRenderer rimOverlay;

    Rigidbody2D basketball;

    BoxCollider2D rim;
    BoxCollider2D rimLeft;
    BoxCollider2D rimRight;

    public float maxVelocity = 10;
    float gravity = 2;

    float screenBoundBottom = -8;
    float screenBoundTop = 12;
    float screenBoundLeft = -10;
    float screenBoundRight = 10;

    float shotBound = .75F;
    float rimBound = 1.9F;

    bool thrown = false;

    void Start()
    {
        basketball = gameObject.GetComponent<Rigidbody2D>();
        basketball.gravityScale = 0;

        rim = GameObject.Find("Rim").GetComponent<BoxCollider2D>();
        rimRight = GameObject.Find("RimRight").GetComponent<BoxCollider2D>();
        rimLeft = GameObject.Find("RimLeft").GetComponent<BoxCollider2D>();
        rimOverlay = GameObject.Find("RimOverlay").GetComponent<SpriteRenderer>();

        gameObject.transform.position = startPosition;
        currentPosition = transform.position;
    }

    void Update()
    {
        lastPosition = currentPosition;
        currentPosition = transform.position;

        if(thrown)
        {
            if (!rim.enabled && lastPosition.y - currentPosition.y > 0) // ball has negavtive trajectory
            {
                ToggleRim();
            }
            else if(rim.enabled && lastPosition.y - currentPosition.y < 0) // ball has positive trajectory
            {
                ToggleRim();
            }

            if (transform.position.y < screenBoundBottom)
                Reset();
            else if (transform.position.x < screenBoundLeft || transform.position.x > screenBoundRight)
                Reset();
            
        }

        else if (transform.position.y > shotBound) // release ball when dragged past shooting range
            OnMouseUp();
        
        /* make ball smaller as it goes away
        else
        {
            if((currentPosition - lastPosition).y > 0F) // ball moving up
            {
                gameObject.transform.localScale.Scale(new Vector3(.001, .001));
            }
        }
        */
    }

    // ToggleRimColliders() - activates/deactivates rim edge colliders and overlay sprite
    // called after ball begins rising/falling to allow it to fall into hoop rather than hitting the bottom
    // TODO: tune rim overlay logic
    void ToggleRim()
    {
        rimLeft.enabled = !rimLeft.enabled;
        rimRight.enabled = !rimRight.enabled;
        rim.enabled = !rim.enabled;

        if (rimRight.isActiveAndEnabled)
            Debug.Log("rim ACTIVATED");
        else Debug.Log("rim deactivated");

        rimOverlay.enabled = !rimOverlay.enabled;
    }

    // Reset() - resets game after ball goes out of bounds
    void Reset()
    {
        gameObject.transform.position = startPosition;
        basketball.gravityScale = 0;
        basketball.velocity = Vector2.zero;
        thrown = false;

        // freeze rotation after respawning ball
        basketball.freezeRotation = true;
        basketball.freezeRotation = false;

        ToggleRim();
    }

    // OnMouseDown() - called when player first grabs ball
    // configures ball to be dragged around by player
    void OnMouseDown()
    {
        if(!thrown)
        {
            //_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
                new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            basketball.gravityScale = 0; // disable gravity while mouse down
            basketball.velocity = new Vector2(0,0);
        }

    }

    // OnMouseUp() - called when player releases ball to be thrown
    // gives ball tragectory and sets thrown = true
    void OnMouseUp()
    {
        // get direction of movement in last frame
        // move object in that direction at velocity
        if(!thrown)
        {
            Vector2 velocity = (currentPosition - lastPosition) * 20;
            if (velocity.y > maxVelocity)
            {
                //velocity.y = maxVelocity
                float yDiff = velocity.y - maxVelocity;
                velocity.y -= yDiff;
                //velocity.x -= yDiff;
            }

            basketball.velocity = velocity;
            basketball.gravityScale = gravity; // disable gravity while mouse down

            thrown = true;
        }
    }

    void OnMouseDrag()
    {
        if(!thrown)
        {
            Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 curPosition = (Vector2) Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            gameObject.transform.position = curPosition;
        }
    }


}