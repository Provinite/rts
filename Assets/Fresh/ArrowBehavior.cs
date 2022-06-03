using UnityEngine;
using UnityEngine.Splines;

public class ArrowBehavior : MonoBehaviour
{
  [HideInInspector]
  public ArrowPool ArrowPool;
  public Spline TraversalSpline
  {
    set
    {
      _traversalSpline = value;
      _splineLength = value.GetLength();
    }
    get => _traversalSpline;
  }
  private Spline _traversalSpline;
  private float _splineLength;

  public GameObject Target;
  public float Velocity = 30f;
  private float _percentComplete = 0f;
  private float _distanceTravelled = 0f;
  private Vector3 _originalTargetPosition;
  private Vector3 _positionOffset
  {
    get => Target.transform.position - _originalTargetPosition;
  }

  public void Fire()
  {
    _percentComplete = 0f;
    _distanceTravelled = 0f;
    _originalTargetPosition = Target.transform.position;
    Velocity = Random.Range(30f, 35f);
  }

  private void _cleanupArrow()
  {
    ArrowPool.Poop(gameObject);
  }

  void FixedUpdate()
  {
    if (Target == null)
    {
      _cleanupArrow();
      return;
    }
    var delta = Time.deltaTime * Velocity;
    Velocity = Mathf.Max(25, Velocity - (delta * .25f));
    var newDistanceTravelled = _distanceTravelled + delta;

    var (positionValid, newPercentComplete, position) = _getPositionAtDistance(
      newDistanceTravelled
    );
    var (nextPositionValid, nextPercentComplete, nextPosition) =
      _getPositionAtDistance(_distanceTravelled + (2 * delta));
    if (!positionValid)
    {
      // hit
      Target.GetComponent<MovableUnit>().TakeDamage(5);
      _cleanupArrow();
      return;
    }

    // offset by the target's offset form our initial destination
    transform.position = position + _positionOffset;
    if (nextPositionValid)
    {
      transform.LookAt(nextPosition + _positionOffset);
      // lay the arrow on its side
      transform.Rotate(new Vector3(90, 0, 0));
    }
    _distanceTravelled = newDistanceTravelled;
    _percentComplete = newPercentComplete;
  }

  protected (bool valid, float ratio, Vector3 position) _getPositionAtDistance(
    float distance
  )
  {
    if (distance < 0 || distance > _splineLength)
    {
      return (false, 0, Vector3.zero);
    }
    Vector3 position = TraversalSpline.GetPointAtLinearDistance(
      0,
      distance,
      out float splineRatio
    );
    return (true, splineRatio, position);
  }
}
