using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

// public enum NPCID {
//     Lovers = "$LoversNPCState",
//     Perseus = "",
// }

public class NPCDialogue : MonoBehaviour
{
    private State currentState;
    public State CurrentState
    {
        get
        {
            return currentState;
        }
    }

    private bool canTalk;

    public DialogueRunner dialogueRunner;
    // Start is called before the first frame update
    void Start()
    {
        currentState = new IdleState(dialogueRunner);
        currentState.OnEnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.Space))
        {
            //dialogueRunner.StartDialogue("LoversNPC");
            string dialogueAnswer;
            dialogueRunner.VariableStorage.TryGetValue("$LoversNPCState", out dialogueAnswer);
            Debug.Log($"LoversNPCState: {dialogueAnswer}");
            if (dialogueAnswer.Equals("Affirmative"))
            {
                ChangeDialogueState(new IncompleteTaskState(dialogueRunner));
            }
            else if (dialogueAnswer.Equals("TalkToNPCAgain"))
            {
                ChangeDialogueState(new CompletedTaskState(dialogueRunner));
            }
            else if (dialogueAnswer.Equals("FinalState"))
            {
                ChangeDialogueState(new AllFinishedState(dialogueRunner));
            }
            else if (dialogueAnswer.Equals("Beginning"))
            {
                ChangeDialogueState(new IdleState(dialogueRunner));
            }
            currentState.OnExecuteState(this);
        }
        if (canTalk && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ChangeDialogueState(State newState)
    {
        currentState.OnExitState(this);
        currentState = newState;
        newState.OnEnterState(this);
        Debug.Log($"STATE: {currentState.GetType()}");
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            canTalk = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            canTalk = false;   
        }
    }
}
