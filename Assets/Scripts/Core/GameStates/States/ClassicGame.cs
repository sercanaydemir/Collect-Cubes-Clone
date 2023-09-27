using UnityEngine;

namespace CaseStudy.Core.GameStates.States
{
    public class ClassicGame : BaseState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            
            Debug.Log("GameState: "+"ClassicGame");
        }

        public override void OnStateUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
        
        
    }
}