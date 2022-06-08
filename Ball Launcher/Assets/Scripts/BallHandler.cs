using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidBody;
    [SerializeField] private SpringJoint2D currentBallSpringJoint;
    [SerializeField] private float detachDelay;

    private Camera mainCamera;
    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidBody == null) 
            return;

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
                LaunchBall();

            isDragging = false;

            return;
        }

        isDragging = true;
        // Take rigid body out of physics control
        currentBallRigidBody.isKinematic = true;

        Vector2 touchPostion = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPostion);

        currentBallRigidBody.position = worldPosition;
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
    }
}
