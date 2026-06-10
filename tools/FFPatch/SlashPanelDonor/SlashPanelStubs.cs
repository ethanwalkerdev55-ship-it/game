using UnityEngine;

public class GameFrame : MonoBehaviour
{
    public static GameFrame myGameFrame;
    public GameObject Option;

    public static bool IsReadyForPlay() => true;
}

public class cnOptionMode : MonoBehaviour
{
    public GUISkin pMenuSkin;
    public Texture backFillImage;
}

public class InventoryManagerScript : MonoBehaviour
{
    public GUISkin FFGuiSkin;
}

public static class GlobalManager
{
    public static GameObject GetManager(int iNum) => null;
}

public static class AssetLoader
{
    public static object Load(string path, System.Type type) => null;
}

public static class FFGUIUtility
{
    public static void ScaleAroundPivot(int pivot) { }
    public static void UnscaleAroundPivot(int pivot) { }
}

public static class SoundUtil
{
    public static void ButtonSound() { }
}

public static class Logger
{
    public static void Log(string message) { }
    public static void Log(object message) { }
}

public static class cnSystemMessageManager
{
    public static bool IsSysPopUp() => false;
}
