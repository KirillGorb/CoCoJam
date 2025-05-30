using System;
using System.Linq;
using CodeScripts.Envir.Envir;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Zenject.SpaceFighter;

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
        TriggerOnEx = 1 << 2,
        TriggerOff = 1 << 3,
        ColliderOn = 1 << 4,
        ColliderOnEx = 1 << 5,
        ColliderOff = 1 << 6,
        Updater = 1 << 7,
        UpdaterFixed = 1 << 8,
        TriggerOnEnt = 1 << 9,
        ColliderOnEnt = 1 << 10,
    }

    [Serializable]
    public abstract class ILogic
    {
        [field: SerializeField] public Collider2D Transform { get; set; }
        [field: SerializeField] public Collider2D Target { get; set; }

        public EPlatform platform { get; set; }
        public abstract void Init();

        public virtual void SetNext(object data)
        {
        }

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
        /// Кого касаемся мы в моменте?
        /// </summary>
        /// <param name="target">результат кого мы каснулись</param>
        public virtual void TargetingOnEnt(GameObject target)
        {
        }

        /// <summary>
        /// Кого  мы ne касаемся?
        /// </summary>
        /// <param name="target">результат кого мы каснулись</param>
        public virtual void TargetingOnEx(GameObject target)
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

    [Serializable]
    public abstract class IState : ILogic
    {
        public BoolReactiveProperty IsNext { get; } = new();
        public ReactiveCommand<object> Next { get; } = new();

        public virtual void Abort(){}
    }

    public class FindTargetParam : IState 
    {
        [SerializeField] private bool isDist;
        [SerializeField, ShowIf("@isDist")] private float distFind;

        [SerializeField] private ETargetType findType = ETargetType.Player;

        [SerializeField] private ContainerFinder _finder;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.Static;
            Find();
        }

        private void Find()
        {
            if (isDist)
            {
                foreach (var obj in _finder.GetContainer(findType))
                    if (Vector2.Distance(Transform.transform.position, obj.transform.position) > distFind &&
                        obj.TryGetComponent(out Collider2D collider))
                        Next.Execute(collider);
            }
            else
            {
                foreach (var obj in _finder.GetContainer(findType))
                    if (obj.TryGetComponent(out Collider2D collider))
                        Next.Execute(collider);
            }
        }
    }

    public class None : IState
    {
        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.Static;
        }
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
            if (Vector3.Distance(Transform.transform.position, Target.transform.position) <= _distance)
                IsNext.Value = true;
        }
    }

    [Serializable]
    public class CheckDoubleClickDown : IState
    {
        [SerializeField] private UnityEvent meConnected = new();
        [SerializeField] private UnityEvent meNoConnected = new();

        [SerializeField] private bool isNextOnTarget;

        private bool isCheck = false;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.TriggerOnEnt | EPlatform.ColliderOnEnt;
            meNoConnected.Invoke();
        }

        public override void TargetingOnEnt(GameObject target)
        {
            isCheck = !isCheck;

            if (isCheck)
            {
                meConnected.Invoke();

                if (isNextOnTarget)
                    IsNext.Value = true;
            }
            else
                meNoConnected.Invoke();
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
    public class CheckClickUp : IState
    {
        [SerializeField] private UnityEvent meConnected = new();
        [SerializeField] private bool isNextOnTarget;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.TriggerOnEx | EPlatform.ColliderOnEx;
        }

        public override void TargetingOnEx(GameObject target)
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
            if (target.GetInstanceID() == Target.gameObject.GetInstanceID() &&
                Target.TryGetComponent(out BaseOptionLogics logic))
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

        [SerializeField] private float expDist = 0.01f;

        private int _to;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.UpdaterFixed;
        }

        public override void Updater()
        {
            _to = Transform.transform.position.x > Target.transform.position.x + _to * (isLoop ? expDist : 0) ? -1 : 1;

            rigidbody2D.velocity = new Vector2(_to * speed, rigidbody2D.velocity.y);

            if (!isLoop && Vector2.Distance(Transform.transform.position, Target.transform.position) <= expDist)
                IsNext.Value = true;
        }
    }

    [Serializable]
    public class MoveTo : IState
    {
        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private float speed;

        [SerializeField] private float expDist = 0.01f;

        public override void Init()
        {
            IsNext.Value = false;
            platform = EPlatform.UpdaterFixed;
        }

        public override void Updater()
        {
            var dir = (Vector2)Target.transform.position - rigidbody2D.position;

            rigidbody2D.velocity = dir.normalized * speed;

            if (Vector2.Distance(rigidbody2D.position, Target.transform.position) <= expDist)
            {
                rigidbody2D.velocity =Vector2.zero;
                IsNext.Value = true;
            }
        }

        public override void Abort()
        {
            rigidbody2D.velocity =Vector2.zero;
        }
    }
}