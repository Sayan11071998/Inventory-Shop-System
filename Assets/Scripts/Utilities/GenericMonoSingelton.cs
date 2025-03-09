using UnityEngine;

public class GenericMonoSingelton<T> : MonoBehaviour where T : GenericMonoSingelton<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }
    public virtual void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        if (instance == null)
            instance = (T)this;
        else
            Destroy(gameObject);
    }
}