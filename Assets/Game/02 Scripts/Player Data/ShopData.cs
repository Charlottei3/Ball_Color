using System.Collections;
using System.Collections.Generic;

public class ShopData
{
    public List<UserItemCollection> TubeDatas = new List<UserItemCollection>();
    public List<UserItemCollection> BallDatas = new List<UserItemCollection>();
    public List<UserItemCollection> ThemeDatas = new List<UserItemCollection>();


    public UserItemCollection getItemCol(TypeItemCollection type, int id)
    {
        return getListItemCol(type)[id];
    }

    public void BuyItem(TypeItemCollection type, int id)
    {
        getListItemCol(type)[id].IsUnlock = true;
    }

    public void EquibItem(TypeItemCollection type, int id, bool value)
    {
        getListItemCol(type)[id].isEquip = value;
    }

    private List<UserItemCollection> getListItemCol(TypeItemCollection type)
    {
        switch (type)
        {
            case TypeItemCollection.Tube:
                return TubeDatas;
            case TypeItemCollection.Ball:
                return BallDatas;
            case TypeItemCollection.Theme:
                return ThemeDatas;
            default:
                return null;
        }
    }
}

[System.Serializable]
public class UserItemCollection
{
    public int Index;
    public bool IsUnlock;
    public bool isEquip;

    public UserItemCollection(int index, bool isBought, bool isEquit)
    {
        Index = index;
        IsUnlock = isBought;
        this.isEquip = isEquit;
    }
}