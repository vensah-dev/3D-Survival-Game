using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject FpsCam;
    public float Range;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            AttackHit();
        }
    }

    void AttackHit()
    {
        RaycastHit hit;

        if(Physics.Raycast(FpsCam.transform.position, FpsCam.transform.forward, out hit, Range))
        {
            NonPlayerHealth ObjectHealth = hit.transform.GetComponent <NonPlayerHealth>();

            if(ObjectHealth != null)
            {
                ObjectHealth.Damage(1000);
            }
        }
    }
}
