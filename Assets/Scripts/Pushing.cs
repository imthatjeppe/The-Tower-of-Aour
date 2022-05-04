using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushing:MonoBehaviour {
    GameObject tempBox;
    public float rayDistance = 2f, boxOffset = 2f;
    public bool hasBox, movingPlayerTowardsBox;

    Vector3 offset;
    public enum lockDirection { LockAxisY, LockAxisX };
    public lockDirection holdLockDirection;

    public enum pushDirection { Up, Down, Left, Right };
    public pushDirection holdPushDirection;

    Rigidbody rgbd;

    void Start() {
        rgbd = GetComponent<Rigidbody>();
    }

    void Update() {
        if(movingPlayerTowardsBox) {
            PlayerTowardsBox();
            return;
        }

        if(hasBox) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                hasBox = false;
                tempBox = null;
                return;
            }

            MovingBox();
            if(Input.GetKeyDown(KeyCode.E)) {
                hasBox = false;
                tempBox = null;
            }
            return;
        }

        if(!Input.GetKeyDown(KeyCode.E))
            return;

        RaycastHit hit;
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.forward * rayDistance, out hit, rayDistance)) {
            if(hit.collider.CompareTag("Box")) {
                tempBox = hit.collider.gameObject;
                Vector3 forcedDir = tempBox.transform.position - transform.position;
                float angle = Mathf.Atan2(forcedDir.z, forcedDir.x) * Mathf.Rad2Deg;
                if(angle < 35 && angle > -35) {
                    //Left
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    offset = Vector3.right * boxOffset;
                    holdLockDirection = lockDirection.LockAxisY;
                    holdPushDirection = pushDirection.Left;
                } else if(angle > 55 && angle < 125) {
                    //Down
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                    offset = Vector3.forward * boxOffset;
                    holdLockDirection = lockDirection.LockAxisX;
                    holdPushDirection = pushDirection.Down;
                } else if(angle < -55 && angle > -125) {
                    //Up
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    offset = -Vector3.forward * boxOffset;
                    holdLockDirection = lockDirection.LockAxisX;
                    holdPushDirection = pushDirection.Up;
                } else if(angle > 125 || angle < -125) {
                    //Right
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                    offset = -Vector3.right * boxOffset;
                    holdLockDirection = lockDirection.LockAxisY;
                    holdPushDirection = pushDirection.Right;
                }
                movingPlayerTowardsBox = true;
            }
        }
    }

    void PlayerTowardsBox() {
        Vector3 forcedDirection = (tempBox.transform.position - offset) - transform.position;
        forcedDirection = forcedDirection.normalized;
        if(Vector3.Distance(transform.position, (tempBox.transform.position - offset)) > 0.2f) {
            rgbd.velocity = forcedDirection * 7;
        } else {
            rgbd.velocity = Vector3.zero;
            movingPlayerTowardsBox = false;
            CancelInvoke(nameof(HasBox));
            Invoke(nameof(HasBox), 0.5f);
        }
    }

    void HasBox() {
        hasBox = true;
    }

    void MovingBox() {
        tempBox.transform.position = transform.position + offset;
    }
}
