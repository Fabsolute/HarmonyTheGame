using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveHarmonyMonoBehaviour : MonoBehaviour {

    public bool IsActionCompleted;
	// Use this for initialization    
	public virtual void DoAction()
    {
		Debug.Log("Oh yes game over");
    }
}
