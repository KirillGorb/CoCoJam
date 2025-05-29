using CodeScripts.PlayerInputs;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Player.Skill
{
    public class WoodSeedSkill : IInitializable
    {
        private GameObject prefab;
        private float timeSpawn;

        private float _timeSpawn;

        private readonly CompositeDisposable _disposable = new();
        private readonly WoodSeed woodSeed = new();

        public void Initialize()
        {
            InputCallback.Skill.WhereU(e => e).Subscribe(_ =>
            {
                if (timeSpawn + _timeSpawn >= Time.time)
                {
                    _timeSpawn = Time.time;
                    woodSeed.Target = Object.Instantiate(prefab).transform;
                }
            }).AddTo(_disposable);
        }
    }

    public class WoodSeed
    {
        private GameObject prefab;
        private float radius;
        private LayerMask layerCheck;
        
        private readonly CompositeDisposable _disposable = new();

        public Transform Target { get; set; }

        public WoodSeed()
        {
            Observable.EveryFixedUpdate().WhereU(_ => Target is not null).Subscribe(_ => SelectCollision()).AddTo(_disposable);
        }

        private void SelectCollision()
        {
            var c = Physics2D.OverlapCircle(Target.position, radius, layerCheck);
            if (c && c.TryGetComponent(out Collision other))
            {
                var w = Object.Instantiate(prefab);
                w.transform.position = other.contacts[0].normal;
                _ = new Wood(w.transform);
                Target = null;
            }
        }
    }

    public class Wood
    {
        private float timeLife;
        private float timeSize;
        private float sizeUp;

        public Wood(Transform target)
        {
            Up(target).Forget();
            Object.Destroy(target, timeSize + timeLife);
        }

        private async UniTaskVoid Up(Transform target)
        {
            while (timeSize > 0)
            {
                timeSize -= Time.deltaTime;
                await UniTask.Delay(10);
                target.localScale += Vector3.one * sizeUp;
            }
        }
    }
}