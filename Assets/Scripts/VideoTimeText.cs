using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTimeText : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Text that displays the current time of the video.")]
    TextMeshProUGUI videoTimeText;

    [SerializeField]
    [Tooltip("Video time slider")]
    Slider slider;

    [SerializeField]
    VideoPlayer videoPlayer;

    void OnEnable()
    {
        slider.onValueChanged.AddListener(OnSliderValueChange);
    }

    void OnSliderValueChange(float sliderValue)
    {
        if (videoPlayer != null && videoTimeText != null)
        {
            var currentTimeTimeSpan = TimeSpan.FromSeconds(videoPlayer.time);
            var totalTimeTimeSpan = TimeSpan.FromSeconds(videoPlayer.length);
            var currentTimeString = string.Format("{0:D2}:{1:D2}",
                currentTimeTimeSpan.Minutes,
                currentTimeTimeSpan.Seconds
            );

            var totalTimeString = string.Format("{0:D2}:{1:D2}",
                totalTimeTimeSpan.Minutes,
                totalTimeTimeSpan.Seconds
            );
            videoTimeText.SetText(currentTimeString + " / " + totalTimeString);
        }
    }
}
