using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
