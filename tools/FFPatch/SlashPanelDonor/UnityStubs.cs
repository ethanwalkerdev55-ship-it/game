namespace UnityEngine;

public class Object
{
    public static bool operator ==(Object a, Object b) => ReferenceEquals(a, b);
    public static bool operator !=(Object a, Object b) => !ReferenceEquals(a, b);
    public static implicit operator bool(Object o) => o != null;
}

public class Component : Object
{
    public Component GetComponent(System.Type type) => null;
}

public class Behaviour : Component { }

public class MonoBehaviour : Behaviour { }

public class GameObject : Object
{
    public Component GetComponent(System.Type type) => null;
}

public struct Vector2
{
    public float x;
    public float y;

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2 operator *(Vector2 a, float b) => a;
}

public struct Rect
{
    public float x;
    public float y;
    public float width;
    public float height;

    public Rect(float x, float y, float w, float h)
    {
        this.x = x;
        this.y = y;
        this.width = w;
        this.height = h;
    }
}

public class Texture { }

public class GUISkin
{
    public GUIStyle GetStyle(string name) => new GUIStyle();
}

public class GUIStyle { }

public static class GUI
{
    public static GUISkin skin { get; set; }
    public static int depth { get; set; }
    public static bool enabled { get; set; } = true;

    public static void DrawTexture(Rect rect, Texture tex, int scaleMode) { }
    public static void BeginGroup(Rect rect) { }
    public static void EndGroup() { }
    public static void Box(Rect rect, string text, GUIStyle style) { }
    public static void Box(Rect rect, string text) { }
    public static void Box(Rect rect) { }
    public static bool Button(Rect rect, string text, GUIStyle style) => false;
}

public static class GUIUtility
{
    public static void ScaleAroundPivot(Vector2 scale, Vector2 pivot) { }
}

public static class Screen
{
    public static int width { get { return 1024; } }
    public static int height { get { return 768; } }
}

public static class Input
{
    public static bool GetKeyDown(int keyCode) => false;
    public static bool GetKeyDown(string name) => false;
    public static bool GetKey(int keyCode) => false;
}

public enum EventType
{
    KeyDown = 4
}

public enum KeyCode
{
    None = 0,
    Escape = 27,
    Slash = 47
}

public class Event
{
    public static Event current;
    public EventType type;
    public KeyCode keyCode;
    public char character;

    public void Use() { }
}

public static class Mathf
{
    public static float Clamp01(float value) => value;
}
