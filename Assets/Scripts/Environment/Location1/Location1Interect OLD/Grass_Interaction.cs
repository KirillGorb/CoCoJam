using UnityEngine;

public class Grass_Interaction : MonoBehaviour
{
    [SerializeField] private Transform tracker;

    private Material grassMat;

    void Start()
    {
        grassMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        grassMat.SetVector("_TrackerPosition", tracker.position);
    }
}