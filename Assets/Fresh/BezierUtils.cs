using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEngine;

static class BezierUtils
{
  public static Spline CreateSpline(this BezierCurve curve)
  {
    var knots = new BezierKnot[]
    {
      new BezierKnot(
        curve.P0,
        float3.zero,
        curve.Tangent0,
        Quaternion.identity
      ),
      new BezierKnot(
        curve.P3,
        curve.Tangent1,
        float3.zero,
        Quaternion.identity
      ),
    };
    return new Spline(knots);
  }
}
