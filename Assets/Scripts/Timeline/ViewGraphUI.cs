using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeScripts.Timeline.Timeline
{
    public class ViewGraphUI : MonoBehaviour
    {
        [SerializeField] private GraphModel graphModel;

        private void OnDrawGizmos()
        {
            if (graphModel == null || graphModel.GraphCollapse == null) return;

            Dictionary<string, Vector3> ss = new();
            Vector3 startPosition = transform.position;

            foreach (var item in graphModel.GraphCollapse)
            {
                if (ss.ContainsKey(item.Current.name))
                    startPosition = ss[item.Current.name];
                DrawNode(item.Current, startPosition);

                if (item.Next != null)
                {
                    int u = item.Next.Count;
                    int i = -u;

                    var end = startPosition;

                    foreach (var next in item.Next)
                    {
                        Vector3 nextPos;
                        if (ss.ContainsKey(next.name))
                            nextPos = ss[next.name];
                        else
                            nextPos = startPosition + Vector3.right * 2 + Vector3.up * i * .2f;
                        DrawNode(next, nextPos);
                        Gizmos.DrawLine(end, nextPos);
                        i += u;

                        if (!ss.ContainsKey(next.name))
                            ss.Add(next.name, nextPos);
                    }
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