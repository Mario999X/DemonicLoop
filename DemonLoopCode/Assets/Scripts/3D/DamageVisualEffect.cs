using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageVisualEffect : MonoBehaviour
{
    [SerializeField] float speedOfEffect = 0.5f;
    [SerializeField] float smothnessEffect = 0.4f;

    bool wait = false;

    Vignette vignette;

    void Start()
    {
        Vignette vig;
        if (GetComponent<Volume>().profile.TryGet<Vignette>(out vig))
            vignette = vig;
    }

    public void Auch()
    {
        if (!wait)
            StartCoroutine(palpitacion());
    }

    IEnumerator palpitacion()
    {
        yield return null;

        wait = true;

        while (vignette.smoothness.value != smothnessEffect)
        {
            yield return new WaitForSeconds(0.0001f);
            vignette.smoothness.value = Mathf.MoveTowards(vignette.smoothness.value, smothnessEffect, speedOfEffect * Time.deltaTime);
        }

        yield return new WaitForSeconds(1f);

        while (vignette.smoothness.value != 0.01f)
        {
            yield return new WaitForSeconds(0.0001f);
            vignette.smoothness.value = Mathf.MoveTowards(vignette.smoothness.value, 0.01f, speedOfEffect * Time.deltaTime);
        }

        wait = false;
    }
}
