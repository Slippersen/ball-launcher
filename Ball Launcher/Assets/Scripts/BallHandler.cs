using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;

    private Camera mainCamera;
    private Rigidbody2D currentBallRigidBody;
    private SpringJoint2D currentBallSpringJoint;
    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        SpawnBall();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidBody == null) 
            return;

        if (Touch.activeTouches.Count == 0)
        {
            if (isDragging)
                LaunchBall();

            isDragging = false;

            return;
        }

        isDragging = true;
        // Take rigid body out of physics control
        currentBallRigidBody.isKinematic = true;

        Vector2 touchPosition = new Vector2();

        foreach (Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition;
        }
        // Use average touch position if multiple touches
        touchPosition /= Touch.activeTouches.Count;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = worldPosition;
    }

    private void SpawnBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }

    private void LaunchBall()
    {
        // Enable physics control again
        currentBallRigidBody.isKinematic = false;
        // Fire and forget the ball
        currentBallRigidBody = null;
        
        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        // Disable spring joint
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        // Spawn new ball
        Invoke(nameof(SpawnBall), respawnDelay);
    }
}
