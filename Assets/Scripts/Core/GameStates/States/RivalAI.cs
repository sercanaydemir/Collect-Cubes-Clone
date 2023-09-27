using UnityEngine;


namespace CaseStudy.Core.GameStates.States
{
    public class RivalAI : BaseState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            Debug.Log("GameState: "+"RivalAI");
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