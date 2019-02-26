﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UMP.Interfaces;
using UMP.Helpers;
using UMP;
using UMP.Events;

public class UMPUIExample : MonoBehaviour, IMediaListener, IPlayerTimeChangedListener, IPlayerPositionChangedListener
{
    private const string BUFFERING = "BUFFERING";

    [SerializeField]
    private RawImage _videoImage;

    [SerializeField]
    private InputField _videoPath;

    [SerializeField]
    private Text _playButtonText;

    [SerializeField]
    private Slider _volume;

    [SerializeField]
    private Slider _rate;

    [SerializeField]
    private Text _timeText;

    [SerializeField]
    private Slider _rewind;

    [SerializeField]
    private Text _debug;

    [SerializeField]
    private Button _snapshot;

    [HideInInspector]
    public MediaPlayer _mediaPlayer;

    private long _mediaLength = -1;

    private IEnumerator _hideDebugEnumerator;

    void Awake()
    {        
        if (_videoImage != null)
        {
            if (MediaPlayerHelper.IsMobilePlatform())
            {
                _mediaPlayer = new MobileMediaPlayer(this, new[] { _videoImage.gameObject });
                _snapshot.gameObject.SetActive(false);
            }
            else
                _mediaPlayer = new StandaloneMediaPlayer(this, new[] { _videoImage.gameObject });

            // Auto clear texture after stop playback
            //_mediaPlayer.AutoClearVideoTexture = true;
            // Audio output of the video will be not muted
            _mediaPlayer.Mute = false;
            // Set default audio output volume
            _mediaPlayer.Volume = (int)_volume.value;
            // Attach scecial listeners to MediaPlayer instance
            AddListeners();
        }
        SetDebugVisibility(false);
    }

    private void SetDebugVisibility(bool visible)
    {
        _debug.transform.parent.gameObject.SetActive(visible);
        _debug.gameObject.SetActive(visible);

        if (_hideDebugEnumerator != null)
            StopCoroutine(_hideDebugEnumerator);

        _hideDebugEnumerator = HideDebugBehaviour();
        StartCoroutine(_hideDebugEnumerator);
    }

    private void SetDebugText(string text)
    {
        SetDebugVisibility(true);
        _debug.text = text;
    }

    private IEnumerator HideDebugBehaviour()
    {
        yield return new WaitForSeconds(2.0f);

        if (_debug.gameObject.activeSelf)
            SetDebugVisibility(false);
    }

    public void OnPlayClick()
    {
        if (_mediaPlayer != null)
        {
            // Check if MediaPlayer initialiazed and have some MRL
            if (!_mediaPlayer.AbleToPlay)
            {
                if (!string.IsNullOrEmpty(_videoPath.text))
                    // Set new data source for MediaPlayer as new specified Uri object
                    _mediaPlayer.SetDataSource(new Uri(_videoPath.text));

                if (_mediaPlayer.Play())
                    _playButtonText.text = "Pause";

                return;
            }

            // Check if MediaPlayer is playing
            if (_mediaPlayer.IsPlaying)
            {
                // Set MediaPlayer to pause state
                _mediaPlayer.Pause();
                _playButtonText.text = "Play";
            }
            else
            {
                // Start play video
                if (_mediaPlayer.Play())
                    _playButtonText.text = "Pause";
            }
        }
    }

    public void OnVolumeChanged()
    {
        // Set new audio output volume
        _mediaPlayer.Volume = (int)_volume.value;
        SetDebugText("Volume: " + _volume.value);
    }

    public void OnRateChanged()
    {
        // Set new video playback rate
        _mediaPlayer.PlaybackRate = (int)_rate.value;
        SetDebugText("Playback rate: " + _rate.value);
    }

    public void OnPositionChanged()
    {
        // Set new video position
        _mediaPlayer.Position = _rewind.value;
    }

    public void OnStopClick()
    {
        // Stop playing current video
        _mediaPlayer.Stop();
        _mediaLength = -1;
        _videoPath.gameObject.SetActive(true);
        _playButtonText.text = "Play";
		SetPlayerTime(0);
        SetMediaLength(0);
        _rewind.value = 0;
        _rewind.enabled = false;
    }

