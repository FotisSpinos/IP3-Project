using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioManager))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    // Building sounds
    [SerializeField] AudioClip buildingDamage;
    [SerializeField] AudioClip buildingDestroy;

    // Character AudioArrays
    [SerializeField] AudioClip[] centipedeAudio;
    [SerializeField] AudioClip[] lobsterAudio;
    [SerializeField] AudioClip[] frogAudio;
    [SerializeField] AudioClip[] spiderAudio;

    Dictionary<int, AudioClip[]> characterSounds;
    AudioSource[] activeSounds;

    private void OnEnable()
    {
        BuildingStats.OnPlaySound += PlayBuildingAudio;
        Player.OnPlaySound += PlayCharacterAudio;
    }

    private void Start()
    {
        characterSounds = new Dictionary<int, AudioClip[]>();
        activeSounds = new AudioSource[4];

        characterSounds[0] = centipedeAudio;
        characterSounds[1] = lobsterAudio;
        characterSounds[2] = frogAudio;
        characterSounds[3] = spiderAudio;
    }

    public enum BuildingEvent
    {
        DAMAGE,
        DESTROY
    };

    public enum PlayerEvent
    {
        WALK,
        DASH,
        ATTACK
    };

    public void PlayBuildingAudio(BuildingEvent buildingEvent, Vector3 pos)
    {
        AudioClip selectedClip = null;
        AudioSource aso = null;

        if (buildingEvent == BuildingEvent.DAMAGE)
            selectedClip = buildingDamage;
        else if(buildingEvent == BuildingEvent.DESTROY)
            selectedClip = buildingDestroy;

        CreateAudioObj(selectedClip, pos, out aso);
        aso.Play();

        StartCoroutine(ClearAudioObj(aso.clip.length, aso.gameObject));
    }

    public void PlayCharacterAudio(int playerNum, Player.Actions playerState, Transform playerTrans)
    {
        playerNum--;
        AudioClip selectedClip = null;
        AudioSource aso = null;

        if (activeSounds[playerNum] != null)
        {
            Destroy(activeSounds[playerNum].gameObject);
            activeSounds[playerNum] = null;
        }


        if (playerState == Player.Actions.WALK)
        {
            selectedClip = characterSounds[playerNum][0];

            CreateAudioObj(selectedClip, playerTrans, out aso);
            aso.Play();

            activeSounds[playerNum] = aso;
        }

        else if (playerState == Player.Actions.DASH)
        {
            selectedClip = characterSounds[playerNum][1];

            CreateAudioObj(selectedClip, playerTrans, out aso);
            aso.Play();

            activeSounds[playerNum] = aso;
        }

        else if (playerState == Player.Actions.ATTACK)
        {
            selectedClip = characterSounds[playerNum][2];

            CreateAudioObj(selectedClip, playerTrans.position, out aso);
            aso.Play();

            StartCoroutine(ClearAudioObj(aso.clip.length, aso.gameObject));
        }
        else
            return;
    }

    private void ClearTmpEso()
    {
        foreach(AudioSource aso in activeSounds)
        {

        }
    }

    private void CreateAudioObj(AudioClip selectedClip, Vector3 pos, out AudioSource aso)
    {
        GameObject audioObj = new GameObject();
        audioObj.transform.position = pos;
        aso = audioObj.AddComponent<AudioSource>();
        aso.clip = selectedClip;

    }

    private void CreateAudioObj(AudioClip selectedClip, Transform parentTrans, out AudioSource aso)
    {
        GameObject audioObj = new GameObject();
        audioObj.transform.parent = parentTrans;
        audioObj.transform.position = parentTrans.position;
        aso = audioObj.AddComponent<AudioSource>();
        aso.clip = selectedClip;
    }

    private IEnumerator ClearAudioObj(float t, GameObject audioObj)
    {
        yield return new WaitForSeconds(t);
        Destroy(audioObj);
    }
}
