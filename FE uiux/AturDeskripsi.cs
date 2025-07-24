using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AturDeskripsi : MonoBehaviour
{
    [Header("Deskripsi Hewan")]
    public TrackAR[] tr;
    public string[] nama;
    public AudioClip[] suara;
    [TextArea]
    public string[] deskripsi;

    [Header("UI Deskripsi")]
    public TextMeshProUGUI txtNama;
    public TextMeshProUGUI txtDeskripsi;
    public GameObject goNama;
    public GameObject goDeskripsi;
    public Button playSoundButton;

    public GameObject Penanda;

    public bool[] cekMarker;
    int countMarker;

    private AudioSource audioSource;
    private int currentSoundIndex = -1;

    private bool isAchievementUnlocked = false; // Melacak apakah achievement sudah terpicu

    void Start()
    {
        cekMarker = new bool[tr.Length];
        audioSource = gameObject.AddComponent<AudioSource>();
        playSoundButton.onClick.AddListener(PlayCurrentSound);
        playSoundButton.gameObject.SetActive(false);
    }

    void Update()
    {
        bool anyMarkerDetected = false;
        for (int i = 0; i < tr.Length; i++)
        {
            if (tr[i].GetMarker())
            {
                anyMarkerDetected = true;
                txtNama.text = nama[i];
                txtDeskripsi.text = deskripsi[i];

                if (!cekMarker[i])
                {
                    countMarker++;
                    cekMarker[i] = true;

                    // Set currentSoundIndex ke indeks yang sesuai
                    currentSoundIndex = i;

                    // Unlock achievement hanya saat pertama kali marker terdeteksi di seluruh aplikasi
                    if (!isAchievementUnlocked)
                    {
                        FindObjectOfType<AchievementHandler>().UnlockAchievement("first ar");
                        isAchievementUnlocked = true;
                    }
                }
            }
            else
            {
                if (cekMarker[i])
                {
                    countMarker--;
                    cekMarker[i] = false;

                    if (currentSoundIndex == i)
                    {
                        currentSoundIndex = -1;
                    }

                    if (audioSource.isPlaying && audioSource.clip == suara[i])
                    {
                        audioSource.Stop();
                    }
                }
            }
        }

        playSoundButton.gameObject.SetActive(anyMarkerDetected);

        DeskripsiPanel();
    }

    private void DeskripsiPanel()
    {
        if (countMarker == 0)
        {
            goNama.SetActive(false);
            goDeskripsi.SetActive(false);
            Penanda.SetActive(true);
        }
        else
        {
            goNama.SetActive(true);
            goDeskripsi.SetActive(true);
            Penanda.SetActive(false);
        }
    }

    private void PlayCurrentSound()
    {
        if (currentSoundIndex != -1 && suara[currentSoundIndex] != null)
        {
            // Ambil nama file atau path dari AudioClip
            string audioClipName = suara[currentSoundIndex].name;

            // Gunakan AudioManager untuk memutar audio berdasarkan nama
            AudioManager.instance.PlaySFX(audioClipName);
        }
        else
        {
            Debug.LogWarning("Suara tidak ditemukan untuk index yang dipilih.");
        }
    }

}
