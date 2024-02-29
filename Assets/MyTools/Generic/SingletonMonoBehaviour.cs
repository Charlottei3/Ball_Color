using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T temp = FindObjectOfType<T>();
                if (temp != null)
                    instance = temp;
                else
                {
                    instance = new GameObject().AddComponent<T>();
                    instance.gameObject.name = instance.GetType().Name;
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        T[] admods = GameObject.FindObjectsOfType<T>();
        if (admods.Length > 1)
            for (int i = 0; i < admods.Length - 1; i++)
            {
                Destroy(admods[i].gameObject);
            }
    }

    public static bool Exists()
    {
        return (instance != null);
    }
}