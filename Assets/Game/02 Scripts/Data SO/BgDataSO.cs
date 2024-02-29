using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bg Datas", menuName = "Data SO/BG Datas", order = 0)]
public class BgDataSO : ScriptableObject
{
    [SerializeField] int index;
    [SerializeField] List<Sprite> _bgList;

    public Sprite getBG(int id)
    {
        return _bgList[id];
    }
}
