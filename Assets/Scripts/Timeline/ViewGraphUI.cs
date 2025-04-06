using System.Collections.Generic;
using CodeScripts.SaveLoadSystem;
using UnityEditor;
using UnityEngine;
using Plugins.Other;
using UniRx;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CodeScripts.Timeline
{
    using Model;

    public class ViewGraphUI : MonoBehaviour
    {
        [SerializeField] private Button prefab;
        [SerializeField] private UILineRenderer rendererLine;
        [SerializeField] private Transform parent;
        [SerializeField] private float stepHeight;
        [SerializeField] private float stepWidth;
        [SerializeField] private Vector2 offsetInitPos;

        [Inject] private readonly GraphModel _graphModel;
        [Inject] private readonly Save<TimelineSD> _save;

        private TimelineSD _saveData;

        private readonly CompositeDisposable _disposable = new();

        private void Start()
        {
            _saveData = _save.LoadData();
            Dictionary<string, Vector2> noCopy = new();
            Vector2 startPosition = (Vector2)transform.position + offsetInitPos;

            int j = 0;
            foreach (var item in _graphModel.GraphCollapse)
            {
                if (noCopy.ContainsKey(item.Current.name))
                {
                    startPosition = noCopy[item.Current.name];
                }
                else
                {
                    SpawnCollapse(startPosition, item.Current);
                }

                float i = -stepHeight * 2;
                var end = startPosition;
                j++;
                foreach (var next in item.Next)
                {
                    i += stepHeight;
                    if (noCopy.TryGetValue(next.name, out var value))
                    {
                        DrawLine(end, value);
                        continue;
                    }

                    var nextPos = (Vector2)transform.position + offsetInitPos + new Vector2(j * stepWidth, i);
                    DrawLine(end, nextPos);
                    SpawnCollapse(nextPos, next);
                    noCopy.Add(next.name, nextPos);
                }
            }
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void SpawnCollapse(Vector3 position, CollapseModel collapse)
        {
            var p = Instantiate(prefab, position, Quaternion.identity);
            p.gameObject.SetActive(true);
            p.transform.SetParent(transform);
            p.transform.localScale = Vector3.one;

            var i = p.GetComponent<Image>();

            ECollapseMode s = ECollapseMode.Inactive;
            if (_saveData?.AllCollapseMode?.Length > collapse.ID)
                s = _saveData.AllCollapseMode[collapse.ID];

            switch (s)
            {
                case ECollapseMode.Active:
                    i.color = Color.yellow;
                    break;
                case ECollapseMode.Inactive:
                    i.color = Color.grey;
                    break;
                case ECollapseMode.End:
                    i.color = Color.green;
                    break;
            }

            p.OnClickAsObservable().Subscribe(_ =>
            {
                if (_saveData?.AllCollapseMode?[collapse.ID] == ECollapseMode.Active)
                {
                    _saveData.IdOpenCollapse = collapse.ID;
                    _save.SaveData(_saveData);
                    SceneManager.LoadScene(collapse.ScenePlay);
                }
            }).AddTo(_disposable);
        }

        private void DrawLine(Vector2 start, Vector2 end)
        {
            var renderers = Instantiate(rendererLine, rendererLine.transform);
            Vector2 canvasStart = RectTransformUtility.WorldToScreenPoint(Camera.main, start);
            Vector2 canvasEnd = RectTransformUtility.WorldToScreenPoint(Camera.main, end);

            renderers.points = new[] { canvasStart, canvasEnd };
            renderers.SetAllDirty();
            
            renderers.transform.parent = parent;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_graphModel == null || _graphModel.GraphCollapse == null) return;

            Dictionary<string, Vector3> ss = new();
            Vector3 startPosition = (Vector2)transform.position + offsetInitPos;
            int j = 0;

            foreach (var item in _graphModel.GraphCollapse)
            {
                if (ss.ContainsKey(item.Current.name))
                    startPosition = ss[item.Current.name];
                else
                    DrawNode(item.Current, startPosition);

                float i = -stepHeight * 2;
                var end = startPosition;
                j++;

                foreach (var next in item.Next)
                {
                    i += stepHeight;
                    if (ss.TryGetValue(next.name, out var value))
                    {
                        Gizmos.DrawLine(end, value);
                        continue;
                    }

                    var nextPos = (Vector2)transform.position + offsetInitPos + new Vector2(j * stepWidth, i);
                    Gizmos.DrawLine(end, nextPos);
                    DrawNode(next, nextPos);
                    ss.Add(next.name, nextPos);
                }
            }
        }

        private void DrawNode(CollapseModel collapseModel, Vector3 position)
        {
            if (collapseModel == null) return;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(position, 0.01f);

            GUIStyle style = new GUIStyle();
            Handles.Label(position + Vector3.up * 0.2f, collapseModel.name, style);
        }
#endif
    }
}