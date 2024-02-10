using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class TutorialVideoPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private VideoClip punchVideoClip;
    [SerializeField] private VideoClip kickVideoClip;
    [SerializeField] private VideoClip hitVideoClip;
    [SerializeField] private VideoClip PKThunderVideoClip;
    [SerializeField] private VideoClip PKFireVideoClip;
    
    public async UniTask PlayPunchVideo()
    {
        AppearingVideo();
        videoPlayer.clip = punchVideoClip;
        videoPlayer.Play();
        await UniTask.Delay((int)punchVideoClip.length * 1000+1000);
        DisappearingVideo();
    }
    
    public async UniTask PlayKickVideo()
    {
        AppearingVideo();
        videoPlayer.clip = kickVideoClip;
        videoPlayer.Play();
        await UniTask.Delay((int)kickVideoClip.length * 1000+1000);
        DisappearingVideo();
    }
    
    public async UniTask PlayHitVideo()
    {
        AppearingVideo();
        videoPlayer.clip = hitVideoClip;
        videoPlayer.Play();
        await UniTask.Delay((int)hitVideoClip.length * 1000+1000);
        DisappearingVideo();
    }
    
    public async UniTask PlayPKThunderVideo()
    {
        AppearingVideo();
        videoPlayer.clip = PKThunderVideoClip;
        videoPlayer.Play();
        await UniTask.Delay((int)PKThunderVideoClip.length * 1000+1000);
        DisappearingVideo();
    }
    
    public async UniTask PlayPKFireVideo()
    {
        AppearingVideo();
        videoPlayer.clip = PKFireVideoClip;
        videoPlayer.Play();
        await UniTask.Delay((int)PKFireVideoClip.length * 1000+1000);
        DisappearingVideo();
    }
    
    public void StopVideo()
    {
        videoPlayer.Stop();
    }
    
    public void PauseVideo()
    {
        videoPlayer.Pause();
    }
    
    public void AppearingVideo()
    {
        canvasGroup.alpha = 1;
    }
    
    public void DisappearingVideo()
    {
        canvasGroup.alpha = 0;
    }
}
