using System;
using UnityEngine;
using Zenject;

namespace CodeScripts.Camera
{
    public class CameraZoomAndMoveController : IInitializable, ILateTickable
    {
        [Inject] private ModelCamera[] _model;
        
        [Inject(Id = "Target")] private Transform _target;
        [Inject(Id = "This")] private Transform _transform;

        private ZoomAndMoveLogic[] _logics;
        private ZoomAndMoveLogic _active;

        public void SetNext(int index = 0)
        {
            _active = _logics[index];
            _active.SetStartPoints();
        }

        public void Initialize()
        {
            var count = _model.Length;
            _logics = new ZoomAndMoveLogic[count];

            for (int i = 0; i < count; i++)
                _logics[i] = new(_model[i], _transform, _target);
            
            SetNext();
        }

        public void LateTick() => _active.LateUpdate();
    }

    [Serializable]
    public class ZoomAndMoveLogic
    {
        private readonly Transform _transform;
        private readonly Transform _target;
        private ModelCamera _model;

        private int _id;
        private int _nextID;

        private float posXThis;
        private float posYThis;
        private float posZThis;

        private float NYmTY;
        private float NXmTX;
        private float NZmTZ;

        public ZoomAndMoveLogic(ModelCamera model, Transform transform, Transform target)
        {
            _transform = transform;
            _target = target;
            _model = model;
            SetStartPoints();
        }

        public void LateUpdate()
        {
            MoveCamera();
            SetNewPoint();
        }

        private void MoveCamera() =>
            _transform.position = MovePosition(_target.position.x);

        private void SetNewPoint()
        {
            if (_target.position.x >= _model.points[_nextID].position.x)
                SetPoint(_id + 1, _nextID + 1);
            else if (_target.position.x <= _model.points[_id].position.x)
                SetPoint(_id - 1, _nextID - 1);
        }

        private Vector3 MovePosition(float x) =>
            new(x, posYThis + (x - posXThis) * NYmTY / NXmTX, -Math.Abs(posZThis + (x - posXThis) * NZmTZ / NXmTX));

        public void SetStartPoints()
        {
            int i = 0, a = _model.points.Count - 1;

            while (true)
            {
                if (i == a - 1 || i == a + 1)
                    break;

                int c = (i + a) / 2;

                if (_model.points[c].position.x > _target.position.x) a = c;
                else i = c;
            }

            SetPoint(i, a);

            _transform.position = MovePosition(_target.position.x);
        }

        public void SetPoint(int id, int nextId)
        {
            _id = id;
            _nextID = nextId;

            posZThis = _model.points[_id].position.z;
            posYThis = _model.points[_id].position.y;
            posXThis = _model.points[_id].position.x;

            NYmTY = _model.points[_nextID].position.y - posYThis;
            NXmTX = _model.points[_nextID].position.x - posXThis;
            NZmTZ = _model.points[_nextID].position.z - posZThis;
        }
    }
}