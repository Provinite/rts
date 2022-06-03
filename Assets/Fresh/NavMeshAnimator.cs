using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class NavMeshAnimator : MonoBehaviour
{
  private NavMeshAgent _navMeshAgent;
  private Animator _animator;
  private Coroutine _updateLoopCoroutine;

  void Start()
  {
    _navMeshAgent =
      transform.parent.parent.gameObject.GetComponent<NavMeshAgent>();
    _animator = GetComponent<Animator>();
    _updateLoopCoroutine = StartCoroutine(UpdateLoop());
  }

  IEnumerator UpdateLoop()
  {
    for (; ; )
    {
      _animator.SetFloat(
        "velX",
        Mathf.Clamp(
          _navMeshAgent.velocity.magnitude / _navMeshAgent.speed,
          0,
          1
        )
      );
      yield return new WaitForSeconds(.1f);
    }
  }

  void OnDestroy()
  {
    StopCoroutine(_updateLoopCoroutine);
  }
}
