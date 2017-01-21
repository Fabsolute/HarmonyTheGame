using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour {
    private Dictionary<string, Coroutine> Routines = new Dictionary<string, Coroutine>();
    public new Coroutine StartCoroutine(IEnumerator routine)
    {
        return base.StartCoroutine(routine);
    }
    private IEnumerator WaitForSecondsEnumerator(string Name, System.Action action, float t)
    {
        yield return new WaitForSeconds(t);
        action();
        Routines.Remove(Name);
    }

    public Coroutine WaitForSeconds(string Name, System.Action action, float t)
    {
        if (Routines.ContainsKey(Name))
        {
            StopCoroutine(Name);
        }
        var routine = StartCoroutine(WaitForSecondsEnumerator(Name, action, t));
        Routines.Add(Name, routine);
        return routine;
    }

    public new void StopCoroutine(string Name)
    {
        if (Routines.ContainsKey(Name))
        {
            var routine = Routines[Name];
            StopCoroutine(routine);
            Routines.Remove(Name);
        }
}
}
