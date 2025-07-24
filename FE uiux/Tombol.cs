
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tombol : MonoBehaviour

{
    [SerializeField] RectTransform fader;

    private void Start()
    {
        fader.gameObject.SetActive(true);
        LeanTween.scale(fader, new Vector3(1, 1, 1), 0);
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });
    }

    public void scale(float scale)
    {
        transform.localScale = new Vector2(1 / scale, 1 * scale);
    }

    public void scene(string scene)
    {
        fader.gameObject.SetActive(true);

        // Animasi fader ke ukuran penuh
        LeanTween.scale(fader, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            StartCoroutine(LoadSceneDelayed(scene));
        });

        AudioManager.instance.PlaySFX("button");
    }

    IEnumerator LoadSceneDelayed(string scene)
    {
        fader.gameObject.SetActive(true);

        // Tunggu sampai animasi selesai sebelum pindah scene
        yield return new WaitForSeconds(0.5f); // Tunggu selama durasi animasi

        // Setelah animasi selesai, pindah ke scene baru
        SceneManager.LoadScene(scene);

        // Animasi untuk mengecilkan fader setelah scene dimuat
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad);
    }
}