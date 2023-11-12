using System;
using Core.GameStates;

namespace Common.Dialogues.Interfaces
{
    public interface IDialogueArgsRequester
    {
        event Func<DialogueStateArgs> RequestArgs;
    }
}