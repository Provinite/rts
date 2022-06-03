using UnityEngine;

public static class GuiUtils
{
  public static (Vector2 topLeft, Vector2 bottomRight) Square(
    Vector2 cornerA,
    Vector2 cornerB,
    int thickness,
    GUIStyle gUIStyle
  )
  {
    var top = Mathf.Min(cornerA.y, cornerB.y);
    var bottom = Mathf.Max(cornerA.y, cornerB.y);

    var left = Mathf.Min(cornerA.x, cornerB.x);
    var right = Mathf.Max(cornerA.x, cornerB.x);

    var width = right - left;
    var height = bottom - top;

    var topLeft = new Vector2(left, top);
    var bottomRight = new Vector2(right, bottom);

    GUI.Box(new Rect(left, top, width, thickness), "", gUIStyle);
    GUI.Box(new Rect(left, top, thickness, height), "", gUIStyle);
    GUI.Box(new Rect(right, top, thickness, height), "", gUIStyle);
    GUI.Box(new Rect(left, bottom, width, thickness), "", gUIStyle);

    return (topLeft, bottomRight);
  }
}
