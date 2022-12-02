using UnityEngine;
using Fungus;

public class PickupItem : MonoBehaviour
{
    public string pickupMessage = "PickupQuestItem";

    Collider col;
    public Renderer ren;

    private void Start()
    {
        //This part collects what the current coliders and renderers are for later
        col = GetComponent<Collider>();

        col.enabled = true;
        ren.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //This is the part that will disable the colliders - the colliders need to be disabled before the fungus message to make sure it doesn't accidentally shoot twice
        col.enabled = false;
        Flowchart.BroadcastFungusMessage(pickupMessage);

        ren.enabled = false;
    }
}
