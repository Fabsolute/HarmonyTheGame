using UnityEngine;
public abstract class ManagerBase : MonoBehaviour
{
    protected static GameObject Managers;

    public abstract void Nullify();
}

public abstract class ManagerBase<T> : ManagerBase where T : ManagerBase<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    if (Managers == null)
                    {
                        Managers = GameObject.Find("Managers");
                    }

                    instance = Managers.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public override void Nullify()
    {
        instance = null;
    }
}