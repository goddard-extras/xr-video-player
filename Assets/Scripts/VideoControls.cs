using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(VideoLibrary))]
public class VideoControls : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Play/pause button")]
    GameObject buttonPlayOrPause;

    [SerializeField]
    [Tooltip("Video time slider")]
    Slider slider;

    [SerializeField]
    Sprite iconPlay;
    [SerializeField]
    Sprite iconPause;

    [SerializeField]
    [Tooltip("Current icon on play/pause button")]
    Image buttonIcon;


    [SerializeField]
    TextMeshProUGUI titleText;

    bool isDragging;
    bool videoIsPlaying;
    bool videoJumpPending;
    long lastFrameBeforeScrub;
    VideoPlayer videoPlayer;
    VideoLibrary library;
    int videoIndex;
    GameObject frameDecoration;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        library = GetComponent<VideoLibrary>();
        frameDecoration = null;
        SelectVideo(0);

        if (!videoPlayer.playOnAwake)
        {
            videoPlayer.playOnAwake = true;

            // load first frame by playing then immediately stopping. nice! elegant! no better api is possible
            videoPlayer.Play();
            VideoStop();
        }
        else
        {
            VideoPlay();
        }

        if (buttonPlayOrPause != null)
            buttonPlayOrPause.SetActive(false);
    }

    public void SelectVideo(int index)
    {
        if (frameDecoration != null)
        {
            GameObject.Destroy(frameDecoration);
        }

        this.videoIndex = index % library.entries.Count;
        VideoStop();
        LibraryEntry libraryEntry = library.entries[videoIndex];
        videoPlayer.clip = libraryEntry.clip;
        titleText.text = libraryEntry.name;
        if (libraryEntry.decorationPrefab != null)
        {
            frameDecoration = Instantiate(libraryEntry.decorationPrefab, transform, false);
        }
        VideoPlay();
    }

    public void SelectNextVideo()
    {
        SelectVideo(videoIndex + 1);
    }
    public void SelectPreviousVideo()
    {
        SelectVideo(videoIndex + (library.entries.Count - 1));
    }

    void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.frame = 0;
            VideoPlay(); // Ensures correct UI state update if paused.
        }

        slider.value = 0.0f;
        slider.gameObject.SetActive(true);
    }

    void Update()
    {
        if (videoJumpPending)
        {
            // We're trying to jump to a new position, but we're checking to make sure the video player is updated to our new jump frame.
            if (lastFrameBeforeScrub == videoPlayer.frame)
                return;

            // If the video player has been updated with desired jump frame, reset these values.
            lastFrameBeforeScrub = long.MinValue;
            videoJumpPending = false;
        }

        if (!isDragging && !videoJumpPending)
        {
            if (videoPlayer.frameCount > 0)
            {
                var progress = (float)videoPlayer.frame / videoPlayer.frameCount;
                slider.value = progress;
            }
        }
    }

    public void OnPointerDown()
    {
        videoJumpPending = true;
        VideoStop();
        VideoJump();
    }

    public void OnRelease()
    {
        isDragging = false;
        VideoPlay();
        VideoJump();
    }

    public void OnDrag()
    {
        isDragging = true;
        videoJumpPending = true;
    }

    void VideoJump()
    {
        videoJumpPending = true;
        var frame = videoPlayer.frameCount * slider.value;
        lastFrameBeforeScrub = videoPlayer.frame;
        videoPlayer.frame = (long)frame;
    }

    public void PlayOrPauseVideo()
    {
        if (videoIsPlaying)
        {
            VideoStop();
        }
        else
        {
            VideoPlay();
        }
    }

    void VideoStop()
    {
        videoIsPlaying = false;
        videoPlayer.Pause();
        buttonIcon.sprite = iconPlay;
        buttonPlayOrPause.SetActive(true);
    }

    void VideoPlay()
    {
        videoIsPlaying = true;
        videoPlayer.Play();
        buttonIcon.sprite = iconPause;
        buttonPlayOrPause.SetActive(false);
    }
}
