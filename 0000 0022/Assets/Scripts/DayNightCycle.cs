using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light Sun;
    
    public float timeV = 90f;
    private Quaternion TimeQ = Quaternion.Euler(0f, 0f, 0f);
    public float exposure = 1;
    public float ExposureS = 1.5f;
    public bool exposureM2 = true;
    private bool exposureM = true;
    public float speed = 1;

    private void Start()
    {
        timeV = 90f;
        exposure = 1f;
        ExposureS = 1.5f;
        speed = 1;

        RenderSettings.skybox.SetFloat("_Exposure", exposure);

        transform.localRotation = Quaternion.Euler (timeV, 0f, 0f);
    }

    void FixedUpdate()
    {

        timeV += 0.006f * speed;

        if (timeV >= 270f)
        {
            timeV = -90f;

        }

        //lightsource exposure


        if (exposure <= 0.1f & timeV <= 270f)
        {
            exposureM = true;
        }

        if (exposure >= 1 & timeV >= 90f)
        {
            exposureM = false;
        }

        if (exposureM == true)
        {
            exposure += 0.00003f * speed;
        }

        if (exposureM == false)
        {
            exposure -= 0.00003f * speed;
        }

        //Sybox exposure


        if (ExposureS <= 0.1f & timeV <= 270f)
        {
            exposureM2 = true;
        }

        if (ExposureS >= 1.5 & timeV >= 90f)
        {
            exposureM2 = false;
        }

        if (exposureM2 == true)
        {
            ExposureS += 0.000045f * speed;
        }

        if (exposureM2 == false)
        {
            ExposureS -= 0.000045f * speed;
        }

        //Setting values


        TimeQ = Quaternion.Euler(timeV, 0f, 0f);

        transform.localRotation = TimeQ;


        RenderSettings.skybox.SetFloat("_Exposure", ExposureS);

        Sun.intensity = exposure;

    }
}
