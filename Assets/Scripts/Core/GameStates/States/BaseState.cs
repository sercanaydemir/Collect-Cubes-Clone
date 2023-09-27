using CaseStudy.Core.Utils;
using CaseStudy.UI.Core;
using UnityEngine;

namespace CaseStudy.Core.GameStates.States
{
    public abstract class BaseState
    {

        public virtual void OnStateEnter()
        {
            
            UIScreen.ChangeUIScreenType(UIScreenType.TapToStartPanel);
        }
        public abstract void OnStateUpdate();

        public virtual void OnStateExit()
        {
            UIScreen.ChangeUIScreenType(UIScreenType.GameEnd);
        }
        
    }
}