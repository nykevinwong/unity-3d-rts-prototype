using UnityEngine;

public static class DebugUtils
{
    public static void DrawRect(Vector3 min, Vector3 max, Color color)
    {
        UnityEngine.Debug.DrawLine(min, new Vector3(min.x, 0, max.z), color);
        UnityEngine.Debug.DrawLine(new Vector3(min.x, 0, max.z), max, color);
        UnityEngine.Debug.DrawLine(max, new Vector3(max.x, 0, min.z), color);
        UnityEngine.Debug.DrawLine(min, new Vector3(max.x, 0, min.z), color);
    }

    public static void DrawRect(Vector3 min, Vector3 max, Color color, float duration)
    {
        UnityEngine.Debug.DrawLine(min, new Vector3(min.x, 0, max.z), color, duration);
        UnityEngine.Debug.DrawLine(new Vector3(min.x, 0, max.z), max, color, duration);
        UnityEngine.Debug.DrawLine(max, new Vector3(max.x, 0, min.z), color, duration);
        UnityEngine.Debug.DrawLine(min, new Vector3(max.x, 0, min.z), color, duration);
    }

    public static void DrawRect(Rect rect, Color color)
    {
        DrawRect(rect.min, rect.max, color);
    }

    public static void DrawRect(Rect rect, Color color, float duration)
    {
        DrawRect(rect.min, rect.max, color, duration);
    }
}