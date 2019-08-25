using ComponenetUtils;
using UniRx;
using UnityEngine;

public class DistanceToTargetModifier : MonoBehaviour
{
    public Transform followTarget;
    public float multDistanceValue = .5f;
    public float addInertiaMult = .1f;
    public float inertiaLerpValuePerSec = .5f;
    public bool snapAndParentOnDisable = true;
    public FakePhysicsWatcher physics;

    private CompositeDisposable disposables = new CompositeDisposable();
    private Vector3 inertia;


    public void OnEnable()
    {
        Observable.EveryFixedUpdate()
            .Subscribe(_ => MultDistance())
            .AddTo(disposables);

        Observable.EveryFixedUpdate()
            .Subscribe(_ => InertiaCalculations())
            .AddTo(disposables);
    }

    private void OnDisable()
    {
        disposables?.Dispose();
        if (snapAndParentOnDisable)
        {
            if (this == null || followTarget == null) return;
            transform.parent = followTarget.parent;
            transform.localPosition = Vector3.zero;
        }
    }

    public void MultDistance()
    {
        var followTargetPosition = followTarget.position;
        var offsetFromTarget = transform.position - followTargetPosition;
        transform.position = followTargetPosition + offsetFromTarget.normalized * multDistanceValue;
    }

    public void InertiaCalculations()
    {
        var currentVelocity = physics.velocity;
        inertia = Vector3.Lerp(inertia, currentVelocity, inertiaLerpValuePerSec * Time.fixedDeltaTime);

        transform.position += inertia * addInertiaMult;
    }
}