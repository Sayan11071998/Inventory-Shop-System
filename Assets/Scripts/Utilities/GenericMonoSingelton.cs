using UnityEngine;

public class GenericMonoSingelton<T> : MonoBehaviour where T : GenericMonoSingelton<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }
    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;        
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
