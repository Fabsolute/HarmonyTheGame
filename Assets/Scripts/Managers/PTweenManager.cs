using System;
using UnityEngine;
using System.Collections;

public class PTweenManager : ManagerBase<PTweenManager>
{
    public IEnumerator To(float duration, float startValue, float endValue, Action<float> callback, Action endCallback = null)
    {
        float start = Time.time;
        float end = start + duration;
        float durationInv = 1f / duration;
        float startMulDurationInv = start / duration;
        for (float t = Time.time; t < end; t = Time.time)
        {
            callback(Mathf.Lerp(startValue, endValue, t * durationInv - startMulDurationInv));
            yield return new WaitForEndOfFrame();
        }
        callback(endValue);
        if (endCallback != null)
        {
            endCallback();
        }
    }

    public IEnumerator To(float duration, Action<float> callback, Action endCallback = null)
    {
        return To(duration, 0f, 1f, callback, endCallback);
    }

    public Coroutine RoutineTo(float duration, Action<float> callback, Action endCallback = null)
    {
        return StartCoroutine(To(duration, callback, endCallback));
    }

    public Coroutine RoutineTo(float duration, float startValue, float endValue, Action<float> callback, Action endCallback = null)
    {
        return StartCoroutine(To(duration, startValue, endValue, callback, endCallback));
    }
}