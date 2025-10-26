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
        Sprite sprite = Resources.Load<Sprite>($"Prefabs/Icons/{name}");
        if (sprite != null)
            img.sprite = sprite;
    }

    public static void LoadBallIcon(Image img, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        Sprite sprite = Resources.Load<Sprite>($"Prefabs/Balls/Icons/{name}");
        img.sprite = sprite;
    }

    public static void GreyImage(Image img)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.2f);
    }
    
    public static void UnGreyImage(Image img)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
    }
}
