using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
public class PortalManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Sponza;

    private Material[] SponzaMaterials;
    private Material PortalPlaneMaterial;

    // Start is called before the first frame update
    void Start()
    {
        SponzaMaterials = Sponza.GetComponent<Renderer>().sharedMaterials;
        PortalPlaneMaterial = GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider collider)
    {
        Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);

        if (camPositionInPortalSpace.y <= 0.0f)
        {
            for (int i = 0; i < SponzaMaterials.Length; i++)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            }
            PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Front);
        }
        else if (camPositionInPortalSpace.y < 0.5f)
        {
            for (int i = 0; i < SponzaMaterials.Length; i++)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Always);
            }
            PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Off);

        }
        else
        {
            for (int i = 0; i < SponzaMaterials.Length; i++)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
            PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Back);

        }
    }
}
