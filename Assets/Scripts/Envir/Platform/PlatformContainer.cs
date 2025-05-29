using System;
using CodeScripts.Envir.Envir;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;

namespace Envir.Platform
{
    /// <summary>
    ///<para>Static            - no logic</para>
    ///<para>Updater           - logic loop</para>
    ///<para>Trigger on        - trigger me => logic</para>
    ///<para>Trigger off       - trigger target => logic</para>
    ///<para>Collider on       - collider me => logic</para>
    ///<para>Collider off      - collider target => logic</para>
    /// </summary>
    [Flags]
    public enum EPlatform
    {
        Static = 0,
        TriggerOn = 1 << 1,
        TriggerOff = 1 << 2,
        ColliderOn = 1 << 3,
        ColliderOff = 1 << 4,
        Updater = 1 << 5,
        UpdaterFixed = 1 << 5,
    }

    [Serializable]
    public abstract class ILogic
    {
        [field: SerializeField] public Transform Transform { get; set; }
        [field: SerializeField] public Transform Target { get; set; }

        public EPlatform Type { get; set; }

        public abstract void Init();

        public virtual void Updater()
        {
        }

        /// <summary>
        /// Кого касаемся мы?
        /// </summary>
        /// <param name="target">результат кого мы каснулись</param>
        public virtual void TargetingOn(GameObject target)
        {
        }

        /// <summary>
        /// Кого касаеться наша цель
        /// </summary>
        /// <param name="target"> результат кого он каснулся</param>
        public virtual void TargetingOff(GameObject target)
        {
        }
    }

    public class PlatformContainer : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();

        public void Subscriber(ILogic logic)
        {
            Observable.EveryFixedUpdate()
                .WhereU(_ => (logic.Type & EPlatform.UpdaterFixed) == EPlatform.UpdaterFixed)
                .Subscribe(_ => logic.Updater()).AddTo(_disposable);

            Observable.EveryUpdate()
                .WhereU(_ => (logic.Type & EPlatform.Updater) == EPlatform.Updater)
                .Subscribe(_ => logic.Updater()).AddTo(_disposable);

            logic.Transform?.OnTriggerStay2DAsObservable()
                .WhereU(e => (logic.Type & EPlatform.TriggerOn) == EPlatform.TriggerOn)
                .Subscribe(e => logic.TargetingOn(e.gameObject)).AddTo(_disposable);

            logic.Target?.OnTriggerStay2DAsObservable()
                .WhereU(_ => (logic.Type & EPlatform.TriggerOff) == EPlatform.TriggerOff)
                .Subscribe(e => logic.TargetingOff(e.gameObject)).AddTo(_disposable);

            logic.Transform?.OnCollisionStay2DAsObservable()
                .WhereU(e =>
                    (logic.Type & EPlatform.ColliderOn) == EPlatform.ColliderOn)
                .Subscribe(e => logic.TargetingOn(e.gameObject)).AddTo(_disposable);

            logic.Target?.OnCollisionStay2DAsObservable()
                .WhereU(_ => (logic.Type & EPlatform.ColliderOff) == EPlatform.ColliderOff)
                .Subscribe(e => logic.TargetingOff(e.gameObject)).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }

    [Serializable]
    public abstract class IState : ILogic
    {
        public EPlatform platform { get; set; }

        public BoolReactiveProperty IsNext { get; } = new();
    }

    public class None : IState
    {
        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.Static;
        }

        public void Next() => IsNext.Value = true;
    }

    [Serializable]
    public class CheckDistance : IState
    {
        [SerializeField] private float _distance;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.Updater;
        }

        public override void Updater()
        {
            if (Vector3.Distance(Transform.position, Target.position) <= _distance)
                IsNext.Value = true;
        }
    }

    [Serializable]
    public class CheckClickDown : IState
    {
        [SerializeField] private UnityEvent meConnected = new();
        [SerializeField] private bool isNextOnTarget;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.TriggerOn | EPlatform.ColliderOn;
        }

        public override void TargetingOn(GameObject target)
        {
            meConnected.Invoke();
            if (isNextOnTarget)
                IsNext.Value = true;
        }
    }

    [Serializable]
    public class CheckOption : IState
    {
        [SerializeField] private BaseOptionLogics targetlogic;
        [SerializeField] private bool isNextOnTarget;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.TriggerOn | EPlatform.ColliderOn;
        }

        public override void TargetingOn(GameObject target)
        {
            targetlogic.NextState();
            if (isNextOnTarget)
                IsNext.Value = true;
        }
    }

    [Serializable]
    public class CheckTargetWithOption : IState
    {
        [SerializeField] private bool isNextOnTarget;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.TriggerOn | EPlatform.ColliderOn;
        }

        public override void TargetingOn(GameObject target)
        {
            if (target.GetInstanceID() == Target.gameObject.GetInstanceID() && Target.TryGetComponent(out BaseOptionLogics logic))
            {
                logic.NextState();
                if (isNextOnTarget)
                    IsNext.Value = true;
            }
        }
    }

    [Serializable]
    public class MoveHorizontalTo : IState
    {
        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private float speed;
        [SerializeField] private bool isLoop;

        [SerializeField, ShowIf("@isLoop == false")]
        private float expDist = 0.01f;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.Updater;
        }

        public override void Updater()
        {
            rigidbody2D.velocity = new Vector2(
                Transform.position.x > Target.position.x ? 1 : -1 * speed,
                rigidbody2D.velocity.y);

            if (!isLoop && Vector2.Distance(Transform.position, Target.position) <= expDist)
                IsNext.Value = true;
        }
    }
}