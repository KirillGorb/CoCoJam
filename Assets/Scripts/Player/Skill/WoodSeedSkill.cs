using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeScripts.Skill
{
    public class WoodSeedSkill : IInitializable
    {
        [Inject] private readonly ClickService _click;

        private GameObject prefab;
        private float timeSpawn;

        private float _timeSpawn;

        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _click.RaycastClick.Subscribe(e =>
            {
                var w = Object.Instantiate(_prefab);
                w.transform.position = e.Item2;
                Set(w.transform);
                Target = null;
            }).AddTo(_disposable);
           // Observable.EveryFixedUpdate().Subscribe(_ => SelectCollision()).AddTo(_disposable);
        }

        private GameObject _prefab;
        private float _radius;
        private LayerMask _layerCheck;

        private float _timeLife;
        private float _timeSize;
        private float _sizeUp;

        public Transform Target { get; set; }

        private void SelectCollision()
        {
            if (Target == null) return;

            var c = Physics2D.OverlapCircle(Target.position, _radius, _layerCheck);
            if (c && c.TryGetComponent(out Collision2D other))
            {
                var w = Object.Instantiate(_prefab);
                w.transform.position = other.contacts[0].point;
                Set(w.transform);
                Target = null;
            }
        }

        private void Set(Transform target)
        {
            Up(target).Forget();
            Object.Destroy(target, _timeSize + _timeLife);
        }

        private async UniTaskVoid Up(Transform target)
        {
            while (_timeSize > 0)
            {
                _timeSize -= Time.deltaTime;
                await UniTask.Yield();
                target.localScale += Vector3.up * (_sizeUp * Time.deltaTime);
                target.position -= Vector3.up * (_sizeUp * Time.deltaTime);
            }
        }
    }
}