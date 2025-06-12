using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    public GameObject GlobalPP;
    public GameObject UnderWaterPP;
    public GameObject player;

    void Update()
    {
        if(player.transform.position.y < 26.2f)
        {
            UnderWaterPP.SetActive(true);
            GlobalPP.SetActive(false);
        }
        else if(player.transform.position.y > 26.2f)
        {
            UnderWaterPP.SetActive(false);
            GlobalPP.SetActive(true);
        }
    }
}
