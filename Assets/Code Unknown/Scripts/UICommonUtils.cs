using UnityEngine;
using SimpleJSON;
using System.IO;
using cfg;
using UnityEngine.UI;

public class UICommonUtils
{
    private static Sprite CreateSpriteFromPath(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    
    public static void LoadArtifectIcon(Image img, string name)
    {
        string path = Application.dataPath + "/Code Unknown/Props/Icons/" + name + ".png";
        img.sprite = CreateSpriteFromPath(path);
    }

    public static void LoadBallIcon(Image img, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        string path = Application.dataPath + "/Code Unknown/Props/Balls/Icons/" + name + ".png";
        img.sprite = CreateSpriteFromPath(path);
    }
    
}
