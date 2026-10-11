using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    
    public void Play()
    {
        videoPlayer.Play();
    }
    
    public void Stop()
    {
        videoPlayer.Stop();
    }
}
