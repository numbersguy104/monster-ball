using UnityEngine;
using SimpleJSON;
using System.IO;
using cfg;

public class LubanTablesMgr
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        Instance = new LubanTablesMgr();
    }

    public static LubanTablesMgr Instance
    {
        get;
        private set;
    }

    public Tables tables;

    private LubanTablesMgr()
    {
        string gameConfDir = Application.dataPath + "/Resources/Data";
        var t = new cfg.Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));
        tables = t;
    }
}
