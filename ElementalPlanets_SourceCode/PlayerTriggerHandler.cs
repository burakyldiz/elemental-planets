using UnityEngine;

public class PlayerTriggerHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<NPCInteraction>()?.OnTriggerEnter(this.GetComponent<Collider>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            NPCInteraction npcInteraction = other.GetComponent<NPCInteraction>();
            if (npcInteraction != null)
            {
                npcInteraction.OnTriggerExit(this.GetComponent<Collider>());
            }
        }
    }
}
