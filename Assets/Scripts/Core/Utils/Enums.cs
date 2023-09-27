using UnityEngine;

namespace CaseStudy.Core.Utils
{
    public enum GameState
    {
        Wait,
        Classic,
        TimeChallenge,
        RivalAI
    }

    public enum UIScreenType
    {
        TapToStartPanel,
        Game,
        StateSelection,
        GameEnd
    }
    
    public enum CollectedItemType
    {
        None,
        Player,
        AI
    }
}