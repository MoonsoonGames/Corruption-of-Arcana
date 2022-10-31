using UnityEngine;
using Fungus;

public class LocationTrigger : MonoBehaviour
{
    public string LocationEnterMessage = "InLocation";
    public string LocationExitMessage = "NotInLocation";

    Collider col;
    private void Start()
    {
        col = GetComponent<Collider>();

        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Flowchart.BroadcastFungusMessage(LocationEnterMessage);
        Debug.Log("Entered Waterfall Trigger");

    }
    private void OnTriggerExit(Collider other)
    {

        Flowchart.BroadcastFungusMessage(LocationExitMessage);
        Debug.Log("Exited Waterfall Trigger");

    }
}
