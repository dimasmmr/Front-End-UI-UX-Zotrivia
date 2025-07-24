using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Soal
{
    public string pertanyaan;
    public string opsiA;
    public string opsiB;
    public string opsiC;
    public string opsiD;
    public char jawabanBenar; // 'A', 'B', 'C', atau 'D'
    public Sprite gambar; // Menambahkan variabel gambar
}

public class SoalController : MonoBehaviour
{
    public List<Soal> daftarSoal; // List soal yang diisi di Inspector



    int indexSoal;
    int maxSoal;
    bool ambilSoal;
    char kunciJ;
    bool[] soalSelesai;
    public TextMeshProUGUI txtSoal, txtOpsiA, txtOpsiB, txtOpsiC, txtOpsiD;
    public TextMeshProUGUI txtTimer;
    public GameObject soundGagal;
    public GameObject popup;
    public Image imgSoal; // Referensi untuk menampilkan gambar soal
    public GameObject panel;
    public GameObject imgPenilaian, imgHasil;
    public TextMeshProUGUI txtHasil;
    bool isHasil;
    private float durasi;
    public float durasiPenilaian;
    public float waktuPerSoal = 10f;
    private float timerSoal;
    private int lastSecond;
    int jwbBenar, jwbSalah;
    float nilai;
    // Variabel untuk bintang
    public GameObject star1, star2, star3;
    private bool isFinishPlayed = false;

    void Start()
    {

        durasi = durasiPenilaian;
        maxSoal = daftarSoal.Count;
        soalSelesai = new bool[maxSoal];
        ambilSoal = true;
        TampilkanSoal();
        timerSoal = waktuPerSoal;
        lastSecond = Mathf.CeilToInt(timerSoal);
    }
    private void TampilkanSoal()
    {
        if (indexSoal < maxSoal)
        {
            if (ambilSoal)
            {
                int randomIndexSoal;
                bool soalBelumSelesai = false;
                while (!soalBelumSelesai)
                {
                    randomIndexSoal = Random.Range(0, maxSoal);
                    if (!soalSelesai[randomIndexSoal])
                    {
                        Soal soal = daftarSoal[randomIndexSoal];
                        txtSoal.text = soal.pertanyaan;
                        txtOpsiA.text = soal.opsiA;
                        txtOpsiB.text = soal.opsiB;
                        txtOpsiC.text = soal.opsiC;
                        txtOpsiD.text = soal.opsiD;
                        kunciJ = soal.jawabanBenar;
                        // Tampilkan gambar yang sesuai dengan soal
                        imgSoal.sprite = soal.gambar;
                        imgSoal.gameObject.SetActive(true);
                        soalSelesai[randomIndexSoal] = true;
                        ambilSoal = false;
                        soalBelumSelesai = true;
                        timerSoal = waktuPerSoal;
                        lastSecond = Mathf.CeilToInt(timerSoal);
                    }
                }
            }
        }
    }
    public void Opsi(string opsiHuruf)
    {
        CheckJawaban(opsiHuruf[0]);
        if (indexSoal == maxSoal - 1)
        {
            isHasil = true;
        }
        else
        {
            indexSoal++;
            ambilSoal = true;
        }
        panel.SetActive(true);
    }
    private float HitungNilai()
    {
        return nilai = (float)jwbBenar / maxSoal * 100;
    }
    public GameObject BenarObj;
    public GameObject SalahObj;
    private void CheckJawaban(char huruf)
    {
        if (huruf.Equals(kunciJ))
        {
            AudioManager.instance.PlaySFX("benar");
            BenarObj.SetActive(true);
            SalahObj.SetActive(false);
            jwbBenar++;
        }
        else
        {
            AudioManager.instance.PlaySFX("salah");
            SalahObj.SetActive(true);
            BenarObj.SetActive(false);
            jwbSalah++;
        }
    }
    void Update()
    {
        if (panel.activeSelf)
        {
            durasiPenilaian -= Time.deltaTime;
            if (isHasil)
            {
                imgPenilaian.SetActive(true);
                imgHasil.SetActive(false);
                if (durasiPenilaian <= 0)
                {
                    txtHasil.text = "Correct : " + jwbBenar + "\nWrong : " + jwbSalah + "\n\nFinal Score : " + HitungNilai();
                    imgPenilaian.SetActive(false);
                    imgHasil.SetActive(true);
                    durasiPenilaian = 0;
                    UnlockNewLevel();
                    // Pastikan hanya dipanggil sekali
                    if (!isFinishPlayed)  // Cek apakah suara finish sudah diputar
                    {
                        AudioManager.instance.PlaySFX("finish");
                        isFinishPlayed = true;  // Tandai bahwa suara finish sudah diputar
                    }
                }
            }
            else
            {
                imgPenilaian.SetActive(true);
                imgHasil.SetActive(false);
                if (durasiPenilaian <= 0)
                {
                    panel.SetActive(false);
                    durasiPenilaian = durasi;
                    TampilkanSoal();
                }
            }
        }
        else
        {
            timerSoal -= Time.deltaTime;
            int currentSecond = Mathf.CeilToInt(timerSoal);
            if (currentSecond < lastSecond)
            {
                AudioManager.instance.PlaySFX("tick");
                lastSecond = currentSecond;
            }
            txtTimer.text = "Times " + currentSecond.ToString();
            if (timerSoal <= 0)
            {
                soundGagal.GetComponent<AudioSource>().Play();
                popup.SetActive(true);
                if (indexSoal < maxSoal - 1)
                {
                    indexSoal++;
                    ambilSoal = true;
                    TampilkanSoal();
                }
                else
                {
                    isHasil = true;
                    panel.SetActive(true);
                }
                timerSoal = waktuPerSoal;
                popup.SetActive(false);
            }
        }
    }
    // Tambahkan referensi animator
    void UnlockNewLevel()
    {
        float skor = HitungNilai();
        // Nonaktifkan semua bintang dulu
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
        if (skor == 100) // Semua soal dijawab benar
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
            StartCoroutine(DelayAndAnimateStar(star1));
            StartCoroutine(DelayAndAnimateStar(star2));
            StartCoroutine(DelayAndAnimateStar(star3));
            // Unlock achievement dengan ID tertentu
            FindObjectOfType<AchievementHandler>().UnlockAchievement("test");
        }
        else if (skor >= 70)
        {
            star1.SetActive(true);
            star2.SetActive(true);
            StartCoroutine(DelayAndAnimateStar(star1));
            StartCoroutine(DelayAndAnimateStar(star2));
        }
        else if (skor >= 50)
        {
            star1.SetActive(true);
            StartCoroutine(DelayAndAnimateStar(star1));
            // Unlock achievement untuk skor pas 50
            if (skor == 50)
            {
                FindObjectOfType<AchievementHandler>().UnlockAchievement("50 multiple quiz");
            }
        }
        else if (skor == 0)
        {
            // Unlock achievement untuk skor 0
            FindObjectOfType<AchievementHandler>().UnlockAchievement("0 multiple quiz");
        }
    }
    // Fungsi untuk memberikan delay sebelum animasi
    private IEnumerator DelayAndAnimateStar(GameObject star)
    {
        yield return new WaitForSeconds(0.05f); // Delay 0.5 detik sebelum animasi
        StartCoroutine(star.GetComponent<ScaleAnimation>().ScaleUp());
    }



}