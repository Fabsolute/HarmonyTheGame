using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : ManagerBase<MusicManager>
{
    public List<AudioSource> Sources = new List<AudioSource>();

    public int PlayCounter = 0;

    public void PlayNext()
    {
        if (Sources.Count > PlayCounter)
        {
            Sources[PlayCounter].volume = 1;
            PlayCounter++;
        }
    }

}
