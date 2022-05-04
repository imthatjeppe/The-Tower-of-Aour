using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement:MonoBehaviour {

    Vector3 playerPosition, relativeForward, relativeRight;
    Rigidbody rgbd;

    public float movementSpeed, rotationSpeed;
    float cameraAngle, startMovementSpeed;

    public GameObject mainCamera;
    Pushing pushScript;

    public bool groundMovement;
    private SlipperyOil slipperyOilMovement;

    void Start() {
        rgbd = GetComponent<Rigidbody>();
        pushScript = GetComponent<Pushing>();
        slipperyOilMovement = GetComponent<SlipperyOil>();
        startMovementSpeed = movementSpeed;
        groundMovement = true;
    }

    void Update() {
        if(pushScript.movingPlayerTowardsBox) {
            return;
        }

        if(pushScript.hasBox) {
            movementSpeed = 4.5f;
            switch(pushScript.holdLockDirection) {
                case Pushing.lockDirection.LockAxisY:
                    rgbd.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                    break;
                case Pushing.lockDirection.LockAxisX:
                    rgbd.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                    break;
            }
        } else {
            movementSpeed = startMovementSpeed;
            rgbd.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if(groundMovement) {
            //Update forward direction relative to camera
            UpdateCameraForward();

            //Update player input
            GetInput();

            //Update player Input
            UpdatePosition();

            if(pushScript.hasBox)
                return;

            //Rotate character towards walking direction
            RotateCharacter();

        } 
    }

    void UpdateCameraForward() {
        relativeForward = mainCamera.transform.forward;
        relativeForward.y = 0;
        Vector3.Normalize(relativeForward);
        relativeRight = Quaternion.Euler(new Vector3(0, 90, 0)) * relativeForward;
    }

    void GetInput() {
        Vector3 forwardDirection = Input.GetAxisRaw("Vertical") * relativeForward;
        Vector3 rightDirection = Input.GetAxisRaw("Horizontal") * relativeRight;
        playerPosition = Vector3.Normalize(rightDirection + forwardDirection) * movementSpeed;
    }

    void UpdatePosition() {
        playerPosition.y = rgbd.velocity.y;
        rgbd.velocity = playerPosition;
    }

    void RotateCharacter() {
        if(rgbd.velocity.magnitude > 0.1) {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(playerPosition.x, 0, playerPosition.z), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
