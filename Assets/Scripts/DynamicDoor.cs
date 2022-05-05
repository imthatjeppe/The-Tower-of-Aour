using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDoor:MonoBehaviour {

    Rigidbody rgbd;

    public bool swiningDoor, openDoor;
    bool holdDoorState;

    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        holdDoorState = openDoor;
    }

    void Update() {

        if(swiningDoor) {
            if(openDoor) {
                rgbd.constraints = RigidbodyConstraints.None;
            } else {
                rgbd.constraints = RigidbodyConstraints.FreezeRotationY;
            }
            return;
        }

        if(holdDoorState == openDoor)
            return;

        if(openDoor) {
            DoorOpen();
        } else {
            DoorClose();
        }

        //Copy paste into new elevator script
        //if(!openDoor) {
        //    if(Vector3.Distance(doorStart, transform.position) > 0.075f) {
        //        MoveDoorTo(doorStart);
        //    } else {
        //        rgbd.velocity = Vector3.zero;
        //        rgbd.constraints = RigidbodyConstraints.FreezeAll;
        //        colliderActive.isTrigger = false;
        //    }
        //    return;
        //}

        //colliderActive.isTrigger = true;
        //rgbd.constraints = RigidbodyConstraints.FreezeRotation;
        //if(Vector3.Distance(doorTarget, transform.position) > 0.05f) {
        //    MoveDoorTo(doorTarget);
        //} else {
        //    rgbd.velocity = Vector3.zero;
        //}
    }

    void DoorOpen() {
        animator.Play("door_open");
    }

    void DoorClose() {
        animator.Play("door_close");
    }

    public void SetDoorOpen() {
        animator.Play("door_open_static");
        holdDoorState = openDoor;
        CancelInvoke();

    }

    public void SetDoorClosed() {
        animator.Play("door_close_static");
        holdDoorState = openDoor;
        CancelInvoke();
    }

    //void MoveDoorTo(Vector3 target) {
    //    Vector3 forcedDirection = target - transform.position;
    //    Vector3.Normalize(forcedDirection);
    //    rgbd.velocity = forcedDirection * speed;
    //}
}
