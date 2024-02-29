using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tube Datas", menuName = "Data SO/Tube Datas", order = 0)]
public class TubeDataSO : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] List<GameObject> _tubePrefab;

    public GameObject getTube(int id)
    {
        return _tubePrefab[id];
    }
}
