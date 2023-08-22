using System;
using System.Collections;
using UnityEngine;

public static class MyExtensions
{
    public static T GetComponent<T>(this GameObject gameObject) where T : Component
    {
        T toGet = gameObject.GetComponent<T>();
        if (toGet != null)
        {
            return toGet;
        }
        else
        {
            throw new Exception($"Missing Component {typeof(T).FullName} Exception On {gameObject.name} ! ");
        }
    }

    public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
    {
        return monoBehaviour.StartCoroutine(InternalOperation());

        IEnumerator InternalOperation()
        {
            yield return new WaitForSeconds(time);

            action?.Invoke();
        }
    }
    
    public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float time, float count = -1)
    {
        return monoBehaviour.StartCoroutine(InternalOperation());

        IEnumerator InternalOperation()
        {
            while (monoBehaviour.enabled)
            {
                yield return new WaitForSeconds(time);
                
                action?.Invoke();

                if (count-- == 0)
                    yield break;
            }
        }
    }

    public static void DoLayersWeight(this MonoBehaviour monoBehaviour, Animator animator, float targetWeight, float time)
    {
        monoBehaviour.StartCoroutine(SetLayersWeightInTime(animator, targetWeight, time));
    }

    private static IEnumerator SetLayersWeightInTime(Animator animator, float targetWeight, float time)
    {
        float currentTime = 0;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;

            animator.SetLayerWeight(1, Mathf.Lerp(0, targetWeight, currentTime / time));

            yield return null;
        }
    }
}