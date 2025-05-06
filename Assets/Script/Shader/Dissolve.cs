using System.Collections;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public float dissolveRate = 0.0125f;

    public float refreshrate = 0.025f;

    public SkinnedMeshRenderer skinnedMaterialsOne, skinnedMaterialsSecond;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DissolveCoS());
            StartCoroutine(DissolveCoT());
        }
    }

    IEnumerator DissolveCoT()
    {
        float counter = 0;

        while (skinnedMaterialsSecond.material.GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            skinnedMaterialsSecond.material.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshrate);
        }
    }

    IEnumerator DissolveCoS()
    {
        float counter = 0;

        while (skinnedMaterialsOne.material.GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            skinnedMaterialsOne.material.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshrate);
        }
    }
}
