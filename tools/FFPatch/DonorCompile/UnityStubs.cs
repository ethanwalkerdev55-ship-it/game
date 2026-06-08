namespace UnityEngine;



public class Object

{

    public static bool operator ==(Object a, Object b) => ReferenceEquals(a, b);

    public static bool operator !=(Object a, Object b) => !ReferenceEquals(a, b);

    public static implicit operator bool(Object o) => o != null;
}

public class Component : Object

{

    public Transform transform;

    public Component GetComponent(System.Type type) => null;

}



public class Behaviour : Component

{

}



public class MonoBehaviour : Behaviour

{

}



public class GameObject : Object

{

}



public struct Vector3

{

    public float x;

    public float y;

    public float z;



    public static Vector3 zero => default;

    public float magnitude => 0f;



    public static Vector3 operator -(Vector3 a, Vector3 b) => default;

}



public class Transform : Component

{

    public Vector3 position;

}



public static class Time

{

    public static float time;

}



public static class Random

{

    public static int Range(int min, int max) => min;

}

