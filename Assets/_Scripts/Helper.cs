using UnityEngine;

public class Helper : MonoBehaviour
{
    public void ChangeObjectState(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }
}