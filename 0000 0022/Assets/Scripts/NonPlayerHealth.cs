using UnityEngine;

public class NonPlayerHealth : MonoBehaviour
{
    public int Health = 100;

    public GameObject[] Collectables;
    public Vector2[] CollectablesDropRateRange;

    public GameObject[] PercentageCollectables;
    public float[] PercentagesOfPercentageCollectables;

    public void Damage(int damage)
    {
        Health -= damage;


        if(Health <= 0)
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);

        void InstantiateCollectable(GameObject Collectable)
        {

            Vector3 DropPos = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + 5, transform.position.z + Random.Range(-10, 10));

            Vector3 DropRotV = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Quaternion DropRot = Quaternion.Euler(DropRotV);

            GameObject CollectableC = Instantiate(Collectable, DropPos, DropRot);

        }


        //Max/Min DropRateInstantiation
        for (int i = 0; i < CollectablesDropRateRange.Length; i++)
        {
            for (int x = 0; x < CollectablesDropRateRange[i].x; x++)
            {
                InstantiateCollectable(Collectables[i]);
            }

        }

        for (int i = 0; i < CollectablesDropRateRange.Length; i++)
        {
            for (int x = 0; x < CollectablesDropRateRange[i].y - CollectablesDropRateRange[i].x; x++)
            {
                if (Random.value > 0.5)
                {
                    InstantiateCollectable(Collectables[i]);

                }

            }

            //Percentage DropRateInstantiation
            for (int z = 0; z < PercentagesOfPercentageCollectables.Length; z++)
            {
                if (Random.value < (PercentagesOfPercentageCollectables[z] / 100))
                {
                    InstantiateCollectable(PercentageCollectables[z]);

                }
            }
        }


    }
}
