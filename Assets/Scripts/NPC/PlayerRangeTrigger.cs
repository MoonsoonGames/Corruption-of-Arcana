using UnityEngine;

public class PlayerRangeTrigger : MonoBehaviour
{

    public Collider col; 

    private void OnTriggerEnter(Collider other)
    {
        col.enabled = true;
        Debug.Log(other.name + "Triggered " + gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        col.enabled = false;
        Debug.Log(other.name + "Not Triggering " + gameObject.name);
    }

}
