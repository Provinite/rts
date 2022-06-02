using UnityEngine;
using UnityEngine.Splines;

public class Splider : MonoBehaviour {
  public GameObject TraversalSpline;
  private Spline _spline { get => TraversalSpline.GetComponent<SplineContainer>().Spline; }

  public (bool valid, Vector3 position, Vector3 tangent) GetPositionAtTime(float time) {
    var offset = 0.1f * time;
    if (offset > _spline.GetLength() || offset < 0) {
      return (false, Vector3.zero, Vector3.zero);
    }
    Vector3 position = _spline.GetPointAtLinearDistance(0, 0.1f * time, out float splineRatio);
    Vector3 tangent = _spline.EvaluateTangent(splineRatio);
    return (true, position + transform.parent.transform.position, tangent);
  }

  public Vector3 GetPosition(float normalizedRatio) {
    if (normalizedRatio < 0 || normalizedRatio > 1) {
      Debug.LogError($"Invalid interpolation ratio ({normalizedRatio}). Clamping to [0,1]");
      normalizedRatio = Mathf.Clamp(normalizedRatio, 0, 1);
    }
    var offset = _spline.GetLength() * normalizedRatio;
    Vector3 position = _spline.GetPointAtLinearDistance(0, offset, out float splineRatio);
    return position + TraversalSpline.transform.position;
  }


}
