using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLever:MonoBehaviour {

    public bool leverActive, startTrue;
    public Animator anim;

    void Start() {
        if(startTrue) {
            anim.Play("lever_True");
        } else {
            anim.Play("lever_False");
        }
    }

    public void LeverUp() {
        anim.Play("lever_MTrue");
        Invoke(nameof(SetLeverTrue), 1);
    }

    public void LeverDown() {
        anim.Play("lever_MFalse");
        Invoke(nameof(SetLeverFalse), 1);
    }

    void SetLeverFalse() {
        anim.Play("lever_False");
        leverActive = false;
    }

    void SetLeverTrue() {
        anim.Play("lever_True");
        leverActive = true;
    }
}
