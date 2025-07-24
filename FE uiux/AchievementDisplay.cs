using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour
{
    public string achievementId;
    public Image iconImage;
    public GameObject lockedOverlay; // Overlay transparan untuk tampilan locked
    public TextMeshProUGUI achievementText; // Objek teks untuk menampilkan nama/deskripsi pencapaian

    private void Start()
    {
        UpdateAchievementStatus();
    }

    public void UpdateAchievementStatus()
    {
        var achievement = AchievementManager.Instance.achievements.Find(a => a.id == achievementId);
        if (achievement != null)
        {
            iconImage.sprite = achievement.icon;
            lockedOverlay.SetActive(!achievement.isUnlocked);

            // Perbarui teks pencapaian berdasarkan statusnya
            achievementText.text = achievement.isUnlocked
                ? achievement.title // Teks jika unlocked
                : "<color=red>Locked</color>"; // Teks jika locked
        }
    }
}
