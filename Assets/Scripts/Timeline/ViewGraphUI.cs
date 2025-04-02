using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Plugins.Other;

namespace CodeScripts.Timeline.Timeline
{
    public class ViewGraphUI : MonoBehaviour
    {
        [SerializeField] private GraphModel graphModel;
        [SerializeField] private GameObject prefab;
        [SerializeField] private UILineRenderer rendererLine;
        [SerializeField] private Transform parent;
        [SerializeField] private float stepHeight;
        [SerializeField] private float stepWidth;
        [SerializeField] private Vector2 offsetInitPos;

        private void Start()
        {
            Dictionary<string, Vector2> noCopy = new();
            Vector2 startPosition = (Vector2)transform.position + offsetInitPos;

            int j = 0;
            foreach (var item in graphModel.GraphCollapse)
            {
                if (noCopy.ContainsKey(item.Current.name))
                {
                    startPosition = noCopy[item.Current.name];
                }
                else
                {
                    SpawnCollapse(startPosition);
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
                    SpawnCollapse(nextPos);
                    noCopy.Add(next.name, nextPos);
                }
            }
        }

        private void SpawnCollapse(Vector3 position)
        {
            var p = Instantiate(prefab, position, Quaternion.identity);
            p.SetActive(true);
            p.transform.SetParent(transform);
            p.transform.localScale = Vector3.one;
        }

        private void DrawLine(Vector2 start, Vector2 end)
        {
            var renderers = Instantiate(rendererLine, parent);
            Vector2 canvasStart = RectTransformUtility.WorldToScreenPoint(Camera.main, start);
            Vector2 canvasEnd = RectTransformUtility.WorldToScreenPoint(Camera.main, end);

            renderers.points = new[] { canvasStart, canvasEnd };
            renderers.SetAllDirty();
        }

        private void OnDrawGizmos()
        {
            if (graphModel == null || graphModel.GraphCollapse == null) return;

            Dictionary<string, Vector3> ss = new();
            Vector3 startPosition = (Vector2)transform.position + offsetInitPos;
            int j = 0;

            foreach (var item in graphModel.GraphCollapse)
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
    }
}