using cfg;
using UnityEngine;

public class BattleCommonUtils
{
    
    public static GameObject GetPinballPrefab(string name)
    {
        TbBallParam tbBallParam = LubanTablesMgr.Instance.tables.TbBallParam;
        var assetID = tbBallParam[name].AssetID;
        
        GameObject go = Resources.Load<GameObject>($"Prefabs/{assetID}");;
        return go;
    }
    
}
