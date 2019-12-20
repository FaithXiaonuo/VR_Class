using UnityEngine;

public class MarkOnGO : MonoBehaviour
{
    public Transform target;
    public float height;

    public Transform vrHead;
    
    void Start()
    {

    }
    
    void Update()
    {
#if UNITY_STANDALONE_WIN
        transform.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(target.position) + new Vector3(0, height, 0);
#endif

#if UNITY_ANDROID
        transform.LookAt(vrHead);
#endif
    }
}
