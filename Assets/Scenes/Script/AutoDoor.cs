using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public Animation doorAnimation;
    public string openAnimation = "Structure_v3_open";
    public string closeAnimation = "Structure_v3_close";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!doorAnimation.isPlaying)
                doorAnimation.Play(openAnimation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!doorAnimation.isPlaying)
                doorAnimation.Play(closeAnimation);
        }
    }
}
