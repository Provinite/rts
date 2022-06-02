using UnityEngine;
using UnityEngine.Splines;
public static class SplineTweener {
  public static BezierCurve CurveBetween(Vector3 origin, Vector3 midPoint, Vector3 destination) {
    var curve = new BezierCurve(origin, midPoint, destination);
    return curve;
  }

  public static Spline SplineBetween(Vector3 origin, Vector3 midPoint, Vector3 destination) {
    var curve = new BezierCurve(origin, midPoint, destination);
    return curve.CreateSpline();
  }
}