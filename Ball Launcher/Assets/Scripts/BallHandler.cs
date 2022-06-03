using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidBody;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Do not take rigid body out of physics control
            currentBallRigidBody.isKinematic = false;
            return;
        }

        // Take rigid body out of physics control
        currentBallRigidBody.isKinematic = true;

        Vector2 touchPostion = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPostion);

        currentBallRigidBody.position = worldPosition;
    }
}