    public void OnSnapshotClick()
    {
        // Check if MediaPlayer initialiazed and have some MRL
        if (!_mediaPlayer.AbleToPlay) return;

        if (_mediaPlayer is StandaloneMediaPlayer)
        {
            // Take a snapshot of the current video window
            (_mediaPlayer as StandaloneMediaPlayer).TakeSnapShot(Application.persistentDataPath);
            SetDebugText("Snapshot path: " + Application.persistentDataPath);
        }
    }

    private void OnDestroy()
    {
        if (_mediaPlayer != null)
        {
            RemoveListeners();
            // Release MediaPlayer
            _mediaPlayer.Release();
        }
    }

    private void AddListeners()
    {
        // Add to MediaPlayer new main group of listeners
        _mediaPlayer.AddMediaListener(this);
        // Add to MediaPlayer new "OnPlayerTimeChanged" listener
        _mediaPlayer.EventManager.PlayerTimeChangedListener += OnPlayerTimeChanged;
        // Add to MediaPlayer new "OnPlayerPositionChanged" listener
        _mediaPlayer.EventManager.PlayerPositionChangedListener += OnPlayerPositionChanged;
    }

    private void RemoveListeners()
    {
        // Remove from MediaPlayer the main group of listeners
        _mediaPlayer.RemoveMediaListener(this);
        // Remove from MediaPlayer "OnPlayerTimeChanged" listener
        _mediaPlayer.EventManager.PlayerTimeChangedListener -= OnPlayerTimeChanged;
        // Remove from MediaPlayer "OnPlayerPositionChanged" listener
        _mediaPlayer.EventManager.PlayerPositionChangedListener -= OnPlayerPositionChanged;
    }

    void SetPlayerTime(long playedTime)
    {
        var time = TimeSpan.FromMilliseconds(playedTime);
        string text = _timeText.text;
        int separatorIndex = text.IndexOf("\n", StringComparison.Ordinal);

        _timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds) + text.Substring(separatorIndex);
    }

    void SetMediaLength(long mediaLength)
    {
        var length = TimeSpan.FromMilliseconds(mediaLength);
        string text = _timeText.text;
        int separatorIndex = text.IndexOf("\n", StringComparison.Ordinal);

        _timeText.text = text.Substring(0, separatorIndex + 1) + string.Format("{0:D2}:{1:D2}:{2:D2}", length.Hours, length.Minutes, length.Seconds);
    }

    public void OnPlayerOpening()
    {
        _videoPath.gameObject.SetActive(false);
        Debug.Log("Opening");
    }

    public void OnPlayerBuffering(float percentage)
    {
        if (!_debug.text.Contains(BUFFERING))
            _debug.text = BUFFERING + ": " + percentage + "%";

        SetDebugText(BUFFERING + ": " + percentage + "%");

        Debug.Log("Buffering: " + percentage + "%");
    }

    public void OnPlayerPrepared(Texture2D videoTexutre)
    {
        Debug.Log("Buffering");
    }

    public void OnPlayerPlaying()
    {
        if (_mediaLength < 0)
        {
            _mediaLength = _mediaPlayer.GetLength();

            SetMediaLength(_mediaLength);
        }

        _rewind.enabled = true;
        Debug.Log("Playing");
    }

    public void OnPlayerPaused()
    {
        Debug.Log("Paused");
    }

    public void OnPlayerStopped()
    {
        Debug.Log("Stopped");
    }

    public void OnPlayerEndReached()
    {
        OnStopClick();
        Debug.Log("OnPlayerEndReached");
    }

    public void OnPlayerEncounteredError()
    {
        OnStopClick();
        if (_mediaPlayer is StandaloneMediaPlayer)
            Debug.Log((_mediaPlayer as StandaloneMediaPlayer).GetLastError());
    }

    public void OnPlayerTimeChanged(long time)
    {
        SetPlayerTime(time);
    }

    public void OnPlayerPositionChanged(float position)
    {
        _rewind.value = position;
    }

    public void OnPlayerBuffering()
    {
        throw new NotImplementedException();
    }
}
