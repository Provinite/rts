using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovableUnit : MonoBehaviour {
  private Projector _projector;
  private GameObject _target;
  private float lastAttackTime = -1f;

  [HideInInspector]
  public int Health = 100;

  [HideInInspector]
  public int MaxHealth = 100;

  public float HealthPercent {
    get => (float)Health / (float)MaxHealth;
  }
  void Start() {
    foreach (Transform childTransform in transform) {
      if (childTransform.CompareTag("SelectionProjector")) {
        _projector = childTransform.GetComponent<Projector>();
      }
    }
  }

  public void TakeDamage(int damage) {
    Health -= damage;
    if (Health < 0) {
      GameObject.Destroy(gameObject);
    }
  }

  public bool SelectionEnabled {
    get => _projector.enabled;
    set => _projector.enabled = value;
  }

  private NavMeshAgent _navMeshAgent {
    get => gameObject.GetComponent<NavMeshAgent>();
  }

  public void MoveTo(Vector3 destination) {
    _navMeshAgent.SetDestination(destination);
  }

  void Update() {
    var range = 10f;
    var cooldown = 0.6f;
    if (lastAttackTime > 0) {
      if (Time.time - lastAttackTime < cooldown) {
        return;
      }
    }
    var colliders = Physics.OverlapSphere(transform.position, range, Physics.AllLayers & ~Globals.TERRAIN_LAYER_MASK & ~Globals.PROJECTILE_LAYER_MASK & ~Globals.CAMERA_FRUSTUM_COLLIDER_LAYER_MASK);
    foreach (var collider in colliders) {
      if (collider.attachedRigidbody != null) {
        var targetGameObject = collider.attachedRigidbody.gameObject;
        if (targetGameObject == gameObject) {
          continue;
        }
        if (targetGameObject == null) {
          continue;
        }
        var movableUnit = targetGameObject.GetComponent<MovableUnit>();
        if (movableUnit) {

          ArrowFactory.Spawn(gameObject, targetGameObject);
          lastAttackTime = Time.time;
          break;
        }
      }
    }
  }

}
