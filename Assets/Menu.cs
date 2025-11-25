using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator animator;
    public VisualEffect effect;
    public TextMeshProUGUI text;
    public string[] questions;
    public GameObject SplashObject;

    void Start()
    {
        effect.Stop();
        text.text = questions[Random.Range(0, questions.Length)];

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        animator.Play("Reset");

        int rand = Random.Range(0, questions.Length);
        text.text = questions[Random.Range(0, questions.Length)];
    }

    public void PlayShred()
    {
        StartCoroutine(Shred());
    }

    public void PlayStamp()
    {
        StartCoroutine(Stamp());

    }

    public void PlaySplash()
    {
        StartCoroutine(Splash());
    }


    public IEnumerator Shred()
    {
        animator.Play("Shred");
        yield return new WaitForSeconds(1.05f);
        effect.Play();
        yield return new WaitForSeconds(1.05f);
        effect.Stop();
        Reset();


    }

    public IEnumerator Stamp()
    {
        animator.Play("Stamp");
        yield return new WaitForSeconds(1.65f);
        Reset();


    }

    public IEnumerator Splash()
    {
        animator.Play("Splash");
        yield return new WaitForSeconds(1f);
        Destroy(SplashObject);
    }



}
