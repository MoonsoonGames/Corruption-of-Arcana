using UnityEngine;

public class MouseClickControl : MonoBehaviour
{
    Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    public void EnableClicking()
    {
        col.enabled = true;
    }
    
    public void DisableClicking()
    {
        col.enabled = false;
    }
}
