using UnityEngine;
using System.Collections;

public class CompetitionManager : MonoBehaviour {

	public GameObject avatars;
    public ProfilePicture avatar1;
    public ProfilePicture avatar2;
    private CharacterBehavior characterBehavior;
    private bool isCompetition;
    public float distance;
    public int nextGoalDistance;
    private CharactersManager charactersManager;

    public void Init()
    {
		return;
//        charactersManager = Game.Instance.GetComponent<CharactersManager>();
//
//      //  if (Data.Instance.userData.facebookId != "0")
//       //     avatar1.SetPicture(Data.Instance.userData.facebookId);
//
//        isCompetition = true;
//        Data.Instance.events.OnCompetitionMissionComplete += OnCompetitionMissionComplete;
//        characterBehavior = GetComponent<CharactersManager>().character;
//        SetGoal();
    }
    void OnDestroy()
    {
        Data.Instance.events.OnCompetitionMissionComplete -= OnCompetitionMissionComplete;
    }
    void OnCompetitionMissionComplete()
    {
       // Social.Instance.hiscores.SetMyScoreWhenPlaying((int)(distance));
      //  SetGoal();
    }
    void SetGoal()
    {
//        Hiscores.Hiscore goalHiscore = Social.Instance.hiscores.GetMyNextGoal();
//        if (goalHiscore == null)
//        {
//            Debug.Log("__________ya no hay rivales!");
//            return;
//        }
//        nextGoalDistance = goalHiscore.score;
//        avatar2.SetPicture(goalHiscore.facebookID);
//        Data.Instance.missions.MissionActive.distance = nextGoalDistance;
    }
    void Update()
    {
        if (!isCompetition) return;

        distance = charactersManager.distance;
    }
}
